using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroomController : MonoBehaviour
{
    public Transform broomFloorPoint;
    [NonSerialized] public bool attachedToFloor;
    /* bottom of broom attaches to the floor upon grabbing it with two hands
     * when held with one hand, it is detached from the floor
     *
     * - attach to floor
     * - track how many hands hold the broom
     */
    

    void Start()
    {
        
    }

    private void ToggleFloorAttach()
    {
        attachedToFloor = !attachedToFloor;
    }

    private void AttachToFloor()
    {
    }
    private void Update()
    {
        
    }
}
