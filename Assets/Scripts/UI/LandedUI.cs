using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LandedUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleTextMesh;
    [SerializeField] private TextMeshProUGUI statsTextMesh;

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
        if(e.landingType == LandingPad.LandingType.Success)
        {
            titleTextMesh.text = "SUCCESSFUL LANDING!";
            Debug.Log("SUCCESSFUL LANDING");
        }
        else
        {
            titleTextMesh.text = "<color = #ff0000>CRASH!</color>";
            Debug.Log("CRASH!");
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
