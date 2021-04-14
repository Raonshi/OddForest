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

    public bool isGame;

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
        }
    }



    public void OnClickGameStart()
    {
        isGame = true;
    }
}
