using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot_Store : Robot_Interaction_State
{
    public Robot_Store(Robot_Interaction_Context _context, Robot_Interaction_State_Machine.ERobotInteractionState estate) : base (_context, estate) {
        Robot_Interaction_Context context = _context;
    }

    public override void EnterState() {
        context.IKController.StartPos = context.IKController.Tooltip.position;
        context.IKController.setOnGround = false;
        context.Anim.SetTrigger("Open");
        context.Anim.SetTrigger("Close");
    }
    public override void ExitState() { if (context.IKController.Target != null) { Director.instance.CopyObject(context.IKController.Target.gameObject); } }  
    public override void UpdateState() { }
    public override Robot_Interaction_State_Machine.ERobotInteractionState GetNextState() {
        if (context.DEBUG_GetState != Robot_Interaction_State_Machine.ERobotInteractionState.BAD)
        {
            Robot_Interaction_State_Machine.ERobotInteractionState temp = context.DEBUG_GetState;
            context.DEBUG_SetState(Robot_Interaction_State_Machine.ERobotInteractionState.BAD);
            return temp;
        }

        if (context.IKController.Timer >= 1) { return Robot_Interaction_State_Machine.ERobotInteractionState.Idle; }

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
