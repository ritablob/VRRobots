using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot_Scan : Robot_Interaction_State
{
    public Robot_Scan(Robot_Interaction_Context _context, Robot_Interaction_State_Machine.ERobotInteractionState estate) : base (_context, estate) {
        Robot_Interaction_Context context = _context;
    }

    private float timer;

    public override void EnterState() {
        timer = 0;
    }
    public override void ExitState() { }
    public override void UpdateState() {
        timer += Time.deltaTime;
    }
    public override Robot_Interaction_State_Machine.ERobotInteractionState GetNextState() {
        if (context.Attentive) { return Robot_Interaction_State_Machine.ERobotInteractionState.Attentive; }

        // After the 'scan', either store or ignore the object. If ignoring it, move directly to search state again
        if (timer > 2) {
            Debug.Log("Scan over");

            if (context.CheckedObjects[0].TryGetComponent<Garbage_Bit>(out Garbage_Bit bit)) {
                context.IKController.Target = context.CheckedObjects[0];
                context.IKController.CatchObject();
                return Robot_Interaction_State_Machine.ERobotInteractionState.Store;
            }
            else {
                Debug.Log("Return to searh");
                return Robot_Interaction_State_Machine.ERobotInteractionState.Search;
            }
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
