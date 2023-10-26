using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject pauseMenu; // asigna el men� de pausa en el inspector
    public GameObject loadingScreen;
    public Slider slider;
    public Animator transition;
    public GameObject playerUI;

    public static bool isPaused;

    float previousTimeScale = 1;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }

    public void LoadLevelAsync(string levelName)
    {
        Debug.Log("Cargando nivel: " + levelName);

        StartCoroutine(LoadAsynchronously(levelName));
    }

    IEnumerator LoadAsynchronously (string levelName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(levelName);

        transition.SetTrigger("Start");

        loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            slider.value = progress;

            yield return null;
        }
    }

    public void QuitGame()
    {
        Debug.Log("Saliendo del juego");
        Application.Quit();
    }

    public void PauseGame()
    {
        Debug.Log("Pausando el juego");
        
        if (Time.timeScale > 0)
        {
            previousTimeScale = Time.timeScale;
            Time.timeScale = 0;
            pauseMenu.SetActive(true);
            playerUI.SetActive(false);

            isPaused = true;
        }
        else if (Time.timeScale == 0)
        {
            Time.timeScale = previousTimeScale;
            pauseMenu.SetActive(false);
            playerUI.SetActive(true);


            isPaused = false;
        }
    }

    public void ResumeGame()
    {
        Debug.Log("Reanudando el juego");
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }
}
