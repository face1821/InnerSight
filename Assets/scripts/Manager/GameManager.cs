using System;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;  //这个类的实例

    public int currentLevel;  //当前所在的关卡
    public int currentSamllLevel;  //当前所在小关卡
    public int currentScore;  //当前关卡所获得的分数
    public int maxtScore;  //当前关卡总共需要获得的分数
    public int[] scoreArr = { 3, 3, 3, 3, 3, 3 };  //每一关通关需要的分数！！！！！
    public int maxLevel = 1;  //当前已通过的最大关卡

    public int nowThrowState = 0;  // 0为没有投掷  1为普通投掷  2为蓄力投掷  用来控制传送音效
    public bool cantThrow = false;



    public Vector2[] WorldCenter1;
    public Vector2[] WorldCenter2;
    public Vector2[] WorldCenter3;
    public Vector2[] WorldCenter4;
    public Vector2[] WorldCenter5;
    public Vector2[][] allWorldCenter;


    private void Awake()
    {
        //++++++++++++++++单例模式的基本写法++++++++++++++++++++
        if (instance != null)
        {
            Destroy(gameObject);  // 销毁新创建的重复实例
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);  //防止切换场景时被销毁
        //+++++++++++++++++++++++++++++++++++++++++++++++++++++

        if (currentLevel == 0)
        {
            currentLevel = 1;
        }

        //在游戏开始时从磁盘里读取出之前存入的值，这一行也是存档的逻辑，第二个参数1为默认值
        maxLevel = PlayerPrefs.GetInt("maxLevel", 1);

        allWorldCenter = new Vector2[][] { WorldCenter1, WorldCenter2, WorldCenter3, WorldCenter4, WorldCenter5 };

        //PlayerPrefs.SetInt("maxLevel", 1);  //这两行用于调试
        //PlayerPrefs.Save();
    }



}
