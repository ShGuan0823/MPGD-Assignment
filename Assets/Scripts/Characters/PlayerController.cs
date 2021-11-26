using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{
    // FIXME: change data sources
    public float moveSpeed;
    public float rotateSpeed;
    public float runSpeed;

    Rigidbody rigid;
    MyInputAction inputActions;
    Animator anim;

    private Vector2 move;
    private float forwardAmount;
    private float rotateAmount;
    private float currentSpeed;
    private bool isRun;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        inputActions = new MyInputAction();
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
        rigid.velocity = forwardAmount * transform.forward * currentSpeed;
        rigid.MoveRotation(rigid.rotation * Quaternion.Euler(0, rotateAmount * rotateSpeed, 0));
    }

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
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

        forwardAmount = localMove.z;
        rotateAmount = Mathf.Atan2(localMove.x, localMove.z);
        Debug.Log(transform.forward);
        Debug.Log(transform.InverseTransformVector(transform.forward));
    }

    // TODO: set animator
    private void SetAnimator()
    {

    }

}
