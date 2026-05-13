using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance; //这个类的实例

    public AudioClip BGM_1; // 音轨0
    public AudioClip Transfer_1; // 音轨1
    public AudioClip Transfer_2; // 音轨1
    public AudioClip ThrowDaoju; // 音轨1
    public AudioClip BuildUp; // 音轨1
    public AudioClip SilentWalk_Fast; // 音轨2
    public AudioClip Soul; // 音轨3
    public AudioClip Vocal_1; // 音轨4
    public AudioClip Vocal_2; // 音轨4
    public AudioClip Vocal_3; // 音轨4
    public AudioClip Vocal_4; // 音轨4
    public AudioClip Vocal_5; // 音轨4
    public AudioClip DaojuImpact; // 音轨5
    public AudioClip Collected; // 音轨6
    public AudioClip MailBox; // 音轨7
    public AudioClip Restart; // 音轨8


    private int AudioSourceNum = 10;
    List<AudioSource> audios = new List<AudioSource>();

    private void Awake()
    {
        //++++++++++++++++单例模式的基本写法++++++++++++++++++++
        if (instance != null)
        {
            Destroy(gameObject); // 销毁新创建的重复实例
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject); //防止切换场景时被销毁
        //+++++++++++++++++++++++++++++++++++++++++++++++++++++

        for (int i = 0; i < AudioSourceNum; i++)
        {
            var audio = this.gameObject.AddComponent<AudioSource>();
            audios.Add(audio);
        }

        Play(0, "BGM_1", true);
    }

    void Start() { }

    public void Play(int index, string name, bool isLoop, bool isForce = false)
    {
        var clip = GetAudioClip(name);
        if (clip != null)
        {
            var audio = audios[index];
            audio.clip = clip;
            audio.loop = isLoop;

            if (isForce)
            {
                audio.Play();
                return;
            }

            if (audio.time > 0)
                audio.UnPause();
            else
                audio.Play();
        }
    }

    public void Stop(int index, string name)
    {
        if (index < 0 || index >= audios.Count)
            return;

        var clip = GetAudioClip(name);
        if (clip == null)
            return;

        var audio = audios[index];
        if (audio.clip == clip)
            audio.Stop();
    }

    public void Pause(int index, string name)
    {
        if (index < 0 || index >= audios.Count)
            return;

        var clip = GetAudioClip(name);
        if (clip == null)
            return;

        var audio = audios[index];
        if (audio.clip == clip)
            audio.Pause();
    }

    public void UnPause(int index, string name)
    {
        if (index < 0 || index >= audios.Count)
            return;

        var clip = GetAudioClip(name);
        if (clip == null)
            return;

        var audio = audios[index];
        if (audio.clip == clip)
            audio.UnPause();
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
            case "BuildUp":
                return BuildUp;
            case "SilentWalk_Fast":
                return SilentWalk_Fast;
            case "Soul":
                return Soul;
            case "Vocal_1":
                return Vocal_1;
            case "Vocal_2":
                return Vocal_2;
            case "Vocal_3":
                return Vocal_3;
            case "Vocal_4":
                return Vocal_4;
            case "Vocal_5":
                return Vocal_5;
            case "DaojuImpact":
                return DaojuImpact;
            case "Collected":
                return Collected;
            case "MailBox":
                return MailBox;
            case "Restart":
                return Restart;
        }

        return null;
    }
}