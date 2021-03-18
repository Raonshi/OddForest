using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{

    public Player player;

    public bool isLeft;
    public bool isRight;


    public int tapCount;
    public float tapDelay;

    // Start is called before the first frame update
    void Start()
    {
        tapCount = 0;
        tapDelay = 0.5f;
        
        isLeft = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isLeft == true && isRight == false)
        {
            player.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (isRight == true && isLeft == false)
        {
            player.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    //Button Input
    #region MoveInput
    public void OnClickLeft(bool press)
    {
        isLeft = press;
        if(isLeft == true)
        {
            player.ChangeState(Player.State.Run);
        }
        else
        {
            player.ChangeState(Player.State.Idle);
        }
    }
    public void OnClickRight(bool press)
    {
        isRight = press;
        if (isRight == true)
        {
            player.ChangeState(Player.State.Run);
        }
        else
        {
            player.ChangeState(Player.State.Idle);
        }
    }

    #endregion
}
