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
        btnText[0].text = string.Format("예");
        btnText[1].text = string.Format("아니오");
    }


    #region ButtonAction

    public void OnClickClose()
    {
        if(gameObject.name == "인터넷연결없음")
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
        gameObject.SetActive(false);
    }

#endregion
}
