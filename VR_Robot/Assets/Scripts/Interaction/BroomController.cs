using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroomController : MonoBehaviour
{
    public Transform broomFloorPoint;
    public bool attachedToFloor;
    public int interactions;

    private Rigidbody rb;
    /* bottom of broom attaches to the floor upon grabbing it with two hands
     * when held with one hand, it is detached from the floor
     *
     * - attach to floor
     * - track how many hands hold the broom
     */
    

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void AddInteraction()
    {
        interactions++;
        Debug.Log("interactions "+interactions);
    }

    public void RemoveInteraction()
    {
        interactions--;
        Debug.Log("interactions "+interactions);

    }
    public void CountInteractions()
    {
        
    }

    private void ToggleFloorAttach()
    {
        attachedToFloor = !attachedToFloor;
    }

    public void AttachToFloor()
    {
        if (interactions == 2)
            attachedToFloor = true;
        
    }

    public void DetachFromFloor()
    {
        attachedToFloor = false;
    }
    private void Update()
    {
        if (attachedToFloor)
        {
            broomFloorPoint.position = new Vector3(broomFloorPoint.position.x, 0, broomFloorPoint.position.z);
            rb.constraints = RigidbodyConstraints.FreezePositionY;
        }
        else
        {
            rb.constraints = RigidbodyConstraints.None;
        }
    }
}
