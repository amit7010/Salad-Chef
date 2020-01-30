using UnityEngine;

//This is a camera script made by Haravin (Daniel Valcour).
//This script is public domain, but credit is appreciated!

[RequireComponent(typeof(Camera))]
public class ThirdPersonCam : MonoBehaviour
{
    protected Transform _XForm_Camera;
    protected Transform _XForm_Parent;

    protected Vector3 _LocalRotation;
    protected float _CameraDistance = 10f;

    public float MouseSensitivity = 4f;
    public float ScrollSensitivity = 2f;
    public float OrbitDamping = 10f;
    public float ScrollDamping = 6f;

    public Transform PlayerTransform;
    public bool LookAtPlayer = true;
    [Range(0.01f, 1.0f)]
    public float SmoothFactor = 0.5f;
    private Vector3 _cameraOffset;

    public bool RotateAroundPlayer = true;
    public float RotationsSpeed = 4f;

    public bool CameraDisabled = false;

    private void Start()
    {
        _XForm_Camera = transform;
        _XForm_Parent = transform.parent;

        _cameraOffset = transform.position - PlayerTransform.position; ;
    }

    private void LateUpdate()
    {
        if(LookAtPlayer)
        {
            transform.LookAt(PlayerTransform);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
            CameraDisabled = !CameraDisabled;

        if(!CameraDisabled)
        {
            //Rotation of the Camera based on Mouse Co-ordination
            //if(Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y")!=0)
            //{
            _LocalRotation.x = Input.GetAxis("Mouse X") * MouseSensitivity;
            _LocalRotation.y = Input.GetAxis("Mouse Y") * MouseSensitivity;

            //Clamp the y Rotation to horizon and not flipping over at the top
            //_LocalRotation.y = Mathf.Clamp(_LocalRotation.y, 0f, 90f);
            //}
            if (RotateAroundPlayer)
            {
                Quaternion camTurnAngle = Quaternion.AngleAxis(_LocalRotation.x, Vector3.up);
                Quaternion camTurnAngle2 = Quaternion.AngleAxis(_LocalRotation.y, Vector3.left);
                _cameraOffset = camTurnAngle *camTurnAngle2* _cameraOffset;

                //Player Movement
                PlayerTransform.Rotate(new Vector3(0f, _LocalRotation.x, 0f));
            }

            //Zooming Input from our Mouse Scroll Weel
            //if (Input.GetAxis("Mouse ScrollWheel") != 0f)
            //{
            //    float ScrollAmount = Input.GetAxis("Mouse ScrollWheel") * ScrollSensitivity;

            //    //Makes camera zoom faster the further away it is from the target 
            //    ScrollAmount *= (_CameraDistance * 0.3f);
            //    _CameraDistance += ScrollAmount * -1f;
            //    _CameraDistance = Mathf.Clamp(_CameraDistance, 1.5f, 100f);
            //}
        }

        //Actual Camera Rig Transformation
        //Quaternion QT = Quaternion.Euler(_LocalRotation.y, _LocalRotation.x, 0);
        //_XForm_Parent.rotation = Quaternion.Lerp(_XForm_Parent.rotation, QT, Time.deltaTime * OrbitDamping);

        //if(_XForm_Camera.localPosition.z != _CameraDistance * -1f)
        //{
            //_XForm_Camera.localPosition = new Vector3(0f, 0f, Mathf.Lerp(_XForm_Camera.localPosition.z, _CameraDistance * -1f, Time.deltaTime * ScrollDamping));
        //}
        Vector3 newPos = PlayerTransform.position + _cameraOffset;
        transform.position = Vector3.Slerp(transform.position, newPos, SmoothFactor);
        if (LookAtPlayer)
        {
            transform.LookAt(PlayerTransform);
        }

    }
    //public Transform PlayerTransform;
    //private Vector3 _cameraOffset;

    //[Range(0.01f, 1.0f)]
    //public float SmoothFactor = 0.5f;

    //public bool RotateAroundPlayer = true;
    //public float RotationsSpeed = 5.0f;
    //public bool LookAtPlayer = false;

    //private void Start()
    //{
    //    _cameraOffset = transform.position - PlayerTransform.position;
    //}

    //private void LateUpdate()
    //{
    //    if(RotateAroundPlayer)
    //    {
    //        Quaternion camTurnAngle = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * RotationsSpeed, Vector3.up);
    //        _cameraOffset = camTurnAngle * _cameraOffset;
    //    }
    //    Vector3 newPos = PlayerTransform.position + _cameraOffset;
    //    transform.position = Vector3.Slerp(transform.position, newPos, SmoothFactor);

    //    if(LookAtPlayer || RotateAroundPlayer)
    //    {
    //        transform.LookAt(PlayerTransform);
    //    }

    //}
}
