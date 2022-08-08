using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTextScript : MonoBehaviour
{
    private void Update()
    {
        if (ControlsManager.Instance.currentLayer == Layer.Player_Veg)
        {
            if (gameObject.transform.parent.gameObject.layer == Layer.Enemy_Veg)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = IngameTutorials.Instance.vegAllyText;
            }
            else if (gameObject.transform.parent.gameObject.layer == Layer.Enemy_Meat)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = IngameTutorials.Instance.meatEnemyText;
            }
        }
        else
        {
            if (gameObject.transform.parent.gameObject.layer == Layer.Enemy_Veg)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = IngameTutorials.Instance.vegEnemyText;
            }
            else if (gameObject.transform.parent.gameObject.layer == Layer.Enemy_Meat)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = IngameTutorials.Instance.meatAlleyText;
            }
        }

        transform.localPosition = Vector2.up;
        if (transform.parent.localScale.x < 0)
        {
            transform.localScale = new Vector3(-1.5f, 1.5f, 1.5f);
        }
        else
        {
            transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        }
    }
}
