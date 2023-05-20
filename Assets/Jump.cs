using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.EventSystems;

public class Jump : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    [SerializeField] private float rotationLimit = 40;
    [SerializeField] private float rotationSpeed = 15;

    private bool jump = false;

    [SerializeField] private PlayerMovement pm;


    void FixedUpdate()
    {
        if (jump && pm.CanJump())
        {
            Debug.Log("Jump");
            pm.Jump();
            jump = false;
        }
    }

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        jump = true;
    }

    public void OnPointerUp(PointerEventData pointerEventData)
    {
        jump = false;
    }
}
