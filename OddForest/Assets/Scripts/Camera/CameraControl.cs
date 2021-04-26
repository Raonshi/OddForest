using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{

    public Transform target;
    public float up;

    public float cameraMoveSpeed;

    public GameObject pos;

    // Update is called once per frame
    void Update()
    {
        if (Main.instance.isGame == true)
        {
            if (pos.transform.position.x <= GameObject.Find("CameraWall_Mid").transform.position.x ||
                pos.transform.position.x >= GameObject.Find("CameraWall_Right").transform.position.x)
            {
                return;
            }

            Vector3 targetPos = new Vector3(target.position.x, target.position.y + up, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, targetPos, cameraMoveSpeed * Time.deltaTime);
        }
    }
}
