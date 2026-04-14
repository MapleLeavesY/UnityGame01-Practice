using UnityEngine;

public class LandingPad : MonoBehaviour
{
    [SerializeField] private int scoreMultiplier;
    public float GetScore() 
    {
        return scoreMultiplier;
    }
}
