using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button exitButton;

    private void Awake()
    {
        playButton.onClick.AddListener(() =>
        {//游戏开启按钮lanbda表达式
            SceneLoader.LoadScene(SceneLoader.SceneName.GameScene);
        });
        exitButton.onClick.AddListener(() =>
        {//游戏退出按钮lanbda表达式
            
        });
    }
}
