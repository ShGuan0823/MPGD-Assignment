using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{

    GameObject Object;
    MyInputAction inputActions;

    private bool isHold;

    private void Awake()
    {
        Object = GetComponent<GameObject>();
        inputActions = new MyInputAction();
    }

    private void Start()
    {
        isHold = false;
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

    private void Hold_performed(InputAction.CallbackContext obj)
    {
        isHold = true;
    }

    private void Release_performed(InputAction.CallbackContext obj)
    {
        isHold = false;
    }

    private void onRotate(Vector2 direction)
    {
        if (!isHold)
            return;
        transform.Rotate(-direction.y, direction.x, 0);
    }






}
