using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    public Text loading;
    public string message;


    // Start is called before the first frame update
    void Start()
    {
        message = "L O A D I N G . . .";

        StartCoroutine(LoadNextScene());
    }

    IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(1.0f);

        AsyncOperation async = SceneManager.LoadSceneAsync(GameManager.Singleton.nextScene);
        async.allowSceneActivation = false;

        while(async.isDone == false)
        {
            //로딩 진행 중
            if(async.progress < 0.9f)
            {
                loading.text = message;
            }
            else
            {
                async.allowSceneActivation = true;
                yield break;
            }
        }
    }
}
