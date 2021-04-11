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
        SavePlayerData();

        LoadPlayerData();
    }

    /// <summary>
    /// 플레이어 데이터 저장
    /// </summary>
    public void SavePlayerData()
    {
        PlayerJson json = new PlayerJson();
        json.SetData(1, 1, 1, 1, 1);

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

        if(Directory.Exists(path) == false)
        {
            Directory.CreateDirectory(path);
        }

        //파일로 저장된 json데이터를 string으로 불러옴
        StreamReader stream = new StreamReader(path + "player.json");
        string json = stream.ReadToEnd();
        stream.Dispose();

        PlayerJson data = JsonUtility.FromJson<PlayerJson>(json);

        Debug.Log(string.Format("hp = {0}, atk = {1}, cri = {2}, gold = {3}, best = {4}", data.maxHp, data.atk, data.cri, data.gold, data.bestScore));
    }
}


#region JSON Data

public class PlayerJson
{
    public int maxHp;
    public int atk;
    public int cri;
    public int gold;
    public int bestScore;

    public void SetData(int _maxHp, int _atk, int _cri, int _gold, int _bestScore)
    {
        maxHp = _maxHp;
        atk = _atk;
        cri = _cri;
        gold = _gold;
        bestScore = _bestScore;
    }

    public string Serealize()
    {
        return JsonUtility.ToJson(this);
    }
}

#endregion