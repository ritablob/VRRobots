using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace UI
{
    public class MainMenuManager : MonoBehaviour
    {
        public Canvas canvas;
        public GameObject mainMenu;
        public GameObject options;
        public GameObject credits;
        public GameObject sign;
        public GameObject arrow;
        public Door door;

        private AudioSource source;

        private void Start()
        {
            options.SetActive(false);
            credits.SetActive(false);
            sign.SetActive(false);
            arrow.SetActive(false);
            //Director.instance.robot.gameObject.SetActive(false);
            mainMenu.SetActive(true);
            door.enabled = false;
            source = GetComponent<AudioSource>();
        }

        public void PlayClick()
        {
            source.Play();
        }

        public void StartGamePressed()
        {
            sign.SetActive(true);
            mainMenu.SetActive(false);
        }

        public void ResetMainMenu()
        {
            options.SetActive(false);
            credits.SetActive(false);
            sign.SetActive(false);
            arrow.SetActive(false);
            //Director.instance.robot.gameObject.SetActive(false);
            mainMenu.SetActive(true);
        }

        public void SignPressed()
        {
            sign.SetActive(false);
            arrow.SetActive(true);
            Director.instance.robot.gameObject.SetActive(true);
            VFXApplicationHelper.instance.RobotCanvasManager.ShowSpeechBubbleMessage("Hi! Let's clean together!");
            door.enabled = true;
            door.Open();
        }

        public void OptionsPressed()
        {
            // hide main menu screen
            // unhide options menu

            mainMenu.SetActive(false);
            options.SetActive(true);
        }

        public void CreditsPressed()
        {
            // hide main menu screen
            // unhide credits menu

            mainMenu.SetActive(false);
            credits.SetActive(true);
        }

        public void QuitGamePressed()
        {
            // close game 
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#endif
            Application.Quit();
        }

        public void BackToMainMenuPressed()
        {
            credits.SetActive(false);
            options.SetActive(false);
            mainMenu.SetActive(true);
        }
    }
}