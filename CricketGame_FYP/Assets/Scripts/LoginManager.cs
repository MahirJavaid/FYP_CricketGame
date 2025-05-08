using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using TMPro;  // Add this to use TextMesh Pro components
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions; // For Email Validation
using System.Threading.Tasks;

public class LoginManager : MonoBehaviour
{
    public TMP_InputField emailInputField;
    public TMP_InputField passwordInputField;
    public TextMeshProUGUI errorMessageText;
    public Button submitButton;
    public Button cancelButton;

    private FirebaseAuth auth;

    void Start()
    {
        // Check Firebase dependencies and initialize FirebaseAuth
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == Firebase.DependencyStatus.Available)
            {
                auth = FirebaseAuth.DefaultInstance;
                submitButton.onClick.AddListener(Login);
                cancelButton.onClick.AddListener(CancelLogin);
            }
            else
            {
                errorMessageText.text = "Firebase is not available.";
                Debug.LogError("Firebase is not available: " + task.Result.ToString());
            }
        });
    }

   async void Login()
{
    string email = emailInputField.text.Trim();  
    string password = passwordInputField.text;

    // Check if email and password are empty
    if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
    {
        errorMessageText.text = "Please fill in both email and password.";
        submitButton.interactable = true;  // Re-enable the button in case of an empty field
        return;
    }

    // Check if email format is valid
    if (!IsValidEmail(email))
    {
        errorMessageText.text = "Invalid email format.";
        submitButton.interactable = true;  // Re-enable the button in case of invalid email
        return;
    }

    submitButton.interactable = false;
    errorMessageText.text = "Logging in...";

    try
    {
        Debug.Log("Attempting to sign in...");
        var loginResult = await auth.SignInWithEmailAndPasswordAsync(email, password);

        if (loginResult.User != null)
        {
            FirebaseUser user = loginResult.User;
            string username = string.IsNullOrEmpty(user.DisplayName) ? "User" : user.DisplayName;
            Debug.Log("Username: " + username);

            PlayerPrefs.SetString("Username", username);
            PlayerPrefs.Save();
            Debug.Log("Username saved to PlayerPrefs: " + PlayerPrefs.GetString("Username"));

            errorMessageText.text = "";
            SceneManager.LoadScene("LoadingPage"); // Proceed to the loading page
        }
        else
        {
            errorMessageText.text = "Login failed. User is null.";
            Debug.LogError("Login failed. User is null.");
        }
    }
    catch (FirebaseException ex)
    {
        // Handle Firebase-specific authentication errors
        errorMessageText.text = HandleFirebaseError(ex);
        Debug.LogError("Firebase Auth Error: " + ex.Message);
        Debug.LogError("Stack Trace: " + ex.StackTrace);
    }
    catch (System.Exception ex)
    {
        // Handle any other errors
        errorMessageText.text = "An error occurred: " + ex.Message;
        Debug.LogError("Login Error: " + ex.Message);
    }
    finally
    {
        // Always re-enable the button when the operation is done
        submitButton.interactable = true;
    }
}


    void CancelLogin()
    {
        // Clear input fields and reset UI
        emailInputField.text = "";
        passwordInputField.text = "";
        errorMessageText.text = "";
        SceneManager.LoadScene("MainMenu"); // Make sure the scene name is correct
    }

    bool IsValidEmail(string email)
    {
        // Basic email format validation using regex
        string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        Regex regex = new Regex(pattern);
        return regex.IsMatch(email);
    }

    string HandleFirebaseError(FirebaseException ex)
    {
        string errorMessage = ex.Message;

        // Handle specific Firebase errors related to authentication
        if (errorMessage.Contains("wrong-password"))
        {
            return "Incorrect password.";
        }
        if (errorMessage.Contains("user-not-found"))
        {
            return "User not found. Please check your email.";
        }
        if (errorMessage.Contains("invalid-email"))
        {
            return "Invalid email format.";
        }
        if (errorMessage.Contains("network-error"))
        {
            return "Network error. Please check your internet connection.";
        }
        if (errorMessage.Contains("user-disabled"))
        {
            return "This account has been disabled.";
        }
        if (errorMessage.Contains("too-many-requests"))
        {
            return "Too many attempts. Please try again later.";
        }

        return "Login failed. Please check your credentials.";
    }
}
