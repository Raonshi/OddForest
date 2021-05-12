using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{

    public static Title instance = null;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        GameManager.Singleton.Init();

        GameManager.Singleton.PlaySound(Resources.Load<AudioClip>("Sounds/BGM/BGM_Title"));
    }

    // Update is called once per frame
    void Update()
    {
        //esc버튼(안드로이드 뒤로가기 버튼)터치 시 게임 종료
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.Singleton.CreateInfoPanel("게임종료", 2);
        }
    }


    public void OnClickGameStart()
    {
        GameManager.Singleton.PlaySound(Resources.Load<AudioClip>("Sounds/SFX/SFX_BtnClick"));
        GameManager.Singleton.LoadNextScene("Main");
    }
}
