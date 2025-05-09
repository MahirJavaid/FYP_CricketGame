using UnityEngine;
using System.Collections.Generic;

public class FieldPlacerFromReference : MonoBehaviour
{
    public GameObject teamManager;              // Parent GameObject holding all team objects
    public string referenceTeamName = "team_pak"; // The team whose positions are correct (e.g., Pakistan)

    void Start()
    {
        if (teamManager == null)
        {
            Debug.LogError("Team Manager is not assigned!");
            return;
        }

        string selectedTeam = GameSettings.SelectedTeam;  // e.g., "india"

        GameObject referenceTeam = null;
        GameObject selectedTeamObj = null;

        foreach (Transform child in teamManager.transform)
        {
            string childName = child.name.ToLower();
            if (childName == referenceTeamName.ToLower())
                referenceTeam = child.gameObject;

            if (childName == "team_" + selectedTeam.ToLower())
                selectedTeamObj = child.gameObject;
        }

        if (referenceTeam == null || selectedTeamObj == null)
        {
            Debug.LogError("Could not find reference team or selected team in hierarchy!");
            return;
        }

        List<Transform> refPlayers = new List<Transform>();
        foreach (Transform player in referenceTeam.transform)
        {
            if (player.gameObject.activeInHierarchy)
                refPlayers.Add(player);
        }

        List<Transform> teamPlayers = new List<Transform>();
        foreach (Transform player in selectedTeamObj.transform)
        {
            if (player.gameObject.activeInHierarchy)
                teamPlayers.Add(player);
        }

        if (refPlayers.Count != teamPlayers.Count)
        {
            Debug.LogWarning($"Player count mismatch: {refPlayers.Count} in reference vs {teamPlayers.Count} in selected.");
        }

        for (int i = 0; i < Mathf.Min(refPlayers.Count, teamPlayers.Count); i++)
        {
            teamPlayers[i].position = refPlayers[i].position;
            teamPlayers[i].rotation = refPlayers[i].rotation;
        }

        Debug.Log($"✅ Field positions assigned to {selectedTeamObj.name} based on {referenceTeam.name}");
        Debug.Log("Selected team from GameSettings: " + selectedTeam);
        Debug.Log("Selected team object found: " + selectedTeamObj.name);


    }
}
