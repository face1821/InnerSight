

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenuUIFun : MonoBehaviour
{
    public void ReturnToMainMenu()
    {
        StartCoroutine(nameof(OnReturnToMainMenu));
    }

    private IEnumerator OnReturnToMainMenu()
    {
        GameObject.FindWithTag("SceneOverlay").GetComponent<OverlayFadeEffect>().PlayFadeOut();
        
        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene("MainMenu");
    }
}