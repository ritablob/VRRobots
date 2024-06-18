using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot_Grab : Robot_Interaction_State
{
    public Robot_Grab(Robot_Interaction_Context _context, Robot_Interaction_State_Machine.ERobotInteractionState estate) : base(_context, estate) {
        Robot_Interaction_Context context = _context;
    }

    public override void EnterState() { context.Anim.SetTrigger("Grab"); }
    public override void ExitState() { }
    public override void UpdateState() { }
    public override Robot_Interaction_State_Machine.ERobotInteractionState GetNextState() {
        if (context.DEBUG_GetState != Robot_Interaction_State_Machine.ERobotInteractionState.BAD) {
            Robot_Interaction_State_Machine.ERobotInteractionState nextState = context.DEBUG_GetState;
            context.DEBUG_SetState(Robot_Interaction_State_Machine.ERobotInteractionState.BAD);

            return nextState;
        }

        if (Vector3.Distance(context.Interactable.position, context.WorldPos) >= 3) {
            return Robot_Interaction_State_Machine.ERobotInteractionState.Search;
        }

        return StateKey;
    }
    public override void OnTriggerEnter(Collider _other) { }
    public override void OnTriggerStay(Collider _other) { }
    public override void OnTriggerExit(Collider _other) { }


    public override void DEBUG_SwitchState(Robot_Interaction_State_Machine.ERobotInteractionState state) {
        DEBUG_NextState = state;
    }
}
