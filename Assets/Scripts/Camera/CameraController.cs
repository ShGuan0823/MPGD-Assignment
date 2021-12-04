using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{

    GameObject Object;
    MyInputAction inputActions;
    //CinemachineVirtualCamera vCam;

    private bool isHold;

    private Quaternion rotation;

    private void Awake()
    {
        Object = GetComponent<GameObject>();
        //vCam = GetComponent<CinemachineVirtualCamera>();
        inputActions = new MyInputAction();
    }

    private void Start()
    {
        isHold = false;
        //rotation = vCam.transform.rotation;
    }


    private void OnEnable()
    {
        inputActions.Enable();
        inputActions.Camera.MouseDrag.performed += MouseDrag_performed;
        inputActions.Camera.Hold.performed += Hold_performed;
        inputActions.Camera.Release.performed += Release_performed;
    }

    private void OnDisable()
    {
        inputActions.Disable();
        inputActions.Camera.MouseDrag.performed -= MouseDrag_performed;
        inputActions.Camera.Hold.performed -= Hold_performed;
        inputActions.Camera.Release.performed -= Release_performed;
    }

    public void MouseDrag_performed(InputAction.CallbackContext obj)
    {
        onRotate(obj.ReadValue<Vector2>()); ;
    }

    public void Hold_performed(InputAction.CallbackContext obj)
    {
        isHold = true;
    }

    public void Release_performed(InputAction.CallbackContext obj)
    {
        isHold = false;
    }

    private void onRotate(Vector2 direction)
    {
        if (!isHold)
            return;

        // FIXME: limite the angle
        transform.Rotate(-direction.y, direction.x, 0);
    }




}
