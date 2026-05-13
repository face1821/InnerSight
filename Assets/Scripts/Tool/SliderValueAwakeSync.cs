using System;
using UnityEngine;
using UnityEngine.UI;

public class SliderValueAwakeSync : MonoBehaviour
{
    public string ValueName;
    public float DefaultValue;

    private void Awake() { GetComponent<Slider>().value = PlayerPrefs.GetFloat(ValueName, DefaultValue); }
}