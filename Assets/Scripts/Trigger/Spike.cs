// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.SceneManagement;

// public class Spike : MonoBehaviour
// {
//     [SerializeField] private PauseMenu pm;

//     void OnTriggerEnter2D(Collider2D other)
//     {
//         Player player = other.GetComponentInParent<Player>();
//         if (player != null)
//         {
//             // player.stateMachine.ChangeState(player.deadState);
//             RefreshScene();
//         }
//     }

//     private void RefreshScene()
//     {
//         GameManager ins = GameManager.instance;
//         pm.RestartCurrentLevel();
        
//         // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
//     }
// }

using System.Collections;
using UnityEngine;

public class Spike : MonoBehaviour
{
    [SerializeField] private PauseMenu pm;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Player player = other.GetComponentInParent<Player>();
        if (player != null)
        {
            // 切换为死亡状态
            player.stateMachine.ChangeState(player.deadState);
            if(player.isMain)
                player.notmainPlayer.stateMachine.ChangeState(player.notmainPlayer.deadState);
            // 启动协程，延迟执行刷新和恢复状态
            StartCoroutine(DeathCoroutine(player));
        }
    }

    private IEnumerator DeathCoroutine(Player player)
    {
        // 等待 1.5 秒
        yield return new WaitForSeconds(1.5f);
        // 刷新当前关卡（重新加载或重置）
        RefreshScene();
        // 刷新后，将玩家状态改为待机状态
        if (player != null)
        {
            player.stateMachine.ChangeState(player.idleState);
            if(player.isMain)
                player.notmainPlayer.stateMachine.ChangeState(player.notmainPlayer.idleState);
        }
    }

    private void RefreshScene()
    {
        pm.RestartCurrentLevel();
    }
}
