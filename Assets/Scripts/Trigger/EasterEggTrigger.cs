using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EasterEggTrigger : MonoBehaviour
{
    public GameObject ShowPanel;
    public TextMeshProUGUI Text;

    [TextArea]
    public List<string> Texts;
    private int _index;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("MainPlayer"))
        {
            Debug.LogWarning("彩蛋");

            ShowPanel.SetActive(true);
        }
    }

    public void ChangeText()
    {
        if (_index >= Texts.Count)
        {
            ShowPanel.SetActive(false);
            _index = 0;
            Text.text = "“恭喜你人类！发现了这个彩蛋！这个彩蛋没有任何提示，任何奖励，任何有用信息。但我相信你看得出来的，这样小小的空间之中居然装满了来自创作人们真挚的心^_^，而当你发现了这处空间，当你阅读这些文字，当你咀嚼它们的用意之时，我们的决心便已经不再局限于这方天地，我们的理想便不再局限于即见所得的现实”\n——铁奎";
        }
        
        Text.text = Texts[_index++];
    }
}