using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject PausePanel;
    private bool isPaused = false;

    [SerializeField] private Player thisMainPlayer;
    [SerializeField] private Player thisNotMainPlayer;

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
        GameManager.instance.cantThrow = false;
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
        GameManager.instance.cantThrow = true;
        
    }


    //重新开始逻辑
    public void RestartCurrentLevel()
    {
        PausePanel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameManager.instance.cantThrow = false;

        GameObject mainPlayerPositions = GameObject.Find("MainPlayerPositions");
        GameObject playerPosB = mainPlayerPositions.transform.Find("PlayerPositions" + GameManager.instance.currentSamllLevel)?.gameObject;
        thisMainPlayer.transform.position = playerPosB.transform.position;

        GameObject notMainPlayerPositions = GameObject.Find("NotMainPlayerPositions");
        GameObject playerPosA = notMainPlayerPositions.transform.Find("PlayerPositions" + GameManager.instance.currentSamllLevel)?.gameObject;
        thisNotMainPlayer.transform.position = playerPosA.transform.position;

        SoundManager.instance.Play(8, "Restart", false);
        // GameManager ins = GameManager.instance;
        // thisMainPlayer.transform.position = ins.MainPlayerPosition[ins.currentSamllLevel - 1].transform.position;
        // thisNotMainPlayer.transform.position = ins.notMainPlayerPosition[ins.currentSamllLevel - 1].transform.position;


    }


}
