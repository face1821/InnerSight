using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance; //这个类的实例

    public int CurrentLevel; //当前所在的关卡
    public int CurrentSamllLevel; //当前所在小关卡
    public int CurrentScore; //当前关卡所获得的分数
    public int MaxtScore; //当前关卡总共需要获得的分数
    public int[] ScoreArr = { 4, 3, 3, 3, 3, 3 }; //每一关通关需要的分数！！！！！
    public int MaxLevel = 1; //当前已通过的最大关卡

    public int NowThrowState = 0; // 0为没有投掷  1为普通投掷  2为蓄力投掷  用来控制传送音效
    public bool CantThrow = false;
    public bool IsPlayedVideo = false;


    public Vector2[] WorldCenter1;
    public Vector2[] WorldCenter2;
    public Vector2[] WorldCenter3;
    public Vector2[] WorldCenter4;
    public Vector2[] WorldCenter5;
    public Vector2[][] AllWorldCenter;


    private void Awake()
    {
        //++++++++++++++++单例模式的基本写法++++++++++++++++++++
        if (Instance != null)
        {
            Destroy(gameObject); // 销毁新创建的重复实例
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); //防止切换场景时被销毁
        //+++++++++++++++++++++++++++++++++++++++++++++++++++++

        if (CurrentLevel == 0)
        {
            CurrentLevel = 1;
        }

        //在游戏开始时从磁盘里读取出之前存入的值，这一行也是存档的逻辑，第二个参数1为默认值
        MaxLevel = PlayerPrefs.GetInt("maxLevel", 1);
        IsPlayedVideo = PlayerPrefs.GetInt("isPlayedVideo", 0) == 1;

        AllWorldCenter = new Vector2[][] { WorldCenter1, WorldCenter2, WorldCenter3, WorldCenter4, WorldCenter5 };

        //PlayerPrefs.SetInt("maxLevel", 1);  //这两行用于调试
        //PlayerPrefs.Save();
    }

    [Button]
    public void ResetPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }
}