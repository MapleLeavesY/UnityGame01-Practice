using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class LandedUI : MonoBehaviour
{
    public static LandedUI Instance
    {
        private set;
        get;
    }

    [SerializeField] private Lander lander;
    [SerializeField] private TextMeshProUGUI titleTextMesh;
    [SerializeField] private TextMeshProUGUI statsTextMesh;
    [SerializeField] private GameObject landerExplosionVfx;
    [SerializeField] private TextMeshProUGUI nextButtonTextMesh;
    [SerializeField] private Button NextButton;
    private float totalScore;
    private Action nextButtonClickAction;
    private Action GameInputClear;
    private void Awake()
    {
        Instance = this;
        NextButton.onClick.AddListener(() =>
        {
            nextButtonClickAction();
        });
    }
    private void Start()
    {
        LandingPad[] pads = FindObjectsOfType<LandingPad>();
        foreach (var pad in pads)
        {
            pad.LandedUIPick += Lander_OnLanded;
        }
        NextButton.Select();
        Hide(); 
    }
    
    private void Lander_OnLanded(object sender, LandingPad.SuccessfulUI e)
    {
        GameInput.Instance.GameInputClear();//输入组件清除

        if(e.landingType == LandingPad.LandingType.Success)
        {
            titleTextMesh.text = "SUCCESSFUL LANDING!";
            nextButtonTextMesh.text = "CONTINUE";
            nextButtonClickAction = GameManager.Instance.GotoNextLevel;

            totalScore = Mathf.Round(e.coinScore + e.otherscore);
            ScoreSave.Instance.AddScore(totalScore);
        }
        else
        {
            titleTextMesh.text = "<color = #ff0000>CRASH!</color>";
            nextButtonTextMesh.text = "RESTART";
            Instantiate(landerExplosionVfx, lander.transform.position, Quaternion.identity);
            lander.gameObject.SetActive(false);
            nextButtonClickAction = GameManager.Instance.RetryLevel;
        }

        statsTextMesh.text = 
        Mathf.Round(e.velocity) + "\n" + 
        Mathf.Round(e.dotvector) + "\n" +
        Mathf.Round(e.scoreMultiplier) + "\n" +
        Mathf.Round(e.coinScore + e.otherscore);
        Show();
    }

    
    private void Show()
    {
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
    public float GetTotalScore()
    {
        return totalScore;
    }
}
