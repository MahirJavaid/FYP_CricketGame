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
        auth = FirebaseAuth.DefaultInstance;

        submitButton.onClick.AddListener(Login);
        cancelButton.onClick.AddListener(CancelLogin);
    }

    async void Login()
    {
        string email = emailInputField.text.Trim();  
        string password = passwordInputField.text;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            errorMessageText.text = "Please fill in both email and password.";
            return;
        }

        if (!IsValidEmail(email))
        {
            errorMessageText.text = "Invalid email format.";
            return;
        }

        submitButton.interactable = false;
        errorMessageText.text = "Logging in...";

        try
        {
            Debug.Log("Attempting to sign in...");
            var loginResult = await auth.SignInWithEmailAndPasswordAsync(email, password);
            FirebaseUser user = loginResult.User;

            string username = string.IsNullOrEmpty(user.DisplayName) ? "User" : user.DisplayName;
            Debug.Log("Username: " + username);

            PlayerPrefs.SetString("Username", username);
            PlayerPrefs.Save();
            Debug.Log("Username saved to PlayerPrefs: " + PlayerPrefs.GetString("Username"));

            errorMessageText.text = "";
            SceneManager.LoadScene("LoadingPage");
        }
        catch (FirebaseException ex)
        {
            errorMessageText.text = HandleFirebaseError(ex);
            Debug.LogError("Firebase Auth Error: " + ex.Message);
        }
        catch (System.Exception ex)
        {
            errorMessageText.text = "An error occurred: " + ex.Message;
            Debug.LogError("Login Error: " + ex.Message);
        }
        finally
        {
            submitButton.interactable = true;
        }
    }

    void CancelLogin()
    {
        emailInputField.text = "";
        passwordInputField.text = "";
        errorMessageText.text = "";
        SceneManager.LoadScene("Login");
    }

    bool IsValidEmail(string email)
    {
        string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        Regex regex = new Regex(pattern);
        return regex.IsMatch(email);
    }

    string HandleFirebaseError(FirebaseException ex)
    {
        string errorMessage = ex.Message;

        if (errorMessage.Contains("wrong-password"))
        {
            return "Incorrect password.";
        }
        if (errorMessage.Contains("user-not-found"))
        {
            return "User not found.";
        }
        if (errorMessage.Contains("invalid-email"))
        {
            return "Invalid email format.";
        }
        return "Login failed. Please check your credentials.";
    }
}
