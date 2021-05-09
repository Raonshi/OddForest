using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanel : MonoBehaviour
{
    public enum Type
    {
        OneButton = 1,
        TwoButton = 2,
    }
    public Type btnType;

    public Text infoTitle;
    public Text content;

    public List<Button> btnList = new List<Button>();
    public List<Text> btnText = new List<Text>();

    public void Init(string name, int type)
    {
        gameObject.name = name;
        infoTitle.text = name;
        infoTitle.fontSize = 100;

        btnType = (Type)type;

        if(btnType == Type.OneButton)
        {
            OneButtonSet();
        }
        else
        {
            TwoButtonSet();
        }
    }


    public void OneButtonSet()
    {
        btnText[0].text = string.Format("닫기");

        switch(gameObject.name)
        {
            case "인터넷연결없음":
                content.text = string.Format("인터넷 연결이 되어 있지 않습니다!\n\n인터넷 연결 후 다시 실행하여 주세요!");
                break;
        }
    }


    public void TwoButtonSet()
    {
        switch (gameObject.name)
        {
            case "게임오버":
                btnText[0].text = string.Format("재시작");
                btnText[1].text = string.Format("귀환");
                content.text = string.Format("점수 : {0}\n최고기록 : {1}\n획득골드 : {2}", Main.instance.score, Main.instance.bestScore, Main.instance.gold);
                break;
            case "게임종료":
                btnText[0].text = string.Format("닫기");
                btnText[1].text = string.Format("게임종료");
                content.text = string.Format("게임을 종료하시겠습니까?");
                break;
        }
    }


    #region ButtonAction
    public void OnClickBtn0()
    {
        switch (gameObject.name)
        {
            case "인터넷연결없음":
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
                gameObject.SetActive(false);
                break;

            case "게임오버":
                GameManager.Singleton.restart = true;
                GameManager.Singleton.LoadNextScene("Main");
                break;
            case "게임종료":
                gameObject.SetActive(false);
                Destroy(gameObject);
                break;
        }
    }

    public void OnClickBtn1()
    {
        switch (gameObject.name)
        {
            case "게임오버":
                GameManager.Singleton.LoadNextScene("Main");
                Main.instance.isGame = false;

                break;
            case "게임종료":
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
                break;
        }
    }

#endregion
}
