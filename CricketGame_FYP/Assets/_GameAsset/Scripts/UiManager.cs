using System;
using TMPro;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    public static UiManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public GameObject accuracymassage;
    public GameObject pauseMenu;
    public GameObject bowlingUi;
    public GameObject batingUi;

    public Transform accuracyParent;
    public Transform scoreParent;

    public void ShowMassage(string text)
    {
        var massage = Instantiate(accuracymassage, accuracyParent);
        massage.GetComponent<TextMeshProUGUI>().text = text;
    }

    public void ShowScores(string text)
    {

        var massage = Instantiate(accuracymassage, scoreParent);
        massage.GetComponent<TextMeshProUGUI>().text = text;
    }

    internal void SetPauseMenu(bool state)
    {
        pauseMenu.SetActive(state);
    }

    public void OnBattingButtonClick()
    {
        GameManager.instance.StartBatting();
        GameManager.instance.PauseMenu(false);
    }

    public void OnBowlingButtonClick()
    {
        GameManager.instance.StartBowling();
        GameManager.instance.PauseMenu(false);
    }

    internal void ShowBowlingUI(bool state)
    {
        bowlingUi.SetActive(state);
    } 
    
    internal void ShowBatingUI(bool state)
    {
        batingUi.SetActive(state);
    }
    public void BowlWithDirection(bool isLeft)
    {
        FindAnyObjectByType<Bowlling>().BowlWithDirection(isLeft);  
    }

    public GameObject FadePanel;
    internal void Fade()
    {
        FadePanel.SetActive(true);
    }

    public void HitBall(int direction)
    {
        UiManager.instance.ShowBatingUI(false);
        FindAnyObjectByType<Batting>().HitBall(direction);
    }
}
