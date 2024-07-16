using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot_PlaceDown : Robot_Interaction_State
{
    public Robot_PlaceDown(Robot_Interaction_Context _context, Robot_Interaction_State_Machine.ERobotInteractionState estate) : base (_context, estate) {
        Robot_Interaction_Context context = _context;
    }

    public override void EnterState() { 
        context.IKController.StartPos = context.IKController.Tooltip.position;
        context.IKController.StartRot = context.IKController.Tooltip.rotation;
        context.IKController.setOnGround = true;
    }
    public override void ExitState() { context.IKController.ReleaseObject(); }  
    public override void UpdateState() { }
    public override Robot_Interaction_State_Machine.ERobotInteractionState GetNextState() {
        if (context.IKController.Target.gameObject.layer == 8) {
            return Robot_Interaction_State_Machine.ERobotInteractionState.Grab;
        }

        if (context.IKController.Timer >= 1 || context.IKController.Target == null) { return Robot_Interaction_State_Machine.ERobotInteractionState.Idle; }

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
