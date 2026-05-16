using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskController : MonoBehaviour
{
    [SerializeField] private GameObject settingPanel; // 蒙版 Panel 对象
    [SerializeField] private List<GameObject> _hideObjects;

    // 显示蒙版
    public void ShowMask()
    {
        settingPanel.SetActive(true);

        foreach (GameObject obj in _hideObjects)
        {
            obj.SetActive(false);
        }
    }

    // 隐藏蒙版
    public void HideMask()
    {
        settingPanel.SetActive(false);

        foreach (GameObject obj in _hideObjects)
        {
            obj.SetActive(true);
        }
    }
}