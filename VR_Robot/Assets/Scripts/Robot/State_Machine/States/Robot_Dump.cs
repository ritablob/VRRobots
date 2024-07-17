using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot_Dump : Robot_Interaction_State
{
    public Robot_Dump(Robot_Interaction_Context _context, Robot_Interaction_State_Machine.ERobotInteractionState estate) : base (_context, estate) {
        Robot_Interaction_Context context = _context;
    }

    private float timer;

    public override void EnterState() {
        context.IKController.DumpObjects();
        context._dump = false;
        timer = 0;  
    }
    public override void ExitState() {
        context.Anim.SetBool("FlapOpen", false);
    }
    public override void UpdateState()
    {
        context.AI.transform.LookAt(context.garbagePos);

        if (context.IKController.storedObjects.Count == 0) {
            timer += Time.deltaTime;
        }
    }
    public override Robot_Interaction_State_Machine.ERobotInteractionState GetNextState() {
        if (timer >= 1) {
            return Robot_Interaction_State_Machine.ERobotInteractionState.Idle;
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
