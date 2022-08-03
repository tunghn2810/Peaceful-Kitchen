using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.InputSystem.Layouts;

////TODO: custom icon for OnScreenStick component

namespace UnityEngine.InputSystem.OnScreen
{
    /// <summary>
    /// A stick control displayed on screen and moved around by touch or other pointer
    /// input.
    /// </summary>
    public class CustomStick : OnScreenControl, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        //Floating joystick package
        public float HandleRange
        {
            get { return handleRange; }
            set { handleRange = Mathf.Abs(value); }
        }

        public float DeadZone
        {
            get { return deadZone; }
            set { deadZone = Mathf.Abs(value); }
        }

        [SerializeField] private float handleRange = 1;
        [SerializeField] private float deadZone = 0;
        [SerializeField] private AxisOptions axisOptions = AxisOptions.Both;

        [SerializeField] protected RectTransform background = null;
        [SerializeField] private RectTransform handle = null;
        private RectTransform baseRect = null;

        private Canvas canvas;
        private Camera cam;

        private Vector2 input = Vector2.zero;

        public AxisOptions AxisOptions { get { return AxisOptions; } set { axisOptions = value; } }
        //end package

        public void OnPointerDown(PointerEventData eventData)
        {
            //Floating joystick package
            background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
            background.gameObject.SetActive(true);

            OnDrag(eventData);
            //end package
        }

        public void OnDrag(PointerEventData eventData)
        {
            //Floating joystick package
            cam = null;
            if (canvas.renderMode == RenderMode.ScreenSpaceCamera)
                cam = canvas.worldCamera;

            Vector2 position = RectTransformUtility.WorldToScreenPoint(cam, background.position);
            Vector2 radius = background.sizeDelta / 2;
            input = (eventData.position - position) / (radius * canvas.scaleFactor);
            FormatInput();
            HandleInput(input.magnitude, input.normalized, radius, cam);
            handle.anchoredPosition = input * radius * handleRange;
            //end package
            SendValueToControl(input);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            //Floating joystick package
            background.gameObject.SetActive(false);
            input = Vector2.zero;
            handle.anchoredPosition = Vector2.zero;
            //end package

            SendValueToControl(Vector2.zero);
        }

        private void Start()
        {
            //Floating joystick package
            HandleRange = handleRange;
            DeadZone = deadZone;
            baseRect = GetComponent<RectTransform>();
            canvas = GetComponentInParent<Canvas>();
            if (canvas == null)
                Debug.LogError("The Joystick is not placed inside a canvas");

            Vector2 center = new Vector2(0.5f, 0.5f);
            background.pivot = center;
            handle.anchorMin = center;
            handle.anchorMax = center;
            handle.pivot = center;
            handle.anchoredPosition = Vector2.zero;

            background.gameObject.SetActive(false);
            //end package
        }

        //Floating joystick package
        protected virtual void HandleInput(float magnitude, Vector2 normalised, Vector2 radius, Camera cam)
        {
            if (magnitude > deadZone)
            {
                if (magnitude > 1)
                {
                    input = normalised;
                }
            }
            else
                input = Vector2.zero;
        }

        //Floating joystick package
        private void FormatInput()
        {
            if (axisOptions == AxisOptions.Horizontal)
                input = new Vector2(input.x, 0f);
            else if (axisOptions == AxisOptions.Vertical)
                input = new Vector2(0f, input.y);
        }

        //Floating joystick package
        protected Vector2 ScreenPointToAnchoredPosition(Vector2 screenPosition)
        {
            Vector2 localPoint = Vector2.zero;

            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(baseRect, screenPosition, cam, out localPoint))
            {
                Vector2 pivotOffset = baseRect.pivot * baseRect.sizeDelta;

                return localPoint - (background.anchorMax * baseRect.sizeDelta) + pivotOffset;
            }

            return Vector2.zero;
        }

        [InputControl(layout = "Vector2")]
        [SerializeField]
        private string m_ControlPath;

        private Vector3 m_StartPos;
        private Vector2 m_PointerDownPos;

        protected override string controlPathInternal
        {
            get => m_ControlPath;
            set => m_ControlPath = value;
        }
    }
    public enum AxisOptions { Both, Horizontal, Vertical }
}