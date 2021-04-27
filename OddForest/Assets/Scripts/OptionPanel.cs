using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionPanel : MonoBehaviour
{
    public Button close;
    public Button lobby;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickClose()
    {
        gameObject.SetActive(false);
    }

    public void OnClickLobby()
    {
        GameManager.Singleton.LoadNextScene("Main");
    }
}
