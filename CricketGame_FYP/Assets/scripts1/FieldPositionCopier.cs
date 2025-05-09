using UnityEngine;

public class FieldPositionCopier : MonoBehaviour
{
    public GameObject team_pak;      // Reference team with correct field positions
    public GameObject team_india;
    public GameObject team_nz;
    public GameObject team_srilanka;
    public GameObject team_aus;

    void Start()
    {
        ApplyPositionsToSelectedTeam();
    }

    void ApplyPositionsToSelectedTeam()
    {
        GameObject selectedTeam = GetTeamByName(GameSettings.SelectedTeam);

        if (selectedTeam == null)
        {
            Debug.LogError("Selected team not found or not assigned!");
            return;
        }

        if (selectedTeam == team_pak)
        {
            // If the selected team is pak, no need to copy positions
            return;
        }

        CopyPositions(team_pak, selectedTeam);
    }

    GameObject GetTeamByName(string teamName)
    {
        switch (teamName)
        {
            case "pak": return team_pak;
            case "india": return team_india;
            case "nz": return team_nz;
            case "srilanka": return team_srilanka;
            case "aus": return team_aus;
            default: return null;
        }
    }

    void CopyPositions(GameObject sourceTeam, GameObject targetTeam)
    {
        if (sourceTeam.transform.childCount != targetTeam.transform.childCount)
        {
            Debug.LogError("Mismatch in player count between source and target teams!");
            return;
        }

        for (int i = 0; i < sourceTeam.transform.childCount; i++)
        {
            Transform sourcePlayer = sourceTeam.transform.GetChild(i);
            Transform targetPlayer = targetTeam.transform.GetChild(i);

            targetPlayer.position = sourcePlayer.position;
            targetPlayer.rotation = sourcePlayer.rotation;
        }
    }
}
