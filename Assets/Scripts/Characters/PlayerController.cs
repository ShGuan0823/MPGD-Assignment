using System;
using UnityEngine;
using UnityEngine.ProBuilder;

public class PlayerController : MonoBehaviour
{
    // FIXME: change data sources
    public float moveSpeed;
    public float rotateSpeed;
    public float runSpeed;

    Rigidbody rigid;
    MyInputAction inputActions;
    Animator anim;
    Camera cam;

    private Vector2 move;
    private Vector3 moveForward;

    private float forwardAmount;
    private float rotateAmount;
    private float currentSpeed;


    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        inputActions = new MyInputAction();

        cam = Camera.main;
    }

    private void Start()
    {
        currentSpeed = moveSpeed;
    }

    private void Update()
    {
        onMove(move);
    }

    private void FixedUpdate()
    {
        rigid.MovePosition(rigid.position + moveForward * Time.deltaTime * currentSpeed);
    }

    private void OnEnable()
    {
        inputActions.Enable();
        inputActions.Player.Movement.performed += Movement_performed;
        inputActions.Player.Run.performed += Run_performed;
        inputActions.Player.Walk.performed += Walk_performed;
    }

    private void OnDisable()
    {
        inputActions.Disable();
        inputActions.Player.Movement.performed -= Movement_performed;
        inputActions.Player.Run.performed -= Run_performed;
        inputActions.Player.Walk.performed -= Walk_performed;
    }

    public void Movement_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        move = obj.ReadValue<Vector2>();
    }

    public void Run_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        currentSpeed = runSpeed; ;
    }

    public void Walk_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        currentSpeed = moveSpeed;
    }


    private void onMove(Vector2 direction)
    {
        Vector3 wordlMove = new Vector3(direction.x, 0, direction.y);
        Vector3 localMove = transform.InverseTransformVector(wordlMove);
        rotateAmount = Mathf.Atan2(localMove.x, localMove.z);
        Vector3 camProjection_z = Vector3.ProjectOnPlane(cam.transform.forward, Vector3.up);
        float turnAngle = Vector3.Angle(new Vector3(0, 0, 1), camProjection_z);
        moveForward = RotateDirection(wordlMove, turnAngle, Vector3.up);
        Vector3 targetDir = Vector3.Slerp(transform.forward, moveForward, System.Math.Abs(rotateAmount * rotateSpeed));
        rigid.rotation = Quaternion.LookRotation(targetDir, Vector3.up);
    }

    // TODO: set animator
    private void SetAnimator()
    {

    }


    private Vector3 RotateDirection(Vector3 forward, float angle, Vector3 axis)
    {
        return Quaternion.AngleAxis(angle, axis) * forward;
    }

}
