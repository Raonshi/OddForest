﻿using System;
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

    //roll
    public bool isRolling;
    public Vector3 rollPos;
    
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
        isRolling = false;

        InitState();
    }

    // Update is called once per frame
    void Update()
    {

        switch(state)
        {
            case State.Run:
                Run();
                break;
            case State.Attack:
                Attack();
                break;
            case State.Shield:
                Shield();
                break;
            case State.Roll:
                if(transform.position == rollPos)
                {
                    transform.position = rollPos;
                    isRolling = false;
                    ChangeState(State.Idle);
                    break;
                }
                Roll();
                break;
        }    

    }

    //이동
    public void Run()
    {
        transform.Translate(new Vector3(moveSpeed * Time.deltaTime, 0, 0));
    }

    //구르기
    public void Roll()
    {
        if (isRolling == false)
        {
            rollPos = new Vector3(transform.position.x + 3, transform.position.y, transform.position.z);
            isRolling = true;
        }
        transform.position = Vector3.MoveTowards(transform.position, rollPos, moveSpeed * Time.deltaTime);

    }

    //공격
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

    //막기
    public void Shield()
    {
        Debug.Log("방패를 들었습니다! 받는 모든 데미지는 절반이 됩니다.");
    }

    //사망처리
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
            time = 0;

            ChangeState(State.Attack);

            isAttack = true;
        }
    }

    public void OnClickShield(bool press)
    {
        if(press == true)
        {
            ChangeState(State.Shield);
        }
        else
        {
            ChangeState(State.Idle);
        }
    }

    public void OnClickRoll()
    {
        ChangeState(State.Roll);
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