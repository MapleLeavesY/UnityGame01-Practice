using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public static class GameEvent
{
    public static event Action<int> OnCoinCollected;
    public static void CoinCollected(int score)
    {
        OnCoinCollected?.Invoke(score);
    }
}

public class Coin : MonoBehaviour
{
    public class CoinScoreCounting : EventArgs
    {
        public float score;
    }
    private int coinScore;
    
    private void Start()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if(collider2D.gameObject.TryGetComponent(out Lander lander))
        {//碰到飞船
            int ScoreFactor = 500; 
            GameEvent.CoinCollected(ScoreFactor);
            DeletionOfCoinObjects();
        }
    }

    public void DeletionOfCoinObjects()
    {
        Destroy(this.gameObject);
    }

    
}
