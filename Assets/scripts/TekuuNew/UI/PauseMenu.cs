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
        if (PausePanel == null)  // 或者 anyComponent == null
        {
            Debug.LogWarning("目标对象已被销毁，跳过暂停逻辑");
            return;
        }

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


}
