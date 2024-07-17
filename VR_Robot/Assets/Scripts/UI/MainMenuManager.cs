using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class MainMenuManager : MonoBehaviour
    {
        public GameObject mainMenu;
        public GameObject options;
        public GameObject credits;

        private void Start()
        {
            options.SetActive(false);
            credits.SetActive(false);
            mainMenu.SetActive(true);
        }

        public void StartGamePressed()
        {
            // TODO: level loading pre-scene (fade)
            
            // start level
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex+1);
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
#else
            Application.Quit();
#endif
        }

        public void BackToMainMenuPressed()
        {
            credits.SetActive(false);
            options.SetActive(false);
            mainMenu.SetActive(true);
        }
    }
}
