using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    [SerializeField] GameObject gameOverCanvas;
    [SerializeField] float reloadLevelDelay = 2f;

    [SerializeField] float loadNextLevelDelay = 2f;
    
    public void ReloadLevel()
    {
        StartCoroutine(ReloadThisLevel());
    }

    IEnumerator ReloadThisLevel()
    {
        gameOverCanvas.SetActive(true);
        yield return new WaitForSeconds(reloadLevelDelay);

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel());
    }
    
    IEnumerator LoadLevel()
    {
        yield return new WaitForSeconds(loadNextLevelDelay);

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }


}
