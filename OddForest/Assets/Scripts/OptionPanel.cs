using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionPanel : MonoBehaviour
{
    public Button close;
    public Button lobby;

    public Slider bgm, sfx;

    void Start()
    {
        bgm.value = GameManager.Singleton.bgmVolume;
        sfx.value = GameManager.Singleton.sfxVolume;
    }

    // Update is called once per frame
    void Update()
    {
        GameManager.Singleton.bgmVolume = bgm.value;
        GameManager.Singleton.sfxVolume = sfx.value;
    }

    public void OnClickClose()
    {
        GameManager.Singleton.PlaySound(Resources.Load<AudioClip>("Sounds/SFX/SFX_BtnClick"));
        gameObject.SetActive(false);
    }

    public void OnClickLobby()
    {
        GameManager.Singleton.PlaySound(Resources.Load<AudioClip>("Sounds/SFX/SFX_BtnClick"));
        GameManager.Singleton.CreateInfoPanel("로비귀환", 2);
    }
}
