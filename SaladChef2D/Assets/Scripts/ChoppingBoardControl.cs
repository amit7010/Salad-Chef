using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SaladChef2D.UI
{
    public class ChoppingBoardControl : MonoBehaviour
    {
        #region Variables
        //Respective Player Data
        public PlayerControl playerData;

        //Time for chopping
        public float choppingTime = 5f;
        public Image choppingLoader;
        public Text choppingText;
        private bool isChopping = false;
        //for calculation purpose
        private float tempTimer = 0f;

        public IDictionary<string, VegDataController> choppedVegetables;
        #endregion

        #region Main Functions 
        private void Awake()
        {
            //initialize Chopped Vegetables
            choppedVegetables = new Dictionary<string, VegDataController>(2);
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            //0. Get Collision Object
            GameObject collPlayer = collision.gameObject;
            PlayerControl collDataPlayer = collPlayer.GetComponent<PlayerControl>();

            //If Player Identified
            if (collDataPlayer.playerData.playerID == playerData.playerData.playerID)
            {
                //If Player has vegetables
                if (collDataPlayer.vegetableList != null)
                {
                    //1. Chop Vegetable
                    isChopping = true;
                    choppingLoader.gameObject.SetActive(true);
                    choppingText.gameObject.SetActive(true);
                    //--------------------------INPROGRESS------------------------------------//
                    foreach(KeyValuePair<string, VegDataController> pair in collDataPlayer.vegetableList)
                    {
                        choppedVegetables.Add(pair);
                        collDataPlayer.vegetableList.Remove(pair.Key);
                        break;
                    }

                    //2.Freeze Player
                    StartCoroutine(EnableChopping(collDataPlayer));
                }
                else
                {
                    //Do Nothing for Now...
                    Debug.Log("Empty Handed");
                }
            }
            else
            {
                StartCoroutine(collDataPlayer.ShowWarning("Wrong Board"));
            }

        }

        private void Update()
        {
            if(isChopping)
            {
                tempTimer += Time.deltaTime;
                Debug.Log(tempTimer);
                choppingLoader.fillAmount = tempTimer / choppingTime;
                choppingText.text = (choppingTime - (tempTimer / choppingTime)).ToString();
                
            }
        }

        #endregion

        #region Helper Methods

        IEnumerator EnableChopping(PlayerControl collDataPlayer)
        {
            collDataPlayer.freezePlayer = true;
            choppingLoader.gameObject.SetActive(true);
            choppingText.gameObject.SetActive(true);
            yield return new WaitForSeconds(choppingTime);
            choppingLoader.gameObject.SetActive(false);
            choppingText.gameObject.SetActive(false);
            isChopping = false;
            collDataPlayer.freezePlayer = false;
            choppingText.text = "";
            tempTimer = 0;
        }

        #endregion
    }

}
