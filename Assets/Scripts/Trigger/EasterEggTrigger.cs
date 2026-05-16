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

    public void ChangeText() { Text.text = Texts[_index++]; }
}