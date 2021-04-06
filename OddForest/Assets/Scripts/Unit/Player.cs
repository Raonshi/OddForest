using System.Numerics;
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

    //roll
    public bool isRolling;
    public Vector3 rollPos;
    
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
   
    
    private void Awake()
    {
        instance = gameObject.GetComponent<Player>();
    }

    // Start is called before the first frame update
    void Start()
    {
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

        CheckTouch();

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
                    rollPos = Vector3.zero;
                    isRolling = false;
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
            ChangeState(State.Die);
        }
    }

    //터치 감지
    public void CheckTouch()
    {
        if(Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            UnityEngine.Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            Ray2D ray = new Ray2D(touchPos, UnityEngine.Vector2.zero);
            RaycastHit2D[] hits = Physics2D.RaycastAll(ray.origin, ray.direction);

            foreach(var hit in hits)
            {
                Debug.Log(hit.transform.name);
            }
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
    }

    //구르기
    public void Roll()
    {
        if (isRolling == false)
        {
            if (isLeft)
            {
                rollPos = new Vector3(transform.position.x - 3, transform.position.y, transform.position.z);
            }
            else if (isRight)
            {
                rollPos = new Vector3(transform.position.x + 3, transform.position.y, transform.position.z);
            }

            isRolling = true;
        }
        transform.position = Vector3.MoveTowards(transform.position, rollPos, moveSpeed * 1.5f * Time.deltaTime);

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
            isRight = false;
            isLeft = true;
            
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
            isRight = true;
            isLeft = false;
            
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