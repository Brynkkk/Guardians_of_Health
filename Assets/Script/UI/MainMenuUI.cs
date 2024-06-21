using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private string sceneName = "GameScene";
    [SerializeField] private GameObject continueButton;
    [SerializeField] UiFade uiFade;
    
    public void Continue()
    {
        StartCoroutine(LoadSceneWithFade(1.5f));
    }

    public void NewGame()
    {
        // SaveManager.instance.DeleteSavedData();
        StartCoroutine(LoadSceneWithFade(1.5f));
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    IEnumerator LoadSceneWithFade(float _delay)
    {
        uiFade.FadeOut();

        yield return new WaitForSeconds(_delay);

        AudioManager.instance.StopAllBGM();

        SceneManager.LoadScene(sceneName);
    }
}
