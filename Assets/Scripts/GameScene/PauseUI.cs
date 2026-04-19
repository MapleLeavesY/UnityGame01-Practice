using System.Collections;
using System.Collections.Generic;
using Unity.Profiling;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button mainMenuButton;

    private GameManager gameManager;
    private CanvasGroup canvasGroup;
    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if(canvasGroup == null)
        {
            Debug.LogError("CanvasGroup missing!");
            return;
        }

        Hide();

        resumeButton.onClick.AddListener(() =>
        {
            GameManager.Instance.UnPauseGame();
        });
        mainMenuButton.onClick.AddListener(() =>
        {
            SceneLoader.LoadScene(SceneLoader.SceneName.MainMenuScene);
            
            GameManager.InitGameLevel();
            ScoreSave.InitTotalScore();
        });
    }
    private void OnEnable()
    {
        Debug.Log("PauseUI OnEnable 被调用");
        StartCoroutine(Rebind());
        
    }
    private void Start()
    {
        StartCoroutine(Rebind());

        resumeButton.Select();
    }
    private IEnumerator Rebind()
    {
        while (GameManager.Instance == null)
        {
            yield return null;
        }

        // 先解绑旧的（防止残留）
        if (gameManager != null)
        {
            gameManager.OnGamePaused -= GameManager_OnGamePaused;
            gameManager.OnGameUnPaused -= GameManager_OnGameUnPaused;
        }
        gameManager = GameManager.Instance;

        if (gameManager != null)
        {
            gameManager.OnGamePaused += GameManager_OnGamePaused;
            gameManager.OnGameUnPaused += GameManager_OnGameUnPaused;
        }
        else
        {
            Debug.LogWarning("GameManager.Instance is null!");
        }
    }
    private void OnDestroy()
    {
        if(gameManager != null)
        {
            gameManager.OnGamePaused -= GameManager_OnGamePaused;
            gameManager.OnGameUnPaused -= GameManager_OnGameUnPaused;
        }
    }
    private void GameManager_OnGamePaused()
    {
        Show();
    }
    private void GameManager_OnGameUnPaused()
    {
        Hide();
    }
    private void Show()
    {
        if(canvasGroup == null) return;

        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
    }
    private void Hide()
    {
        if (canvasGroup == null) return;

        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
    }

    private void Update()
{
    if (Time.timeScale == 0f && canvasGroup.alpha == 0)
    {
        Debug.Log("兜底触发 Show()");
        Show();
    }
}
}
