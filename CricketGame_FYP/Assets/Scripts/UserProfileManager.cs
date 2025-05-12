using Firebase;
using Firebase.Auth;
using Firebase.Database;
using MongoDB.Bson;
using MongoDB.Driver;
using UnityEngine;
using UnityEngine.UI;  // For using Button
using TMPro;  // For TextMeshPro InputField and TextMeshProUGUI components
using UnityEngine.SceneManagement;

public class UserProfileManager : MonoBehaviour
{
    // Display components (TextMeshProUGUI for TextMeshPro)
    public TextMeshProUGUI displayNameText; // Updated to TextMeshProUGUI
    public TextMeshProUGUI displayEmailText; // Updated to TextMeshProUGUI
    public TextMeshProUGUI displayPasswordText; // Updated to TextMeshProUGUI

    // Editable fields (TMP_InputFields for TextMeshPro)
    public TMP_InputField nameInputField; // Updated to TMP_InputField
    public TMP_InputField emailInputField; // Updated to TMP_InputField
    public TMP_InputField passwordInputField; // Updated to TMP_InputField

    // Buttons
    public Button changeNameButton;
    public Button changeEmailButton;
    public Button changePasswordButton;
    public Button saveButton;
    public Button backButton;

    private FirebaseAuth auth;
    private DatabaseReference database;
    private string userId;

    // MongoDB connection details
    private string connectionString = "mongodb+srv://cric_admin:o5VCAoVJqEf2J7QA@fyp-cricket-db.vze2z.mongodb.net/?retryWrites=true&w=majority&appName=FYP-Cricket-DB"; // MongoDB connection string
    private string dbName = "cricket_game";
    private string collectionName = "users"; // 'users' collection in MongoDB

