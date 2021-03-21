using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    //player's stat
    public int maxHp;       //player's maximum health point
    public int atk;         //player's attack point
    public int cri;         //player's critical attack rate;

    public float moveSpeed;
    public float rollSpeed;
    public float shieldSpeed;

    //Singleton instance
    public static Player instance = null;

    //player's animation controller;
    public Animator anim;

    //player attack
    public float time;
    public float delay;
    public float attackCount;
    public bool isAttack;

    
    public enum State
    {
        Idle,
        Run,
        Roll,
        Attack,
        Shield,
        Die,
    }
    public State state;
   
    
    private void Awake()
    {
        instance = gameObject.GetComponent<Player>();
    }

    // Start is called before the first frame update
    void Start()
    {
        isAttack = false;

        InitState();
    }

    // Update is called once per frame
    void Update()
    {
        if(state == State.Run)
        {
            Run();
        }
        else if(state == State.Attack)
        {
            Attack();
        }

    }

    public void Run()
    {
        transform.Translate(new Vector3(moveSpeed * Time.deltaTime, 0, 0));
    }
    public void Roll()
    {

    }

    public void Attack()
    {
        time += Time.deltaTime;

        if(time >= delay || attackCount > 3)
        {
            isAttack = false;
            attackCount = 0;
            time = 0;

            ChangeState(State.Idle);
        }

        switch (attackCount)
        {
            case 1:
                anim.Play("Player_Attack_0");
                break;
            case 2:
                anim.Play("Player_Attack_1");
                break;
            case 3:
                anim.Play("Player_Attack_2");
                break;
        }
    }

    public void Shield()
    {

    }

    public void Die()
    {

    }

    //Player Input
    #region MoveInput
    public void OnClickLeft(bool press)
    {
        if (press == true)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            ChangeState(Player.State.Run);
        }
        else
        {
            ChangeState(Player.State.Idle);
        }
    }
    public void OnClickRight(bool press)
    {
        if (press == true)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            ChangeState(Player.State.Run);
        }
        else
        {
            ChangeState(Player.State.Idle);
        }
    }
    #endregion

    #region ActionInput

    public void OnClickAttack()
    {
        if(isAttack == false)
        {
            attackCount++;
            ChangeState(State.Attack);

            isAttack = true;
        }
    }

    #endregion

    //FSM
    #region FSM

    public void InitState()
    {
        state = State.Idle;
        
        anim.SetBool("isRun", false);
        anim.SetBool("isRoll", false);
        anim.SetBool("isAttack", false);
        anim.SetBool("isShield", false);
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
            case State.Run:
                RunAnim();
                break;
            case State.Roll:
                RollAnim();
                break;
            case State.Attack:
                AttackAnim();
                break;
            case State.Shield:
                ShieldAnim();
                break;
            case State.Die:
                DieAnim();
                break;
        }
    }
    
    public void ChangeState(State change)
    {
        InitState();
        
        EnterState(change);
    }

    public void IdleAnim()
    {
        anim.SetBool("isIdle", true);
    }

    public void RunAnim()
    {
        anim.SetBool("isRun", true);
    }

    public void RollAnim()
    {
        anim.SetBool("isRoll", true);
    }

    public void AttackAnim()
    {
        anim.SetBool("isAttack", true);
    }

    public void ShieldAnim()
    {
        anim.SetBool("isShield", true);
    }

    public void DieAnim()
    {
        anim.SetBool("isDie", true);
    }

    #endregion


    //Animation Event
    public void EndEvent()
    {
        isAttack = false;
    }
}