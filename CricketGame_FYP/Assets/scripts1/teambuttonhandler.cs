using UnityEngine;
using UnityEngine.SceneManagement;

public class TeamButtonHandler : MonoBehaviour
{
    public string teamName; // e.g., "pak", "india", etc.

    public void SelectTeam()
    {
        GameSettings.SelectedTeam = teamName;
        Debug.Log("Selected Team: " + teamName);  // Replace with your actual gameplay scene name
    }
}
