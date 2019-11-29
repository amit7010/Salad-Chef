using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SaladChef2D.UI
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class PowerUpManager : MonoBehaviour
    {
        [Tooltip("Choose Type Of PowerUp")]
        public PowerUpType powerUpType;

        private int scoreBoostValue = 10;
        private int speedBoostValue = 5;
        private int timeBoostValue = 10;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            GameObject player = collision.gameObject;

            if(player.tag == "Player")
            {
                PlayerControl playerScript = player.GetComponent<PlayerControl>();

                if (powerUpType == PowerUpType.SCOREBOOST)
                {
                    playerScript.playerScore += scoreBoostValue;
                    string messageToShow = scoreBoostValue + " pts";
                    StartCoroutine(PopUpNPowerUp.Instance.ShowPopup(true, messageToShow, player.name));
                }
                else if(powerUpType == PowerUpType.SPEEDBOOST)
                {
                    playerScript.speed += speedBoostValue;
                    string messageToShow = speedBoostValue + " SPEED";
                    StartCoroutine(PopUpNPowerUp.Instance.ShowPopup(true, messageToShow, player.name));
                }
                else if(powerUpType == PowerUpType.TIMEBOOST)
                {
                    playerScript.speed += timeBoostValue;
                    string messageToShow = timeBoostValue + " TIME";
                    StartCoroutine(PopUpNPowerUp.Instance.ShowPopup(true, messageToShow, player.name));
                }
                else
                {
                    return;
                }

            }
        }
    }
}