    void Start()
    {
        Debug.Log("Firebase initialization starting...");

        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                Debug.Log("Firebase initialized successfully.");

                auth = FirebaseAuth.DefaultInstance;
                FirebaseUser user = auth.CurrentUser;

                if (user == null)
                {
                    Debug.LogError("User is not logged in.");
                    return;
                }

                Debug.Log("User logged in successfully.");
                Debug.Log("User ID: " + user.UserId);

                userId = user.UserId;

                FirebaseDatabase firebaseDatabase = FirebaseDatabase.GetInstance(FirebaseApp.DefaultInstance);
                database = firebaseDatabase.RootReference;

                // Set listeners for the buttons
                changeNameButton.onClick.AddListener(() => ActivateEditField("name"));
                changeEmailButton.onClick.AddListener(() => ActivateEditField("email"));
                changePasswordButton.onClick.AddListener(() => ActivateEditField("password"));
                saveButton.onClick.AddListener(SaveChanges);
                backButton.onClick.AddListener(GoBack);

                // Display user info
                DisplayUserInfo();
            }
            else
            {
                Debug.LogError("Firebase initialization failed: " + task.Result);
            }
        });
    }

    // Display current user info from Firebase Realtime Database and Authentication
    private void DisplayUserInfo()
    {
        FirebaseUser user = auth.CurrentUser;

        if (user != null)
        {
            Debug.Log("User is logged in. Fetching data...");

            // Display name, email, and password (masked) on the UI
            displayNameText.text = user.DisplayName ?? "Not Set";
            displayEmailText.text = user.Email ?? "Not Set";
            displayPasswordText.text = "••••••";  // Mask password for security

            // Pre-fill InputFields with current values from Firebase Authentication
            nameInputField.text = user.DisplayName ?? "";
            emailInputField.text = user.Email ?? "";
            passwordInputField.text = ""; // Clear password field

            // Attempt to fetch additional user data from Firebase Realtime Database first
            FetchUserDataFromDatabase(user);
        }
        else
        {
            Debug.LogError("User is not logged in or Firebase user is null.");
        }
    }

    // Fetch user data (name, email, etc.) from Firebase Realtime Database
    private void FetchUserDataFromDatabase(FirebaseUser user)
    {
        // Check if user is valid
        if (user == null)
        {
            Debug.LogError("FirebaseUser is null. Cannot fetch data.");
            return;
        }

        Debug.Log("Attempting to fetch user data from Firebase Realtime Database...");

        // Reference to the Firebase Realtime Database
        DatabaseReference reference = FirebaseDatabase.GetInstance(FirebaseApp.DefaultInstance).RootReference;

        // Fetch user info from Firebase Realtime Database using userId
        reference.Child("users").Child(user.UserId).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                // Handle the error if fetching fails
                Debug.LogError("Error occurred while fetching data from Firebase Realtime Database: " + task.Exception);
            }
            else if (task.IsCompleted)
            {
                // Data has been fetched successfully
                DataSnapshot snapshot = task.Result;

                // Check if the snapshot exists
                if (snapshot.Exists)
                {
                    Debug.Log("Data fetched from Firebase Realtime Database.");

                    // Extract name and email from Firebase
                    string name = snapshot.Child("name").Value.ToString();
                    string email = snapshot.Child("email").Value.ToString();

                    // Update UI with the fetched data
                    displayNameText.text = name;
                    displayEmailText.text = email;

                    // Pre-fill InputFields with fetched data
                    nameInputField.text = name;
                    emailInputField.text = email;
                }
                else
                {
                    // If no data is found in Firebase, attempt to fetch from MongoDB
                    Debug.LogWarning("No data found in Firebase Realtime Database. Fetching from MongoDB...");
                    FetchUserDataFromMongoDB(user.UserId);
                }
            }
        });
    }

    // Fetch user data from MongoDB if not found in Firebase
    private void FetchUserDataFromMongoDB(string userId)
    {
        try
        {
            Debug.Log("Fetching data from MongoDB...");

            var client = new MongoClient(connectionString);
            var mongoDatabase = client.GetDatabase(dbName);
            var collection = mongoDatabase.GetCollection<BsonDocument>(collectionName);

            var filter = Builders<BsonDocument>.Filter.Eq("userId", userId);
            var userDocument = collection.Find(filter).FirstOrDefault();

            if (userDocument != null)
            {
                string name = userDocument["name"].ToString();
                string email = userDocument["email"].ToString();

                // Update UI with the MongoDB data
                displayNameText.text = name;
                displayEmailText.text = email;

                // Pre-fill InputFields with MongoDB data
                nameInputField.text = name;
                emailInputField.text = email;
            }
            else
            {
                Debug.LogError("User data not found in MongoDB either.");
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error fetching data from MongoDB: " + ex.Message);
        }
    }

    // Activate InputField for editing
    private void ActivateEditField(string fieldType)
    {
        // Hide the corresponding text and show the input field
        if (fieldType == "name")
        {
            displayNameText.gameObject.SetActive(false); // Hide Text
            nameInputField.gameObject.SetActive(true); // Show InputField
        }
        else if (fieldType == "email")
        {
            displayEmailText.gameObject.SetActive(false); // Hide Text
            emailInputField.gameObject.SetActive(true); // Show InputField
        }
        else if (fieldType == "password")
        {
            displayPasswordText.gameObject.SetActive(false); // Hide Text
            passwordInputField.gameObject.SetActive(true); // Show InputField
        }

        // Change Change button text to "Cancel"
        SetButtonText("Cancel");

        // Disable other fields to prevent simultaneous editing
        if (fieldType != "name")
            nameInputField.gameObject.SetActive(false);
        if (fieldType != "email")
            emailInputField.gameObject.SetActive(false);
        if (fieldType != "password")
            passwordInputField.gameObject.SetActive(false);
    }

    // Handle switching between Change and Cancel
    private void SetButtonText(string text)
    {
        changeNameButton.GetComponentInChildren<Text>().text = text;
        changeEmailButton.GetComponentInChildren<Text>().text = text;
        changePasswordButton.GetComponentInChildren<Text>().text = text;
    }

    // Cancel editing and return to display mode
    private void CancelEdit()
    {
        // Switch back to display mode without saving changes
        displayNameText.gameObject.SetActive(true);
        displayEmailText.gameObject.SetActive(true);
        displayPasswordText.gameObject.SetActive(true);

        nameInputField.gameObject.SetActive(false);
        emailInputField.gameObject.SetActive(false);
        passwordInputField.gameObject.SetActive(false);

        // Reset Change button text
        SetButtonText("Change");
    }

    // Save changes to Firebase, MongoDB, and Authentication
    private void SaveChanges()
    {
        string newName = nameInputField.text.Trim();
        string newEmail = emailInputField.text.Trim();
        string newPassword = passwordInputField.text.Trim();

        Debug.Log($"Saving Changes: Name: {newName}, Email: {newEmail}, Password: {newPassword}");

        // Update name if changed
        if (newName != displayNameText.text)
        {
            UpdateName(newName);
        }

        // Update email if changed
        if (newEmail != displayEmailText.text)
        {
            UpdateEmail(newEmail);
        }

        // Update password if changed
        if (!string.IsNullOrEmpty(newPassword))
        {
            UpdatePassword(newPassword);
        }

        // Switch back to display mode after saving changes
        CancelEdit();
    }

    // Update username in Firebase Authentication and Realtime Database
    private void UpdateName(string newName)
    {
        FirebaseUser user = auth.CurrentUser;
        UserProfile profile = new UserProfile { DisplayName = newName };
        user.UpdateUserProfileAsync(profile).ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                displayNameText.text = newName; // Update UI with new name
                Debug.Log("Name updated in Firebase Authentication.");
                // Update in Firebase Realtime Database
                database.Child("users").Child(userId).Child("name").SetValueAsync(newName);
                Debug.Log("Name updated in Firebase Realtime Database.");
                // Update in MongoDB as well
                UpdateMongoDB("name", newName);
            }
            else
            {
                Debug.LogError("Error updating name in Firebase: " + task.Exception);
            }
        });
    }

    // Update email in Firebase Authentication and Realtime Database
    private void UpdateEmail(string newEmail)
    {
        FirebaseUser user = auth.CurrentUser;
        user.UpdateEmailAsync(newEmail).ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                displayEmailText.text = newEmail; // Update UI with new email
                Debug.Log("Email updated in Firebase Authentication.");
                // Update in Firebase Realtime Database
                database.Child("users").Child(userId).Child("email").SetValueAsync(newEmail);
                Debug.Log("Email updated in Firebase Realtime Database.");
                // Update in MongoDB as well
                UpdateMongoDB("email", newEmail);
            }
            else
            {
                Debug.LogError("Error updating email in Firebase: " + task.Exception);
            }
        });
    }

    // Update password in Firebase Authentication and MongoDB
    private void UpdatePassword(string newPassword)
    {
        FirebaseUser user = auth.CurrentUser;
        user.UpdatePasswordAsync(newPassword).ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("Password updated in Firebase Authentication.");
                // You may want to update the password in MongoDB here as well
                UpdateMongoDB("password", newPassword);
            }
            else
            {
                Debug.LogError("Error updating password in Firebase: " + task.Exception);
            }
        });
    }

    // Update data in MongoDB (username, email, or password)
    private void UpdateMongoDB(string field, string newValue)
    {
        try
        {
            var client = new MongoClient(connectionString);
            var mongoDatabase = client.GetDatabase(dbName);
            var collection = mongoDatabase.GetCollection<BsonDocument>(collectionName);

            var filter = Builders<BsonDocument>.Filter.Eq("userId", userId);
            var update = Builders<BsonDocument>.Update.Set(field, newValue);

            collection.UpdateOne(filter, update);
            Debug.Log("User profile data updated in MongoDB.");
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error updating MongoDB: " + ex.Message);
        }
    }

    // Navigate back to the previous screen
    private void GoBack()
    {
        SceneManager.LoadScene("HomePage");
    }
}
