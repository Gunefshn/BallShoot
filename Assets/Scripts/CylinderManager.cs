using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CylinderManager : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    bool ButtonPressed;
    public GameObject CylinderObject;
    [SerializeField]private float TurnCircle;
    [SerializeField] private string Direction;

    public void OnPointerDown(PointerEventData eventData)
    {
        ButtonPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        ButtonPressed = false;
    }

    void Update()
    {
        if(ButtonPressed)
        {
            if(Direction == "Left")
            {
                CylinderObject.transform.Rotate(0, TurnCircle * Time.deltaTime, 0, Space.Self);
            }
            else if(Direction =="Right")
            {
                CylinderObject.transform.Rotate(0, -TurnCircle * Time.deltaTime, 0, Space.Self);
            }
        }   
        else
        {

        }
    }
}
