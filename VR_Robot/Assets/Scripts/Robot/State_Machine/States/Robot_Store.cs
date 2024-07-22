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
        context.Anim.SetTrigger("Store");
        context.Anim.SetBool("FlapOpen", true);
        context.LerpArmWeight(0, 2);
        VFXApplicationHelper.instance.RobotCanvasManager.ShowSpeechBubbleMessage("Storing Trash");
    }
    public override void ExitState() {
        context.Anim.SetBool("FlapOpen", false);
        context.Anim.SetBool("Start Grab", false);
        context.LerpArmWeight(0, 1);
        if (context.IKController.Target != null) { Director.instance.CopyObject(context.IKController.Target.gameObject); }
        VFXApplicationHelper.instance.RobotCanvasManager.HideSpeechBubble();
    }  
    public override void UpdateState() { }
    public override Robot_Interaction_State_Machine.ERobotInteractionState GetNextState() {
        if (context.IKController.Timer >= 1) {
            if (context.Attentive) { return Robot_Interaction_State_Machine.ERobotInteractionState.Attentive; }

            return Robot_Interaction_State_Machine.ERobotInteractionState.Idle; 
        }
        if (context.IKController.Timer == 0) {
            if (context.Attentive) { return Robot_Interaction_State_Machine.ERobotInteractionState.Attentive; }

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
