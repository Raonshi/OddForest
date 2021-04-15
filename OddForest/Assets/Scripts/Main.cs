using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Main : MonoBehaviour
{

    public Slider hpBar;
    public Text goldText;
    public Text killText;

    public Transform lobbyPoint;
    public Transform startPoint;

    public GameObject bottomGroup;
    public GameObject topGroupPlaying;
    public GameObject topGroupLobby;
    public GameObject lobbyGroup;

    public bool isGame;                     //게임 시작시  true
    public bool isArriveCenter;             //플레이어가 중앙 위치시 true;
    public bool isArriveLobby;              //플레이어가 로비 위치시 true;

    public Spawner spawner1, spawner2;

    public static Main instance = null;
    
    // Start is called before the first frame update
    void Start()
    {
        topGroupPlaying.SetActive(false);
        topGroupLobby.SetActive(false);
        lobbyGroup.SetActive(false);
        bottomGroup.SetActive(false);

        instance = this;
        isGame = false;
        isArriveCenter = false;
        isArriveLobby = false;
        Player.instance.transform.position = lobbyPoint.position;
    }

    // Update is called once per frame
    void Update()
    {
        //게임 중일 경우
        if(isGame == true)
        {
            //UI패널 변경
            topGroupLobby.SetActive(false);
            lobbyGroup.SetActive(false);
            topGroupPlaying.SetActive(true);
            bottomGroup.SetActive(true);

            //플레이어 체력을 체력바에 반영
            hpBar.maxValue = Player.instance.maxHp;
            hpBar.value = Player.instance.currentHp;

            //골드
            goldText.text = string.Format("테스트중...");
            killText.text = string.Format("테스트중...");

            //플레이어 위치 이동
            if(isArriveCenter == false)
            {
                Player.instance.ChangeState(Player.State.Run);
                MoveToCenter();
            }


            //몹 소환
            //각 스포너에서 번갈아가면서 생성
            //몹 젠시간 : 5~10초 중 랜덤
            spawner1.SpawnEnemy(Random.Range(5, 10));
            spawner2.SpawnEnemy(Random.Range(5, 10));
        }
        //로비일 경우
        else
        {
            topGroupLobby.SetActive(true);
            lobbyGroup.SetActive(true);
            topGroupPlaying.SetActive(false);
            bottomGroup.SetActive(false);

            //플레이어 대기 위치로 이동
            if(isArriveLobby == false)
            {
                MoveToLobby();
            }
        }
    }

    /// <summary>
    /// 플레이어의 위치를 중앙으로 이동
    /// </summary>
    public void MoveToCenter()
    {
        if (Vector3.Distance(Player.instance.transform.position, startPoint.position) <= 0.1f)
        {
            Player.instance.InitState();
            isArriveCenter = true;
            return;
        }
        Transform transform = Player.instance.transform;
        Vector3 pos = new Vector3(transform.position.x, transform.position.y, 0);
        Vector3 target = new Vector3(startPoint.position.x, startPoint.position.y, 0);

        transform.localPosition = Vector3.MoveTowards(pos, target, Player.instance.moveSpeed * Time.deltaTime);
    }

    //플레이어의 위치를 로비로 이동
    public void MoveToLobby()
    {
        if (Vector3.Distance(Player.instance.transform.position, lobbyPoint.position) <= 0.1f)
        {
            isArriveLobby = true;
            return;
        }
        Transform transform = Player.instance.transform;
        Vector3 pos = new Vector3(transform.position.x, transform.position.y, 0);
        Vector3 target = new Vector3(lobbyPoint.position.x, lobbyPoint.position.y, 0);

        transform.position = Vector3.MoveTowards(pos, target, Player.instance.moveSpeed * Time.deltaTime);
    }


    public void OnClickGameStart()
    {
        isGame = true;
    }
}
