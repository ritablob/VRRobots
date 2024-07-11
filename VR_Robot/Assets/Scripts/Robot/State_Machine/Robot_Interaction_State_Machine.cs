//This script is the 'brain' of the state machine
//It holds references to all possible states

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.XR.Interaction.Toolkit;

public class Robot_Interaction_State_Machine : StateManager<Robot_Interaction_State_Machine.ERobotInteractionState>
{
    //States 
    public enum ERobotInteractionState
    {
        Idle,
        Search,
        Grab,
        Store,
        Dump,
        BAD //Use for null/exit cases
    }

    //Vars
    private Robot_Interaction_Context _context;

    [SerializeField] Animator anim;
    [SerializeField] Transform lookatTarget;
    [SerializeField] Transform storage;
    [SerializeField] CCDIK ikController;
    [SerializeField] Transform garbageCanGeneral;
    [SerializeField] Transform garbageCanPlastic;
    [SerializeField] Transform garbageCanPaper;

    //Validation & Context setup
    private void Awake()
    {
        ValidateConstraints();

        _context = new Robot_Interaction_Context(anim, ikController, transform.position);

        InitializeStates();
    }

    private void ValidateConstraints()
    {
    }

    private void InitializeStates() {
        //Add states to inherited state manager "states" dictionary and set the initial state
        states.Add(ERobotInteractionState.Idle, new Robot_Idle(_context, ERobotInteractionState.Idle));
        states.Add(ERobotInteractionState.Search, new Robot_Search(_context, ERobotInteractionState.Search));
        states.Add(ERobotInteractionState.Grab, new Robot_Grab(_context, ERobotInteractionState.Grab));
        states.Add(ERobotInteractionState.Store, new Robot_Store(_context, ERobotInteractionState.Store));
        states.Add(ERobotInteractionState.Dump, new Robot_Dump(_context, ERobotInteractionState.Dump));

        currentState = states[ERobotInteractionState.Idle];
    }

    public void SearchForObjects() {
        Context.DEBUG_SetState(ERobotInteractionState.Search);
    }

    public void TossItems(ActivateEventArgs args) {
        Context._dump = true;
    }

    public Robot_Interaction_Context Context => _context;
}
