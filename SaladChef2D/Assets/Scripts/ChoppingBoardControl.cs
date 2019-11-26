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
        public int maxChoppedVegetables = 3;
        //Time for chopping
        public float choppingTime = 5f;
        public Image choppingLoader;
        public Text choppingTimeText;
        public TextMesh choppingInstruction;
        private bool isChopping = false;
        //for calculation purpose
        private float tempTimer = 0f;

        public IList<VegDataController> choppedVegetables;
        #endregion

        #region Main Functions 
        private void Awake()
        {
            //initialize Chopped Vegetables
            choppedVegetables = new List<VegDataController>(maxChoppedVegetables);
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
                if (collDataPlayer.playerStatus == PlayerStatus.RAWVEG)
                {
                    //1. Chop Vegetable
                    ChopVegetable();
                    //2.Freeze Player
                    StartCoroutine(EnableChopping(playerData));

                }
                else if((playerData.playerStatus == PlayerStatus.CHOPPEDVEG || playerData.playerStatus == PlayerStatus.EMPTY) && choppedVegetables.Count != 0 )
                {
                    TransferSaladToPlayer();
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
            //Chopping Animation
            if(isChopping)
            {
                tempTimer += Time.deltaTime;
                choppingLoader.fillAmount = tempTimer / choppingTime;
                choppingTimeText.text = (choppingTime - (tempTimer / choppingTime)).ToString();
                
            }
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Function to chop vegetables
        /// </summary>
        private void ChopVegetable()
        {
            //1.1 Show Loader
            isChopping = true;
            choppingLoader.gameObject.SetActive(true);
            choppingTimeText.gameObject.SetActive(true);
            //1.2 Remove Veg from player and keep on plate
            VegDataController VegToBeChopped = playerData.rawVegetableList[0];
            playerData.rawVegetableList.Remove(VegToBeChopped);
            playerData.AssignPlayerStatus();
            VegToBeChopped.Data.isChopped = true;
            choppedVegetables.Add(VegToBeChopped);
            //1.3 Show VegetableNames on Player & Chopping Board
            playerData.ShowVegetableNames();
            ShowBoardMessage();
        }

        private void TransferSaladToPlayer()
        {
            playerData.choppedVegetableList = choppedVegetables;
            choppedVegetables = new List<VegDataController>();
            playerData.AssignPlayerStatus();
            playerData.ShowVegetableNames();
            ShowBoardMessage();
        }
        
        /// <summary>
        /// Function to show chopping message
        /// </summary>
        /// <param name="msg"></param>
        private void ShowBoardMessage()
        {
            choppingInstruction.text = "";
            if(choppedVegetables != null)
            {
                foreach (VegDataController veg in choppedVegetables)
                {
                    choppingInstruction.text += veg.Data.VegName;
                }
            }
            else
            {
                choppingInstruction.text = "";
            }
        }

        /// <summary>
        /// Function to show loader in UI while chopping
        /// </summary>
        /// <param name="collDataPlayer"></param>
        /// <returns></returns>
        IEnumerator EnableChopping(PlayerControl collDataPlayer)
        {
            collDataPlayer.freezePlayer = true;
            choppingLoader.gameObject.SetActive(true);
            choppingTimeText.gameObject.SetActive(true);
            yield return new WaitForSeconds(choppingTime);
            choppingLoader.gameObject.SetActive(false);
            choppingTimeText.gameObject.SetActive(false);
            isChopping = false;
            collDataPlayer.freezePlayer = false;
            choppingTimeText.text = "";
            ShowBoardMessage();
            tempTimer = 0;
        }

        #endregion
    }

}
