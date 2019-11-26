using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SaladChef2D.UI
{
    public class BinControl : MonoBehaviour
    {
        void OnCollisionEnter2D(Collision2D collision)
        {
            GameObject player = collision.gameObject;
            //1. Clear Vegetables
            PlayerControl playerData = player.GetComponent<PlayerControl>();
            playerData.rawVegetableList = new List<VegDataController>();
            playerData.choppedVegetableList = new List<VegDataController>();
            playerData.AssignPlayerStatus();
            playerData.ShowVegetableNames();
            //2. Reduce Score


        }
    }
}
