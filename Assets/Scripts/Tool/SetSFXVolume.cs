using UnityEngine;
using UnityEngine.UI;

public class SetSFXVolume : MonoBehaviour
{
    public void SetVolume(Slider slider) { SoundManager.Instance.SetSFXVolume(slider.value); }
}