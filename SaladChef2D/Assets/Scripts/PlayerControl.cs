﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SaladChef2D.UI
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerControl : MonoBehaviour
    {
        #region Variables

        [SerializeField]
        KeyCode moveUp;
        [SerializeField]
        KeyCode moveDown;
        [SerializeField]
        KeyCode moveRight;
        [SerializeField]
        KeyCode moveLeft;
        //Controls Speed of Player
        public float speed = 10f;

        //Vector2 movement;
        Rigidbody2D playerBody;

        //Freeze Player to 1 Spot
        bool freezePlayer = false;

        public int carryCapacity = 2;

        private Dictionary<string,VegDataController> vegetableList ;
        public TextMesh VegetablesText;
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
        //void Update()
        //{
        // # Can use GetAxisRaw for Joystick integration and other advantages 
        //movement.x = Input.GetAxisRaw("Horizontal");
        //movement.y = Input.GetAxisRaw("Vertical");

        //Vector2 moveDirection = playerBody.velocity;
        //if (moveDirection != Vector2.zero)
        //{
        //    float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        //    playerBody.rotation = angle;
        //}
        //}

        private void FixedUpdate()
        {
            ControlPlayer();
            //playerBody.MovePosition(playerBody.position + movement * speed * Time.fixedDeltaTime);
            ////  # Here Time.fixedDeltaTime is used so that the speed of our movement doesn't depennd on the number of time Fixed Update function is called

            ////  #Player to look where it's going
            ////float angle = Mathf.Atan2(transform.position.y, transform.position.x) * Mathf.Rad2Deg;
            ////playerBody.rotation = angle;
        }

        void OnCollisionEnter2D(Collision2D collision)
        {

            if (collision.gameObject.tag == "Vegetable")
            {
                if(vegetableList.Count < 2)
                {
                    GameObject vegetable = collision.gameObject;
                    if (vegetableList.ContainsKey(vegetable.name))
                    {
                        Debug.Log("Vegetable Already Caring!");
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
                    Debug.Log("Full");
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
                    playerBody.velocity = new Vector2(transform.position.x, speed * Time.fixedDeltaTime);
                }
                else if (Input.GetKey(moveDown))
                {
                    playerBody.velocity = new Vector2(transform.position.x, speed * -1 * Time.fixedDeltaTime);
                }
                else if (Input.GetKey(moveRight))
                {
                    playerBody.velocity = new Vector2(speed * Time.fixedDeltaTime, transform.position.y );
                }
                else if (Input.GetKey(moveLeft))
                {
                    playerBody.velocity = new Vector2(speed * -1 * Time.fixedDeltaTime, transform.position.y );
                }
                else
                {
                    GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
                }
            }
            
        }

    }
}
    