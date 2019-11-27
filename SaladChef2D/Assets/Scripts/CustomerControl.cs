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
        //Customer selection of salad needed
        private List<VegDataController> neededSalad;
        [SerializeField]
        [Tooltip("Max. Number of Vegs required for Salad")]
        private int maxNumberOfVegs = 3;
        [Tooltip("Plate text for Salad")]
        public TextMesh platetxt;
        public Text loadingTxt;
        public Image loadingImage;
        private int giveScore = 0;
        [SerializeField]
        private float totalCustomerWaitingTime = 10f;
        private float customerTimeRemaining = 10f;
        private CustomerStatus customerStatus = CustomerStatus.WAITING;
        //For Timer Invoking Purpose
        bool invokeOp = false;

        private void Awake()
        {
            neededSalad = new List<VegDataController>();
            totalCustomerWaitingTime = customerTimeRemaining;
            StartCoroutine(InitCustomer());
        }
        
        private void OnCollisionEnter2D(Collision2D collision)
        {
            //1. Get player
            GameObject player = collision.gameObject;
            //2. Check Salad
            IList<VegDataController> playersSalad = player.GetComponent<PlayerControl>().choppedVegetableList;
            bool SaladVerified = CheckSalad(playersSalad);
            if(SaladVerified)
            {
                //Reset Player Salads;
                playersSalad = new List<VegDataController>();
                //Reset neededSalads;
                neededSalad = new List<VegDataController>();
                //Init Customer Again.
                StartCoroutine(InitCustomer());
                Debug.Log("Scored");
            }
            else
            {
                Debug.Log("Failed");
            }
            //3. Check Time & Assign Status
            //4. Give Score/ Add Score to Player
            //5. Init Customer Again
        }

        private void Update()
        {
            if(customerTimeRemaining > 0)
            {
                loadingImage.fillAmount = customerTimeRemaining/totalCustomerWaitingTime;
                loadingTxt.text = (customerTimeRemaining).ToString();
            }
            else
            {
                ResetTimer(totalCustomerWaitingTime);
            }
            
        }

        /// <summary>
        /// Function to Initialize Customer
        /// </summary>
        /// <returns></returns>
        private IEnumerator InitCustomer()
        {
            //0. Wait for some time before start
            yield return new WaitForSeconds(UnityEngine.Random.Range(0.5f,4f));

            //1. Get All Veggies
            //-- Got from Inspector --

            //2. Pick Vegg Salad required
            //2.1 Finalize Salad needed
            for(int i = 0; i < maxNumberOfVegs; i++)
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
        /// </summary>
        void StartTimer()
        {
            if(customerTimeRemaining > 0)
            {
                customerTimeRemaining -= 1;
            }
            //GiveCustomerStatus();
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
        /// Function to Give Customer Status Based on Time
        /// </summary>
        private void GiveCustomerStatus()
        {
            float percentage = 100 * (customerTimeRemaining / totalCustomerWaitingTime);
            if (percentage >= 70)
            {
                //Spawn PowerUps
                Debug.Log("PowerUp");
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

        private void SetCustomerStatus(CustomerStatus status)
        {
            customerStatus = status;
        }
    }

}
