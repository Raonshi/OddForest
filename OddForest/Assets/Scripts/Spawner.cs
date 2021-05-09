using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public float delta;
    public bool isSpawn;

    // Start is called before the first frame update
    void Start()
    {
        isSpawn = false;
    }

    public void SpawnEnemy(int spawnTime)
    {
        if(isSpawn == false)
        {
            isSpawn = true;
            delta = spawnTime;
        }

        delta -= Time.deltaTime;

        if (delta <= 0)
        {
            GameObject obj;
            int rand = 0;

            if(Main.instance.killCount <= 10)
            {
                rand = Random.Range(0, 2);
            }
            else
            {
                rand = Random.Range(0, 4);
            }

            switch (rand)
            {
                case 0:
                    obj = Instantiate(Resources.Load<GameObject>("Prefabs/Bat"));
                    obj.transform.SetParent(this.transform);
                    //obj.transform.position = this.transform.position;
                    obj.transform.position = new Vector3(this.transform.position.x, Player.instance.transform.localPosition.y, this.transform.position.z);
                    break;
                case 1:
                    obj = Instantiate(Resources.Load<GameObject>("Prefabs/Wolf"));
                    obj.transform.SetParent(this.transform);
                    //obj.transform.position = this.transform.position;
                    obj.transform.position = new Vector3(this.transform.position.x, Player.instance.transform.localPosition.y, this.transform.position.z);
                    break;
                case 2:
                    obj = Instantiate(Resources.Load<GameObject>("Prefabs/Vampire"));
                    obj.transform.SetParent(this.transform);
                    //obj.transform.position = this.transform.position;
                    obj.transform.position = new Vector3(this.transform.position.x, Player.instance.transform.localPosition.y, this.transform.position.z);
                    break;
                case 3:
                    obj = Instantiate(Resources.Load<GameObject>("Prefabs/Ogre"));
                    obj.transform.SetParent(this.transform);
                    //obj.transform.position = this.transform.position;
                    obj.transform.position = new Vector3(this.transform.position.x, Player.instance.transform.localPosition.y, this.transform.position.z);
                    break;
            }

            isSpawn = false;
        }
    }
}
