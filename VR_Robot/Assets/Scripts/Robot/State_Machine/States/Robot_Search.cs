using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot_Search : Robot_Interaction_State
{
    public Robot_Search(Robot_Interaction_Context _context, Robot_Interaction_State_Machine.ERobotInteractionState estate) : base(_context, estate) {
        Robot_Interaction_Context context = _context;
    }

    Transform target;

    // Find nearest object to explore
    public override void EnterState() {
        // Get all interactable objects within a specified range
        Collider[] cols = new Collider[0];
        int iterations = 0;

        while(cols.Length < 2 || iterations < 10) {
            cols = Physics.OverlapSphere(context.AI.transform.position, 3 + iterations, context.LayerMask);
            iterations++;
        }

        // If we find nothing, return to the idle state
        if (cols.Length == 0) { DEBUG_NextState = Robot_Interaction_State_Machine.ERobotInteractionState.Idle; }

        float nearest = 9999;
        int nearestID = 0;

        // If we find more than 1 valid collider, navigate to the nearest one
        for (int i = 0; i < cols.Length; i++) {
            float dist = Vector3.Distance(cols[i].transform.position, context.AI.transform.position);
            if (dist < nearest && !context.CheckedObjects.Contains(cols[i].transform)) {
                nearest = dist;
                nearestID = i;
            }
        }

        // Once we have a nearest object, navigate to it
        context.AI.SetDestination(cols[nearestID].transform.position);
        target = cols[nearestID].transform;
        context._searching = true;
    }
    public override void ExitState() { DEBUG_NextState = Robot_Interaction_State_Machine.ERobotInteractionState.BAD; }
    public override void UpdateState() { }
    public override Robot_Interaction_State_Machine.ERobotInteractionState GetNextState() {
        if (context.Attentive) { return Robot_Interaction_State_Machine.ERobotInteractionState.Attentive; }

        if (DEBUG_NextState != Robot_Interaction_State_Machine.ERobotInteractionState.BAD) {
            return DEBUG_NextState;
        }

        // When we have reached out destination, enter the SCAN state. Add the target to the context list
        if (Vector3.Distance(context.AI.transform.position, context.AI.destination) < 0.33f) {
            context.AddChekedObjects(target);
            return Robot_Interaction_State_Machine.ERobotInteractionState.Scan;
        }

        return StateKey;
    }
    public override void LateUpdateState() { }
    public override void OnTriggerEnter(Collider _other) { }
    public override void OnTriggerStay(Collider _other) { }
    public override void OnTriggerExit(Collider _other) { }
    public override void Interact() { }

    public override void DEBUG_SwitchState(Robot_Interaction_State_Machine.ERobotInteractionState state) { 
        DEBUG_NextState = state; 
    }
}
