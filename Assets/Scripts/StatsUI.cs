using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statsTextMash;
    int score = Coin.Instance.GetScore();
    float timer = Coin.Instance.GetTimer();

    float speedX = Lander.Instance.GetSpeedX();
    float speedY = Lander.Instance.GetSpeedY();
    float fuelVolume = Lander.Instance.GetFuel();

    private void Update()
    {
        UpdateStatsTextMash();
    }
    private void UpdateStatsTextMash()
    {
        statsTextMash.text = 
            score + "\n" +
            timer + "\n" +
            speedX + "\n" +
            speedY + "\n" +
            fuelVolume;
    }

}
