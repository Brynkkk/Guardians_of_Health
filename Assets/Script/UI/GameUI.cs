using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField] private string sceneName = "MainMenu";
    [SerializeField] private UiFade uiFade;
    [SerializeField] private Image gameOverScreen; // Reference to the Game Over UI Image

    public void Respawn()
    {
        StartCoroutine(LoadSceneWithFade(1.5f));
    }

    public void MainMenu()
    {
        StartCoroutine(LoadSceneWithFade(1.5f));
    }

    IEnumerator LoadSceneWithFade(float _delay)
    {
        uiFade.FadeOut();
        yield return new WaitForSeconds(_delay);
        SceneManager.LoadScene(sceneName);
    }

    public void ShowGameOverScreen()
    {
        gameOverScreen.gameObject.SetActive(true);
    }
}
