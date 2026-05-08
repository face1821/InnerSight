using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;  //这个类的实例

    public AudioClip BGM_1;         // 音轨0
    public AudioClip Transfer_1;        // 音轨1
    public AudioClip Transfer_2;        // 音轨1
    public AudioClip ThrowDaoju;        // 音轨1

    private int AudioSourceNum = 4;
    List<AudioSource> audios = new List<AudioSource>();

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

        for(int i = 0; i < AudioSourceNum - 1; i++)
        {
            var audio = this.gameObject.AddComponent<AudioSource>();
            audios.Add(audio);
        }

        Play(0, "BGM_1", true);
    }

    void Start()
    {

    }

    public void Play(int index, string name, bool isLoop)
    {
        var clip = GetAudioClip(name);
        if(clip != null)
        {
            var audio = audios[index];
            audio.clip = clip;
            audio.loop = isLoop;
            audio.Play();
        }
    }

    AudioClip GetAudioClip(string name)
    {
        switch (name)
        {
            case "BGM_1":
                return BGM_1;
            case "Transfer_1":
                return Transfer_1;
            case "Transfer_2":
                return Transfer_2;
            case "ThrowDaoju":
                return ThrowDaoju;
        }
        return null;
    }

}
