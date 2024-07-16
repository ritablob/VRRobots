using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot_Idle : Robot_Interaction_State
{
    public Robot_Idle(Robot_Interaction_Context _context, Robot_Interaction_State_Machine.ERobotInteractionState estate) : base (_context, estate) {
        Robot_Interaction_Context context = _context;
    }

    public override void EnterState() {
        context.IKController.Target = null;
        context.IKController.StartPos = Vector3.zero;
    }
    public override void ExitState() { }
    public override void UpdateState() { }
    public override Robot_Interaction_State_Machine.ERobotInteractionState GetNextState() {
        if (context.DEBUG_GetState != Robot_Interaction_State_Machine.ERobotInteractionState.BAD) {
            Robot_Interaction_State_Machine.ERobotInteractionState temp = context.DEBUG_GetState;
            context.DEBUG_SetState(Robot_Interaction_State_Machine.ERobotInteractionState.BAD);
            return temp;
        }

        if (context._dump == true) { return Robot_Interaction_State_Machine.ERobotInteractionState.Dump; }

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
