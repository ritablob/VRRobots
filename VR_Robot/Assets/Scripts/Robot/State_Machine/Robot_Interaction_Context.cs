//Context scripts hold all variables shared between states.
//Robot_Interaction_Context holds all variables shared between robot states
//All variables are protected

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Robot_Interaction_Context : MonoBehaviour
{
    // Vars
    private CCDIK _IKController;
    private Animator _anim;
    private Vector3 _worldPos;
    private Robot_Interaction_State_Machine.ERobotInteractionState _DEBUG_state;
    private Transform _head;
    private LayerMask _layerMask;
    private List<Transform> _checkedObjects = new List<Transform>(0);
    public bool _dump, _attentive, _searching;
    private List<GameObject> _garbageBits = new List<GameObject>(0);
    private NavMeshAgent _AI;

    //Constructor
    public Robot_Interaction_Context(Animator anim, Transform head, CCDIK ikController, NavMeshAgent ai, LayerMask layerMask, Vector3 worldPos) {
        _anim = anim;
        _head = head;
        _IKController = ikController;
        _AI = ai;
        _layerMask = layerMask;
        _worldPos = worldPos;

        AddChekedObjects(_AI.transform);
    }

    // Read-only
    public CCDIK IKController => _IKController;
    public Animator Anim => _anim;
    public Vector3 WorldPos => _worldPos;
    public List<GameObject> GarbageBits => _garbageBits;
    public Transform Head => _head;
    public LayerMask LayerMask => _layerMask;
    public List<Transform> CheckedObjects => _checkedObjects;
    public NavMeshAgent AI => _AI;
    public bool Attentive => _attentive;

    public Robot_Interaction_State_Machine.ERobotInteractionState DEBUG_GetState => _DEBUG_state;

    public void DEBUG_SetState(Robot_Interaction_State_Machine.ERobotInteractionState _state) {
        _DEBUG_state = _state;
    }

    public void AddChekedObjects(Transform checkedObj) {
        _checkedObjects.Insert(0, checkedObj);
    }

    public void SetAttentive(bool state) {
        _attentive = state;
    }
}
