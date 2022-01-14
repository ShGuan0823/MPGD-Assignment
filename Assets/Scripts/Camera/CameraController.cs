using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{

    MyInputAction inputActions;
    CinemachineVirtualCamera virtualCam;

    private bool isHold;


    private void Awake()
    {
        virtualCam = GetComponent<CinemachineVirtualCamera>();
        inputActions = new MyInputAction();
    }

    private void Start()
    {
        isHold = false;
    }

    private void Update()
    {
        //Debug.Log("vCam: " + virtualCam.transform.rotation);
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
        //Debug.Log(direction);
        virtualCam.transform.eulerAngles += new Vector3(-direction.y, direction.x, 0);
    }




}
