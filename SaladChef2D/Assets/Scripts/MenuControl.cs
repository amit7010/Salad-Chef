using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SaladChef2D.UI
{
    public class MenuControl : MonoBehaviour
    {
        public static bool isGamePaused = false;

        public GameObject pauseMenuUI;

        void Update()
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                if(isGamePaused)
                {
                    Resume();
                }
                else
                {
                    Pause();
                }
            }
        }

        /// <summary>
        /// Function to resume the paused game
        /// </summary>
        private void Resume()
        {
            pauseMenuUI.SetActive(false);
            Time.timeScale = 1f;
            isGamePaused = false;
        }

        /// <summary>
        /// Function to pause game
        /// </summary>
        private void Pause()
        {
            pauseMenuUI.SetActive(true);
            Time.timeScale = 0f;
            isGamePaused = true;
        }
    }
}
