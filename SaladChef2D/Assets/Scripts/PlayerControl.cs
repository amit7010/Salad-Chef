using SaladChef2D.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SaladChef2D.UI
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerControl : MonoBehaviour
    {
        #region Variables
        //Serializable Player Data
        public PlayerData playerData;
        //Total Player Store
        private int playerScore = 0;

        [SerializeField]
        KeyCode moveUp;
        [SerializeField]
        KeyCode moveDown;
        [SerializeField]
        KeyCode moveRight;
        [SerializeField]
        KeyCode moveLeft;
        //Controls Speed of Player
        public float speed = 5f;

        //Vector to Move Players based on KeyDown
        Vector2 movement;
        Rigidbody2D playerBody;

        //Freeze Player to 1 Spot
        public bool freezePlayer = false;

        public int carryCapacity = 2;

        public IList<VegDataController> rawVegetableList;
        public IList<VegDataController> choppedVegetableList;
        public TextMesh VegetablesText;

        //Text to Show error or Warning
        public TextMesh ErrorText;
        
        public PlayerStatus playerStatus = PlayerStatus.EMPTY;
        #endregion

        #region Main Functions

        private void Awake()
        {
            //Get RigidBodyComponent
            playerBody = GetComponent<Rigidbody2D>();
            //Initialize Vegetable Carrying Capacity List
            rawVegetableList = new List<VegDataController>(carryCapacity);
            choppedVegetableList = new List<VegDataController>(carryCapacity+1);
            playerScore = 0;


        }

        // Update is called once per frame
        void Update()
        {
            //# Can use GetAxisRaw for Joystick integration and other advantages 
            //movement.x = Input.GetAxisRaw("Horizontal");
            //movement.y = Input.GetAxisRaw("Vertical");
            ControlPlayer();
            
        }

        private void FixedUpdate()
        {
            //ControlPlayer();
            playerBody.MovePosition(playerBody.position + movement * speed * Time.fixedDeltaTime);
            //  # Here Time.fixedDeltaTime is used so that the speed of our movement doesn't depennd on the number of time Fixed Update function is called

            ////  #Player to look where it's going
            ////float angle = Mathf.Atan2(transform.position.y, transform.position.x) * Mathf.Rad2Deg;
            ////playerBody.rotation = angle;
            Vector2 moveDirection = playerBody.velocity;
            if (moveDirection != Vector2.zero)
            {
                float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
                playerBody.rotation = angle;
            }
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            //If Vegetable
            if (collision.gameObject.tag == "Vegetable")
            {
                string warningMsg = string.Empty;

                if (playerStatus == PlayerStatus.CHOPPEDVEG)
                {
                    warningMsg = "Have Chopped Veggies";
                    Debug.Log(warningMsg);
                    StartCoroutine(ShowWarning(warningMsg));
                    return;
                }

                //If Vegetables are less then CarryCapacity
                if (rawVegetableList.Count < carryCapacity)
                {
                    VegDataController vegetable = collision.gameObject.GetComponent<VegDataController>();
                    if (!PlayerHasVegetable(vegetable))
                    {
                        //Adding Vegetable to Player
                        AddVegetableToBag(vegetable,PlayerStatus.RAWVEG);
                    }
                    else
                    {
                        warningMsg = "Vegetable Already Carrying!";
                        Debug.Log(warningMsg);
                        StartCoroutine(ShowWarning(warningMsg));
                    }
                    Debug.Log("Vegetable : " + vegetable.name);
                }
                else
                {
                    warningMsg = "Bag Full";
                    Debug.Log(warningMsg);
                    StartCoroutine(ShowWarning(warningMsg));
                }
            }
        }

        /// <summary>
        /// Function to Add Vegetable to Player's Bag
        /// </summary>
        /// <param name="vegetable"></param>
        public bool AddVegetableToBag(VegDataController vegetableData,PlayerStatus status)
        {
            bool isAdded = false;
            
            if (status == PlayerStatus.RAWVEG || status == PlayerStatus.EMPTY)
            {
                if ( rawVegetableList.Count < 2)
                {
                    rawVegetableList.Add(vegetableData);
                    isAdded = true;
                }
                else
                {
                    isAdded = false;
                }
                
            }
            else 
            {
                if (choppedVegetableList.Count < 3)
                {
                    vegetableData.Data.isChopped = true;
                    choppedVegetableList.Add(vegetableData);
                    isAdded = true;
                }
                else
                {
                    isAdded = false;
                }
                    
            }
            AssignPlayerStatus();
            ShowVegetableNames();
            return isAdded;
        }

        /// <summary>
        /// Function to Remove Vegetable from Player's Bag
        /// </summary>
        public VegDataController RemoveRawVegetableFromBag()
        {
            VegDataController vegetableToRemove = rawVegetableList[0];
            rawVegetableList.RemoveAt(0);
            AssignPlayerStatus();
            ShowVegetableNames();
            return vegetableToRemove;
        }
        /// <summary>
        /// Function to Remove Vegetable from Player's Bag
        /// </summary>
        /// <param name="vegetable"></param>
        public void RemoveChoppedVegetableFromBag(VegDataController vegetable)
        {
            VegDataController vegetableData = vegetable.GetComponent<VegDataController>();
            choppedVegetableList.Remove(vegetableData);
            AssignPlayerStatus();
            ShowVegetableNames();
        }

        #endregion
        /// <summary>
        /// Function to Assign Status(On the basis of vegetable it is carrying) to Player
        /// </summary>
        public void AssignPlayerStatus()
        {
            if(rawVegetableList.Count == 0 && choppedVegetableList.Count == 0)
            {
                playerStatus = PlayerStatus.EMPTY;
            }
            else if(rawVegetableList.Count > 0)
            {
                playerStatus = PlayerStatus.RAWVEG;
            }
            else if(choppedVegetableList.Count > 0)
            {
                playerStatus = PlayerStatus.CHOPPEDVEG;
            }
            else
            {
                Debug.LogError("Player Status Error");
            }
        }

        /// <summary>
        /// Function to check if player has vegetables
        /// </summary>
        /// <param name="vegetable"></param>
        /// <returns></returns>
        private bool PlayerHasVegetable(VegDataController vegetable)
        {
            foreach (VegDataController veg in rawVegetableList)
            {
                if (vegetable.Data.VegId == veg.Data.VegId)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Function to show vegetable Names
        /// </summary>
        /// <param name="vegetableList"></param>
        public void ShowVegetableNames()
        {
            VegetablesText.text = "";
            if (playerStatus == PlayerStatus.RAWVEG)
            {
                foreach (VegDataController veg in rawVegetableList)
                {
                    VegetablesText.color = Color.yellow;
                    VegetablesText.text += veg.Data.VegName;
                }
            }
            else
            {
                foreach (VegDataController veg in choppedVegetableList)
                {
                    VegetablesText.color = new Color(100, 255, 0, 255);
                    VegetablesText.text += veg.Data.VegName;
                }
            }
            
        }

        /// <summary>
        /// Function to Control Player with keyboard keys
        /// </summary>
        private void ControlPlayer()
        {
            if(!freezePlayer)
            {
                if (Input.GetKey(moveUp))
                {
                    //playerBody.velocity = new Vector2(transform.position.x, speed );
                    movement.x = 0;
                    movement.y = 1;
                }
                else if (Input.GetKey(moveDown))
                {
                    //playerBody.velocity = new Vector2(transform.position.x, speed * -1 );
                    movement.x = 0;
                    movement.y = -1;
                }
                else if (Input.GetKey(moveRight))
                {
                    //playerBody.velocity = new Vector2(speed , transform.position.y );
                    movement.y = 0;
                    movement.x = 1;
                }
                else if (Input.GetKey(moveLeft))
                {
                    //playerBody.velocity = new Vector2(speed * -1 , transform.position.y );
                    movement.y = 0;
                    movement.x = -1;
                }
                else
                {
                    //GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
                    movement.x = 0;
                    movement.y = 0;
                }
            }
            
        }
        
        public IEnumerator ShowWarning(string msg)
        {
            ErrorText.text = msg;
            //Red Color
            ErrorText.color = new Color(255,0,0,255);
            ErrorText.gameObject.SetActive(true);
            yield return new WaitForSeconds(1f);
            ErrorText.gameObject.SetActive(false);
            yield return null;
        }
    }
}
    