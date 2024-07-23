using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Enables and disables rooms to save on resources
/// </summary>
public class LevelManager : MonoBehaviour
{
    public List<OneSidedDoor> Doors;

    public static LevelManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }
}

[System.Serializable]
public class OneSidedDoor
{
    public Door door;
    public GameObject roomToEnter;
    public GameObject roomToExit;

    public void EnterNextRoom()
    {
        if (!door.isOpen)
        {
            roomToEnter.SetActive(true);
            door.Open();
        }
    }

    public void ExitPreviousRoom()
    {
        if (door.isOpen)
        {
            door.Close();
            roomToExit.SetActive(false);
        }
    }
}