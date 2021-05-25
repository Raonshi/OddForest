using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    //인게임 상단바
    public Slider hpBar;
    public Text goldTextInGame;
    public Text scoreTextInGame;
    public Button optionInGame;

    //인게임 사이드바
    public Text gameLevelText;
    public Text killText;

    //로비 상단바
    public Text goldTextLobby;
    public Text bestScoreTextLobby;
    public Button optionLobby;

    //로비, 인게임 캐릭터 시작위치
    public Transform lobbyPoint;
    public Transform startPoint;

    //패널그룹
    public GameObject bottomGroup;
    public GameObject topGroupPlaying;
    public GameObject topGroupLobby;
    public GameObject lobbyGroup;
    public GameObject optionPanel;
    public GameObject statusInfoPanel;
    public GameObject sideGroup;

    //한 게임에서 획득한 골드
    public int gold;

    //점수
    public int score;
    public int bestScore;

    //인게임 난이도 레벨
    public int gameLevel;
    public int killCount;
    int currentKillCount;

    //체크용 불값
    public bool isGame;                     //게임 시작시  true
    public bool isGameOver;
    public bool isArriveCenter;             //플레이어가 중앙 위치시 true;
    public bool isArriveLobby;              //플레이어가 로비 위치시 true;

    //인게임 스포너
    public Spawner spawner1, spawner2;

    //싱글턴 객체
    public static Main instance = null;
    
    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        bestScore = GameManager.Singleton.bestScore;
        gameLevel = 1;
        killCount = 0;

        topGroupPlaying.SetActive(false);
        topGroupLobby.SetActive(false);
        lobbyGroup.SetActive(false);
        bottomGroup.SetActive(false);
        statusInfoPanel.SetActive(false);
        optionPanel.SetActive(false);
        sideGroup.SetActive(false);

        instance = this;

        isGame = GameManager.Singleton.restart;
        isGameOver = false;
        isArriveCenter = false;
        isArriveLobby = false;
        Player.instance.transform.position = lobbyPoint.position;
    }

    // Update is called once per frame
    void Update()
    {
        //게임 중일 경우
        if (isGame == true)
        {
            GameManager.Singleton.PlaySound(Resources.Load<AudioClip>("Sounds/BGM/BGM_InGame"));
            //UI패널 변경
            topGroupLobby.SetActive(false);
            lobbyGroup.SetActive(false);
            topGroupPlaying.SetActive(true);
            bottomGroup.SetActive(true);
            statusInfoPanel.SetActive(false);
            sideGroup.SetActive(true);

            //플레이어 체력을 체력바에 반영
            hpBar.maxValue = Player.instance.maxHp;
            hpBar.value = Player.instance.currentHp;

            //레벨 증가 체크
            double lv = killCount / 30;
            gameLevel = System.Convert.ToInt32(System.Math.Truncate(lv)) + 1;

            //골드
            goldTextInGame.text = GameManager.Singleton.gold.ToString();
            scoreTextInGame.text = score.ToString();

            //킬 카운트 UI표시
            killText.text = killCount.ToString();

            //레벨 UI표시
            gameLevelText.text = gameLevel.ToString();

            //플레이어 위치 이동
            if(isArriveCenter == false)
            {
                Player.instance.ChangeState(Player.State.Run);
                MoveToCenter();
            }

            //몹 소환
            //각 스포너에서 번갈아가면서 생성
            //몹 젠시간 : 5~10초 중 랜덤
            if(isGameOver == false)
            {
                spawner1.SpawnEnemy(Random.Range(5, 5));
                spawner2.SpawnEnemy(Random.Range(5, 5));
            }
            //게임 오버일 경우
            else
            {
                if(GameObject.Find("게임오버") == null && GameObject.Find("광고시청") == null)
                {
                    GameManager.Singleton.CreateInfoPanel("게임오버", 2);
                }
            }

            //뒤로가기 버튼 누를 경우
            if(Input.GetKey(KeyCode.Escape))
            {
                OnClickOption(true);
            }
        }
        //로비일 경우
        else
        {
            GameManager.Singleton.PlaySound(Resources.Load<AudioClip>("Sounds/BGM/BGM_Lobby"));

            topGroupLobby.SetActive(true);
            lobbyGroup.SetActive(true);
            topGroupPlaying.SetActive(false);
            bottomGroup.SetActive(false);
            statusInfoPanel.SetActive(true);
            sideGroup.SetActive(false);

            //골드
            goldTextLobby.text = GameManager.Singleton.gold.ToString();

            //베스트스코어 표시
            bestScoreTextLobby.text = bestScore.ToString();


            //플레이어 대기 위치로 이동
            if (isArriveLobby == false)
            {
                MoveToLobby();
            }

            //뒤로가기 버튼 누를 경우
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GameManager.Singleton.CreateInfoPanel("게임종료", 2);
            }

            //상단바 갱신
            UpdateTopBar();
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

            if(GameManager.Singleton.restart == true)
            {
                GameManager.Singleton.restart = false;
            }

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


    public void UpdateTopBar()
    {
        
    }


    public void OnClickOption(bool _isGame)
    {
        if(_isGame == true)
        {
            optionPanel.GetComponent<OptionPanel>().lobby.gameObject.SetActive(true);
        }
        else
        {
            optionPanel.GetComponent<OptionPanel>().lobby.gameObject.SetActive(false);
        }
        GameManager.Singleton.PlaySound(Resources.Load<AudioClip>("Sounds/SFX/SFX_BtnClick"));
        optionPanel.SetActive(true);
    }

    public void OnClickGameStart()
    {
        GameManager.Singleton.AllSoundStop();

        GameManager.Singleton.PlaySound(Resources.Load<AudioClip>("Sounds/SFX/SFX_BtnClick"));
        isGame = true;
    }
}
