using TMPro;  // Add this at the top of your script for TextMesh Pro support
using UnityEngine;
using Firebase.Auth;
using Firebase.Extensions;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class SignupManager : MonoBehaviour
{
    public TMP_InputField nameInputField;
    public TMP_InputField emailInputField;
    public TMP_InputField passwordInputField;
    public TMP_InputField confirmPasswordInputField;
    
    public Button submitButton;
    public Button backButton;

    private FirebaseAuth auth;

    void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
        submitButton.onClick.AddListener(SignUp);
        backButton.onClick.AddListener(BackToLogin);
    }

    async void SignUp()
    {
        string name = nameInputField.text;
        string email = emailInputField.text;
        string password = passwordInputField.text;
        string confirmPassword = confirmPasswordInputField.text;

        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
        {
            Debug.LogError("Please fill in all fields.");
            return;
        }

        if (password != confirmPassword)
        {
            Debug.LogError("Passwords do not match.");
            return;
        }

        try
        {
            // Create user with Firebase Authentication
            var userCredential = await auth.CreateUserWithEmailAndPasswordAsync(email, password);
            FirebaseUser user = userCredential.User;
            Debug.Log("User registered: " + user.Email);

            // Optionally, set the user's display name
            UserProfile profile = new UserProfile { DisplayName = name };
            await user.UpdateUserProfileAsync(profile);
            
            Debug.Log("User profile updated with name: " + user.DisplayName);
            
            // Navigate to the HomePage after successful signup
            SceneManager.LoadScene("HomePage");
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Sign Up Failed: " + ex.Message);
        }
    }

    void BackToLogin()
    {
        SceneManager.LoadScene("Login"); // Go back to the Login scene
    }
}
