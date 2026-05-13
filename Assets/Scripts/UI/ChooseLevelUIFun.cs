using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class ChooseLevelUIFun : MonoBehaviour
{
    [SerializeField] private Image blackOverlay;
    [SerializeField] private VideoPlayer vp;
    [SerializeField] private Button[] buttons;
    [SerializeField] private Sprite[] buttonFinishSprite;

    void Awake()
    {
        if (GameManager.Instance.IsPlayedVideo)
        {
            ColseVideo();
            StartCoroutine(nameof(FadeOutBlackOverlay));
        }
        else
        {
            GameManager.Instance.IsPlayedVideo = true;
            PlayerPrefs.SetInt("isPlayedVideo", 1);

            StartCoroutine(nameof(DetectWhenVideoEnd));
        }

        for (int i = 0; i < GameManager.Instance.MaxLevel; i++)
        {
            buttons[i].GetComponent<Image>().sprite = buttonFinishSprite[i];
        }
    }

    //返回主菜单函数
    public static void ReturenToMainMenu()
    {
        SceneManager.LoadScene("MainMenu"); //跳转至MainMenu场景
    }

    //进入关卡函数
    public static void EnterTheLevel(int LevelNum) //参数是几就进入第几关
    {
        GameManager gm = GameManager.Instance;
        if (LevelNum > gm.MaxLevel)
        {
            Debug.Log("暂未解锁当前关卡");
            return;
        }

        if (gm != null)
        {
            gm.CurrentLevel = LevelNum;
            gm.CurrentSamllLevel = 1;
            gm.CurrentScore = 0;
            gm.CantThrow = false;
            if (LevelNum >= 1 && LevelNum <= gm.ScoreArr.Length)
            {
                gm.MaxtScore = gm.ScoreArr[LevelNum - 1];
            }
            else
            {
                gm.MaxtScore = 0;
            }
        }

        SceneManager.LoadScene("Level" + LevelNum); //跳转场景
        //GameManager instance = GameManager.instance;  //获取实例
        //instance.currentLevel = i;  //将当前关卡数复制为i
        // instance.maxtScore = instance.scoreArr[i - 1];  //将当前关卡所需的分数从数组里拿出来
        // instance.currentScore = 0;  //设置当前分数为0
    }

    public void ColseVideo()
    {
        Destroy(vp.gameObject);
        SoundManager.instance.Play(0, "BGM_1", true);
    }

    private IEnumerator DetectWhenVideoEnd()
    {
        yield return new WaitForSeconds(1f);
        
        yield return new WaitUntil(() => !vp.isPlaying);
        
        ColseVideo();

        StartCoroutine(nameof(FadeOutBlackOverlay));
    }

    private IEnumerator FadeOutBlackOverlay()
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();

            blackOverlay.color = new Color(0, 0, 0, blackOverlay.color.a - 0.02f);

            if (blackOverlay.color.a <= 0.02f)
                break;
        }

        blackOverlay.gameObject.SetActive(false);
    }
}