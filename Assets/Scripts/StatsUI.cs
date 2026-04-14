using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statsTextMash;
    private Lander lander;
    private Coin coin;
    private void Start()
    {
        lander = Lander.Instance;
        coin = Coin.Instance;
    }
        
    

    private void Update()
    {
        UpdateStatsTextMash();
    }
    private void UpdateStatsTextMash()
    {
        int score = coin.GetScore();
        float timer = Mathf.Round(lander.GetTimer());
        float speedX = Mathf.Round(lander.GetSpeedX());
        float speedY = Mathf.Round(lander.GetSpeedY());
        float fuelVolume = lander.GetFuel();
        statsTextMash.text = 
            score + "\n" +
            timer + "\n" +
            speedX + "\n" +
            speedY + "\n" +
            $"{fuelVolume:F2}";
    }
}
