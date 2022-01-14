using System;
using UnityEngine;
using UnityEngine.ProBuilder;
using Math = System.Math;

public class PlayerController : MonoBehaviour
{
    // FIXME: change data sources
    public float moveSpeed;
    public float turnSpeed;
    public float runSpeed;

    Rigidbody rigid;
    MyInputAction inputActions;
    Animator anim;
    Camera cam;
    CharacterStatus characterStats;

    private Vector2 move;
    private Vector3 moveForward;

    private float turnAmount;
    private float currentSpeed;
    private float forwardAmount;

    private float lastAttackTime;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        inputActions = new MyInputAction();
        characterStats = GetComponent<CharacterStatus>();

        cam = Camera.main;
    }

    private void Start()
    {
        currentSpeed = 0;
        characterStats.PlayerStats = CharacterStats.Idle;
        GameManager.Instance.RegisterPlayer(characterStats);
    }

    private void Update()
    {
        onMove(move);
        //Attack();
        SetAnimator();
        lastAttackTime -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        rigid.MovePosition(rigid.position + moveForward * Time.deltaTime * currentSpeed);
        //rigid.velocity = forwardAmount * moveForward * currentSpeed;
        //rigid.MoveRotation(rigid.rotation * Quaternion.Euler(0, turnAmount * turnSpeed, 0));

    }

    private void OnEnable()
    {
        inputActions.Enable();
        inputActions.Player.Movement.performed += Movement_performed;
        inputActions.Player.Run.performed += Run_performed;
        inputActions.Player.Walk.performed += Walk_performed;
        inputActions.Player.Combat.performed += Combat_performed;
    }

    private void OnDisable()
    {
        inputActions.Disable();
        inputActions.Player.Movement.performed -= Movement_performed;
        inputActions.Player.Run.performed -= Run_performed;
        inputActions.Player.Walk.performed -= Walk_performed;
        inputActions.Player.Combat.performed += Combat_performed;
    }

    public void Movement_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        move = obj.ReadValue<Vector2>();
        //if (Walkable())
        characterStats.PlayerStats = CharacterStats.Walk;
    }

    public void Run_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        currentSpeed = runSpeed;
        //if (Walkable())
        characterStats.PlayerStats = CharacterStats.Run;
    }

    public void Walk_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        currentSpeed = moveSpeed;
        //if (Walkable())
        characterStats.PlayerStats = CharacterStats.Walk;
    }

    public void Combat_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        characterStats.PlayerStats = CharacterStats.Attack;
        Attack();
    }

    private void Attack()
    {
        //if (characterStats.PlayerStats != CharacterStats.Attack)
        //    return;
        if (lastAttackTime <= 0)
        {
            lastAttackTime = characterStats.attackData.coolDown;
            anim.SetTrigger("Attack");
            SetAnimator();
        }
        characterStats.PlayerStats = CharacterStats.Idle;
    }

    private void onMove(Vector2 direction)
    {
        if (characterStats.PlayerStats == CharacterStats.Walk)
        {
            currentSpeed = moveSpeed;
        }
        if (characterStats.PlayerStats == CharacterStats.Run)
        {
            currentSpeed = runSpeed;
        }
        Vector3 worldMove = new Vector3(direction.x, 0, direction.y);
        Vector3 localMove = transform.InverseTransformVector(worldMove);
        forwardAmount = (float)Math.Sqrt(localMove.x * localMove.x + localMove.z * localMove.z);
        turnAmount = Mathf.Atan2(localMove.x, localMove.z);
        Vector3 camProjection_z = Vector3.ProjectOnPlane(cam.transform.forward, Vector3.up);
        float turnAngle = Vector3.Angle(new Vector3(0, 0, 1), camProjection_z);
        if (camProjection_z.x < 0)
        {
            turnAngle = -turnAngle;
        }
        moveForward = RotateDirection(worldMove, turnAngle, Vector3.up);
        Vector3 targetDir = Vector3.Slerp(transform.forward, moveForward, Math.Abs(turnAmount * turnSpeed));
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetDir), 10 * Time.deltaTime);
    }

    // TODO: set animator
    private void SetAnimator()
    {
        //Debug.Log("stats:" + characterStats.PlayerStats);
        switch (characterStats.PlayerStats)
        {

            case CharacterStats.Walk:
                anim.SetFloat("Forward", forwardAmount);
                break;
            case CharacterStats.Run:
                anim.SetFloat("Forward", forwardAmount * 2);
                break;
            case CharacterStats.Idle:
                currentSpeed = 0;
                break;
            case CharacterStats.Attack:
                //anim.SetTrigger("Attack");
                break;
            case CharacterStats.Dead:
                break;
        }

    }


    private Vector3 RotateDirection(Vector3 forward, float angle, Vector3 axis)
    {
        return Quaternion.AngleAxis(angle, axis) * forward;
    }

    public bool Walkable()
    {
        //Debug.Log("stats:" + characterStats.PlayerStats);
        if (characterStats.PlayerStats == CharacterStats.Walk || characterStats.PlayerStats == CharacterStats.Idle
            || characterStats.PlayerStats == CharacterStats.Run)
            return true;
        return false;
    }
}
