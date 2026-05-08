using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Spike : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Player player = other.GetComponentInParent<Player>();
        if (player != null)
        {
            RefreshScene();
        }
    }

    private static void RefreshScene()
    {
        GameManager ins = GameManager.instance;
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
