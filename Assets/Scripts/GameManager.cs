using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    [SerializeField] private static int levelNumber = 1;
    [SerializeField] private List<GameLevel> gameLevels;

    public event Action<CameraSize> OnLevelLoaded;
    
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
    }
    private void LoadCurrentLevel()
    {
        foreach(GameLevel gameLevel in gameLevels)
        {
            if(gameLevel.GetLevelNumber() == levelNumber)
            {
                GameLevel spawnLevelNumber = Instantiate(gameLevel, Vector3.zero, Quaternion.identity);
                Lander.Instance.transform.position = spawnLevelNumber.GetLanderStartPosition();//场景创建

                CameraSize size = spawnLevelNumber.GetComponentInChildren<CameraSize>();//场景获取传入参数
                if (size == null)
                    Debug.LogError("CameraSize component NOT found on level prefab!");
                OnLevelLoaded?.Invoke(size);
            }
        }
    }


    public void RetryLevel()
    {

        SceneManager.LoadScene(0);
    }
    public void GotoNextLevel()
    {
        levelNumber++;
        SceneManager.LoadScene(0);
    }
    public int GetLevel()
    {
        return levelNumber;
    }
}
