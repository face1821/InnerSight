using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChooseLevelUIFun : MonoBehaviour
{
    //返回主菜单函数
    public static void ReturenToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");  //跳转至MainMenu场景
    }

    //进入关卡函数
    //TODO：(这函数是从别的项目里粘过来的，可能要改逻辑)
    public static void EnterTheLevel(int LevelNum)  //参数是几就进入第几关
    {
        GameManager gm = GameManager.instance;
        if(gm != null)
        {
            gm.currentLevel = LevelNum;
            gm.currentScore = 0;
            if(LevelNum >= 1 && LevelNum <= gm.scoreArr.Length)
            {
                gm.maxtScore = gm.scoreArr[LevelNum - 1];
            }
            else
            {
                gm.maxtScore = 0;
            }
        }
        SceneManager.LoadScene("Level" + LevelNum);  //跳转场景
        //GameManager instance = GameManager.instance;  //获取实例
        //instance.currentLevel = i;  //将当前关卡数复制为i
        // instance.maxtScore = instance.scoreArr[i - 1];  //将当前关卡所需的分数从数组里拿出来
        // instance.currentScore = 0;  //设置当前分数为0
    }

}
