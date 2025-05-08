using Firebase.Auth;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Text.RegularExpressions;
using UnityEngine.UI;

public class SignupManager : MonoBehaviour
{
    public TMP_InputField emailInputField;      // Email Input Field (TextMesh Pro)
    public TMP_InputField passwordInputField;   // Password Input Field (TextMesh Pro)
    public TMP_InputField confirmPasswordInputField; // Confirm Password Field
    public TMP_InputField nameInputField;       // Name Input Field (TextMesh Pro)
    public TextMeshProUGUI errorMessageText;    // Error Message Text (TextMesh Pro)
    public Button signUpButton;                 // Sign Up Button
    public Button backButton;                   // Back Button
    private FirebaseAuth auth;                  // Firebase Authentication reference

    void Start()
    {
        // Initialize FirebaseAuth
        auth = FirebaseAuth.DefaultInstance;

        // Add button listeners
        signUpButton.onClick.AddListener(SignUp);
        backButton.onClick.AddListener(GoBack); // Add listener to the back button
    }

    // Method to validate email format using regex
    public bool IsValidEmail(string email)
    {
        string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        return Regex.IsMatch(email, pattern);
    }

    // SignUp Method
    async void SignUp()
    {
        // Get email, password, confirm password, and name input from the user
        string email = emailInputField.text.Trim();
        string password = passwordInputField.text.Trim();
        string confirmPassword = confirmPasswordInputField.text.Trim();
        string name = nameInputField.text.Trim(); // Get name input

        // Reset previous error messages
        errorMessageText.text = string.Empty;

        // Validate email format
        if (!IsValidEmail(email))
        {
            Debug.LogError("Invalid email format.");
            errorMessageText.text = "Invalid email format."; // Display error message on UI
            return; // Stop execution and prevent scene navigation
        }

        // Validate password and confirm password
        if (password != confirmPassword)
        {
            Debug.LogError("Passwords do not match.");
            errorMessageText.text = "Passwords do not match."; // Display error message on UI
            return; // Stop execution and prevent scene navigation
        }

        // Validate password length (Example: At least 6 characters)
        if (password.Length < 6)
        {
            Debug.LogError("Password must be at least 6 characters.");
            errorMessageText.text = "Password must be at least 6 characters."; // Display error message on UI
            return; // Stop execution and prevent scene navigation
        }

        // If validation passes, proceed with Firebase signup
        try
        {
            // Create the user in Firebase Authentication
            var authResult = await auth.CreateUserWithEmailAndPasswordAsync(email, password);
            Debug.Log("User created successfully.");

            // Access the FirebaseUser object
            FirebaseUser user = authResult.User;

            // Optionally, you can set the user's name here in Firebase
            UserProfile profile = new UserProfile { DisplayName = name };
            await user.UpdateUserProfileAsync(profile);  // Update the profile with the name

            // After successful signup, navigate to the login screen
            SceneManager.LoadScene("Login");
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Signup failed: " + ex.Message);
            errorMessageText.text = "Signup failed: " + ex.Message; // Display error message on UI
        }
    }

    // Back button functionality
    void GoBack()
    {
        // You can load a specific scene (like the main menu) or simply go back to the previous scene
        SceneManager.LoadScene("MainMenu");  
    }
}
