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

    //행동 상태
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
    
    //Attack
    public int attackCount;
    public float attackDelay;
    public float delta;
    public bool isEnd;
    
    private void Awake()
    {
        instance = gameObject.GetComponent<Player>();
    }

    // Start is called before the first frame update
    void Start()
    {
        InitState();
        attackDelay = 0.5f;
        isEnd = false;
    }

    // Update is called once per frame
    void Update()
    {

        switch (state)
        {
            case State.Run:
                transform.Translate(new Vector3(moveSpeed * Time.deltaTime, 0, 0));
                break;
            case State.Attack:
                Attack();
                break;
        }
        
    }

    public void Attack()
    {
        if (isEnd == true)
        {
            attackCount++;
            
            if (attackCount > 2)
            {
                attackDelay = 0.0f;
                attackCount = 0;
            }

            anim.SetBool("firstAttack", false);
            anim.SetBool("secondAttack", false);
            anim.SetBool("thirdAttack", false);
            ChangeState(State.Idle);
        }

        isEnd = false;
        switch (attackCount)
        {
            //first
            case 0:
                delta = Time.time;
                if (CheckAttackCount() == true)
                {
                    anim.SetBool("firstAttack", true);
                }
                break;
            case 1:
                if (CheckAttackCount() == true)
                {
                    delta = Time.time;
                    anim.SetBool("secondAttack", true);
                }
                break;

        }
        
    }

    public bool CheckAttackCount()
    {
        if (attackCount >= 2 || Time.time - delta > attackDelay)
        {
            delta = 0.0f;
            attackCount = 0;
            return false;
        }
        else
        {
            return true;
        }
    }

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
    
    
}
