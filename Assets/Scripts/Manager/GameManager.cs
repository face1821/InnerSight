using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


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

        //注册回调
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public static void PlayIfEmptyMouseClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //如果鼠标点到了按钮，返回
            if (EventSystem.current.IsPointerOverGameObject())
            {
                var obj = GameManager.GetClickUIObject();
                if (obj != null && obj.GetComponent<Button>() != null)
                    return;
            }

            //否则播放空点击音效
            SoundManager.Instance.Play(9, "MouseClick");
        }
    }

    public static GameObject GetClickUIObject()
    {
        if (EventSystem.current == null) return null;

        PointerEventData pointerData = new PointerEventData(EventSystem.current);
        pointerData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        // 返回最顶层点击到的UI物体
        if (results.Count > 0)
        {
            return results[0].gameObject;
        }

        return null;
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        //如果加载到关卡内
        if (arg0.name.StartsWith("Level"))
        {
            GameObject.FindWithTag("SceneOverlay").GetComponent<OverlayFadeEffect>().PlayFadeIn();
        }
    }

    [Button]
    public void ResetPlayerPrefs() { PlayerPrefs.DeleteAll(); }

    private void OnDestroy() { SceneManager.sceneLoaded -= OnSceneLoaded; }
}