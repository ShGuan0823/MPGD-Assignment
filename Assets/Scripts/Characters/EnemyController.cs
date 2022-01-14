using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public enum EnemyStats { GUARD, PATROL, CHASE, DEAD, IDLE, ATTACK }
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(CharacterStatus))]
public class EnemyController : MonoBehaviour, IEndGameObserver
{
    public EnemyStats enemyStats;
    private GameObject attackTarget;
    private GameObject gameObject;
    private Animator anim;
    private CharacterStatus characterStatus;
    //private CapsuleCollider collider;

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
    private bool alive;

    // 动画控制
    bool isWalk, isChase, isFollow;


    void Awake()
    {
        alive = true;
        anim = GetComponent<Animator>();
        gameObject = GetComponent<GameObject>();
        characterStatus = GetComponent<CharacterStatus>();
        //collider = GetComponent<CapsuleCollider>();
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
    //private void OnEnable()
    //{
    //    GameManager.Instance.AddObserver(this);

    //}

    //private void OnDisable()
    //{
    //    GameManager.Instance.RemoveObserver(this);
    //}

    private void Update()
    {
        CheckAlive();
        if (alive)
        {
            SwitchStates();
            SetAnimation();
            lastAttackTime -= Time.deltaTime;
        }

    }

    void CheckAlive()
    {
        if (characterStatus.CurrentHealth <= 0)
            Dead();
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
                Chase();
                break;
            case EnemyStats.DEAD:
                Dead();
                break;
        }
    }

    void SetAnimation()
    {
        //Debug.Log("anim: " + enemyStats);
        anim.SetBool("Patrol", enemyStats == EnemyStats.PATROL);
        anim.SetBool("Chase", enemyStats == EnemyStats.CHASE);
        anim.SetBool("Follow", isFollow);
        anim.SetBool("Dead", enemyStats == EnemyStats.DEAD);
    }

    void Guard()
    {
        enemyStats = EnemyStats.GUARD;
        if (Vector3.Distance(transform.position, guardPos) > 1)
        {
            transform.LookAt(guardPos);
        }
        transform.Translate((guardPos - transform.position) * Time.deltaTime * characterStatus.CurrentSpeed * 0.1f, Space.World);
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

    void Chase()
    {
        //enemyStats = EnemyStats.CHASE;
        if (!FoundPlayer())
        {

            //stop for watching
            if (remainLookAtTime > 0)
            {
                enemyStats = EnemyStats.IDLE;
                transform.Translate(new Vector3(0, 0, 0), Space.World);
                remainLookAtTime -= Time.deltaTime;
            }
            remainLookAtTime = lookAtTime;
            if (isGuard)
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
                transform.Translate((attackTarget.transform.position - transform.position) * Time.deltaTime * characterStatus.CurrentSpeed * 0.2f, Space.World);
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
    void Dead()
    {
        Debug.Log("GG");
        //collider.enabled;
        transform.Translate(new Vector3(0, 0, 0), Space.World);
        enemyStats = EnemyStats.DEAD;
        alive = false;
        SetAnimation();
        Destroy(gameObject, 2f);
        gameObject.SetActive(false);
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
        if (Vector3.Distance(transform.position, GameManager.Instance.playerStats.transform.position) < characterStatus.attackData.attackRange)
        {
            targetStats.TakeDamage(characterStatus, targetStats);
        }
    }

    public void EndNotify()
    {
        Debug.Log("Game Over");
        transform.Translate(new Vector3(0, 0, 0), Space.World);
        enemyStats = EnemyStats.IDLE;
        SetAnimation();
        alive = false;
    }
}

