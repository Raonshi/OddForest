using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool isNew;
    public float internetCheckTime;

    public int hpLevel, atkLevel, criLevel;
    public const int hp = 100;
    public const int atk = 10;
    public const int cri = 0;
    public int gold, bestScore;

    public bool restart;


    //씬 변환
    public string nextScene;

    private static GameManager instance = null;

    public static GameManager Singleton
    {
        get
        {
            if(instance == null)
            {
                GameObject obj = GameObject.Find("GameManager");

                if(obj == null)
                {
                    obj = new GameObject("GameManager");
                    obj.AddComponent<GameManager>();
                    DontDestroyOnLoad(obj);
                }
                instance = obj.GetComponent<GameManager>();
            }
            return instance;
        }
    }

    public void Init()
    {
        //기본 변수 셋팅
        internetCheckTime = 1.0f;
        restart = false;

        //저장 정보 불러와야함
        SaveManager.Singleton.Init();
        
        //신규유저일 경우 
        if (isNew == true)
        {
            //초기 정보 설정
            Debug.Log("new Player!");

            hpLevel = atkLevel = criLevel = 1;
            gold = 0;
            bestScore = 0;

            isNew = false;

            SaveManager.Singleton.SavePlayerData();
        }

        Debug.Log("GameManager Created!");
    }

    // Update is called once per frame
    void Update()
    {
        //매초 인터넷 연결 체크
        internetCheckTime -= Time.deltaTime;
        if(internetCheckTime < 0)
        {
            CheckInternet();
            internetCheckTime = 1.0f;
        }
    }

    /// <summary>
    /// 게임 앱이 실행되는 동안 인터넷 연결 상태를  체크한다.
    /// </summary>
    public void CheckInternet()
    {
        switch (Application.internetReachability)
        {
            //인터넷에 연결되어 있지 않을 경우
            case NetworkReachability.NotReachable:
                CreateInfoPanel("인터넷연결없음", 1);

                Debug.Log(string.Format("Please Connect Internet"));
                break;
            //인터넷에 연결되어 있을 경우
            case NetworkReachability.ReachableViaCarrierDataNetwork:
            case NetworkReachability.ReachableViaLocalAreaNetwork:
                Debug.Log(string.Format("Internet Connected"));
                break;
        }
    }


    public void CreateInfoPanel(string name, int type)
    {
        GameObject obj;

        if (type == 1)
        {
            obj = Instantiate(Resources.Load<GameObject>("Prefabs/OneButtonInfoPanel"), GameObject.Find("Canvas").transform);
        }
        else
        {
            obj = Instantiate(Resources.Load<GameObject>("Prefabs/TwoButtonInfoPanel"), GameObject.Find("Canvas").transform);
        }
        obj.GetComponent<InfoPanel>().Init(name, type);
    }


    public void LoadNextScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("Loading");
    }
}
