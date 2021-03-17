using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{

    public Player player;

    public bool isLeft;
    public bool isRight;

    // Start is called before the first frame update
    void Start()
    {
        isLeft = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isLeft == true && isRight == false)
        {
            player.transform.rotation = Quaternion.Euler(0, 180, 0);
            //player.transform.Translate(new Vector3(player.moveSpeed * Time.deltaTime, 0, 0));
        }
        else if (isRight == true && isLeft == false)
        {
            player.transform.rotation = Quaternion.Euler(0, 0, 0);
            //player.transform.Translate(new Vector3(player.moveSpeed * Time.deltaTime, 0, 0));
        }
    }

    public void OnClickLeft(bool press)
    {
        isLeft = press;
        if(isLeft == true)
        {
            player.state = Player.State.Run;
        }
        else
        {
            player.state = Player.State.Idle;
        }
    }
    public void OnClickRight(bool press)
    {
        isRight = press;
        if (isRight == true)
        {
            player.state = Player.State.Run;
        }
        else
        {
            player.state = Player.State.Idle;
        }
    }
}
