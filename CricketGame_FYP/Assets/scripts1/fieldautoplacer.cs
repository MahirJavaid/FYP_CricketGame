using UnityEngine;
using System.Collections.Generic;

public class FieldAutoPlacer : MonoBehaviour
{
    public GameObject teamManager;
    public Transform fieldPositionsParent;

    void Start()
    {
        string selectedTeam = GameSettings.SelectedTeam;
        GameObject activeTeam = null;

        // Find the active team
        foreach (Transform child in teamManager.transform)
        {
            if (child.name.ToLower().Contains(selectedTeam.ToLower()))
            {
                activeTeam = child.gameObject;
                break;
            }
        }

        if (activeTeam == null)
        {
            Debug.LogWarning("Selected team not found.");
            return;
        }

        // Collect only active players
        List<Transform> activePlayers = new List<Transform>();
        foreach (Transform player in activeTeam.transform)
        {
            if (player.gameObject.activeSelf)
                activePlayers.Add(player);
        }

        // Apply field positions
        for (int i = 0; i < Mathf.Min(activePlayers.Count, fieldPositionsParent.childCount); i++)
        {
            activePlayers[i].position = fieldPositionsParent.GetChild(i).position;
        }

        if (activePlayers.Count != fieldPositionsParent.childCount)
        {
            Debug.LogWarning("Number of players and field positions don't match exactly.");
        }
    }
}
