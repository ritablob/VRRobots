using TMPro;
using UnityEngine;

public class RobotCanvasManager : MonoBehaviour
{
    public GameObject speechBubble;
    private TextMeshProUGUI text;

    void Start()
    {
        text = speechBubble.GetComponentInChildren<TextMeshProUGUI>();
        speechBubble.SetActive(false);
    }


    public void ShowSpeechBubbleMessage(string mssg)
    {
        text.text = mssg;
        speechBubble.SetActive(true);
    }

    public void HideSpeechBubble()
    {
        speechBubble.SetActive(false);
    }
}