using TMPro;
using UnityEngine;

public class RobotCanvasManager : MonoBehaviour
{
    public GameObject speechBubble;
    private TextMeshProUGUI text;
    private AudioSource sound;

    void Start()
    {
        text = speechBubble.GetComponentInChildren<TextMeshProUGUI>();
        speechBubble.SetActive(false);
        sound = gameObject.GetComponent<AudioSource>();
    }


    public void ShowSpeechBubbleMessage(string mssg)
    {
        text.text = mssg;
        speechBubble.SetActive(true);
        sound.Play();
    }

    public void HideSpeechBubble()
    {
        speechBubble.SetActive(false);
    }
}