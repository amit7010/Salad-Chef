using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public class InputProps
    {
        public Vector2 Damping;
        public Vector2 Sensitivity;
    }

    //[SerializeField]
    //float speed = 2.0f;
    [SerializeField]
    Joystick MovePad;

    public Animator playerAnimator;

    public float m_Dampling = 0.15f;

    private readonly int m_hashHorizontalPara = Animator.StringToHash("Horizontal");
    private readonly int m_HashVerticalPara = Animator.StringToHash("Vertical");

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = MovePad.Horizontal + Input.GetAxis("Horizontal");
        float vertical = MovePad.Vertical + Input.GetAxis("Vertical");
        Vector2 input = new Vector2(horizontal, vertical).normalized;

        playerAnimator.SetFloat(m_hashHorizontalPara, input.x, m_Dampling, Time.deltaTime);
        playerAnimator.SetFloat(m_HashVerticalPara, input.y, m_Dampling, Time.deltaTime);
    }
}
