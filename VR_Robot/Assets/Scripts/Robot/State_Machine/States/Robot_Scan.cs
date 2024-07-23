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
        context.Anim.SetBool("Walking", false);
        context.Anim.SetBool("Scanning", true);
        timer = 0;
        VFXApplicationHelper.instance.RobotCanvasManager.ShowSpeechBubbleMessage("Scanning...");
    }
    public override void ExitState() {
        VFXApplicationHelper.instance.RobotCanvasManager.HideSpeechBubble();
    }
    public override void UpdateState() {
        timer += Time.deltaTime;
    }
    public override Robot_Interaction_State_Machine.ERobotInteractionState GetNextState() {
        if (context.Attentive) { return Robot_Interaction_State_Machine.ERobotInteractionState.Attentive; }

        if (context.IKController.Target != null)
        {
            return Robot_Interaction_State_Machine.ERobotInteractionState.Grab;
        }

        // After the 'scan', either store or ignore the object. If ignoring it, move directly to search state again
        if (timer > 2) {
            if (context.CheckedObjects[0].TryGetComponent<Garbage_Bit>(out Garbage_Bit bit)) {
                context.IKController.Target = context.CheckedObjects[0];
                return Robot_Interaction_State_Machine.ERobotInteractionState.Grab;
            }
            else {
                context.Anim.SetBool("Scanning", false);
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
