using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class OverlayFadeEffect : MonoBehaviour
{
    [ReadOnly]
    public bool IsFinished;
    [SerializeField] private float _duration = 1f;

    public void PlayFadeIn(float duration = 1f)
    {
        if (duration != 0f)
            _duration = duration;
        StartCoroutine(nameof(OnFadeIn));
    }

    public void PlayFadeOut(float duration = 1f)
    {
        if (duration != 0f)
            _duration = duration;
        StartCoroutine(nameof(OnFadeOut));
    }

    private IEnumerator OnFadeIn()
    {
        IsFinished = false;
        Image pic = GetComponent<Image>();

        var startTime = Time.time;

        while (pic.color.a > 0f)
        {
            yield return null;
            pic.color = new Color(pic.color.r, pic.color.g, pic.color.b, 1f - (Time.time - startTime) / _duration);

            if (pic.color.a <= 0f)
            {
                pic.color = new Color(pic.color.r, pic.color.g, pic.color.b, 0f);
            }
        }

        IsFinished = true;
    }

    private IEnumerator OnFadeOut()
    {
        IsFinished = false;
        Image pic = GetComponent<Image>();

        var startTime = Time.time;

        while (pic.color.a < 1f)
        {
            yield return null;
            pic.color = new Color(pic.color.r, pic.color.g, pic.color.b, (Time.time - startTime) / _duration);

            if (pic.color.a >= 1f)
            {
                pic.color = new Color(pic.color.r, pic.color.g, pic.color.b, 1f);
            }
        }

        IsFinished = true;
    }
}