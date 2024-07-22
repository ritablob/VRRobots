using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot_Idle : Robot_Interaction_State
{
    public Robot_Idle(Robot_Interaction_Context _context, Robot_Interaction_State_Machine.ERobotInteractionState estate) : base (_context, estate) {
        Robot_Interaction_Context context = _context;
    }

    private float timer;

    public override void EnterState() {
        if (context._searching) { timer = 3; }
        else { timer = 0; }
        context.IKController.Target = null;
        context.IKController.StartPos = Vector3.zero;
    }
    public override void ExitState() { }
    public override void UpdateState() {
        if (context.AI.remainingDistance <= context.AI.stoppingDistance) {
            timer += Time.deltaTime;
        }
    }
    public override Robot_Interaction_State_Machine.ERobotInteractionState GetNextState() {
        // If attention is active or buffered, transition to the 'Attentive' state
        if (context.Attentive) { return Robot_Interaction_State_Machine.ERobotInteractionState.Attentive; }

        if (context._headpat) { return Robot_Interaction_State_Machine.ERobotInteractionState.Headpat; }

        // If we've been idling for a while, move to the 'Search' state
        if (timer > 30) {
            return Robot_Interaction_State_Machine.ERobotInteractionState.Search;
        }

        // If dumping command is buffered, move to the 'Dump' state
        if (context._dump == true) { return Robot_Interaction_State_Machine.ERobotInteractionState.Dump; }

        // If there is a valid target for the robot to grab, transition to the 'Grab' state
        if (context.IKController.Target != null) {
            return Robot_Interaction_State_Machine.ERobotInteractionState.Grab;
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
