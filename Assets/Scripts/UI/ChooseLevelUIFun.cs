using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class ChooseLevelUIFun : MonoBehaviour
{
    public static OverlayFadeEffect SceneOverlay;

    [SerializeField] private Image blackOverlay;
    [SerializeField] private VideoPlayer vp;
    [SerializeField] private Button[] buttons;
    [SerializeField] private Sprite[] buttonFinishSprite;

    private static bool _isLocked;

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

    private void Update() { GameManager.PlayIfEmptyMouseClick(); }

    //返回主菜单函数
    public static void ReturenToMainMenu()
    {
        SceneManager.LoadScene("MainMenu"); //跳转至MainMenu场景
    }

    //进入关卡函数
    public static void EnterTheLevel(int levelNum) //参数是几就进入第几关
    {
        if (_isLocked) return;

        GameManager gm = GameManager.Instance;
        if (levelNum > gm.MaxLevel)
        {
            Debug.Log("暂未解锁当前关卡");
            return;
        }

        if (gm != null)
        {
            gm.CurrentLevel = levelNum;
            gm.CurrentSamllLevel = 1;
            gm.CurrentScore = 0;
            gm.CantThrow = false;
            if (levelNum >= 1 && levelNum <= gm.ScoreArr.Length)
            {
                gm.MaxtScore = gm.ScoreArr[levelNum - 1];
            }
            else
            {
                gm.MaxtScore = 0;
            }
        }

        _isLocked = true;

        SceneOverlay = GameObject.FindWithTag("SceneOverlay").GetComponent<OverlayFadeEffect>();
        SceneOverlay.PlayFadeOut();

        GameManager.Instance.StartCoroutine(WaitForSceneOverlay(levelNum));
    }

    private static IEnumerator WaitForSceneOverlay(int levelNum)
    {
        yield return new WaitUntil(() => SceneOverlay.IsFinished);

        //跳转场景
        SceneManager.LoadScene("Level" + levelNum);
        _isLocked = false;
    }

    public void ColseVideo()
    {
        if (vp != null)
            Destroy(vp.gameObject);
        SoundManager.Instance.Play(0, "BGM_1", true);
    }

    private IEnumerator DetectWhenVideoEnd()
    {
        yield return new WaitForSeconds(1f);

        yield return new WaitUntil(() => vp.isPrepared);
        yield return new WaitUntil(() => vp == null || !vp.isPlaying);

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