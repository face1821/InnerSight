using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public static string previousScene;//记录上一个场景


    //要跳转的场景
    public static void GoToScene(string sceneName)
    {
        //记录当前场景
        previousScene = SceneManager.GetActiveScene().name;
        Time.timeScale = 1f;
        if (sceneName == "MainMenu")
        {
            SoundManager.Instance.Play(0, "BGM_1", true);
        }
        SceneManager.LoadScene(sceneName);
    }

    public static void GoBack()
    {
        if (!string.IsNullOrEmpty(previousScene))
        {
            //返回到被记录的场景
            Time.timeScale = 1f;
            SceneManager.LoadScene(previousScene);
        }
        else
        {
            //若没有记录场景，则返回到默认场景，比如主菜单
            Debug.LogWarning("没有记录上一个场景，返回默认界面");
            Time.timeScale = 1f;
            SceneManager.LoadScene("MainMenu");
        }
    }
}
