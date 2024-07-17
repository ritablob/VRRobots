using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class StateManager<Estate> : MonoBehaviour where Estate : Enum
{
    protected Dictionary<Estate, BaseState<Estate>> states = new Dictionary<Estate, BaseState<Estate>>();
    protected BaseState<Estate> currentState;

    protected bool isTransitioningState = false;

    private void Start() {
        currentState.EnterState();
    }

    private void Update() {
        Estate nextStateKey = currentState.GetNextState();

        if (!isTransitioningState && nextStateKey.Equals(currentState.StateKey)) {
            currentState.UpdateState();
            Director.instance.Log(currentState.ToString());
        } else if (!isTransitioningState) {
            Debug.Log("IDNJFIUJFGJDGF" + currentState);
            TransitionToState(nextStateKey);
        }
    }

    private void LateUpdate()
    {
        Estate nextStateKey = currentState.GetNextState();

        if (!isTransitioningState && nextStateKey.Equals(currentState.StateKey)) {
            currentState.LateUpdateState();
        }
    }

    public void TransitionToState(Estate _stateKey) {
        isTransitioningState = true;
        currentState.ExitState();
        currentState = states[_stateKey];
        currentState.EnterState();
        isTransitioningState = false;
    }

    private void OnTriggerEnter(Collider other) {
        currentState.OnTriggerEnter(other);
    }

    private void OnTriggerStay(Collider other) {
        currentState.OnTriggerStay(other);
    }

    private void OnTriggerExit(Collider other) {
        currentState.OnTriggerExit(other);
    }
}

