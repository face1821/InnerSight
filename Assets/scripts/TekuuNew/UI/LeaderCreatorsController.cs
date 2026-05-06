using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderCreatorsController : MonoBehaviour
{
    [SerializeField] private GameObject LeaderCreatorsPanel;   // 蒙版 Panel 对象

    // 显示蒙版
    public void ShowMask()
    {
        LeaderCreatorsPanel.SetActive(true);
    }

    // 隐藏蒙版
    public void HideMask()
    {
        LeaderCreatorsPanel.SetActive(false);
    }
}
