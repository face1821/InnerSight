using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject PausePanel;
    private bool isPaused = false;


    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }

        }
        
    }

    public void ResumeGame()
    {
        PausePanel.SetActive(false);
;       Time.timeScale = 1f;
        isPaused = false;
    }

    private void PauseGame()
    {
        PausePanel.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }


    //重新开始逻辑
    public void RestartCurrentLevel()
    {
        PausePanel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

    public void LoadSettingsScene()
    {
        // 恢复时间，避免设置场景动画卡住
        Time.timeScale = 1f;
        // 标记暂停状态为 false（可选）
        isPaused = false;
        // 关闭暂停面板（可选，因为即将跳转场景）
        PausePanel.SetActive(false);
        // 跳转到设置场景
        SceneManager.LoadScene("OptionScene");
    }


}
