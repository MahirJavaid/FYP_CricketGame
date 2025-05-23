using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NavigationScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void comingSoon()
    {
        SceneManager.LoadScene("ComingSoon");
    }

    public void inventory()
    {
        SceneManager.LoadScene("InventoryScene");
    }

    public void selectionScene()
    {
        SceneManager.LoadScene("TeamSelectScene");
    }

    public void store()
    {
        SceneManager.LoadScene("StoreScene");
    }

    public void settings()
    {
        SceneManager.LoadScene("Settings");
    }

    public void profile()
    {
        SceneManager.LoadScene("UserProfile");
    }

    public void scenarioMatch()
    {
        SceneManager.LoadScene("ScenarioMatch");
    }

    public void tournaments()
    {
        SceneManager.LoadScene("TournamentSelect");
    }

    public void skillGame()
    {
        SceneManager.LoadScene("SkillGame");
    }

    public void leaderboard()
    {
        SceneManager.LoadScene("Leaderboard");
    }

    public void market()
    {
        SceneManager.LoadScene("ExchangeMarket");
    }
}
