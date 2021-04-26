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
    }


    public void OnClickAtk()
    {
        Debug.Log("Atk Upgraded");
    }


    public void OnClickCri()
    {
        Debug.Log("Cri Upgraded");
    }
}
