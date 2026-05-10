using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Spike : MonoBehaviour
{
    [SerializeField] private PauseMenu pm;

    void OnTriggerEnter2D(Collider2D other)
    {
        Player player = other.GetComponentInParent<Player>();
        if (player != null)
        {
            RefreshScene();
        }
    }

    private void RefreshScene()
    {
        GameManager ins = GameManager.instance;
        pm.RestartCurrentLevel();
        
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
