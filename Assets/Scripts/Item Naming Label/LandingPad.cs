using System;
using UnityEngine;

public class LandingPad : MonoBehaviour
{
    [SerializeField] private int scoreMultiplier;
    public event EventHandler<OnLanderEventArgs> OnLandered;
    public class OnLanderEventArgs : EventArgs
    {
        public float score;
    }
    public static LandingPad Instance
    {
        private set;
        get; 
    }
    private void Awake()
    {
        Instance = this;
    }
    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        float score = 0;
        float minimumDot = .90f;
        float minimumRelativeSpeed = 3f;
        float Dot = Vector2.Dot(Vector2.up, collision2D.gameObject.transform.up);
        if(collision2D.gameObject.TryGetComponent(out Lander lander))
        {//判断撞击者是否为Lander

            if(Dot < minimumDot)
            {//方向点积过偏，降落失败
                Debug.Log("Angle exceeds the limit value");
                return;
            }
            if(collision2D.relativeVelocity.magnitude > minimumRelativeSpeed)
            {//速度过大，降落失败
                Debug.Log("Velocity exceeds the limit value");
                return;
            }

            float speedScoreFactor = 10f;
            float angleScoreFactor = 100f;
            float speedScore = Mathf.Abs(100 - (collision2D.relativeVelocity.magnitude * speedScoreFactor));
            float angleScore = Dot * angleScoreFactor;
            score = (speedScore + angleScore) * scoreMultiplier;
            Debug.Log("Score: " + score);
            OnLandered?.Invoke(this, new OnLanderEventArgs
            {
                score = score
            });
        }

    }
}
