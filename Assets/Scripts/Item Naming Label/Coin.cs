using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem.iOS;



public class Coin : MonoBehaviour
{

    public static Coin Instance
    {
        private set;
        get;
    }
    private int score;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        Lander.Instance.CoinPickUp += Lander_CoinPickUp;
    }
    

    private void Lander_CoinPickUp(object sender, System.EventArgs e)
    {
        CoinAdd(500);
    }

    public void CoinAdd(int coinFactor)
    {
        score += coinFactor;
        Debug.Log(score);
    }

     public void DeletionOfCoinObjects()
    {
        Destroy(gameObject);
    }

    public int GetScore()
    {
        return score;
    }
}
