using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task : MonoBehaviour
{
    protected void Start()
    {
        Director.instance.completeObjective += CompleteTask;
    }

    protected virtual void CompleteTask(string name) { }

    private void OnDestroy()
    {
        Director.instance.completeObjective -= CompleteTask;
    }
}
