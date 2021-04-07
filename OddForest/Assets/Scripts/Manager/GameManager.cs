using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool isNew = true; //세이브 매니저로 데이터 저장하면 true값 없애야함


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
        //저장 정보 불러와야함

        //신규유저일 경우 
        if(isNew == true)
        {
            //초기 정보 설정
        }

        Debug.Log("GameManager Created!");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
