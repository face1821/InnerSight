using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskController : MonoBehaviour
{
    [SerializeField] private GameObject settingPanel; // 蒙版 Panel 对象
    [SerializeField] private GameObject objectwithShow;
    [SerializeField] private GameObject objectWithHide;

    // 显示蒙版
    public void ShowMask()
    {
        settingPanel.SetActive(true);

        objectwithShow.SetActive(true);
        objectWithHide.SetActive(false);
    }

    // 隐藏蒙版
    public void HideMask()
    {
        settingPanel.SetActive(false);

        objectwithShow.SetActive(false);
        objectWithHide.SetActive(true);
    }
}