using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot_Grab : Robot_Interaction_State
{
    public Robot_Grab(Robot_Interaction_Context _context, Robot_Interaction_State_Machine.ERobotInteractionState estate) : base(_context, estate) {
        Robot_Interaction_Context context = _context;
    }


    public override void EnterState() { 
        context.LerpArmWeight(1, 2.5f);
        context.Anim.SetBool("Start Grab", true);
        context.Anim.SetBool("Scanning", false);
        context.Anim.SetTrigger("End Walk");
    }
    public override void ExitState() { 
    }
    public override void UpdateState() {
        context.Head.LookAt(context.IKController.Target); 
    }
    public override Robot_Interaction_State_Machine.ERobotInteractionState GetNextState() {
        if (context.Attentive) { return Robot_Interaction_State_Machine.ERobotInteractionState.Attentive; }

        if (context.IKController.Target == null) { return Robot_Interaction_State_Machine.ERobotInteractionState.Idle; }

        if (context.IKController.Target.gameObject.layer == 8 &&
            Vector3.Distance(context.IKController.Tooltip.position, context.IKController.Target.position) < context.IKController.distanceThreshold) 
        {
            context.IKController.CatchObject();
            return Robot_Interaction_State_Machine.ERobotInteractionState.Store; 
        } 
        else if (Vector3.Distance(context.IKController.Tooltip.position, context.IKController.Target.position) < context.IKController.distanceThreshold) 
        {
            context.IKController.CatchObject();
            return Robot_Interaction_State_Machine.ERobotInteractionState.Place;
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
