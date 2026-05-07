using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class maskController : MonoBehaviour
{
    [SerializeField] private GameObject settingPanel;   // 蒙版 Panel 对象

    // 显示蒙版
    public void ShowMask()
    {
        settingPanel.SetActive(true);
    }

    // 隐藏蒙版
    public void HideMask()
    {
        settingPanel.SetActive(false); 
    }

}