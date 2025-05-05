using UnityEngine;
using UnityEngine.UI;  // Add this to use Unity's UI Text components

public class HomePageScript : MonoBehaviour
{
    // Reference to the Unity UI Text element for the username
    public Text usernameText;

    void Start()
    {
        // Get the stored username from PlayerPrefs
        string username = PlayerPrefs.GetString("Username", "User");  // Default to "User" if not found
        Debug.Log("Retrieved username: " + username);

        // Display the username in the UI Text element
        usernameText.text = "Welcome, " + username + "!";
    }
}
