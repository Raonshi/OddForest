using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum Name
    {
        Bat,
        Wolf,
        Vampire,
        Orge,
    }
    public Name name;

    public enum State
    {
        Idle,
        Move,
        Attack,
        Die,
    }
    public State state;

    public Animator anim;
    public SpriteRenderer sprite;

    public int hp;
    public int atk;
    public float moveSpeed;

    public float attackRange;
    public bool attackEvent;

    public Transform target;

    public bool dieEvent;

    private void Start()
    {
        attackEvent = false;
        dieEvent = false;

        InitState();
        target = GameObject.Find("Player").transform;
    }

    private void Update()
    {
        if(hp <= 0)
        {
            Died();
            return;
        }

        CheckDistance();
    }


    /// <summary>
    /// 타겟과 자신의 거리를 측정
    /// </summary>
    public void CheckDistance()
    {
        //공격범위 밖에 타겟이 있을 경우
        if (Vector3.Distance(target.position, transform.position) >= attackRange)
        {
            Chasing();
        }
        else
        {
            Attack();
        }
    }

    /// <summary>
    /// 타겟 추적
    /// </summary>
    public void Chasing()
    {
        if(anim.GetBool("isMove") == false)
        {
            StartCoroutine(ChangeState(State.Move));
            //ChangeState(State.Move);
        }

        //타겟이 오른쪽에 있을 경우
        if(target.position.x - transform.position.x <= 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
    }

    /// <summary>
    /// 공격
    /// </summary>
    public void Attack()
    {
        if (anim.GetBool("isAttack") == false)
        {
            StartCoroutine(ChangeState(State.Attack));
        }

        if(attackEvent == true)
        {
            Player.instance.currentHp -= atk;
            attackEvent = false;
        }
    }


    /// <summary>
    /// 사망 처리
    /// </summary>
    public void Died()
    {
        StartCoroutine(ChangeState(State.Die));

        if(dieEvent == true)
        {
            switch(name)
            {
                case Name.Wolf:
                    GameManager.Singleton.gold += 100;
                    Main.instance.score += 100;
                    break;
                case Name.Bat:
                    GameManager.Singleton.gold += 50;
                    Main.instance.score += 50;
                    break;
                case Name.Orge:
                    GameManager.Singleton.gold += 250;
                    Main.instance.score += 250;
                    break;
                case Name.Vampire:
                    GameManager.Singleton.gold += 200;
                    Main.instance.score += 200;
                    break;
            }
            Main.instance.killCount++;
            Destroy(gameObject);
        }
    }

    //FSM
    #region FSM

    public void InitState()
    {
        state = State.Idle;

        anim.SetBool("isMove", false);
        anim.SetBool("isAttack", false);
        anim.SetBool("isDie", false);
        anim.SetBool("isIdle", true);
    }

    public void EnterState(State enter)
    {
        anim.SetBool("isIdle", false);
        state = enter;

        switch (enter)
        {
            case State.Idle:
                IdleAnim();
                break;
            case State.Move:
                MoveAnim();
                break;
            case State.Attack:
                AttackAnim();
                break;
            case State.Die:
                DieAnim();
                break;
        }
    }

    IEnumerator ChangeState(State change)
    {
        InitState();
        yield return new WaitForSeconds(0.2f);
        EnterState(change);
    }

    public void IdleAnim()
    {
        anim.SetBool("isIdle", true);
    }

    public void MoveAnim()
    {
        anim.SetBool("isMove", true);
    }

    public void AttackAnim()
    {
        anim.SetBool("isAttack", true);
    }
    /*
    public void FlyAnim()
    {
        anim.SetBool("isFly", true);
    }
    */
    public void DieAnim()
    {
        anim.SetBool("isDie", true);
    }

    #endregion

    public void AttackEvent()
    {
        attackEvent = true;
    }

    public void DieEvent()
    {
        dieEvent = true;
    }
}
