using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SaladChef2D.UI
{
    public class MenuControl : MonoBehaviour
    {
        [SerializeField]
        PlayerControl player1;
        [SerializeField]
        PlayerControl player2;

        public static bool isGamePaused = false;

        public GameObject pauseMenuUI;
        public GameObject GameOverMenuUI;
        public Text Winnertxt;

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
            if(player1.playerTimeLeft == 0 && player2.playerTimeLeft == 0)
            {
                //Stop Game
                Time.timeScale = 0f;
                GameOverMenuUI.SetActive(true);
                if (player1.playerScore > player2.playerScore)
                {
                    Winnertxt.text = player1.name + " WINS!!";
                }
                else if(player1.playerScore < player2.playerScore)
                {
                    Winnertxt.text = player2.name + " WINS!!";
                }
                else
                {
                    Winnertxt.text = "DRAW";
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
