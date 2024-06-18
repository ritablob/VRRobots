//Context scripts hold all variables shared between states.
//Robot_Interaction_Context holds all variables shared between robot states
//All variables are protected

using UnityEngine;

public class Robot_Interaction_Context : MonoBehaviour
{
    //Vars
    private Animator _anim;
    private Robot_Interaction_State_Machine.ERobotInteractionState _DEBUG_state;

    //Constructor
    public Robot_Interaction_Context(Animator anim) {
        _anim = anim;
    }

    //Read-only
    public Animator Anim => _anim;
    public Robot_Interaction_State_Machine.ERobotInteractionState DEBUG_GetState => _DEBUG_state;
    public void DEBUG_SetState(Robot_Interaction_State_Machine.ERobotInteractionState _state) {
        _DEBUG_state = _state;
    }
}
