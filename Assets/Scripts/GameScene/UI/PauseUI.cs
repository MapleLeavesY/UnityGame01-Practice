using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Profiling;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button mainMenuButton;
    
    [SerializeField] private Button backGroundVolumeButton;
    [SerializeField] private Button gameVolumeButton;

    [SerializeField] private TextMeshProUGUI soundVolumeTextMesh;
    [SerializeField] private TextMeshProUGUI musicVolumeTextMesh;


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
        backGroundVolumeButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.ChangeSoundVolume();
            soundVolumeTextMesh.text = "SOUND: " + SoundManager.Instance.GetSoundVolume();
        });
        gameVolumeButton.onClick.AddListener(() =>
        {
            MusicManager.Instance.ChangeMusicVolume();
            musicVolumeTextMesh.text = "MUSIC: " + MusicManager.Instance.GetMusicVolume();
        });

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
        soundVolumeTextMesh.text = "SOUND: " + SoundManager.Instance.GetSoundVolume();
        musicVolumeTextMesh.text = "MUSIC: " + MusicManager.Instance.GetMusicVolume();
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
