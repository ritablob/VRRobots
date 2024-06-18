//This script is an abstract class that communicates between the context & states of the given state machine

using UnityEngine;

public abstract class Robot_Interaction_State : BaseState<Robot_Interaction_State_Machine.ERobotInteractionState>
{
    protected Robot_Interaction_Context context;
    protected Robot_Interaction_State_Machine.ERobotInteractionState DEBUG_NextState;

    public Robot_Interaction_State(Robot_Interaction_Context _context, Robot_Interaction_State_Machine.ERobotInteractionState stateKey) : base(stateKey) {
        context = _context;
    }

    public abstract void DEBUG_SwitchState(Robot_Interaction_State_Machine.ERobotInteractionState state);
}
