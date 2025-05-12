using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;
using System.Collections;

public class FirebaseManager : MonoBehaviour
{
    private DatabaseReference reference;

    // Example data structure
    [System.Serializable]
    public class User
    {
        public string username;
        public string email;

        public User(string username, string email)
        {
            this.username = username;
            this.email = email;
        }
    }

    void Start()
    {
        // Initialize Firebase
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            if (task.IsCompleted)
            {
                FirebaseApp app = FirebaseApp.DefaultInstance;
                reference = FirebaseDatabase.DefaultInstance.RootReference;
                Debug.Log("Firebase Initialized Successfully!");

                // Write data after Firebase is initialized
               // WriteNewUser("user2", "Aimen", "aimen@example.com");

                // Read data after a delay to ensure the write operation completes
                StartCoroutine(ReadDataAfterDelay("user1"));
            }
            else
            {
                Debug.LogError("Firebase initialization failed: " + task.Exception);
            }
        });
    }

    // Write data to Firebase Realtime Database
    private void WriteNewUser(string userId, string name, string email)
    {
        User user = new User(name, email);
        string json = JsonUtility.ToJson(user);
        
        reference.Child("users").Child(userId).SetRawJsonValueAsync(json)
            .ContinueWithOnMainThread(task => {
                if (task.IsCompleted)
                    Debug.Log("Data written successfully to Firebase!");
                else
                    Debug.LogError("Error writing data: " + task.Exception);
            });
    }

    // Read data from Firebase Realtime Database with a delay to ensure the write completes
    private IEnumerator ReadDataAfterDelay(string userId)
    {
        // Wait for 1 second (adjust delay as necessary)
        yield return new WaitForSeconds(1);

        // Read data from Firebase
        FirebaseDatabase.DefaultInstance
            .GetReference("users/" + userId)
            .GetValueAsync().ContinueWithOnMainThread(task => {
                if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    Debug.Log("Reading data from path: users/" + userId);  // Debugging statement
                    if (snapshot.Exists)
                    {
                        Debug.Log("Data: " + snapshot.Value.ToString());
                    }
                    else
                    {
                        Debug.LogWarning("No data found at path 'users/" + userId + "'");
                    }
                }
                else
                {
                    Debug.LogError("Error reading data: " + task.Exception);
                }
            });
    }
}
