using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingBarManager : MonoBehaviour
{
    public string loadScene;
    AsyncOperation loadingOperation;
    [SerializeField] private Image loadingBar;
    [SerializeField] private Text percentage;

    private void Awake() => loadScene = PlayerPrefs.GetString("SceneName");
    private void Start() => loadingOperation = SceneManager.LoadSceneAsync(loadScene);

    private void Update()
    {
        LoadingBar();
    }

    private void LoadingBar()
    {
        float loadingBarProgress = Mathf.Clamp01(loadingOperation.progress / 0.9f);
        loadingBar.fillAmount = loadingBarProgress;
        percentage.text = Mathf.Round(loadingBarProgress * 100) + "%";

        Debug.LogError("Loading");
    }
}
