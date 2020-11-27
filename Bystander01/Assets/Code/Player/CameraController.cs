using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Transforms")]
    [SerializeField] Transform playerCam = null;


    [Header("Vectors")]
    Vector2 currentMouseDelta = Vector2.zero;
    Vector2 currentMouseDeltaVelocity = Vector2.zero;

    [Header("Floats")]
    [SerializeField] public float mouseSensitivity = 3.5f;
    [SerializeField] [Range(0.0f, 0.5f)] float mouseSmoothTime = 0.3f;


    //private float cameraPitch = 0.0f;
    public float cameraYaw = 0.0f;

    
    private void Start()
    {
       
    }

    private void Update()
    {
        MouseInput();
    }


    //Vertical mouse movement will be the pitch on the camera using the X axis

    //Currently moving its horizontal movement using the parent vector however looking at options to confine this to camrea,
    //so movement wont interfere and create any unusual angles.
    private void MouseInput()
    {
        Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseDeltaVelocity, mouseSmoothTime);

        //transform.Rotate(Vector3.up * currentMouseDelta.x * mouseSensitivity);


        //Inverted Y axis due to the way camera pitches downwards when positive by default.
        //clamping the pitch of the camera to create a realistic feeling of the players neck and to prevent any upside-down camera position

        cameraYaw -= currentMouseDelta.x * mouseSensitivity;

        cameraYaw = Mathf.Clamp(cameraYaw, -80.0f, 80.0f);
        playerCam.localEulerAngles = -Vector3.up * cameraYaw;

    }

}
