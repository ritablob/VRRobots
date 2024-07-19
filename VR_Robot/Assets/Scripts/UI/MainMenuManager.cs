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

        //public GameObject head;
        public Door door;
        
        private void Start()
        {
            options.SetActive(false);
            credits.SetActive(false);
            sign.SetActive(false);
            arrow.SetActive(false);
            //Director.instance.robot.gameObject.SetActive(false);
            mainMenu.SetActive(true);
            door.enabled = false;
        }
        
        public void StartGamePressed()
        {
            sign.SetActive(true);
            mainMenu.SetActive(false);
        }

        public void SignPressed()
        {
            sign.SetActive(false);
            arrow.SetActive(true);
            //Director.instance.robot.gameObject.SetActive(true);
            door.enabled = true;
            door.Open();
        }
        public void OptionsPressed()
        {
            // hide main menu screen
            // unhide options menu
            options.SetActive(true);
            mainMenu.SetActive(false);
        }

        public void CreditsPressed()
        {
            // hide main menu screen
            // unhide credits menu
            credits.SetActive(true);
            mainMenu.SetActive(false);
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
            mainMenu.SetActive(true);
            credits.SetActive(false);
            options.SetActive(false);
        }
    }
}
