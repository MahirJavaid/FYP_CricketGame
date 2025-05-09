using UnityEngine;

public class TeamSelector : MonoBehaviour
{
    public GameObject team_pak;
    public GameObject team_india;
    public GameObject team_nz;
    public GameObject team_srilanka;
    public GameObject team_aus;

    void Start()
    {
        ShowTeam(GameSettings.SelectedTeam);
    }

    public void ShowTeam(string teamName)
    {
        team_pak.SetActive(teamName == "pak");
        team_india.SetActive(teamName == "india");
        team_nz.SetActive(teamName == "nz");
        team_srilanka.SetActive(teamName == "srilanka");
        team_aus.SetActive(teamName == "aus");
    }
}
