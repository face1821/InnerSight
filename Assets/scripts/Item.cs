using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Player player = other.GetComponentInParent<Player>();
        if (player != null)
        {
            UIFun.instance.GetOneItem();
            UIFun.instance.OpenInvisibleWall();
            Debug.Log("玩家收集到了item");
            Destroy(gameObject);
        }
    }

}
