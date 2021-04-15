using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool isNew = true; //세이브 매니저로 데이터 저장하면 true값 없애야함
    public float internetCheckTime;


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

        //저장 정보 불러와야함

        //신규유저일 경우 
        if(isNew == true)
        {
            //초기 정보 설정
            Debug.Log("new Player!");
        }

        Debug.Log("GameManager Created!");

        SaveManager.Singleton.Init();
    }

    // Start is called before the first frame update
    void Start()
    {
        CreateInfoPanel("인터넷연결없음", 1);
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
