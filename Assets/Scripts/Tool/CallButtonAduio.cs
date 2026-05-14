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

    public void PlayButtonClick() { SoundManager.Instance.Play(10, "ButtonClick"); }
    public void PlayButtonStartClick() { SoundManager.Instance.Play(11, "ButtonStartClick"); }
    public void PlayButtonReturnClick() { SoundManager.Instance.Play(12, "ButtonReturnClick"); }

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