using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Firebase.Auth;
using UnityEngine.UI; // For Slider functionality

public class SettingsScript : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown dropdown; // Dropdown for graphics quality
    public Slider volumeSlider;  // Reference to the Volume Slider UI

    private AudioSource audioSource; // Reference to the AudioSource component from AudioManager

    // Start is called before the first frame update
    void Start()
    {
        // Find the AudioManager and assign its AudioSource
        audioSource = FindObjectOfType<AudioManager>().GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogError("AudioManager does not have an AudioSource component!");
            return;
        }

        // Set the initial slider value based on the current audio volume (from 0 to 100)
        volumeSlider.value = Mathf.Round(audioSource.volume * 100);  // Convert from 0-1 range to 0-100 range

        // Add listener for the volume slider to adjust audio volume in real-time
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    // Update is called once per frame
    void Update()
    {
        // Optionally, you can add real-time updates here if necessary
    }

    // Method to control the audio volume using the slider
    public void SetVolume(float volume)
    {
        // Round the volume value to the nearest multiple of 10
        int roundedVolume = Mathf.RoundToInt(volume / 10) * 10;

        // Map the slider value (0-100) to the AudioSource volume (0.0-1.0)
        audioSource.volume = roundedVolume / 100f; // Convert the value to 0.0-1.0 scale
    }

    // Get the selected value of the dropdown (used for debugging)
    public void getDropdownValue()
    {
        int pickedEntryIndex = dropdown.value;
        Debug.Log(pickedEntryIndex); // Log the selected value (used for debugging)
    }

    // Set the quality level based on the dropdown selection
    public void setQuality()
    {
        int qualityIndex = dropdown.value;
        QualitySettings.SetQualityLevel(qualityIndex); // Apply the selected quality level
    }

    // Go back to the home page
    public void goBack()
    {
        // Use SceneManager to load the previous or specific scene
        SceneManager.LoadScene("HomePage"); // Replace "HomePage" with your actual home scene name
    }

    // Method to logout the user
    public void logout()
    {
        // Sign out from Firebase
        FirebaseAuth.DefaultInstance.SignOut(); 

        // Optionally, log out message for debugging
        Debug.Log("User logged out");

        // Load the Login scene after logout
        SceneManager.LoadScene("Login"); // Replace "Login" with your actual scene name
    }
}
