using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class score : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)  //这个参数是你碰到的物体
    {
        if (null != other.GetComponent<Player>()) 
        {
            GameManager instance = GameManager.instance;  //拿到游戏管理器的实例
            if (instance.currentScore >= instance.maxtScore)  //如果当前分数大于等于此关要收集的分数，说明通关了
            {
                //在进入下一关之前要判断一下要不要刷新玩家的最大通关数，也就是记录玩家玩到哪一关了
                if (instance.currentLevel + 1 > instance.maxLevel)  //若当前关卡数+1大于了玩家的最大关卡数
                {
                    instance.maxLevel = instance.currentLevel + 1;  //那就给最大关卡数重新赋值，进行刷新
                    PlayerPrefs.SetInt("maxLevel", instance.maxLevel);
                    PlayerPrefs.Save(); // 立即保存
                }
                ChooseLevelUIFun.EnterTheLevel(instance.currentLevel + 1);  //切换场景
            }
        }
    }

}
