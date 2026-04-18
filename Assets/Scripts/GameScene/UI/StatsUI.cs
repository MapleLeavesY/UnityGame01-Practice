using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class StatsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statsTextMash;
    [SerializeField] private GameObject speedUpArrowGameObject;
    [SerializeField] private GameObject speedDownArrowGameObject;
    [SerializeField] private GameObject speedLeftArrowGameObject;
    [SerializeField] private GameObject speedRightArrowGameObject;
    [SerializeField] private Image FuelBar;
    

    

    private Lander lander;
    
    
    int coinScore = 0;
    private void Start()
    {
        lander = Lander.Instance;

        GameEvent.OnCoinCollected += AddScore;
    }
    private void Update()
    {
        UpdateStatsTextMash();
    }
    private void UpdateStatsTextMash()
    {
        int level = GameManager.Instance.GetLevel();
        int score = coinScore;
        float timer = Mathf.Round(lander.GetTimer());
        float speedX = Mathf.Abs(Mathf.Round(lander.GetSpeedX()));
        float speedY = Mathf.Abs(Mathf.Round(lander.GetSpeedY()));
        float fuelVolume = lander.GetFuel();

        FuelBar.fillAmount = lander.GetFuelAmount();

        speedDownArrowGameObject.SetActive(lander.GetSpeedY() < 0);
        speedUpArrowGameObject.SetActive(lander.GetSpeedY() > 0);
        speedLeftArrowGameObject.SetActive(lander.GetSpeedX() < 0);
        speedRightArrowGameObject.SetActive(lander.GetSpeedX() > 0);

        statsTextMash.text =
            level + "\n" +
            score + "\n" +
            timer + "\n" +
            speedX + "\n" +
            speedY;
    }
    private void AddScore(int score)
    {
        coinScore += score;
    }

    
}
