using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Checkbox : Task
{
    [SerializeField] TextMeshProUGUI objectiveDesc;

    public void Initialize(string name) {
        gameObject.name = name;

        switch (name) {
            case "organizeToys":
                objectiveDesc.text = "Move the toys to the storage box.";
                break;
            default: break;
        }
    }

    protected override void CompleteTask(string name) {
        if (name == gameObject.name) { Destroy(gameObject); }
    }
}
