using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LandedUI : MonoBehaviour
{
    [SerializeField] private Lander lander;
    [SerializeField] private TextMeshProUGUI titleTextMesh;
    [SerializeField] private TextMeshProUGUI statsTextMesh;
    [SerializeField] private GameObject landerExplosionVfx;
    [SerializeField] private Button nextButton;
    private void Awake()
    {
        nextButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(0);
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
        if(e.landingType == LandingPad.LandingType.Success)
        {
            titleTextMesh.text = "SUCCESSFUL LANDING!";
        }
        else
        {
            titleTextMesh.text = "<color = #ff0000>CRASH!</color>";
            Instantiate(landerExplosionVfx, lander.transform.position, Quaternion.identity);
            lander.gameObject.SetActive(false);
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
