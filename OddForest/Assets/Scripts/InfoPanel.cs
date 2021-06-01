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
            case "강화성공":
                content.text = string.Format("강화에 성공하여 능력이 상승하였습니다!");
                break;
            case "골드부족":
                content.text = string.Format("강화에 필요한 골드가 부족합니다.\n게임 플레이를 통해 골드를 획득할 수 있습니다.");
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
            case "로비귀환":
                btnText[0].text = string.Format("아니오");
                btnText[1].text = string.Format("예");
                content.text = string.Format("로비로 돌아가시겠습니까?\n<color=red>획득한 점수와 골드는 반영되지 않습니다!</color>");
                break;
            case "체력강화":
                int hpPrice = Convert.ToInt32(GameManager.Singleton.hpLevel * 100 * 5.75f);
                btnText[0].text = string.Format("닫기");
                btnText[1].text = string.Format("강화");
                content.text = string.Format("강화 비용은 {0}골드입니다.\n소지금 : {1}gold\n강화하시겠습니까?",
                    hpPrice, GameManager.Singleton.gold);
                break;
            case "공격강화":
                int atkPrice = Convert.ToInt32(GameManager.Singleton.atkLevel * 100 * 5.75f);
                btnText[0].text = string.Format("닫기");
                btnText[1].text = string.Format("강화");
                content.text = string.Format("강화 비용은 {0}골드입니다.\n소지금 : {1}gold\n강화하시겠습니까?",
                    atkPrice, GameManager.Singleton.gold);
                break;
            case "치명강화":
                int criPrice = Convert.ToInt32(GameManager.Singleton.criLevel * 150 * 5.75f);
                btnText[0].text = string.Format("닫기");
                btnText[1].text = string.Format("강화");
                content.text = string.Format("강화 비용은 {0}골드입니다.\n소지금 : {1}gold\n강화하시겠습니까?",
                    criPrice, GameManager.Singleton.gold);
                break;
            case "광고시청":
                btnText[0].text = string.Format("아니오");
                btnText[1].text = string.Format("예");
                content.text = string.Format("광고를 시청할 경우 현재 획득 골드의 1.5배를 획득할 수 있습니다.\n광고를 시청하겠습니까?");
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
                Destroy(gameObject);
                break;

            case "게임오버":
                GameManager.Singleton.CreateInfoPanel("광고시청", 2);
                gameObject.SetActive(false);
                break;
            case "광고시청":
                GameManager.Singleton.gold += Main.instance.gold;
                if (Main.instance.score > GameManager.Singleton.bestScore)
                {
                    GameManager.Singleton.bestScore = Main.instance.score;
                }
                SaveManager.Singleton.SavePlayerData();

                GameManager.Singleton.restart = true;
                GameManager.Singleton.LoadNextScene("Main");
                break;
            case "게임종료":
            case "로비귀환":
            case "체력강화":
            case "공격강화":
            case "치명강화":
            case "강화성공":
            case "골드부족":
                gameObject.SetActive(false);
                Destroy(gameObject);
                break;
        }
        GameManager.Singleton.PlaySound(Resources.Load<AudioClip>("Sounds/SFX/SFX_BtnClick"));
    }

    public void OnClickBtn1()
    {
        switch (gameObject.name)
        {
            case "게임오버":
                GameManager.Singleton.gold += Main.instance.gold;
                GameManager.Singleton.LoadNextScene("Main");
                break;
            case "광고시청":
                AdManager.Singleton.gameObject.GetComponent<RewardAd>().WatchAd();
                gameObject.SetActive(false);
                break;
            case "게임종료":
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
                break;
            case "로비귀환":
                GameManager.Singleton.LoadNextScene("Main");
                break;
            case "체력강화":
                int hpPrice = Convert.ToInt32(GameManager.Singleton.hpLevel * 100 * 5.75f);

                if (GameManager.Singleton.gold >= hpPrice)
                {
                    GameManager.Singleton.gold -= hpPrice;
                    GameManager.Singleton.hpLevel++;

                    SaveManager.Singleton.SavePlayerData();

                    GameManager.Singleton.CreateInfoPanel("강화성공", 1);
                }
                else
                {
                    GameManager.Singleton.CreateInfoPanel("골드부족", 1);
                }
                break;
            case "공격강화":
                int atkPrice = Convert.ToInt32(GameManager.Singleton.atkLevel * 100 * 5.75f);

                if (GameManager.Singleton.gold >= atkPrice)
                {
                    GameManager.Singleton.gold -= atkPrice;
                    GameManager.Singleton.atkLevel++;

                    SaveManager.Singleton.SavePlayerData();

                    GameManager.Singleton.CreateInfoPanel("강화성공", 1);
                }
                else
                {
                    GameManager.Singleton.CreateInfoPanel("골드부족", 1);
                }
                break;
            case "치명강화":
                int criPrice = Convert.ToInt32(GameManager.Singleton.criLevel * 100 * 5.75f);

                if (GameManager.Singleton.gold >= criPrice)
                {
                    GameManager.Singleton.gold -= criPrice;
                    GameManager.Singleton.criLevel++;

                    SaveManager.Singleton.SavePlayerData();

                    GameManager.Singleton.CreateInfoPanel("강화성공", 1);
                }
                else
                {
                    GameManager.Singleton.CreateInfoPanel("골드부족", 1);
                }
                break;
        }

        GameManager.Singleton.PlaySound(Resources.Load<AudioClip>("Sounds/SFX/SFX_BtnClick"));

        gameObject.SetActive(false);
        Destroy(gameObject);
    }

#endregion
}
