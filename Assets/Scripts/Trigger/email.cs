using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class email : MonoBehaviour
{
    [SerializeField] private Sprite OpenSprite;
    [SerializeField] private Sprite CloseSprite;

    private SpriteRenderer SpriteRenderer;

    private void Awake() { SpriteRenderer = GetComponent<SpriteRenderer>(); }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (null != other.GetComponent<Player>())
        {
            GameManager instance = GameManager.Instance; //拿到游戏管理器的实例

            //游戏结束的判断
            if (instance.CurrentLevel >= instance.MaxLevel)
            {
                SoundManager.Instance.Play(7, "MailBox");
                SpriteRenderer.sprite = OpenSprite;
                
                StartCoroutine(nameof(OnGameOver));

                return;
            }

            if (instance.CurrentScore >= instance.MaxtScore) //如果当前分数大于等于此关要收集的分数，说明通关了
            {
                //在进入下一关之前要判断一下要不要刷新玩家的最大通关数，也就是记录玩家玩到哪一关了
                if (instance.CurrentLevel + 1 > instance.MaxLevel) //若当前关卡数+1大于了玩家的最大关卡数
                {
                    instance.MaxLevel = instance.CurrentLevel + 1; //那就给最大关卡数重新赋值，进行刷新
                    PlayerPrefs.SetInt("maxLevel", instance.MaxLevel);
                    PlayerPrefs.Save(); // 立即保存
                }

                SoundManager.Instance.Play(7, "MailBox");
                SpriteRenderer.sprite = OpenSprite;

                StartCoroutine(nameof(OnClose));
            }
        }
    }

    private IEnumerator OnClose()
    {
        yield return new WaitForSeconds(1f);

        SpriteRenderer.sprite = CloseSprite;

        ChooseLevelUIFun.EnterTheLevel(GameManager.Instance.CurrentLevel + 1); //切换场景
    }
    
    private IEnumerator OnGameOver()
    {
        yield return new WaitForSeconds(1f);

        SpriteRenderer.sprite = CloseSprite;
        
        SoundManager.Instance.Play(0, "BGM_1", true, true);
        SceneManager.LoadScene("GameOver");
    }
}