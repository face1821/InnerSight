using UnityEngine;

public class email : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (null != other.GetComponent<Player>())
        {
            GameManager instance = GameManager.Instance; //拿到游戏管理器的实例
            if (instance.CurrentScore >= instance.MaxtScore) //如果当前分数大于等于此关要收集的分数，说明通关了
            {
                //在进入下一关之前要判断一下要不要刷新玩家的最大通关数，也就是记录玩家玩到哪一关了
                if (instance.CurrentLevel + 1 > instance.MaxLevel) //若当前关卡数+1大于了玩家的最大关卡数
                {
                    instance.MaxLevel = instance.CurrentLevel + 1; //那就给最大关卡数重新赋值，进行刷新
                    PlayerPrefs.SetInt("maxLevel", instance.MaxLevel);
                    PlayerPrefs.Save(); // 立即保存
                }

                SoundManager.Instance.Play(7, "MailBox", false);
                ChooseLevelUIFun.EnterTheLevel(instance.CurrentLevel + 1); //切换场景
            }
        }
    }
}