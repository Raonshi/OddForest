using System.Collections;
//using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class Player : MonoBehaviour
{
    //player's stat
    public int maxHp;       //player's maximum health point
    public int currentHp;
    public int atk;         //player's attack point
    public int cri;         //player's critical attack rate;

    public float moveSpeed;
    public float rollSpeed;
    public float shieldSpeed;

    //Singleton instance
    public static Player instance = null;

    //player's animation controller;
    public Animator anim;

    //player direction
    public bool isLeft;
    public bool isRight;
    
    //player attack
    public float time;
    public float delay;
    public float attackCount;
    public bool isAttack;
    public Transform hitRange;

    //roll
    public bool isRolling;
    public Transform rollPos;
    Vector3 tempPos;
    
    //camera edge
    public Transform leftEdge;
    public Transform rightEdge;
    
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

    // Start is called before the first frame update
    void Start()
    {
        //플레이어 초기 정보 세팅
        maxHp = UpdateHp();
        atk = UpdateAttack();
        cri = UpdateCritical();

        instance = this;

        isAttack = false;
        isRolling = false;

        isLeft = false;
        isRight = true;

        currentHp = maxHp;

        InitState();
    }

    // Update is called once per frame
    void Update()
    {

        //플레이어 체력 체크
        CheckHp();

        CameraAreaCheck();

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
                if(transform.position == tempPos)
                {
                    transform.position = tempPos;
                    isRolling = false;

                    tempPos = Vector3.zero;
                    ChangeState(State.Idle);
                    break;
                }
                Roll();
                break;
        }    

    }

    public void CheckHp()
    {
        if(currentHp <= 0)
        {
            Die();
        }
    }

    public int UpdateHp()
    {
        return GameManager.Singleton.hp* System.Convert.ToInt32(GameManager.Singleton.hpLevel * 1.15f);
    }


    public int UpdateAttack()
    {
        return GameManager.Singleton.atk* System.Convert.ToInt32(GameManager.Singleton.atkLevel * 1.15f);
    }


    public int UpdateCritical()
    {
        if(GameManager.Singleton.criLevel == 1)
        {
            return 0;
        }
        
        if(GameManager.Singleton.cri <= 10)
        {
            return System.Convert.ToInt32(System.Math.Pow(2, GameManager.Singleton.criLevel - 1));
        }
        else
        {
            int cri = GameManager.Singleton.cri;
            int level = GameManager.Singleton.criLevel;

            return cri + (level - 6);
        }

    }


    //카메라 영역 내 이동 체크
    public void CameraAreaCheck()
    {
        if (transform.position.x <= leftEdge.position.x)
        {
            transform.position = new Vector3(leftEdge.position.x, transform.position.y, transform.position.z);
        }
        else if (transform.position.x >= rightEdge.position.x)
        {
            transform.position = new Vector3(rightEdge.position.x, transform.position.y, transform.position.z);
        }
    }

    //이동
    public void Run()
    {
        transform.Translate(new Vector3(moveSpeed * Time.deltaTime, 0, 0));

        int rand = Random.Range(0, 4);
        GameManager.Singleton.PlaySound(Resources.Load<AudioClip>("Sounds/SFX/SFX_Run" + rand));
    }

    //구르기
    public void Roll()
    {
        if (isRolling == false)
        {
            tempPos = new Vector3(rollPos.position.x, rollPos.position.y, rollPos.position.z);
            isRolling = true;
        }
        transform.position = Vector3.MoveTowards(transform.position, tempPos, moveSpeed * 1.5f * Time.deltaTime);
    }

    //공격
    public void Attack()
    {
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
                attackCount = 0;
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
        if(state != State.Die)
        {
            ChangeState(State.Die);
        }

        StartCoroutine(GameOver());
    }


    IEnumerator GameOver()
    {
        yield return new WaitForSeconds(0.3f);
        Main.instance.isGameOver = true;
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


    //Animation Event
    public void EndEvent()
    {
        isAttack = false;
        ChangeState(Player.State.Idle);
    }

    public void CreateHitCollider()
    {
        GameObject hit = new GameObject("HitCollider");
        hit.transform.SetParent(transform);
        hit.transform.position = hitRange.position;

        hit.AddComponent<BoxCollider2D>();
        hit.GetComponent<BoxCollider2D>().isTrigger = true;

        hit.AddComponent<HitCollider>();
    }
}