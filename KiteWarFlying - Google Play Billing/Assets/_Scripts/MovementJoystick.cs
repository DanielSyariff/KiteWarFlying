using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MovementJoystick : MonoBehaviour
{
    public GameObject joystick;
    public GameObject joystickBG;
    public Vector2 joystickVec;
    private Vector2 joystickTouchPos;
    private Vector2 joystickOriginalPos;
    private float joystickRadius;

    // Start is called before the first frame update
    void Start()
    {
        VisibleJoystick();

        joystickOriginalPos = joystickBG.transform.position;
        joystickRadius = joystickBG.GetComponent<RectTransform>().sizeDelta.y / 8;
    }

    public void PointerDown()
    {
        //if (GameManager.instance.GetOnPause() == false)
        //{
            joystick.transform.position = Input.mousePosition;
            joystickBG.transform.position = Input.mousePosition;
            joystickTouchPos = Input.mousePosition;
        //}
        //else
        //{
        //    PointerUp();
        //}
    }

    public void Drag(BaseEventData baseEventData)
    {
        //if (GameManager.instance.GetOnPause() == false)
        //{
            PointerEventData pointerEventData = baseEventData as PointerEventData;
            Vector2 dragPos = pointerEventData.position;
            joystickVec = (dragPos - joystickTouchPos).normalized;

            float joystickDist = Vector2.Distance(dragPos, joystickTouchPos);

            if (joystickDist < joystickRadius)
            {
                joystick.transform.position = joystickTouchPos + joystickVec * joystickDist;
            }

            else
            {
                joystick.transform.position = joystickTouchPos + joystickVec * joystickRadius;
            }
        //}
        //else
        //{
        //    PointerUp();
        //}
    }

    public void PointerUp()
    {
        joystickVec = Vector2.zero;
        joystick.transform.position = joystickOriginalPos;
        joystickBG.transform.position = joystickOriginalPos;
    }

    public void VisibleJoystick()
    {
        if (PlayerPrefs.GetInt("JoystickToggle", 0) == 0)
        {
            joystick.GetComponent<Image>().color = new Color32(255, 255, 255, 100);
            joystickBG.GetComponent<Image>().color = new Color32(255, 255, 255, 100);
        }
        else
        {
            joystick.GetComponent<Image>().color = new Color32(255, 255, 255, 0);
            joystickBG.GetComponent<Image>().color = new Color32(255, 255, 255, 0);
        }
    }
}
