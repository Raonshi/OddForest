using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public float spawnTime;
    public float delta;

    // Start is called before the first frame update
    void Start()
    {
        delta = spawnTime;
    }

    // Update is called once per frame
    void Update()
    {
        delta -= Time.deltaTime;

        if (delta <= 0)
        {
            //소환
            SpawnEnemy();
            delta = spawnTime;
        }
    }

    public void SpawnEnemy()
    {
        int rand = Random.Range(0, 4);

        GameObject obj = new GameObject();
        obj.AddComponent<Enemy>();
        obj.AddComponent<SpriteRenderer>();

        Enemy enemy = obj.GetComponent<Enemy>();
        enemy.name = (Enemy.Name)rand;

        obj.name = enemy.name.ToString();
        obj.transform.SetParent(this.transform);
        obj.transform.position = this.transform.position;

        enemy.Init();
    }
}
