using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot_Attentive : Robot_Interaction_State
{
    public Robot_Attentive(Robot_Interaction_Context _context, Robot_Interaction_State_Machine.ERobotInteractionState estate) : base (_context, estate) {
        Robot_Interaction_Context context = _context;
    }

    public override void EnterState()
    {
        DEBUG_NextState = Robot_Interaction_State_Machine.ERobotInteractionState.BAD;
        context.SetAttentive(false);
    }
    public override void ExitState()
    {
        context.Head.localEulerAngles = new Vector3(0, 0, 79.192f);
    }
    public override void UpdateState() { 
        context.Head.LookAt(Director.instance.playerCamera);
        context.Head.Rotate(0, 90, 0);
    }
    public override Robot_Interaction_State_Machine.ERobotInteractionState GetNextState() {
        if (DEBUG_NextState != Robot_Interaction_State_Machine.ERobotInteractionState.BAD) {
            return DEBUG_NextState;
        }

        if (context.IKController.Target != null)
        {
            return Robot_Interaction_State_Machine.ERobotInteractionState.Grab;
        }

        return StateKey;
    }
    public override void LateUpdateState() { }
    public override void OnTriggerEnter(Collider _other) { }
    public override void OnTriggerStay(Collider _other) { }
    public override void OnTriggerExit(Collider _other) { }
    public override void Interact() { Director.instance.HighlightObjects(true); }

    public override void DEBUG_SwitchState(Robot_Interaction_State_Machine.ERobotInteractionState state) { 
        DEBUG_NextState = state;
    }
}
