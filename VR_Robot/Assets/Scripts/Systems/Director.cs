using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Director : MonoBehaviour
{
    public static Director instance;

    #region
    public event Action<string, ObjectiveType> addObjective;
    public void AddObjective(string name, ObjectiveType objectiveType) { if (addObjective != null) { addObjective.Invoke(name, objectiveType); } }


    public event Action<string> completeObjective;
    public void CompleteObjective(string name) { if (completeObjective != null) { completeObjective.Invoke(name); } }
    #endregion

    private void Awake()
    {
        instance = this;
    }
}
