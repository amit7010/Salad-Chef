using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SaladChef2D.UI
{
    public class BinControl : MonoBehaviour
    {
        [SerializeField]
        private int reduceScoreValue = 5;
        void OnCollisionEnter2D(Collision2D collision)
        {
            GameObject player = collision.gameObject;
            
            PlayerControl playerData = player.GetComponent<PlayerControl>();
            if(playerData.choppedVegetableList.Count > 0 || playerData.rawVegetableList.Count > 0)
            {
                //1. Clear Vegetables
                playerData.rawVegetableList = new List<VegDataController>();
                playerData.choppedVegetableList = new List<VegDataController>();
                playerData.AssignPlayerStatus();
                playerData.ShowVegetableNames();
                //2. Reduce Score
                playerData.playerScore -= reduceScoreValue;
                string scoreMsg = reduceScoreValue + " pts";
                StartCoroutine(PopUpNPowerUp.Instance.ShowPopup(false, scoreMsg, player.name));
            }
            else
            {
                return;
            }
        }
    }
}
