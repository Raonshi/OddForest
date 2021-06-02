using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool isNew;
    public float internetCheckTime;

    public int hpLevel, atkLevel, criLevel;
    public int hp = 100;
    public int atk = 10;
    public int cri = 0;
    public int gold, bestScore;

    public bool restart;

    //볼륨 조절
    public List<AudioSource> audio = new List<AudioSource>();
    public int audioSourceCount = 10;
    public float sfxVolume = 0.5f;
    public float bgmVolume = 0.5f;

    //씬 변환
    public string nextScene;

    //전역 싱글톤 -> 게임이 종료될때까지 유지
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

        //유저 데이터 불러오기
        SaveManager.Singleton.LoadPlayerData();


        //사운드 초기화
        for(int i = 0; i < audioSourceCount; i++)
        {
            audio.Add(gameObject.AddComponent<AudioSource>());
            audio[i].Stop();
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

        //매 프레임마다 볼륨 체크
        VolumeControl();
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

    /// <summary>
    /// GameManager의 sfx, bgm값을 통해 볼륨을 조절한다.
    /// </summary>
    public void VolumeControl()
    {
        for(int i = 0; i < audio.Count; i++)
        {
            if(audio[i].clip == null)
            {
                continue;
            }
            else if(audio[i].clip.name.Contains("BGM_") == true)
            {
                audio[i].volume = bgmVolume;
            }
            else if(audio[i].clip.name.Contains("SFX_") == true)
            {
                audio[i].volume = sfxVolume;
            }
        }
    }

    /// <summary>
    /// 현재 재생 중인 모든 사운드를 중지한다.
    /// </summary>
    public void AllSoundStop()
    {
        for(int i = 0; i < audio.Count; i++)
        {
            audio[i].Stop();
            audio[i].clip = null;
        }
    }

    /// <summary>
    /// clip을 재생한다.
    /// </summary>
    /// <param name="clip">재생할 사운드 파일</param>
    public void PlaySound(AudioClip clip)
    {
        for (int i = 0; i < audio.Count; i++)
        {
            //i번째 사운드가 재생 중인 경우
            if (audio[i].isPlaying == true)
            {
                //clip과 i번째 사운드의 파일명이 같다면
                if(clip.name == audio[i].clip.name)
                {
                    if(clip.name.Contains("SFX") == true)
                    {
                        audio[i].Play();
                    }
                    return;
                }
                //파일명이 다르면 재생 안함.
                else
                {
                    continue;
                }
            }
            //재생중이 아닌 경우
            else if (audio[i].isPlaying == false)
            {
                //i번째에 추가
                audio[i].clip = clip;
                //재생시간 초기화
                audio[i].time = 0;

                //BGM
                if (clip.name.Contains("BGM") == true)
                {
                    audio[i].volume = 0.5f;
                    audio[i].loop = true;
                }
                //SFX
                else if (clip.name.Contains("SFX") == true)
                {
                    audio[i].loop = false;
                }

                audio[i].Play();
                return;
            }

        }
    }


    /// <summary>
    /// 팝업창을 인스턴스로 생성한다.
    /// </summary>
    /// <param name="name">생성할 팝업창의 이름</param>
    /// <param name="type">팝업창 버튼 타입(1 : 1버튼, 2 : 2버튼)</param>
    public void CreateInfoPanel(string name, int type)
    {
        if(GameObject.Find("Canvas") == null)
        {
            return;
        }
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

        PlaySound(Resources.Load<AudioClip>("Sounds/SFX/SFX_InfoPopup"));
    }


    public void LoadNextScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("Loading");
    }
}
