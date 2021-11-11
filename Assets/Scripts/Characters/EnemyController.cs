using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public enum EnemyStates { GUARD, PATORL, CHASE, DEAD }
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour
{
    public EnemyStates enemyStates;
    private NavMeshAgent agent;
    private GameObject attackTarget;
    private Animator anim;
    private CharacterStatus characterStatus;

    [Header("Basic Settings")]
    public float sightRadius;
    public bool isGuard;
    private float speed;
    public float lookAtTime;
    private float remainLookAtTime;
    private float lastAttackTime;

    [Header("Patrol State")]
    public float patrolRange;

    private Vector3 wayPoint;
    private Vector3 guardPos;

    // 动画控制
    bool isWalk, isChase, isFollow;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        characterStatus = GetComponent<CharacterStatus>();
        speed = agent.speed;
        guardPos = transform.position;
        remainLookAtTime = lookAtTime;
    }

    private void Start()
    {
        if (isGuard)
        {
            enemyStates = EnemyStates.GUARD;
        }
        else
        {
            enemyStates = EnemyStates.PATORL;
            GetNewWayPoint();
        }
    }

    private void Update()
    {
        SwitchStates();
        SwitchAnimation();
        lastAttackTime -= Time.deltaTime;
    }

    void SwitchStates()
    {
        // 发现Player 切换CHASE
        if (FoundPlayer())
        {
            enemyStates = EnemyStates.CHASE;
            //Debug.Log("Find Player");
        }

        switch (enemyStates)
        {
            case EnemyStates.GUARD:
                break;
            case EnemyStates.PATORL:
                Patorl();
                break;
            case EnemyStates.CHASE:
                AttackPlayer();
                break;
            case EnemyStates.DEAD:
                break;
        }
    }

    void SwitchAnimation()
    {
        anim.SetBool("Walk", isWalk);
        anim.SetBool("Chase", isChase);
        anim.SetBool("Follow", isFollow);
        anim.SetBool("Critical", characterStatus.isCritical);
    }

    void Patorl()
    {
        isChase = false;
        agent.speed = speed * 0.5f;
        // 判断是否到了随机点
        if (Vector3.Distance(wayPoint, transform.position) <= agent.stoppingDistance)
        {
            isWalk = false;
            if (remainLookAtTime > 0)
            {
                remainLookAtTime -= Time.deltaTime;
            }
            else
            {
                GetNewWayPoint();
            }

        }
        else
        {
            isWalk = true;
            agent.destination = wayPoint;
        }
    }

    bool FoundPlayer()
    {
        var colliders = Physics.OverlapSphere(transform.position, sightRadius);
        foreach (var target in colliders)
        {
            if (target.CompareTag("Player"))
            {
                attackTarget = target.gameObject;
                return true;
            }

        }
        attackTarget = null;
        return false;
    }

    void AttackPlayer()
    {
        isWalk = false;
        isChase = true;
        agent.speed = speed;
        if (!FoundPlayer())
        {
            isFollow = false;
            // 停下观望一下
            //if (remainLookAtTime > 0)
            //{
            //    agent.destination = transform.position;
            //    remainLookAtTime -= Time.deltaTime;
            //}
            //else
            if (isGuard)
            {
                enemyStates = EnemyStates.GUARD;
            }
            else
            {
                enemyStates = EnemyStates.PATORL;
            }
        }
        else
        {
            isFollow = true;
            agent.isStopped = false;
            agent.destination = attackTarget.transform.position;
        }
        // 在攻击范围内则攻击
        if (TargetInAttackRange() || TargetInSkillRange())
        {
            isFollow = false;
            agent.isStopped = true;
            if (lastAttackTime <= 0)
            {
                lastAttackTime = characterStatus.attackData.coolDown;

                // 执行攻击
                Attack();
            }
        }
    }

    void Attack()
    {
        transform.LookAt(attackTarget.transform);
        if (TargetInAttackRange())
        {
            // 普通攻击
            anim.SetTrigger("Attack");
        }
        if (TargetInSkillRange())
        {
            // 技能攻击
            anim.SetTrigger("Skill");
        }
    }

    bool TargetInAttackRange()
    {
        if (attackTarget != null)
        {
            return Vector3.Distance(attackTarget.transform.position, transform.position) <= characterStatus.attackData.attackRange;
        }
        else
            return false;
    }

    bool TargetInSkillRange()
    {
        if (attackTarget != null)
            return Vector3.Distance(attackTarget.transform.position, transform.position) <= characterStatus.attackData.skillRange;
        else
            return false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, patrolRange);
    }

    void GetNewWayPoint()
    {
        remainLookAtTime = lookAtTime;

        float randomX = Random.Range(-patrolRange, patrolRange);
        float randomZ = Random.Range(-patrolRange, patrolRange);

        Vector3 randomPoint = new Vector3(guardPos.x + randomX, transform.position.y, guardPos.z + randomZ);

        NavMeshHit hit;
        wayPoint = NavMesh.SamplePosition(wayPoint, out hit, patrolRange, 1) ? hit.position : transform.position;


        wayPoint = randomPoint;
    }

    // Animation Event
    void Hit()
    {
        if (attackTarget != null)
        {
            var targetStatus = attackTarget.GetComponent<CharacterStatus>();

            targetStatus.TakeDamage(characterStatus, targetStatus);
        }
    }
}

