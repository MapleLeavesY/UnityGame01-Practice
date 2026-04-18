using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{

    [SerializeField] private Button gameOverButton;
    [SerializeField] private TextMeshProUGUI scoreTextMesh;
    private void Awake()
    {
        gameOverButton.onClick.AddListener(() =>
        {
            SceneLoader.LoadScene(SceneLoader.SceneName.MainMenuScene);
        });
    }
    private void Start()
    {
        scoreTextMesh.text = ScoreSave.Instance.GetTotalScore().ToString();

        GameManager.InitGameLevel();
        ScoreSave.InitTotalScore();
    }
}
