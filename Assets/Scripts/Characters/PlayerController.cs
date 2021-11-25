using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{
    //private NavMeshAgent agent;
    //private Animator anim;
    //private CharacterStatus characterStatus;
    //private GameObject attackTarget;
    //private float lastAttackTime;

    //private void Awake()
    //{
    //    agent = GetComponent<NavMeshAgent>();
    //    anim = GetComponent<Animator>();
    //    characterStatus = GetComponent<CharacterStatus>();
    //}

    //private void Start()
    //{
    //    MouseManager.instance.onMouseClicked += MoveToTargetByMouse;
    //    MouseManager.instance.onEnemyClicked += EventAttack;
    //}

    //private void Update()
    //{
    //    //MoveByKeyBoard();
    //    SwitchAnimation();
    //    lastAttackTime -= Time.deltaTime;
    //}

    //void SwitchAnimation()
    //{
    //    anim.SetFloat("Speed", agent.velocity.sqrMagnitude);
    //}

    //public void MoveToTargetByMouse(Vector3 target)
    //{
    //    StopAllCoroutines();
    //    agent.isStopped = false;
    //    agent.destination = target;
    //}

    //public void MoveByKeyBoard()
    //{
    //    StopAllCoroutines();
    //    agent.isStopped = false;
    //    agent.enabled = false;
    //    // move value
    //    float xm = 0, ym = 0, zm = 0;

    //    if (Input.GetKey(KeyCode.W))
    //    {
    //        zm += Time.deltaTime * characterStatus.characterData.currentSpeed;
    //    }
    //    else if (Input.GetKey(KeyCode.A))
    //    {
    //        xm -= -1 * Time.deltaTime * characterStatus.characterData.currentSpeed;
    //    }
    //    else if (Input.GetKey(KeyCode.S))
    //    {
    //        zm -= -1 * Time.deltaTime * characterStatus.characterData.currentSpeed;
    //    }
    //    else if (Input.GetKey(KeyCode.D))
    //    {
    //        xm += Time.deltaTime * characterStatus.characterData.currentSpeed;
    //    }

    //    transform.position = new Vector3(xm, ym, zm);
    //    agent.enabled = true;
    //}

    //private void EventAttack(GameObject target)
    //{
    //    if (target != null)
    //    {
    //        attackTarget = target;
    //        characterStatus.isCritical = Random.value < characterStatus.attackData.criticalChance;
    //        StartCoroutine(MoveToAttackTarget());
    //    }
    //}

    //IEnumerator MoveToAttackTarget()
    //{
    //    agent.isStopped = false;
    //    transform.LookAt(attackTarget.transform);

    //    while (Vector3.Distance(attackTarget.transform.position, transform.position) > characterStatus.attackData.attackRange)
    //    {
    //        agent.destination = attackTarget.transform.position;
    //        yield return null;
    //    }

    //    agent.isStopped = true;
    //    // Attack

    //    if (lastAttackTime < 0)
    //    {
    //        anim.SetBool("Critical", characterStatus.isCritical);
    //        anim.SetTrigger("Attack");
    //        // 重置攻击冷却时间
    //        lastAttackTime = characterStatus.attackData.coolDown;
    //    }
    //}

    //// Animation Event
    //void Hit()
    //{
    //    var targetStatus = attackTarget.GetComponent<CharacterStatus>();

    //    targetStatus.TakeDamage(characterStatus, targetStatus);
    //}

    // TODO: change data sources
    public float moveSpeed;
    public float rotateSpeed;

    Rigidbody rigid;
    MyInputAction inputActions;
    Vector2 move;

    private float forwardAmount;
    private float rotateAmount;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        inputActions = new MyInputAction();
    }

    private void OnEnable()
    {
        inputActions.Enable();
        inputActions.GamePlay.Movement.performed += Movement_performed;
    }

    private void OnDisable()
    {
        inputActions.Disable();
        inputActions.GamePlay.Movement.performed -= Movement_performed;
    }

    private void Update()
    {
        onMove(move);
    }

    private void FixedUpdate()
    {
        rigid.velocity = forwardAmount * transform.forward * moveSpeed;
        rigid.MoveRotation(rigid.rotation * Quaternion.Euler(0, rotateAmount * rotateSpeed, 0));
    }


    public void Movement_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        move = obj.ReadValue<Vector2>();
    }


    private void onMove(Vector2 direction)
    {
        Vector3 wordlMove = new Vector3(direction.x, 0, direction.y);
        Vector3 localMove = transform.InverseTransformVector(wordlMove);

        forwardAmount = localMove.z;
        rotateAmount = Mathf.Atan2(localMove.x, localMove.z);
    }


}
