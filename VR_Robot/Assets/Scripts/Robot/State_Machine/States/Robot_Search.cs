using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot_Search : Robot_Interaction_State
{
    public Robot_Search(Robot_Interaction_Context _context, Robot_Interaction_State_Machine.ERobotInteractionState estate) : base (_context, estate) {
        Robot_Interaction_Context context = _context;
    }

    public override void EnterState() { context.Anim.SetTrigger("Search"); }
    public override void ExitState() { }
    public override void UpdateState() { }
    public override Robot_Interaction_State_Machine.ERobotInteractionState GetNextState() {
        if (context.DEBUG_GetState != Robot_Interaction_State_Machine.ERobotInteractionState.BAD) {
            Robot_Interaction_State_Machine.ERobotInteractionState nextState = context.DEBUG_GetState;
            context.DEBUG_SetState(Robot_Interaction_State_Machine.ERobotInteractionState.BAD);

            return nextState;
        }

        //If an object is close by, change state to the grab state.
        Collider[] colliders = Physics.OverlapSphere(context.GrabPivot.position, 3);
        for (int i = 0; i < colliders.Length; i++) {
            if (colliders[i].TryGetComponent<Task_Interactable>(out Task_Interactable task) && Vector3.Distance(colliders[i].transform.position, context.WorldPos) < 3) {
                context._lookatTarget = colliders[i].transform;
                task.grabbed = true;
                return Robot_Interaction_State_Machine.ERobotInteractionState.Grab;
            }
        }

        return StateKey;
    }
    public override void LateUpdateState() { }
    public override void OnTriggerEnter(Collider _other) { }
    public override void OnTriggerStay(Collider _other) { }
    public override void OnTriggerExit(Collider _other) { }

    public override void DEBUG_SwitchState(Robot_Interaction_State_Machine.ERobotInteractionState state) { 
        DEBUG_NextState = state; 
    }
}
