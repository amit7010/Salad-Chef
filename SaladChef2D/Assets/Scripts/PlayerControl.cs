using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerControl : MonoBehaviour
{
    [SerializeField]
    KeyCode moveUp;
    [SerializeField]
    KeyCode moveDown;
    [SerializeField]
    KeyCode moveRight;
    [SerializeField]
    KeyCode moveLeft;

    public float speed = 10f;

    Vector2 movement;
    Rigidbody2D playerBody;

    private void Awake()
    {
        playerBody = GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame
    void Update()
    {
        //movement.x = Input.GetAxisRaw("Horizontal");
        //movement.y = Input.GetAxisRaw("Vertical");


        
        ControlPlayer();

        Vector2 moveDirection = playerBody.velocity;
        if (moveDirection != Vector2.zero)
        {
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            playerBody.rotation = angle;
        }
    }

    private void FixedUpdate()
    {
            
        //playerBody.MovePosition(playerBody.position + movement * speed * Time.fixedDeltaTime);
        //  # Here Time.fixedDeltaTime is used so that the speed of our movement doesn't depennd on the number of time Fixed Update function is called

        //  #Player to look where it's going
        //float angle = Mathf.Atan2(transform.position.y, transform.position.x) * Mathf.Rad2Deg;
        //playerBody.rotation = angle;
    }

    private void ControlPlayer()
    {
        if (Input.GetKey(moveUp))
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(transform.position.x, speed);
        }
        else if (Input.GetKey(moveDown))
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(transform.position.x, speed * -1);
        }
        else if (Input.GetKey(moveRight))
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(speed, transform.position.y);
        }
        else if (Input.GetKey(moveLeft))
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(speed * -1, transform.position.y);
        }
        else
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
        }
    }

}
