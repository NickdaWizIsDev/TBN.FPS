using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndOfGame : MonoBehaviour
{
    public GameObject loadingScreen;
    public Slider slider;
    public Animator transition;

    public void LoadLevelAsync(string sceneIndex)
    {
        Debug.Log("Cargando nivel: " + sceneIndex);

        StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    void Start()
    {
        transition.SetTrigger("End");
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
            LoadLevelAsync("GameEnd");
    }
}
