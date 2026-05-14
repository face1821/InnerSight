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
            GameManager ins = GameManager.Instance;

            UIFun.instance.GetOneItem();
            Debug.Log("玩家收集到了item");
            SoundManager.Instance.Play(6, "Collected");
            ins.CurrentScore++;
            UIFun.instance.OpenInvisibleWall();
            Destroy(gameObject);
            
            //显影1下（1s）
            var mainPlayer = GameObject.FindWithTag("MainPlayer").GetComponent<Player>();
            mainPlayer.KeepShow(1);
        }
    }

}
