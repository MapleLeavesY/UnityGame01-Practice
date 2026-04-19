using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
public class Lander : MonoBehaviour
{
    private const float GRAVITY_NORMAL = 0.7f;
    private const float GRAVITY_WAITINGTOSTART = 0f;
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
    public event Action FuelPickUp;
    public event EventHandler EmergencyLanding;
    public event Action OnLandingType;
    
    public enum State
    {
        WaitingToStart,
        Normal,
        GameOver,
    }
    private Rigidbody2D _landerRigidbody2D;
    private State state;
    float fuelVolume = 10f;
    float fuelAmountMax;
    private float timer = 0;
    private void Awake()
    {
        Instance = this;
        fuelAmountMax = fuelVolume;
        state = State.WaitingToStart;        
        _landerRigidbody2D = GetComponent<Rigidbody2D>();
        _landerRigidbody2D.gravityScale = GRAVITY_WAITINGTOSTART;
    }
    private void Start()
    {
        LandingPad[] pads = FindObjectsOfType<LandingPad>();
        foreach(var pad in pads)
        {
            pad.GameState += AlterTheState;
        }
    }
    private void AlterTheState(object sender, System.EventArgs e)
    {
        state = State.GameOver;
    }
    private void Update()
    {
        if(state == State.Normal)
            timer += Time.deltaTime;
    }
    private void FixedUpdate()
    {
        OnBeforeForce?.Invoke(this, EventArgs.Empty);
        
        
        switch(state)
        {
            default:
            case State.WaitingToStart:
            
            if( GameInput.Instance.IsUpactionPressed()||
                GameInput.Instance.IsRightactionPressed()||
                GameInput.Instance.IsLeftactionPressed() || 
                (GameInput.Instance.GetMovementInputVector2() != Vector2.zero))
            {
                _landerRigidbody2D.gravityScale = GRAVITY_NORMAL;
                state = State.Normal;
            }

            break;
            case State.Normal:
            if(fuelVolume <= 0f) return;
            if( GameInput.Instance.IsUpactionPressed()   ||
                GameInput.Instance.IsRightactionPressed()||
                GameInput.Instance.IsLeftactionPressed()|| 
                (GameInput.Instance.GetMovementInputVector2() != Vector2.zero))
            {
                FuelConsumption();
            }
            float gamepadDeadzone = .2f;
            if (GameInput.Instance.IsUpactionPressed() || 
               (GameInput.Instance.GetMovementInputVector2().y > gamepadDeadzone))
            {
                float force = 700f;
                _landerRigidbody2D.AddForce(transform.up * (Time.deltaTime * force));
                OnUpForce?.Invoke(this, EventArgs.Empty);
                
            }
            if (GameInput.Instance.IsLeftactionPressed() || 
               (GameInput.Instance.GetMovementInputVector2().x < (-gamepadDeadzone)))
            {
                float turnSpeed = +100f;
                _landerRigidbody2D.AddTorque(turnSpeed * Time.deltaTime);
                OnLeftForce?.Invoke(this, EventArgs.Empty);
                
            }
            if (GameInput.Instance.IsRightactionPressed() || 
               (GameInput.Instance.GetMovementInputVector2().x > gamepadDeadzone))
            {
                float turnSpeed = -100f;
                _landerRigidbody2D.AddTorque(turnSpeed * Time.deltaTime);
                OnRightForce?.Invoke(this, EventArgs.Empty);
                
            }
            break;
            case State.GameOver:
            break;
        }


    }
    private void OnCollisionEnter2D(Collision2D collision2D)
    {//迫降地面，游戏完全失败
        if(!collision2D.gameObject.TryGetComponent(out LandingPad landingPad))
        {
            EmergencyLanding?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            OnLandingType?.Invoke();
        }
        state = State.GameOver;
    }

    private void OnTriggerEnter2D(Collider2D collision2D)
    {
        if(collision2D.gameObject.TryGetComponent(out Fuel fuel))
        {
            float fuelRechargingFactor = 10f;
            FuelPickUp?.Invoke();

            fuelVolume += fuelRechargingFactor; 
            if(fuelVolume > fuelAmountMax)
            {
                fuelVolume = fuelAmountMax;
            }
            
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
        return _landerRigidbody2D.velocity.x;
    }
    public float GetSpeedY()
    {
        return _landerRigidbody2D.velocity.y;
    }
    public float GetFuelAmount()
    {
        return fuelVolume / fuelAmountMax;
    }
    public float GetFuel()
    {
        return fuelVolume;
    }
    public float GetTimer()
    {
        return timer;
    }
}
