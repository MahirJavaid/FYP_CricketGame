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
    public TextMeshProUGUI displayNameText;
    public TextMeshProUGUI displayEmailText;
    public TextMeshProUGUI displayPasswordText;

    // Editable fields (TMP_InputFields for TextMeshPro)
    public TMP_InputField nameInputField;
    public TMP_InputField emailInputField;
    public TMP_InputField passwordInputField;

    // Buttons
    public Button changeButton;  // Change button to toggle edit mode
    public Button saveButton;    // Save button to save changes
    public Button backButton;    // Back button

    private FirebaseAuth auth;
    private DatabaseReference database;
    private string userId;

    // MongoDB connection details
    private string connectionString = "mongodb+srv://cric_admin:o5VCAoVJqEf2J7QA@fyp-cricket-db.vze2z.mongodb.net/?retryWrites=true&w=majority&appName=FYP-Cricket-DB"; // MongoDB connection string
    private string dbName = "cricket_game";
    private string collectionName = "users"; // 'users' collection in MongoDB

    void Start()
    {
        // Initially hide the editable input fields
        nameInputField.gameObject.SetActive(false);
        emailInputField.gameObject.SetActive(false);
        passwordInputField.gameObject.SetActive(false);

        // Initialize Firebase
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

                userId = user.UserId;
                Debug.Log("User ID: " + user.UserId);
                Debug.Log("User Display Name: " + user.DisplayName);
                Debug.Log("User Email: " + user.Email);

                FirebaseDatabase firebaseDatabase = FirebaseDatabase.GetInstance(FirebaseApp.DefaultInstance);
                database = firebaseDatabase.RootReference;

                // Set listeners for the buttons
                changeButton.onClick.AddListener(ToggleEditFields);  // Toggle edit mode on Change button click
                saveButton.onClick.AddListener(SaveChanges);        // Save button to save changes
                backButton.onClick.AddListener(GoBack);             // Back button to go to previous screen

                // Fetch user info from Firebase Database
                FetchUserDataFromDatabase(user);
            }
            else
            {
                Debug.LogError("Firebase initialization failed: " + task.Result);
            }
        });
    }

    // Fetch user data (name, email, etc.) from Firebase Realtime Database
    public void FetchUserDataFromDatabase(FirebaseUser user)
    {
        if (user != null)
        {
            string userPath = "users/" + user.UserId;  // Path to the user's data in Firebase Realtime Database
            Debug.Log("Fetching user data from Firebase Realtime Database at: " + userPath);

            // Reference to the Firebase Realtime Database
            DatabaseReference reference = FirebaseDatabase.GetInstance(FirebaseApp.DefaultInstance).RootReference;

            // Fetch user info from Firebase Realtime Database using userId
            reference.Child(userPath).GetValueAsync().ContinueWith(task =>
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

                    // Check if the snapshot exists and contains data
                    if (snapshot.Exists)
                    {
                        Debug.Log("Data fetched from Firebase Realtime Database.");
                        Debug.Log("Snapshot Data: " + snapshot.GetRawJsonValue());  // Log the raw data

                        // Extract name and email from Firebase
                        string name = snapshot.Child("name").Value.ToString();
                        string email = snapshot.Child("email").Value.ToString();

                         
                          // Check if the fetched data is correct
                        Debug.Log("Fetched Name: " + name);
                        Debug.Log("Fetched Email: " + email);

// Update UI with the fetched data
                        displayNameText.text = name;
                        displayEmailText.text = email;

// Confirm the UI update
                        Debug.Log("Updated Display Name: " + displayNameText.text);
                        Debug.Log("Updated Display Email: " + displayEmailText.text);

                        
                    }
                    else
                    {
                        Debug.LogWarning("No data found in Firebase Realtime Database.");
                        Debug.Log("Attempting to fetch data from MongoDB...");
                        FetchUserDataFromMongoDB(user.UserId);
                    }
                }
            });
        }
        else
        {
            Debug.LogError("FirebaseUser is null. Cannot fetch data.");
        }
    }

    // Fetch user data from MongoDB if not found in Firebase
    public void FetchUserDataFromMongoDB(string userId)
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

    // Toggle between display and edit mode for all fields
    public void ToggleEditFields()
    {
        bool isEditing = nameInputField.gameObject.activeSelf;  // Check if input fields are currently visible

        // Toggle visibility of the display and input fields
        displayNameText.gameObject.SetActive(isEditing); 
        displayEmailText.gameObject.SetActive(isEditing);
        displayPasswordText.gameObject.SetActive(isEditing);

        nameInputField.gameObject.SetActive(!isEditing);
        emailInputField.gameObject.SetActive(!isEditing);
        passwordInputField.gameObject.SetActive(!isEditing);

        // Change button text to "Cancel" if editing or "Change" if not editing
        changeButton.GetComponentInChildren<Text>().text = isEditing ? "Change" : "Cancel";
    }

    // Save changes to Firebase, MongoDB, and Authentication
    public void SaveChanges()
    {
        string newName = nameInputField.text.Trim();
        string newEmail = emailInputField.text.Trim();
        string newPassword = passwordInputField.text.Trim();

        Debug.Log($"Saving Changes: Name: {newName}, Email: {newEmail}, Password: {newPassword}");

        // Update name, email, and password if changed
        if (newName != displayNameText.text)
        {
            UpdateName(newName);
        }

        if (newEmail != displayEmailText.text)
        {
            UpdateEmail(newEmail);
        }

        if (!string.IsNullOrEmpty(newPassword))
        {
            UpdatePassword(newPassword);
        }

        // Switch back to display mode after saving changes
        ToggleEditFields();
    }

    // Update username in Firebase Authentication and Realtime Database
    public void UpdateName(string newName)
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
    public void UpdateEmail(string newEmail)
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
    public void UpdatePassword(string newPassword)
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
    public void UpdateMongoDB(string field, string newValue)
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
    public void GoBack()
    {
        SceneManager.LoadScene("HomePage");
    }
}
