using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitCollider : MonoBehaviour
{
    float time = 0.5f;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Debug.Log("Enter!");
            collision.GetComponent<Enemy>().hp -= Player.instance.atk;

            Dispose();
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, 0.5f);
    }

    private void Update()
    {
        time -= Time.deltaTime;

        if(time <= 0)
        {
            Dispose();
        }
    }

    void Dispose()
    {
        DestroyImmediate(gameObject);
    }
}
