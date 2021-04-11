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
    }

    // Update is called once per frame
    void Update()
    {
        //esc버튼(안드로이드 뒤로가기 버튼)터치 시 게임 종료
        if(Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }


    public void OnClickGameStart()
    {
        SceneManager.LoadScene("Main");
    }
}
