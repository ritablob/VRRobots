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
        public GameObject head;
        
        private void Start()
        {
            options.SetActive(false);
            credits.SetActive(false);
            //mainMenu.SetActive(true);
            Invoke(nameof(PositionANdRevealMainMenu), 1f);
        }

        private void PositionANdRevealMainMenu()
        {
            // get camera height / 2
            // get head rotation and make the canvas face it
            //Vector3 playerPosition = headPositionInpu
            Vector3 menuPosition = new Vector3(head.transform.position.x, head.transform.position.y,
                head.transform.position.z + 0.5f);
            canvas.transform.position = menuPosition;
            canvas.transform.eulerAngles = new Vector3(0, head.transform.eulerAngles.y,
                0);
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
