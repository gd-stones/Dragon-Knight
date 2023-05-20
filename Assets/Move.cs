using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Move : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public int direction;
    [SerializeField] private PlayerMovement pm;

    public void OnPointerDown(PointerEventData eventData)
    {
        pm.ChangeDirection(direction);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        pm.ResetDirection();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
