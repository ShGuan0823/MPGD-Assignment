using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public enum EnemyStats { GUARD, PATROL, CHASE, DEAD, IDLE, ATTACK }
public class EnemyController : MonoBehaviour
{
    public EnemyStats enemyStats;
    private Transform mTransform;
    private GameObject attackTarget;
    private Animator anim;
    private CharacterStatus characterStatus;

    [Header("Basic Settings")]
    public float sightRadius;
    public bool isGuard;
    public float lookAtTime;
    private float remainLookAtTime;
    private float lastAttackTime;

    [Header("Patrol State")]
    public float patrolRange;
    public float stoppingDistance;

    private Vector3 wayPoint;
    private Vector3 guardPos;

    // 动画控制
    bool isWalk, isChase, isFollow;


    void Awake()
    {
        mTransform = GetComponent<Transform>();
        anim = GetComponent<Animator>();
        characterStatus = GetComponent<CharacterStatus>();
        guardPos = transform.position;
        remainLookAtTime = lookAtTime;
        lastAttackTime = characterStatus.attackData.coolDown;
    }

    private void Start()
    {
        if (isGuard)
        {
            enemyStats = EnemyStats.GUARD;
        }
        else
        {
            enemyStats = EnemyStats.PATROL;
            GetNewWayPoint();
        }
    }

    private void Update()
    {
        SwitchStates();
        SetAnimation();
        lastAttackTime -= Time.deltaTime;
    }

    void SwitchStates()
    {
        // 发现Player 切换CHASE
        if (FoundPlayer())
        {
            enemyStats = EnemyStats.CHASE;
            //Debug.Log("Find Player");
        }

        switch (enemyStats)
        {
            case EnemyStats.GUARD:
                Guard();
                break;
            case EnemyStats.PATROL:
                Patrol();
                break;
            case EnemyStats.CHASE:
                AttackPlayer();
                break;
            case EnemyStats.DEAD:
                break;
        }
    }

    void SetAnimation()
    {
        //Debug.Log("anim: " + enemyStats);
        anim.SetBool("Patrol", enemyStats == EnemyStats.PATROL);
        anim.SetBool("Chase", enemyStats == EnemyStats.CHASE);
        anim.SetBool("Follow", isFollow);
        //anim.SetBool("Critical", characterStatus.isCritical);
    }

    void Guard()
    {
        enemyStats = EnemyStats.GUARD;
        transform.Translate((guardPos - transform.position) * Time.deltaTime * characterStatus.CurrentSpeed * 0.2f, Space.World);
    }

    void Patrol()
    {
        enemyStats = EnemyStats.PATROL;
        // 判断是否到了随机点
        if (Vector3.Distance(wayPoint, transform.position) <= stoppingDistance)
        {
            //enemyStats = EnemyStats.IDLE;
            //if (remainLookAtTime > 0)
            //{
            //    Debug.Log("time: " + remainLookAtTime);
            //    transform.Translate(new Vector3(0, 0, 0));
            //    remainLookAtTime -= Time.deltaTime;
            //}
            //remainLookAtTime = lookAtTime;
            GetNewWayPoint();


        }
        else
        {
            enemyStats = EnemyStats.PATROL;
            transform.Translate((wayPoint - transform.position) * Time.deltaTime * characterStatus.CurrentSpeed * 0.02f, Space.World);
            transform.LookAt(wayPoint);
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
        //enemyStats = EnemyStats.CHASE;
        if (!FoundPlayer())
        {
            enemyStats = EnemyStats.IDLE;
            //stop for watching
            if (remainLookAtTime > 0)
            {
                transform.Translate(new Vector3(0, 0, 0), Space.World);
                remainLookAtTime -= Time.deltaTime;
            }
            else if (isGuard)
            {
                enemyStats = EnemyStats.GUARD;
            }
            else
            {
                enemyStats = EnemyStats.PATROL;
            }
        }
        else
        {


            // check in attack range
            if (TargetInAttackRange() || TargetInSkillRange())
            {
                if (lastAttackTime <= 0)
                {
                    lastAttackTime = characterStatus.attackData.coolDown;
                    Attack();
                }
            }
            else
            {
                enemyStats = EnemyStats.CHASE;
                isFollow = true;
                transform.Translate((attackTarget.transform.position - mTransform.position) * Time.deltaTime * characterStatus.CurrentSpeed * 0.2f, Space.World);
                transform.LookAt(attackTarget.transform.position);
            }
        }
    }

    void Attack()
    {
        enemyStats = EnemyStats.ATTACK;
        transform.Translate(new Vector3(0, 0, 0), Space.World);
        transform.LookAt(attackTarget.transform);
        isFollow = false;
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

    //void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawWireSphere(transform.position, sightRadius);
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(transform.position, patrolRange);
    //}

    void GetNewWayPoint()
    {
        remainLookAtTime = lookAtTime;

        float randomX = Random.Range(-patrolRange, patrolRange);
        float randomZ = Random.Range(-patrolRange, patrolRange);

        wayPoint = new Vector3(guardPos.x + randomX, transform.position.y, guardPos.z + randomZ);

    }

    // Animation Event
    void Hit()
    {
        var targetStats = GameManager.Instance.playerStats;

        targetStats.TakeDamage(characterStatus, targetStats);
    }

}

