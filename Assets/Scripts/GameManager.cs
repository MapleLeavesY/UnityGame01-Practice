using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    [SerializeField] private static int levelNumber = 1;
    [SerializeField] private List<GameLevel> gameLevels;
    
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
                
                Lander.Instance.transform.position = spawnLevelNumber.GetLanderStartPosition();
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
