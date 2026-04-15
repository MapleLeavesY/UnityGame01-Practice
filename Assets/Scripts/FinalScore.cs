using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalScore : MonoBehaviour
{
    Coin coin;
    private void Awake()
    {
        coin = Coin.Instance;
    }
    private void Start()
    {
        LandingPad.Instance.OnLandered += Lander_OnLandered;
    }
    private void Lander_OnLandered(object sender, LandingPad.OnLanderEventArgs e)
    {
        
        coin.CoinAdd(Mathf.RoundToInt(e.score));
        Debug.Log("ccc: " + Mathf.RoundToInt(e.score));
    }
    
}
