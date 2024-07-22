using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    /// <summary>
    /// All things pause menu - gaze-determined rotation, head position, 
    /// </summary>
    public class PauseMenuController : MonoBehaviour
    {
        /* - follows gaze position
         * - rotation updates once you are no longer facing it for a certain amount of time
         *
         */
        public GameObject pauseMenu;
        public GameObject optionsMenu;
        public float zOffset;

        private bool facingMenu;
        private Vector3 newRotation;

        private void Start()
        {
            pauseMenu.SetActive(false);
            optionsMenu.SetActive(false);
            zOffset = pauseMenu.transform.position.z;
        }

        private void Update()
        {
            CheckForHeadRotation();
        }

        private void CheckForHeadRotation()
        {
            if (!facingMenu)
            {
                UpdateRotation();
            }
        }

        private void UpdateRotation()
        {
            // get new rotation of the head
            facingMenu = true;
            // call IEnumerator to lerp from last position to the new one 
            // rotate according to the player's head rotation
        }

        #region Button Callbacks

        public void ResumePressed()
        {
            // close pause menu
            pauseMenu.SetActive(false);
        }

        public void MainMenuPressed()
        {
            // switch to main menu scene
            SceneManager.LoadSceneAsync(0);
        }

        public void OptionsPressed()
        {
            // disable pause menu
            // enable options menu
            pauseMenu.SetActive(false);
            optionsMenu.SetActive(true);
        }

        public void QuitPressed()
        {
            // close application
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#else
            Application.Quit();
#endif
        }

        #endregion
    }
}