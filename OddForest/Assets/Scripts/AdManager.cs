using UnityEngine;

public class AdManager : MonoBehaviour
{
    private static AdManager instance = null;

    public static AdManager Singleton
    {
        get
        {
            if (instance == null)
            {
                GameObject obj = GameObject.Find("AdManager");
                if (obj == null)
                {
                    obj = new GameObject("AdManager");
                    obj.AddComponent<AdManager>();
                    DontDestroyOnLoad(obj);
                }
                instance = obj.GetComponent<AdManager>();
            }
            return instance;
        }
    }

    
    public void Init()
    {
        gameObject.AddComponent<RewardAd>();
    }
}
