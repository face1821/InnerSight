using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonAduio : MonoBehaviour, IPointerClickHandler
{
    private enum Option
    {
        Regular,
        Start,
        Return
    }

    [SerializeField] private Option ButtonType;

    public void PlayButtonClick() { SoundManager.instance.Play(10, "ButtonClick"); }
    public void PlayButtonStartClick() { SoundManager.instance.Play(11, "ButtonStartClick"); }
    public void PlayButtonReturnClick() { SoundManager.instance.Play(12, "ButtonReturnClick"); }

    public void OnPointerClick(PointerEventData eventData)
    {
        switch (ButtonType)
        {
            case Option.Regular:
                PlayButtonClick();
                break;
            case Option.Start:
                PlayButtonStartClick();
                break;
            case Option.Return:
                PlayButtonReturnClick();
                break;
        }
    }
}