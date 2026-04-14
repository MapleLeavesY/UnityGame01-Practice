using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.iOS;



public class Coin : MonoBehaviour
{
    private int score;

    private void Start()
    {
        Lander.Instance.CoinPickUp += Lander_CoinPickUp;
        Lander.Instance.OnLandered += Lander_OnLanded;
    }

    private void Lander_OnLanded(object sender, Lander.OnLanderedEventArgs e)
    {
        CoinAdd(e.finalScore);
    }
    private void Lander_CoinPickUp(object sender, System.EventArgs e)
    {
        CoinAdd(500);
    }

    private void CoinAdd(int coinFactor)
    {
        score += coinFactor;
        Debug.Log(score);
    }

     public void DeletionOfCoinObjects()
    {
        Destroy(gameObject);
    }
}
