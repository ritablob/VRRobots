//Context scripts hold all variables shared between states.
//Robot_Interaction_Context holds all variables shared between robot states
//All variables are protected

using UnityEngine;

public class Robot_Interaction_Context : MonoBehaviour
{
    //Vars
    private Animator _anim;
    private Transform _interactable;
    private Vector3 _worldPos;
    private Robot_Interaction_State_Machine.ERobotInteractionState _DEBUG_state;

    //Constructor
    public Robot_Interaction_Context(Animator anim, Transform interactable, Vector3 worldPos) {
        _anim = anim;
        _interactable = interactable;
        _worldPos = worldPos;
    }

    //Read-only
    public Animator Anim => _anim;
    public Transform Interactable => _interactable;
    public Vector3 WorldPos => _worldPos;

    public Robot_Interaction_State_Machine.ERobotInteractionState DEBUG_GetState => _DEBUG_state;
    public void DEBUG_SetState(Robot_Interaction_State_Machine.ERobotInteractionState _state) {
        _DEBUG_state = _state;
    }
}
