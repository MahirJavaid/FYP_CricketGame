using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject bowlingSetup;
    public GameObject battingSetup;
    public bool isBowling = false;


    bool isBattingActionDecided = false;

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

    public float BestAccuracy = 0.8f;
    public float GoodAccuracy = 0.4f;

    public int MaxOverInGame = 1;
    public int OversCount = 1;
    public int CurrentBallCount = 0;

    public bool OnBallDone()
    {
        var switchFides = false;

        CurrentBallCount++;
        OversCount = (int)CurrentBallCount / 6;

        if (CurrentBallCount > 0 && CurrentBallCount % 6 == 0)
        {
            int overNumber = CurrentBallCount / 6;

            FindAnyObjectByType<BowlerAnimation>().ChangeBowllingStyle();

            UiManager.instance.ShowMassage("Over Change");

            Debug.Log("Over " + overNumber + " completed.");
        }

        if (OversCount >= MaxOverInGame)
        {
            Invoke("SwitchSides", 1f);

            OversCount = 0;
            CurrentBallCount = 0;
            switchFides = true;
        }

        return switchFides;
    }

    private void SwitchSides()
    {
        if (isBowling)
        {
            UiManager.instance.ShowMassage("Side Change");
            UiManager.instance.ShowBowlingUI(false);
            bowlingSetup.SetActive(false);

            StartBatting();
        }
        else
        {
            UiManager.instance.ShowMassage("Side Change");
            UiManager.instance.ShowBatingUI(false);
            battingSetup.SetActive(false);

            StartBowling();
        }
    }

    public void ShowAccuracyMassage(float value)
    {
        if (value >= BestAccuracy)
        {
            UiManager.instance.ShowMassage("Best");
        }
        else if (value >= GoodAccuracy)
        {
            UiManager.instance.ShowMassage("Good");
        }
        else
        {
            UiManager.instance.ShowMassage("Bad");
        }
    }

    
    public void Bat(float accuracy)
    {
        isBattingActionDecided = true;
        ShowAccuracyMassage(accuracy);
    }


    private void OnEnable()
    {
        PauseMenu(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("GamePlay");
        }
    }

    public void PauseMenu(bool state)
    {
        UiManager.instance.SetPauseMenu(state);
        Time.timeScale = state ? 0 : 1;
    }

    public void StartBatting()
    {
        isBowling = false;
        battingSetup.SetActive(true);

        StartBattingLogic();
    }

    private void StartBattingLogic()
    {
        isBattingActionDecided = false;
        Invoke("DelayBowl", 2f);
        BatiingOption();
    }

    private  void DelayBowl()
    {
        FindAnyObjectByType<Bowlling>().AiBowl();
    }

    private void BatiingOption()
    {
        UiManager.instance.ShowBatingUI(true);
        FindAnyObjectByType<Batting>().InBattingState();
    }

    public void StartBowling()
    {
        bowlingSetup.SetActive(true);
        isBowling = true;

        StartBowlingLogic();
    }

    public void StartBowlingLogic()
    {
        UiManager.instance.ShowBowlingUI(true);
        FindAnyObjectByType<Bowlling>().InBowlingState();
    }

    public void Bowled(float accuracy, Transform currentBall, bool isAiBowl)
    {
        FindAnyObjectByType<Batting>().HitBall(currentBall, isBowling, accuracy);
    }
    public void showAccuracy(float acc)
    {
        ShowAccuracyMassage(acc);
    }

    internal void CompleteLogic()
    {
        if (!OnBallDone())
        {
            if (isBowling)
            {
                Invoke("StartBowlingLogic", 2f);
            }
            else
            {
                Invoke("StartBattingLogic", 2f);

            }
        }
    }

    internal void MakePlayerDoAnimaiton(float value)
    {
        if(value >= BestAccuracy || value >= GoodAccuracy)
        {
            // Best
            FindAnyObjectByType<BatterAnimation>().SetStrikeAnimaiont();
        }
        else
        {
            // Bad
            FindAnyObjectByType<BatterAnimation>().SetMissAnimaiont();
        }
    }

    internal void MakeSureActionDone()
    {
        if (!isBattingActionDecided)
        {
            UiManager.instance.ShowBatingUI(false);
            UiManager.instance.ShowMassage("TooLate");
        }
    }
}
