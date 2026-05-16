using System;
using UnityEngine;
using UnityEngine.UI;

public class SliderValueInitSync : MonoBehaviour
{
    public string ValueName;
    public float DefaultValue;

    private void Start() { GetComponent<Slider>().value = PlayerPrefs.GetFloat(ValueName, DefaultValue); }
}