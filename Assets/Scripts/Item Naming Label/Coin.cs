using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.iOS;



public class Coin : MonoBehaviour
{
    public static Coin Instance
    {
        private set;
        get;
    }
    private void Awake()
    {
        Instance = this;
    }


    public event EventHandler<CoinScoreCounting> CoinPickUp;
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
            float ScoreFactor = 500f; 
            CoinPickUp?.Invoke(this, new CoinScoreCounting
            {
              score = ScoreFactor,  
            });

            DeletionOfCoinObjects();
        }
    }

    public void DeletionOfCoinObjects()
    {
        Destroy(this.gameObject);
    }

    
}
