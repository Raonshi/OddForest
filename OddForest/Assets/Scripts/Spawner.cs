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
            //소환 -> 
            SpawnEnemy();
            delta = spawnTime;
        }
    }

    public void SpawnEnemy()
    {
        int rand = Random.Range(0, 4);

        GameObject obj;
        switch(rand)
        {
            case 0:
                obj = Instantiate(Resources.Load<GameObject>("Prefabs/Bat"));
                obj.transform.SetParent(this.transform);
                obj.transform.position = this.transform.position;
                break;
            case 1:
                obj = Instantiate(Resources.Load<GameObject>("Prefabs/Wolf"));
                obj.transform.SetParent(this.transform);
                obj.transform.position = this.transform.position;
                break;
            case 2:
                obj = Instantiate(Resources.Load<GameObject>("Prefabs/Vampire"));
                obj.transform.SetParent(this.transform);
                obj.transform.position = this.transform.position;
                break;
            case 3:
                obj = Instantiate(Resources.Load<GameObject>("Prefabs/Ogre"));
                obj.transform.SetParent(this.transform);
                obj.transform.position = this.transform.position;
                break;
        }
    }
}
