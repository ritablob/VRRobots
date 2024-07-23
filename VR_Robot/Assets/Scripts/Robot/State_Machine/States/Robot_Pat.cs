using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot_Pat : Robot_Interaction_State
{
    public Robot_Pat(Robot_Interaction_Context _context, Robot_Interaction_State_Machine.ERobotInteractionState estate) : base (_context, estate) {
        Robot_Interaction_Context context = _context;
    }

    private float timer;

    public override void EnterState() {
        context._headpat = false;
        context.Anim.SetBool("Headpat", true);
        VFXApplicationHelper.instance.RobotCanvasManager.ShowSpeechBubbleMessage(":3");
    }
    public override void ExitState() {
        context.Anim.SetBool("Headpat", false);
        VFXApplicationHelper.instance.RobotCanvasManager.HideSpeechBubble();
    }
    public override void UpdateState()
    {
    }
    public override Robot_Interaction_State_Machine.ERobotInteractionState GetNextState() {
        if (context._headpat == false) {
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
