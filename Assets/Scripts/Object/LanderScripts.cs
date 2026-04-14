using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
public class Lander : MonoBehaviour
{
    public static Lander Instance
    {
        private set;
        get;
    }
    public event EventHandler OnUpForce;
    public event EventHandler OnRightForce;
    public event EventHandler OnLeftForce;
    public event EventHandler OnBeforeForce;
    public event EventHandler CoinPickUp;
    public event EventHandler<OnLanderedEventArgs> OnLandered;
    public class OnLanderedEventArgs : EventArgs
    {
        public int finalScore;
    }
    private Rigidbody2D _landerRigidbody2D;
    float fuelVolume = 10f;
    private void Awake()
    {
        Instance = this;
        _landerRigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        OnBeforeForce?.Invoke(this, EventArgs.Empty);
        
        Debug.Log("Fuel: " + fuelVolume);
        if(fuelVolume <= 0f) return;

        if( Keyboard.current.wKey.isPressed||
            Keyboard.current.aKey.isPressed||
            Keyboard.current.dKey.isPressed)
        {
            FuelConsumption();
        }

        if (Keyboard.current.wKey.isPressed)
        {
            float force = 700f;
            _landerRigidbody2D.AddForce(transform.up * (Time.deltaTime * force));
            OnUpForce?.Invoke(this, EventArgs.Empty);
            
        }
        if (Keyboard.current.aKey.isPressed)
        {
            float turnSpeed = +100f;
            _landerRigidbody2D.AddTorque(turnSpeed * Time.deltaTime);
            OnLeftForce?.Invoke(this, EventArgs.Empty);
            
        }
        if (Keyboard.current.dKey.isPressed)
        {
            float turnSpeed = -100f;
            _landerRigidbody2D.AddTorque(turnSpeed * Time.deltaTime);
            OnRightForce?.Invoke(this, EventArgs.Empty);
            
        }
    }



    private void OnCollisionEnter2D(Collision2D collision2D)
    {//降落台碰撞判断
        float minimumAngle = .90f;
        float minimumRelativeSpeed = 3f;
        float angle = Vector2.Dot(Vector2.up, transform.up);
        float angleScore,speedScore;
        if(angle < minimumAngle)
        {//角度降落判断失败
            Debug.Log("Angle exceeds the limit value");
            return;
        }
        if(collision2D.relativeVelocity.magnitude > minimumRelativeSpeed)
        {//速度降落判断失败
            Debug.Log("Speed exceeds the limit value");
            return;
        }
    

        float maxAngleScore = 100f;
        float maxVelocityScore = 100f;
        float multiplicationFactorAngle = 100f;
        float multiplicationFactorSpeed = 10f;
        
        angleScore = maxAngleScore - Mathf.Abs(angle - 1f) * multiplicationFactorAngle;
        speedScore = maxVelocityScore - (collision2D.relativeVelocity.magnitude) * multiplicationFactorSpeed;
        Debug.Log("AngleScore: " + angleScore);
        Debug.Log("SpeedScore: " + speedScore);    
        LandingPad landingPad;
        if(!collision2D.gameObject.TryGetComponent(out landingPad)) return;
        int finalScore = Mathf.RoundToInt((angleScore + speedScore) * landingPad.GetScore());
        Debug.Log("FinalScore: " + finalScore);
        OnLandered?.Invoke(this, new OnLanderedEventArgs {
            finalScore = finalScore,
        });
    }
    
    private void OnTriggerEnter2D(Collider2D collision2D)
    {
        if(collision2D.gameObject.TryGetComponent(out Fuel fuel))
        {
            float fuelRechargingFactor = 10f;
            fuelVolume += fuelRechargingFactor; 
            fuel.DeletionOfFuelObjects();
        }
        if(collision2D.gameObject.TryGetComponent(out Coin coin))
        {
            CoinPickUp?.Invoke(this, EventArgs.Empty);
            coin.DeletionOfCoinObjects();
        }
    }
    private void FuelConsumption()
    {
        float consumptionFactor = 1f;
        fuelVolume -= consumptionFactor * Time.deltaTime;
    }

    public float GetSpeedX()
    {
        return Mathf.Abs(_landerRigidbody2D.velocity.x);
    }
    public float GetSpeedY()
    {
        return Mathf.Abs(_landerRigidbody2D.velocity.y);
    }
    public float GetFuel()
    {
        return fuelVolume;
    }
}
