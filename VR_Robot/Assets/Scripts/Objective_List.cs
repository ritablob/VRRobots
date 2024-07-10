using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objective_List : MonoBehaviour
{
    public GameObject checkboxTask, progressBarTask;
    [SerializeField] private Transform taskHolder;

    private void Start()
    {
        Director.instance.addObjective += AddObjective;
    }

    private void AddObjective(string name, ObjectiveType objectiveType) { 
        // Create the objective. Ensure it doesn't share a name with another task

        switch (objectiveType) {
            case ObjectiveType.Checkbox: 
                if (!HasObjective(name)) {
                    Instantiate(checkboxTask, taskHolder).GetComponent<Checkbox>().Initialize(name);
                }
                break;
            case ObjectiveType.ProgressBar:
                if (!HasObjective(name)) {
                    Instantiate(progressBarTask, taskHolder);
                    
                }
                break;
        }
    }

    private bool HasObjective(string name) { 
        for (int i = 0; i < taskHolder.childCount; i++) { 
            if (taskHolder.GetChild(i).gameObject.name == name) { return true; }
        }

        return false;
    }
}

public enum ObjectiveType
{
    Checkbox,
    ProgressBar
}
