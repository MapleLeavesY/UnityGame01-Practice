using System;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class LandingPad : MonoBehaviour
{
    [SerializeField] private int scoreMultiplier;
    public enum LandingType
    {
        Success,
        WrongLandingArea,
        TooSteepAngle,
        TooFastLanding,
    }
    public event EventHandler<SuccessfulUI> LandedUIPick;
    public class SuccessfulUI : EventArgs
    {
        public LandingType landingType;
        public float coinScore;
        public float otherscore;
        public float velocity;
        public float dotvector;
        public int scoreMultiplier;
        
    } 
    private Coin coin;
    float coinScore = 0;
    public static LandingPad Instance
    {
        private set;
        get; 
    }


    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        coin = Coin.Instance;
        coin.CoinPickUp += GetCoinScore;
    }



    public void GetCoinScore(object sender, Coin.CoinScoreCounting e)
    {
        AddScore(e.score);
    }



    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        
        float finalScore = 0;
        float score = 0;
        float minimumDot = .90f;
        float minimumRelativeSpeed = 3f;
        float Dot = Vector2.Dot(Vector2.up, collision2D.gameObject.transform.up);
        if(collision2D.gameObject.TryGetComponent(out Lander lander))
        {//判断撞击者是否为Lander

            if(Dot < minimumDot)
            {//方向点积过偏，降落失败
                Debug.Log("Angle exceeds the limit value");
                LandedUIPick?.Invoke(this, new SuccessfulUI
             {
                landingType = LandingType.TooSteepAngle,
                coinScore = 0f,
                otherscore = 0f,
                velocity = collision2D.relativeVelocity.magnitude,
                dotvector = Dot,
                scoreMultiplier = scoreMultiplier,
             });

                return;
            }
            if(collision2D.relativeVelocity.magnitude > minimumRelativeSpeed)
            {//速度过大，降落失败
                Debug.Log("Velocity exceeds the limit value");

                LandedUIPick?.Invoke(this, new SuccessfulUI
             {
                landingType = LandingType.TooFastLanding,
                coinScore = 0f,
                otherscore = 0f,
                velocity = collision2D.relativeVelocity.magnitude,
                dotvector = Dot,
                scoreMultiplier = scoreMultiplier,
             });
                return;
            }

            float speedScoreFactor = 10f;
            float angleScoreFactor = 100f;
            float speedScore = Mathf.Abs(100 - (collision2D.relativeVelocity.magnitude * speedScoreFactor));
            float angleScore = Dot * angleScoreFactor;
            score = (speedScore + angleScore) * scoreMultiplier;
            Debug.Log("Score: " + score);
            
            finalScore = score + coinScore;
             Debug.Log("finalScore: " + finalScore);

             //撞击之后，着陆之后，计算成功条件事件
             LandedUIPick?.Invoke(this, new SuccessfulUI
             {
                landingType = LandingType.Success,
                coinScore = coinScore,
                otherscore = score,
                velocity = collision2D.relativeVelocity.magnitude,
                dotvector = Dot,
                scoreMultiplier = scoreMultiplier,
             });
             Debug.Log("LandedUIPick?.Invoke(this, new SuccessfulUI");

        }   

        

    }
    private void AddScore(float score)
    {
        coinScore += score;
    }
}