using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;  // Make sure this is included
using UnityEngine;

public class FirebaseInit : MonoBehaviour
{
    private FirebaseApp firebaseApp;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Initializing Firebase...");

        InitializeFirebase();
    }

    void InitializeFirebase()
    {
        // Log that we are checking dependencies
        Debug.Log("Checking Firebase dependencies...");

        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            // Log if the task has completed or failed
            if (task.IsCompleted)
            {
                firebaseApp = FirebaseApp.DefaultInstance;
                Debug.Log("Firebase Initialized successfully.");
            }
            else
            {
                Debug.LogError("Firebase initialization failed: " + task.Exception);
            }
        });
    }
}
