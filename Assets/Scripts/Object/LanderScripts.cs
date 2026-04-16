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
    
    
    private Rigidbody2D _landerRigidbody2D;
    float fuelVolume = 10f;
    float fuelAmountMax;
    private float timer = 0;
    private void Awake()
    {
        Instance = this;
        fuelAmountMax = fuelVolume;

        _landerRigidbody2D = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        timer += Time.deltaTime;
    }
    private void FixedUpdate()
    {
        OnBeforeForce?.Invoke(this, EventArgs.Empty);
        
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


    private void OnTriggerEnter2D(Collider2D collision2D)
    {
        if(collision2D.gameObject.TryGetComponent(out Fuel fuel))
        {
            float fuelRechargingFactor = 10f;
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
