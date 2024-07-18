//This script is the 'brain' of the state machine
//It holds references to all possible states

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

public class Robot_Interaction_State_Machine : StateManager<Robot_Interaction_State_Machine.ERobotInteractionState>
{
    //States 
    public enum ERobotInteractionState
    {
        Idle,
        Search,
        Scan,
        Grab,
        Store,
        Dump,
        Attentive,
        Place,
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
    [SerializeField] Transform head;
    [SerializeField] LayerMask layerMask;
    [SerializeField] NavMeshAgent AI;
    [SerializeField] AnimationCurve curve;

    float timer = 1;

    //Validation & Context setup
    private void Awake()
    {
        ValidateConstraints();

        _context = new Robot_Interaction_Context(anim, head, ikController, AI, layerMask, transform.position);

        anim.Play("Robot@FlapOpen 0", 2, 1);

        InitializeStates();
    }

    private void ValidateConstraints()
    {
    }

    private void InitializeStates() {
        //Add states to inherited state manager "states" dictionary and set the initial state
        states.Add(ERobotInteractionState.Idle, new Robot_Idle(_context, ERobotInteractionState.Idle));
        states.Add(ERobotInteractionState.Grab, new Robot_Grab(_context, ERobotInteractionState.Grab));
        states.Add(ERobotInteractionState.Store, new Robot_Store(_context, ERobotInteractionState.Store));
        states.Add(ERobotInteractionState.Dump, new Robot_Dump(_context, ERobotInteractionState.Dump));
        states.Add(ERobotInteractionState.Attentive, new Robot_Attentive(_context, ERobotInteractionState.Attentive));
        states.Add(ERobotInteractionState.Place, new Robot_PlaceDown(_context, ERobotInteractionState.Place));
        states.Add(ERobotInteractionState.Search, new Robot_Search(_context, ERobotInteractionState.Search));
        states.Add(ERobotInteractionState.Scan, new Robot_Scan(_context, ERobotInteractionState.Scan));

        currentState = states[ERobotInteractionState.Idle];
    }

    public void SearchForObjects() {
        Context.DEBUG_SetState(ERobotInteractionState.Search);
    }

    public void TossItems(ActivateEventArgs args) {
        Context._dump = true;
    }

    public void LerpArmWeight(int desiredWeight, float speed)
    {
        StopAllCoroutines();
        StartCoroutine(E_LerpArmWeight(_context.IKController.ik.GetComponent<Rig>().weight, desiredWeight, speed));
    }

    private IEnumerator E_LerpArmWeight(float startWeight, int desiredWeight, float speed)
    {
        timer = 1 - timer;

        Debug.Log($"start weight = {startWeight}, timer = {timer}, desired Weight = {desiredWeight}");

        while (timer < 1)
        {
            timer += Time.deltaTime * speed;
            _context.IKController.ik.GetComponent<Rig>().weight = Mathf.Lerp(startWeight, desiredWeight, curve.Evaluate(timer));
            yield return null;
        }

        Mathf.Clamp(timer, 0, 1);
    }

    public void InteractWithRobot() { 
        // If attentive, highlight all interactable objects
        if (currentState.StateKey == ERobotInteractionState.Attentive) {
            Director.instance.HighlightObjects(true);
        }
    }

    public ERobotInteractionState GetCurrentState => currentState.StateKey;
    public Robot_Interaction_State GetState => (Robot_Interaction_State)currentState;

    public Robot_Interaction_Context Context => _context;
}
