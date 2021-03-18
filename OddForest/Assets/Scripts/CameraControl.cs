using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{

    public Transform target;
    public float up;
    public float zoom;

    public float cameraMoveSpeed;


    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPos = new Vector3(target.position.x, target.position.y + up, zoom);
        transform.position = Vector3.MoveTowards(transform.position, targetPos, cameraMoveSpeed * Time.deltaTime);
    }
}
