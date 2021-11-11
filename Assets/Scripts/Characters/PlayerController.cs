using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator anim;
    private CharacterStatus characterStatus;
    private GameObject attackTarget;
    private float lastAttackTime;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        characterStatus = GetComponent<CharacterStatus>();
    }

    private void Start()
    {
        MouseManager.instance.onMouseClicked += MoveToTarget;
        MouseManager.instance.onEnemyClicked += EventAttack;
    }



    private void Update()
    {
        SwitchAnimation();
        lastAttackTime -= Time.deltaTime;
    }

    void SwitchAnimation()
    {
        anim.SetFloat("Speed", agent.velocity.sqrMagnitude);
    }

    public void MoveToTarget(Vector3 target)
    {
        StopAllCoroutines();
        agent.isStopped = false;
        agent.destination = target;
    }

    private void EventAttack(GameObject target)
    {
        if (target != null)
        {
            attackTarget = target;
            characterStatus.isCritical = Random.value < characterStatus.attackData.criticalChance;
            StartCoroutine(MoveToAttackTarget());
        }
    }

    IEnumerator MoveToAttackTarget()
    {
        agent.isStopped = false;
        transform.LookAt(attackTarget.transform);

        while (Vector3.Distance(attackTarget.transform.position, transform.position) > characterStatus.attackData.attackRange)
        {
            agent.destination = attackTarget.transform.position;
            yield return null;
        }

        agent.isStopped = true;
        // Attack

        if (lastAttackTime < 0)
        {
            anim.SetBool("Critical", characterStatus.isCritical);
            anim.SetTrigger("Attack");
            // 重置攻击冷却时间
            lastAttackTime = characterStatus.attackData.coolDown;
        }
    }

    // Animation Event
    void Hit()
    {
        var targetStatus = attackTarget.GetComponent<CharacterStatus>();

        targetStatus.TakeDamage(characterStatus, targetStatus);
    }
}
