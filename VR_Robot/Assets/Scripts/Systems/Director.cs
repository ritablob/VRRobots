using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

public class Director : MonoBehaviour
{
    public static Director instance;

    public TextMeshProUGUI text, timeStat, robotCleanPerc;
    public AnimationCurve interactLerpCurve;
    public Transform leftHand, rightHand, playerCamera;
    public Transform robot;
    [HideInInspector] public Transform currentHighlighted;
    public Image[] starImages;
    public GameObject highlightObject;
    public int maxPiecesOfTrash, maxTimeToClean;

    [HideInInspector] public float highlightLerpTimer;

    private int points;
    private int piecesOfTrash;
    private int robotTrash;
    private int timeToClean;

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

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        for (int i = 0; i < starImages.Length; i++)
        {
            starImages[i].transform.parent.gameObject.SetActive(false);
        }

        timeStat.gameObject.SetActive(false);
        robotCleanPerc.gameObject.SetActive(false);

        piecesOfTrash = maxPiecesOfTrash;
        timeToClean = maxTimeToClean;
        StartCoroutine(CountDown());
    }

    public void SetCurrentHighlighted(Transform target, bool state) {
        if (!state && currentHighlighted == target) { 
            currentHighlighted = null;
            highlightLerpTimer = 0;
        }
        else if (state) {
            currentHighlighted = target;
            highlightLerpTimer = 0;
        }
    }

    private void Update()
    {
        highlightLerpTimer += Time.deltaTime;
    }

    public void Log(string msg) {
        if (text == null) { return; }
        text.text = msg;
    }

    public void GetPoints(int pts) {
        points += pts;
        piecesOfTrash--;

        if (piecesOfTrash == 0) {
            ShowScore();
        }
    }

    public void ShowScore() {
        StopAllCoroutines();

        // 180 = max points! 
        float _points = points;
        // Can only recieve up to 4 stars if sorting perfectly WITHOUT the robot
        float pointsPerStar = ((float)maxPiecesOfTrash * 5) / 3;

        timeStat.gameObject.SetActive(true);
        robotCleanPerc.gameObject.SetActive(true);

        // Add full star if level is completed with 1/3rd time to spare
        if (timeToClean > (float)maxTimeToClean / 3) { _points += pointsPerStar; }
        // Add half-a-star worth of points if level is completed before the time limit
        else if (timeToClean > 0) { _points += pointsPerStar / 2; }

        timeStat.text = $"Remaining time = {timeToClean}";

        // Remove a full star if the robot threw away everything!
        if (robotTrash >= (float)maxPiecesOfTrash * 0.9f) { _points -= pointsPerStar; }
        else if (robotTrash >= (float)maxPiecesOfTrash / 3) { _points += pointsPerStar; }
        // Add half-a-star worth of points if the robot threw away at least a QUARTER of all trash
        else if (robotTrash >= (float)maxPiecesOfTrash / 4) { _points += pointsPerStar / 2; }

        robotCleanPerc.text = $"Robot clean % = {(robotTrash / (float)maxPiecesOfTrash) * 100}";

        for (int i = 0; i < starImages.Length; i++) {
            starImages[i].transform.parent.gameObject.SetActive(true);
        }

        for (int i = 0; i < starImages.Length; i++) {
            // Each star fills up with [pointsPerStar] points
            starImages[i].fillAmount = _points / pointsPerStar;

            // Remove the points if the star is full and there are extra points. Otherwise, continue.
            if (_points != pointsPerStar && starImages[i].fillAmount == 1) {
                _points -= pointsPerStar;
            }
            else { 
                break; 
            }
        }
    }

    private IEnumerator CountDown() { 
        while (timeToClean > 0) {
            yield return new WaitForSeconds(1);
            timeToClean--;
        }
    }

    public void RobotTrash() { robotTrash++; }
    public int Points => points;
    public float PlayerCameraXRot => playerCamera.localEulerAngles.x;
}
