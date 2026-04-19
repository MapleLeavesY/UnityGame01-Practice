using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    private const int LEVELNUMBERINIT = 1;
    [SerializeField] private static int levelNumber = LEVELNUMBERINIT;
    [SerializeField] private List<GameLevel> gameLevels;

    public event Action<CameraSize> OnLevelLoaded;
    public event Action OnGamePaused;
    public event Action OnGameUnPaused;
    public static GameManager Instance
    {
        private set;
        get;
    }
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        LoadCurrentLevel();
        GameInput.Instance.OnMenuButtonPressed += GameInput_OnMenuButtonPressed;
    }
    
    private void LoadCurrentLevel()
    {

        GameLevel gameLevel = GetGameLevel();

        GameLevel spawnLevelNumber = Instantiate(gameLevel, Vector3.zero, Quaternion.identity);
        Lander.Instance.transform.position = spawnLevelNumber.GetLanderStartPosition();//场景创建
    
        CameraSize size = spawnLevelNumber.GetComponentInChildren<CameraSize>();//场景获取传入参数
        if (size == null)
            Debug.LogError("CameraSize component NOT found on level prefab!");
        OnLevelLoaded?.Invoke(size);
    }

    private GameLevel GetGameLevel()
    {
        foreach(GameLevel gameLevel in gameLevels)
        {
            if(gameLevel.GetLevelNumber() == levelNumber)
            {
                return gameLevel;
            }
        }
        return null;
    }

    private void GameInput_OnMenuButtonPressed()
    {
        PauseUnPauseGame();
    }
    public void PauseUnPauseGame()
    {
        if(Time.timeScale == 1f)//时间规定为零 
            PauseGame();
    }
    public void RetryLevel()
    {

        SceneLoader.LoadScene(SceneLoader.SceneName.GameScene);
    }
    public void GotoNextLevel()
    {
        levelNumber++;
        SceneLoader.LoadScene(SceneLoader.SceneName.GameScene);

        if(GetGameLevel() == null)
        {
            SceneLoader.LoadScene(SceneLoader.SceneName.GameOverScene);
        }
        else
        {
            SceneLoader.LoadScene(SceneLoader.SceneName.GameScene);
        }
    }
    public int GetLevel()
    {
        return levelNumber;
    }
    public void PauseGame()
    {
        Time.timeScale = 0f;
        Debug.Log("PauseGame 调用了");
        OnGamePaused?.Invoke();
    }
    public void UnPauseGame()
    {
        Time.timeScale = 1f;
        Debug.Log("PauseUI 收到暂停事件");
        OnGameUnPaused?.Invoke();
    }
    public static void InitGameLevel()
    {
        levelNumber = LEVELNUMBERINIT;
    }
    
}
