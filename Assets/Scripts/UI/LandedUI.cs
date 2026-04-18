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
using UnityEngine.UI;

public class LandedUI : MonoBehaviour
{
    [SerializeField] private Lander lander;
    [SerializeField] private TextMeshProUGUI titleTextMesh;
    [SerializeField] private TextMeshProUGUI statsTextMesh;
    [SerializeField] private GameObject landerExplosionVfx;
    [SerializeField] private TextMeshProUGUI nextButtonTextMesh;
    [SerializeField] private Button NextButton;


    private Action nextButtonClickAction;
    private Action GameInputClear;
    private void Awake()
    {
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
    
}
