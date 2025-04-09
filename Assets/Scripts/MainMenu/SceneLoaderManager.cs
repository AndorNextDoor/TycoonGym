using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System;

public class SceneLoaderManager : MonoBehaviour
{
    public static SceneLoaderManager Instance;

    [Header("UI References")]
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Slider progressBar;
    [SerializeField] private String MAIN_SCENE_NAME;

    public event Action OnGameLoaded;


    private void Awake()
    {
        // Singleton setup
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // persist across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        loadingScreen.SetActive(true);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        while (operation.progress < 0.9f)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            if (progressBar != null)
                progressBar.value = progress;

            yield return null;
        }

        // Optionally add a wait for a nicer UX
        yield return new WaitForSeconds(0.5f);

        if (progressBar != null)
            progressBar.value = 1f;

        operation.allowSceneActivation = true;

        yield return null;
        loadingScreen.SetActive(false);
        OnGameLoaded?.Invoke();
    }

    public void LoadMainScene()
    {
        StartCoroutine(LoadSceneAsync(MAIN_SCENE_NAME));
    }
}
