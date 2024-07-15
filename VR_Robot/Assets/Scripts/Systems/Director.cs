using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class Director : MonoBehaviour
{
    public static Director instance;

    public TextMeshProUGUI text;
    public AnimationCurve interactLerpCurve;
    public Transform leftHand, rightHand, playerCamera;
    public Transform robot;

    private int points;

    #region
    public event Action<string, ObjectiveType> addObjective;
    public void AddObjective(string name, ObjectiveType objectiveType) { if (addObjective != null) { addObjective.Invoke(name, objectiveType); } }


    public event Action<string> completeObjective;
    public void CompleteObjective(string name) { if (completeObjective != null) { completeObjective.Invoke(name); } }


    public event Action<Transform> setCurrentLookat;
    public void SetCurrentLookat(Transform lookAtTarget) { if (setCurrentLookat != null) { setCurrentLookat.Invoke(lookAtTarget); } }


    public event Action<Transform> releaseObject;
    public void ReleaseObject(Transform target) { if (releaseObject != null) { releaseObject.Invoke(target); } }


    public event Action<bool> grabbedObject;
    public void GrabObject(bool state) { if (grabbedObject != null) { grabbedObject.Invoke(state); } }


    public event Action<GameObject> copyObject;
    public void CopyObject(GameObject origin) { if (copyObject != null) { copyObject.Invoke(origin); } }

    public event Action<bool> highlightObjects;
    public void HighlightObjects(bool state) { if (highlightObjects != null) { highlightObjects.Invoke(state); } }
    #endregion

    public void Log(string msg) {
        text.text = msg;
    }

    private void Awake()
    {
        instance = this;
    }

    public void GetPoints(int pts) {
        points += pts;
        Debug.Log(points);
    }

    public int Points => points;
    public float PlayerCameraXRot => playerCamera.localEulerAngles.x;
}
