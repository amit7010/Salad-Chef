using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera2 : MonoBehaviour
{
    [SerializeField]
    Vector3 cameraOffset;
    [SerializeField]
    float Damping;

    Transform cameraLookTarget;

    [SerializeField]GameObject localPlayer;

    protected Vector3 _LocalRotation;
    protected float _CameraDistance = 10f;

    public float MouseSensitivity = 4f;

    [SerializeField]
    Joystick CamStick;

    // Start is called before the first frame update
    void Start()
    {
        HandleOnLocalPlayerJoined();
    }

    private void HandleOnLocalPlayerJoined()
    {
        cameraLookTarget = localPlayer.transform.Find("cameraLookTarget");

        if (cameraLookTarget == null)
        {
            cameraLookTarget = localPlayer.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        _LocalRotation.x = /*(Input.GetAxis("Mouse X") * MouseSensitivity) +*/ (CamStick.Horizontal * MouseSensitivity);
        _LocalRotation.y = /*(Input.GetAxis("Mouse Y") * MouseSensitivity) +*/ (CamStick.Vertical * MouseSensitivity);

        if((_LocalRotation.x <0.1f && _LocalRotation.x >- 0.1f) || (_LocalRotation.y < 0.1f && _LocalRotation.y > -0.1f))
        {
            //_LocalRotation.y = Mathf.Clamp(_LocalRotation.y, 0f, 90f);
            Quaternion camTurnAngle = Quaternion.AngleAxis(_LocalRotation.x, Vector3.up);
            Quaternion camTurnAngle2 = Quaternion.AngleAxis(_LocalRotation.y, Vector3.left);
            cameraOffset = camTurnAngle * camTurnAngle2 * cameraOffset;
        }

        Vector3 targetPosition = cameraLookTarget.position + localPlayer.transform.forward * cameraOffset.z +
            localPlayer.transform.up * cameraOffset.y + localPlayer.transform.right * cameraOffset.x;
        
        Quaternion targetRotation = Quaternion.LookRotation(cameraLookTarget.position - targetPosition, Vector3.up);

        transform.position = Vector3.Slerp(transform.position, targetPosition, Damping * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Damping * Time.deltaTime);
    }
}
