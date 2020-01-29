using UnityEngine;

public class LocomotionSMB : StateMachineBehaviour
{
    public float m_Dampling = 0.15f;

    private readonly int m_hashHorizontalPara = Animator.StringToHash("Horizontal");
    private readonly int m_HashVerticalPara = Animator.StringToHash("Vertical");

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector2 input = new Vector2(horizontal, vertical).normalized;

        animator.SetFloat(m_hashHorizontalPara, input.x, m_Dampling, Time.deltaTime);
        animator.SetFloat(m_HashVerticalPara, input.y, m_Dampling, Time.deltaTime);
    }
}
