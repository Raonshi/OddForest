using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Main : MonoBehaviour
{

    public Slider hpBar;
    public Text goldText;
    public Text killText;

    public Transform lobbyPoint;
    public Transform startPoint;

    public GameObject bottomGroup;
    public GameObject topGroupPlaying;
    public GameObject topGroupLobby;
    public GameObject lobbyGroup;

    public bool isGame;

    public static Main instance = null;
    
    // Start is called before the first frame update
    void Start()
    {
        topGroupPlaying.SetActive(false);
        topGroupLobby.SetActive(false);
        lobbyGroup.SetActive(false);
        bottomGroup.SetActive(false);

        instance = this;
        isGame = false;
        Player.instance.transform.position = lobbyPoint.position;
    }

    // Update is called once per frame
    void Update()
    {
        //게임 중일 경우
        if(isGame == true)
        {
            topGroupLobby.SetActive(false);
            lobbyGroup.SetActive(false);
            topGroupPlaying.SetActive(true);
            bottomGroup.SetActive(true);

            hpBar.maxValue = Player.instance.maxHp;
            hpBar.value = Player.instance.currentHp;

            goldText.text = string.Format("테스트중...");
            killText.text = string.Format("테스트중...");
        }
        //로비일 경우
        else
        {
            topGroupLobby.SetActive(true);
            lobbyGroup.SetActive(true);
            topGroupPlaying.SetActive(false);
            bottomGroup.SetActive(false);
        }
    }

    public void OnClickGameStart()
    {
        isGame = true;
    }
}
