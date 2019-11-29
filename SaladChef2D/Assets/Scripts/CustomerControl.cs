using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SaladChef2D.UI
{
    public class CustomerControl : MonoBehaviour
    {
        //Have List of all vegetables
        public List<VegDataController> VegetableMenu;
        //List of All Players
        public List<PlayerControl> players;
        //Customer selection of salad needed
        private List<VegDataController> neededSalad;
        [SerializeField]
        [Tooltip("Max. Number of Vegs required for Salad")]
        private int maxNumberOfVegs = 3;
        [Tooltip("Plate text for Salad")]
        public TextMesh platetxt;
        public Text loadingTxt;
        public Image loadingImage;
        public Text refreshTxt;
        public Image refreshImage;
        private int giveScore = 20;
        [SerializeField]
        private float totalCustomerWaitingTime = 10f;
        [SerializeField]
        private float refreshTime = 3f;
        private float refreshTimeLeft = 0f;
        private float customerTimeRemaining;
        private CustomerStatus customerStatus = CustomerStatus.WAITING;

        private int reduceScoreValueOnTimeUp = 5;
        //For Timer Invoking Purpose
        bool invokeOp = false;

        private void Awake()
        {
            neededSalad = new List<VegDataController>();
            refreshTimeLeft = refreshTime;
            StartCoroutine(InitCustomer());
        }
        
        private void OnCollisionEnter2D(Collision2D collision)
        {
            //1. Get player
            PlayerControl player = collision.gameObject.GetComponent<PlayerControl>();

            //2. Check Salad
            IList<VegDataController> playersSalad = player.choppedVegetableList;
            bool SaladVerified = CheckSalad(playersSalad);

            if (SaladVerified)
            {
                //Reset Player Salads;
                player.choppedVegetableList = new List<VegDataController>();
                player.AssignPlayerStatus();
                player.ShowVegetableNames();
                player.playerScore += giveScore;
                string scoreMsg = giveScore + " pts";
                StartCoroutine(PopUpNPowerUp.Instance.ShowPopup(true, scoreMsg, player.name));
                //Show PowerUp
                GivePowerUps();
                StartCoroutine(RefreshCustomer());

            }
            else
            {
                Debug.Log("Failed");
                player.playerScore -= 2;
                return;
            }
            //3. Check Time & Assign Status
            //4. Give Score/ Add Score to Player
            //5. Init Customer Again

        }

        private void Update()
        {
            if(customerTimeRemaining >= 0)
            {
                loadingImage.fillAmount = customerTimeRemaining/totalCustomerWaitingTime;
                loadingTxt.text = (customerTimeRemaining).ToString();
            }
            else
            {
                refreshImage.fillAmount = refreshTimeLeft / refreshTime;

            }
            
        }

        /// <summary>
        /// Function to Refresh Customer
        /// </summary>
        /// <returns></returns>
        private IEnumerator RefreshCustomer()
        {
            refreshImage.gameObject.SetActive(true);
            refreshTxt.gameObject.SetActive(true);
            loadingImage.gameObject.SetActive(false);
            loadingTxt.gameObject.SetActive(false);
            yield return new WaitForSeconds(refreshTime);
            loadingImage.gameObject.SetActive(true);
            loadingTxt.gameObject.SetActive(true);
            StartCoroutine(InitCustomer());
            yield return new WaitForSeconds(1f);
            refreshImage.gameObject.SetActive(false);
            refreshTxt.gameObject.SetActive(false);
        }

        /// <summary>
        /// Function to Initialize Customer
        /// </summary>
        /// <returns></returns>
        private IEnumerator InitCustomer()
        {
            //0. Wait for some time before start
            yield return new WaitForSeconds(UnityEngine.Random.Range(0.5f,1f));
            // Reset/Set Customer Time
            customerTimeRemaining = totalCustomerWaitingTime;
            //1. Get All Veggies
            //-- Got from Inspector --
            //Reset neededSalads;
            neededSalad = new List<VegDataController>();
            //2. Pick Vegg Salad required
            //2.1 Finalize Salad needed
            for (int i = 0; i < maxNumberOfVegs; i++)
            {
                int vegNumber = UnityEngine.Random.Range(0, VegetableMenu.Count);
                if(!neededSalad.Contains(VegetableMenu[vegNumber]))
                {
                    //Vegetable to Salad
                    VegetableMenu[vegNumber].Data.isChopped = true;
                    neededSalad.Add(VegetableMenu[vegNumber]);
                }
                else
                {
                    --i;
                }
            }

            //2.2 Show on Text
            platetxt.text = "";
            foreach(VegDataController salad in neededSalad)
            {
                platetxt.text += salad.Data.VegName;
            }

            //2.3 Set Status
            SetCustomerStatus(CustomerStatus.WAITING);

            //3. Start Timer
            if (!invokeOp)
            {
                InvokeRepeating("StartTimer", 1.0f, 1.0f);
                invokeOp = true;
            }
            else
            {
                //Reset Timer
                ResetTimer(totalCustomerWaitingTime);
            }
            //4. Start Wait

        }

        /// <summary>
        /// Function to Start Customer Timer
        /// The function runs every Second
        /// </summary>
        void StartTimer()
        {
            if(customerTimeRemaining > 0)
            {
                customerTimeRemaining -= 1;
            }
            else if(customerTimeRemaining == 0)
            {
                StartCoroutine(RefreshCustomer());
                refreshTimeLeft -= 1;
                customerTimeRemaining -= 1;
                //Function to reduce all players score
                ReduceAllPlayersScore();
            }
            else
            {
                refreshTimeLeft -= 1;
            }
        }

        /// <summary>
        /// Function to reduce all player's Score
        /// </summary>
        private void ReduceAllPlayersScore()
        {
            //Get All Players
                //Got From Inspector
            
            //Reduce All Players Score
            foreach (PlayerControl player in players)
            {
                player.playerScore -= reduceScoreValueOnTimeUp;
            }
            //Show Point reduction PopUp
            string scoreMsg = reduceScoreValueOnTimeUp + " pts";
            StartCoroutine(PopUpNPowerUp.Instance.ShowPopup(false, scoreMsg , "ALL Players"));
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Function to Check Delivered Salads from Chef/Player
        /// </summary>
        /// <param name="playersSalad"></param>
        /// <returns></returns>
        private bool CheckSalad(IList<VegDataController> playersSalad)
        {
            if(playersSalad.Count >= maxNumberOfVegs)
            {
                foreach (VegDataController saladContent in playersSalad)
                {
                    if ((!saladContent.Data.isChopped) || (!neededSalad.Contains(saladContent)))
                    {
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }
            
            return true;
        }

        /// <summary>
        /// Function to Give Player PowerUps based on TimeLine
        /// </summary>
        private void GivePowerUps()
        {
            float percentage = 100 * (customerTimeRemaining / totalCustomerWaitingTime);
            if (percentage >= 70)
            {
                //Spawn PowerUps
                Debug.Log("PowerUp");
                StartCoroutine(PopUpNPowerUp.Instance.ShowPowerUps());
            }
            else
            {

            }
        }

        /// <summary>
        /// Function to RESET Time
        /// </summary>
        /// <param name="requiredTime"></param>
        void ResetTimer(float requiredTime)
        {
            customerTimeRemaining = requiredTime;
        }

        /// <summary>
        /// Function to set Customer Status
        /// </summary>
        /// <param name="status"></param>
        private void SetCustomerStatus(CustomerStatus status)
        {
            customerStatus = status;
        }
    }

}
