using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public RectTransform stick;
    private RectTransform center;


    public Canvas uiCanvas;
    public float stickRange;

    private void Awake()
    {
        center = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (Player.instance.state != Player.State.Idle)
        {
            return;
        }

        Vector2 pos = eventData.position - center.anchoredPosition;

        Vector2 vect;
        if (pos.magnitude < stickRange)
        {
            vect = new Vector2(pos.x, 0);
        }
        else
        {
            vect = new Vector2(pos.normalized.x * stickRange, 0);
        }
        stick.anchoredPosition = vect;

        //현재 조이스틱이 중앙에서 왼쪽으로 치우쳤다면
        if(stick.position.x < center.position.x)
        {
            //왼쪽으로 이동
            Player.instance.isRight = false;
            Player.instance.isLeft = true;

            Player.instance.transform.rotation = Quaternion.Euler(0, 180, 0);
            Player.instance.ChangeState(Player.State.Run);
        }
        //그 외
        else
        {
            //오른쪽으로 이동
            Player.instance.isRight = true;
            Player.instance.isLeft = false;

            Player.instance.transform.rotation = Quaternion.Euler(0, 0, 0);
            Player.instance.ChangeState(Player.State.Run);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (Player.instance.state != Player.State.Run)
        {
            return;
        }

        Vector2 pos = eventData.position - center.anchoredPosition;

        Vector2 vect;
        if (pos.magnitude < stickRange)
        {
            vect = new Vector2(pos.x, 0);
        }
        else
        {
            vect = new Vector2(pos.normalized.x * stickRange, 0);
        }
        stick.anchoredPosition = vect;

        //현재 조이스틱이 중앙에서 왼쪽으로 치우쳤다면
        if (stick.position.x < center.position.x)
        {
            //왼쪽으로 이동
            Player.instance.isRight = false;
            Player.instance.isLeft = true;

            Player.instance.transform.rotation = Quaternion.Euler(0, 180, 0);
            Player.instance.ChangeState(Player.State.Run);
        }
        //그 외
        else
        {
            //오른쪽으로 이동
            Player.instance.isRight = true;
            Player.instance.isLeft = false;

            Player.instance.transform.rotation = Quaternion.Euler(0, 0, 0);
            Player.instance.ChangeState(Player.State.Run);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        stick.anchoredPosition = Vector2.zero;
        Player.instance.ChangeState(Player.State.Idle);

        Debug.Log("End");
    }


    public void OnClickAttack()
    {
        Player player = Player.instance;
        if (player.isAttack == false)
        {
            player.attackCount++;
            player.time = 0;

            if(player.state != Player.State.Attack)
            {
                player.ChangeState(Player.State.Attack);
            }

            player.isAttack = true;
        }
    }

    public void OnClickShield(bool press)
    {
        if (press == true)
        {
            Player.instance.ChangeState(Player.State.Shield);
        }
        else
        {
            Player.instance.ChangeState(Player.State.Idle);
        }
    }

    public void OnClickRoll()
    {
        Player.instance.ChangeState(Player.State.Roll);
    }
}
