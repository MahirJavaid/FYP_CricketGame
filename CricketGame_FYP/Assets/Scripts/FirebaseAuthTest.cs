using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using UnityEngine;

public class FirebaseAuthTest : MonoBehaviour
{
    private FirebaseAuth auth;

    void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
        RegisterUser("test@example.com", "password123");
    }

    // Register a new user with email and password
    public void RegisterUser(string email, string password)
    {
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                FirebaseUser newUser = task.Result.User;
                Debug.Log("User registered: " + newUser.Email);
            }
            else
            {
                Debug.LogError("Registration failed: " + task.Exception);
            }
        });
    }

    // Sign in with email and password
    public void SignInUser(string email, string password)
    {
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                FirebaseUser user = task.Result.User;
                Debug.Log("User signed in: " + user.Email);
            }
            else
            {
                Debug.LogError("Sign-in failed: " + task.Exception);
            }
        });
    }
}
