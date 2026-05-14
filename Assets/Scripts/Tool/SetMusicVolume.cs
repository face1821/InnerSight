using UnityEngine;
using UnityEngine.UI;

public class SetMusicVolume : MonoBehaviour
{
    public void SetVolume(Slider slider) { SoundManager.Instance.SetMusicVolume(slider.value); }
}