using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private static SaveManager instance = null;

    public static SaveManager Singleton
    {
        get
        {
            if (instance == null)
            {
                GameObject obj = GameObject.Find("SaveManager");
                if(obj == null)
                {
                    obj = new GameObject("SaveManager");
                    obj.AddComponent<SaveManager>();
                    DontDestroyOnLoad(obj);
                }
                instance = obj.GetComponent<SaveManager>();
            }
            return instance;
        }
    }


    public void Init()
    {
        Debug.Log("SaveManager has created");
        LoadPlayerData();
    }

    /// <summary>
    /// 플레이어 데이터 저장
    /// </summary>
    public void SavePlayerData()
    {
        PlayerJson json = new PlayerJson();
        json.SetData(
            GameManager.Singleton.hpLevel,
            GameManager.Singleton.atkLevel,
            GameManager.Singleton.criLevel,
            GameManager.Singleton.gold,
            GameManager.Singleton.bestScore,
            GameManager.Singleton.isNew);

        string data = json.Serealize();
        string path = Application.persistentDataPath + "/Save/";

        //저장 폴더가 존재하지 않을 경우
        if(Directory.Exists(path) == false)
        {
            Directory.CreateDirectory(path);
        }

        //데이터 스트림 생성 및 데이터 저장
        StreamWriter stream = new StreamWriter(path + "player.json");
        stream.Write(data);
        stream.Dispose();

        Debug.Log("End!");
    }


    public void LoadPlayerData()
    {
        string path = Application.persistentDataPath + "/Save/";

        if(Directory.Exists(path) == false || File.Exists(path + "player.json") == false)
        {
            Debug.Log("저장 파일이 존재하지 않습니다.");
            GameManager.Singleton.isNew = true;

            return;
        }

        //파일로 저장된 json데이터를 string으로 불러옴
        StreamReader stream = new StreamReader(path + "player.json");
        string json = stream.ReadToEnd();
        stream.Dispose();

        PlayerJson data = JsonUtility.FromJson<PlayerJson>(json);

        GameManager.Singleton.hpLevel = data.hpLevel;
        GameManager.Singleton.atkLevel = data.atkLevel;
        GameManager.Singleton.criLevel = data.criLevel;
        GameManager.Singleton.gold = data.gold;
        GameManager.Singleton.bestScore = data.bestScore;
    }
}

#region JSON Data

public class PlayerJson
{
    public int hpLevel;
    public int atkLevel;
    public int criLevel;
    public int gold;
    public int bestScore;
    public bool isNew;

    public void SetData(int _hpLevel, int _atkLevel, 
        int _criLevel, int _gold, int _bestScore, bool _isNew)
    {
        hpLevel = _hpLevel;
        atkLevel = _atkLevel;
        criLevel = _criLevel;
        gold = _gold;
        bestScore = _bestScore;
        isNew = _isNew;
    }

    public string Serealize()
    {
        return JsonUtility.ToJson(this);
    }
}

#endregion