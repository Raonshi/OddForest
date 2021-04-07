using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Main : MonoBehaviour
{

    public Slider hpBar;
    public Text goldText;
    public Text killText;
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        hpBar.maxValue = Player.instance.maxHp;
        hpBar.value = Player.instance.currentHp;

        goldText.text = string.Format("테스트중...");
        killText.text = string.Format("테스트중...");
    }
}
