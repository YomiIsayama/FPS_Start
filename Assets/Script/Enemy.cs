using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public enum FSMstate
    {
        None,
        Patrol,//idle
        Chase,//run
        Attack,
    }
    public FSMstate curState;

    public Transform m_transform;
    public Player m_player;


    public NavMeshAgent agent;


    public Animator m_ani;
    public float m_movSpeed = 2.5f;
    public float m_rotSpeed = 0.5f;
    public float m_timer = 2;
    public int m_life = 15;

    protected EnemySpawn m_spawn;

    void Start()
    {
        curState = FSMstate.Patrol;
        agent = this.GetComponent<NavMeshAgent>();
        m_player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

}
void Update()
    {
        Debug.Log(m_life);
        //UpdateDmagDvec(Dup,Dright);
        switch (curState)
        {
            case FSMstate.Patrol:
                StatePatrol();
                break;
            case FSMstate.Chase:
                StateChase();
                break;
            case FSMstate.Attack:
                StateAttack();
                break;
             
        }
        CheckHP();
    }
    private void StatePatrol()
    {
        m_ani.SetBool("idle", true);
        m_ani.SetBool("attack", false);
        m_ani.SetBool("death", false);
        m_ani.SetBool("run", false);
        agent.ResetPath();
        if (Enemy_trigger.is_trigger == true)
        {
            curState = FSMstate.Chase;
        }

    }
    private void StateChase()
    {
        m_ani.SetBool("idle", false);
        m_ani.SetBool("attack", false);
        m_ani.SetBool("death", false);
        m_ani.SetBool("run", true);
        agent.SetDestination(m_player.transform.position);
        if (Vector3.Distance(m_transform.position, m_player.transform.position) < 2f)
        {
            curState = FSMstate.Attack;
        }
        else if (Vector3.Distance(m_transform.position, m_player.transform.position) > 8f)
        {
            curState = FSMstate.Patrol;
        }


    }
    private void StateAttack()
    {
        AnimatorStateInfo stateInfo = m_ani.GetCurrentAnimatorStateInfo(0);
        m_ani.SetBool("idle", false);
        m_ani.SetBool("attack", true);
        m_ani.SetBool("death", false);
        m_ani.SetBool("run", false);
        agent.ResetPath();


        if (Vector3.Distance(m_transform.position, m_player.transform.position) > 4f)
        {
            curState = FSMstate.Chase;
        }

    }

    public void OnDamage(int damage)
    {
        m_life -= damage;
    }

    public void WeaponEnable()
    {
        m_player.OnDamage(2);
    }
    public void WeaponDisable()
    {
        // nothing to do
    }
    private void CheckHP()
    {
        AnimatorStateInfo stateInfo = m_ani.GetCurrentAnimatorStateInfo(0);
        if (m_life <= 0)
        {
            m_ani.SetBool("death", true);
            m_ani.SetBool("idle", false);
            m_ani.SetBool("attack", false);
            m_ani.SetBool("run", false);
            if (stateInfo.fullPathHash == Animator.StringToHash("Base Layer.death") && !m_ani.IsInTransition(0))
            {
                m_ani.SetBool("death", false);
                if (stateInfo.normalizedTime >= 1.0f)
                {
                    m_spawn.m_enemyCount--;
                    Destroy(this.gameObject);
                    GameManager.Instance.SetScore(100);

                }

            }

        }
    }

    public void Init(EnemySpawn spawn)
    {
        m_spawn = spawn;
        m_spawn.m_enemyCount++;
    }


}
