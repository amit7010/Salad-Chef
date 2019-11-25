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
        public PlayerData playerData;

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

        public IDictionary<string,VegDataController> vegetableList ;
        public TextMesh VegetablesText;

        //Text to Show error or Warning
        public TextMesh ErrorText;
        #endregion

        #region Main Functions

        private void Awake()
        {
            //Get RigidBodyComponent
            playerBody = GetComponent<Rigidbody2D>();
            //Initialize Vegetable Carrying Capacity List
            vegetableList = new Dictionary<string,VegDataController>(carryCapacity);
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
                if (vegetableList.Count < 2)
                {
                    GameObject vegetable = collision.gameObject;
                    if (vegetableList.ContainsKey(vegetable.name))
                    {
                        warningMsg = "Vegetable Already Caring!";
                        Debug.Log(warningMsg);
                        StartCoroutine(ShowWarning(warningMsg ));
                    }
                    else
                    {
                        VegDataController vegController = vegetable.GetComponent<VegDataController>();
                        vegetableList.Add(vegetable.name, vegController);
                        VegetablesText.text += vegController.Data.VegName;
                    }
                    Debug.Log("Vegetable : " + vegetable.name);
                }
                else
                {
                    warningMsg = "Full";
                    Debug.Log(warningMsg);
                    StartCoroutine(ShowWarning(warningMsg));
                }
            }
        }

        #endregion

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
    