using Firebase;
using Firebase.Auth;
using Firebase.Database;
using MongoDB.Bson;
using MongoDB.Driver;
using UnityEngine;
using System;
using System.Collections.Generic;
using TMPro;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using Firebase.Extensions;  // Firebase specific extensions for ContinueWithOnMainThread
using UnityEngine.SceneManagement;  // For scene management
using System.Threading.Tasks;

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
    private DatabaseReference database;         // Firebase Realtime Database reference
    private string connectionString = "mongodb+srv://cric_admin:o5VCAoVJqEf2J7QA@fyp-cricket-db.vze2z.mongodb.net/?retryWrites=true&w=majority&appName=FYP-Cricket-DB"; // MongoDB connection string
    private string dbName = "cricket_game";
    private string collectionName = "users";     // Changed to 'users' collection
    private string databaseURL = "https://cricketgamefyp-default-rtdb.firebaseio.com/"; // Specify your Firebase Realtime Database URL

    void Start()
    {
        // Initialize Firebase with manual Database URL setup
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                // Initialize FirebaseApp instance
                FirebaseApp app = FirebaseApp.DefaultInstance;

                // Set up the Firebase instance and Database manually with the URL
                FirebaseDatabase firebaseDatabase = FirebaseDatabase.GetInstance(app, databaseURL); // Manually specify the Database URL
                database = firebaseDatabase.RootReference; // Get the RootReference (DatabaseReference) to interact with the database
                Debug.Log("Firebase Realtime Database initialized successfully.");

                // Initialize FirebaseAuth
                auth = FirebaseAuth.DefaultInstance;

                // Add button listeners
                signUpButton.onClick.AddListener(SignUp);
                backButton.onClick.AddListener(GoBack); // Go back to the previous screen
            }
            else
            {
                Debug.LogError("Firebase initialization failed: " + task.Result);
            }
        });
    }

    // Method to validate email format using regex
    public bool IsValidEmail(string email)
    {
        string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        return Regex.IsMatch(email, pattern);
    }

    // SignUp Method (with async/await)
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

            // Optionally, you can set the user's name here in Firebase Authentication
            UserProfile profile = new UserProfile { DisplayName = name };
            await user.UpdateUserProfileAsync(profile);  // Update the profile with the name

            // Now, store additional user data in both Firebase Realtime Database and MongoDB
            await StoreUserDataInFirebase(user.UserId, name, email);
            StoreUserDataInMongoDB(user.UserId, name, email);

            // After successful signup, navigate to the login screen
            SceneManager.LoadScene("Login");
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Signup failed: " + ex.Message);
            errorMessageText.text = "Signup failed: " + ex.Message; // Display error message on UI
        }
    }

    // Store user data in Firebase Realtime Database
    private async Task StoreUserDataInFirebase(string userId, string name, string email)
    {
        // Create a dictionary to store the user's profile data
        var userData = new Dictionary<string, object>
        {
            { "name", name },
            { "email", email }
        };

        // Store the data in Realtime Database under the "users" node using the userId as the key
        try
        {
            await database.Child("users").Child(userId).SetValueAsync(userData);
            Debug.Log("User data stored in Firebase Realtime Database.");
        }
        catch (Exception ex)
        {
            Debug.LogError("Error storing user data in Firebase: " + ex.Message);
        }
    }

    // Store user data in MongoDB (now in 'users' collection)
    private void StoreUserDataInMongoDB(string userId, string name, string email)
    {
        try
        {
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(dbName);
            var collection = database.GetCollection<BsonDocument>(collectionName); // Collection 'users'

            // Create a dictionary to store user data
            var userProfileData = new Dictionary<string, object>
            {
                { "userId", userId },
                { "name", name },
                { "email", email },
                { "createdAt", DateTime.Now }
            };

            // Create a BSON document from the dictionary
            var document = new BsonDocument(userProfileData);

            // Insert the document into the MongoDB collection
            collection.InsertOne(document);
            Debug.Log("User profile data stored in MongoDB.");
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error storing user profile data in MongoDB: " + ex.Message);
        }
    }

    // Back button functionality
    void GoBack()
    {
        // Navigate back to the main menu or previous scene
        SceneManager.LoadScene("MainMenu");
    }
}
