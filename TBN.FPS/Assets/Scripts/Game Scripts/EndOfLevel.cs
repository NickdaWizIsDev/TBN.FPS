using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndOfLevel : MonoBehaviour
{
    public GameObject loadingScreen;
    public Slider slider;
    public Animator transition;

    public void LoadLevelAsync(string levelName)
    {
        Debug.Log("Cargando nivel: " + levelName);

        StartCoroutine(LoadAsynchronously(levelName));
    }


    IEnumerator LoadAsynchronously(string levelName)
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

    private void OnTriggerEnter(Collider other)
    {
            LoadLevelAsync("Level 2");
    }
}
