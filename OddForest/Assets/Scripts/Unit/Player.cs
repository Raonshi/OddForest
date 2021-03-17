using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    //player's stat
    public int maxHp;       //player's maximum health point
    public int atk;         //player's attack point
    public int cri;         //player's critical attack rate;

    //Singleton instance
    public static Player instance = null;

    //player's animation controller;
    public Animator anim;
    
    //FSM
    public FSM fsm;

    private void Awake()
    {
        instance = gameObject.GetComponent<Player>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

}

public class FSM
{
    //player's behavior
    public enum State
    {
        Null = 99,
        Idle = 0,
        Run = 1,
        Roll = 2,
        Shield = 3,
        Attack = 4,
    }
    public State state;

    //This function will be call when Initialize FSM state.
    public void Initialize()
    {
        state = State.Idle;
    }
    
    //This function will be call when enter each state
    public void Enter()
    {
        
    }

    //This function will be call when change each state
    public void Change(State enter)
    {
        Initialize();

        state = enter;
        
        Enter();
    }
}
