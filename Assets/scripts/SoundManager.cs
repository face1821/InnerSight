using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;  //这个类的实例

    public AudioSource BGM;
    public AudioSource Transfer_1;
    public AudioSource ThrowDaoju;

    private void Awake()
    {
        //++++++++++++++++单例模式的基本写法++++++++++++++++++++
        if (instance != null)
        {
            Destroy(gameObject);  // 销毁新创建的重复实例
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);  //防止切换场景时被销毁
        //+++++++++++++++++++++++++++++++++++++++++++++++++++++

        BGM.Play();
    }
}
