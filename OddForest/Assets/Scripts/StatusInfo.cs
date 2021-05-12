using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusInfo : MonoBehaviour
{
    public Text hpText, atkText, criText;
    public Button hpReinforce, atkReinforce, criReinforce;

    // Update is called once per frame
    void Update()
    {
        hpText.text = string.Format("{0} (Lv{1})", Player.instance.maxHp, GameManager.Singleton.hpLevel);
        atkText.text = string.Format("{0} (Lv{1})", Player.instance.atk, GameManager.Singleton.atkLevel);
        criText.text = string.Format("{0}% (Lv{1})", Player.instance.cri, GameManager.Singleton.criLevel);
    }

    public void OnClickHp()
    {
        Debug.Log("Hp Upgraded");
        GameManager.Singleton.PlaySound(Resources.Load<AudioClip>("Sounds/SFX/SFX_BtnClick"));
        GameManager.Singleton.CreateInfoPanel("체력강화", 2);
    }


    public void OnClickAtk()
    {
        Debug.Log("Atk Upgraded");
        GameManager.Singleton.PlaySound(Resources.Load<AudioClip>("Sounds/SFX/SFX_BtnClick"));
        GameManager.Singleton.CreateInfoPanel("공격강화", 2);
    }


    public void OnClickCri()
    {
        Debug.Log("Cri Upgraded");
        GameManager.Singleton.PlaySound(Resources.Load<AudioClip>("Sounds/SFX/SFX_BtnClick"));
        GameManager.Singleton.CreateInfoPanel("치명강화", 2);
    }
}
