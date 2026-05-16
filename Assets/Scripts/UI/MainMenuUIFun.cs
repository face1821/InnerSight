using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUIFun : MonoBehaviour
{
    private void Awake() { GameObject.FindWithTag("SceneOverlay").GetComponent<OverlayFadeEffect>().PlayFadeIn(); }

    //开始游戏函数
    public static void StartGame()
    {
        SceneManager.LoadScene("ChooseLevel"); //跳转至ChooseLevel场景

        if (!GameManager.Instance.IsPlayedVideo)
            SoundManager.Instance.Stop(0, "BGM_1");
    }

    private void Update() { GameManager.PlayIfEmptyMouseClick(); }

    //结束游戏函数
    public static void ExittGame()
    {
        //关闭游戏窗口
        Application.Quit(); //打包后使用
        //EditorApplication.ExitPlaymode();  //开发状态下使用
    }
}