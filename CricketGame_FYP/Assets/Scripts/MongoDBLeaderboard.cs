using System.Collections.Generic;
using UnityEngine;
using MongoDB.Bson;
using MongoDB.Driver;
using TMPro;
using System;
using UnityEngine.SceneManagement;  // Import for SceneManager
using UnityEngine.UI; 
public class MongoDBLeaderboard : MonoBehaviour
{
    public GameObject leaderboardItemPrefab;  // Prefab for leaderboard item (TextMeshPro)
    public Transform contentPanel;            // The Content panel inside the Scroll View to hold leaderboard items

    private string connectionString = "mongodb+srv://cric_admin:o5VCAoVJqEf2J7QA@fyp-cricket-db.vze2z.mongodb.net/?retryWrites=true&w=majority&appName=FYP-Cricket-DB";  
    private MongoClient client;
    private IMongoDatabase database;
    private IMongoCollection<BsonDocument> leaderboardCollection;

    void Start()
    {
        TestMongoDBConnection();
        FetchLeaderboardData();
    }

    // Test MongoDB connection
    void TestMongoDBConnection()
    {
        try
        {
            var settings = MongoClientSettings.FromConnectionString(connectionString);
            settings.ServerSelectionTimeout = System.TimeSpan.FromSeconds(60);  // Increase timeout to 60 seconds

            var client = new MongoClient(settings);
            var database = client.GetDatabase("cricket_game");  // Use your MongoDB database name
            var collections = database.ListCollectionNames().ToList();

            Debug.Log("Successfully connected to MongoDB!");
            foreach (var collection in collections)
            {
                Debug.Log(" - " + collection);
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Failed to connect to MongoDB: " + ex.Message);
        }
    }

    // Fetch leaderboard data from MongoDB
    void FetchLeaderboardData()
    {
        try
        {
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase("cricket_game");  // Use your MongoDB database name
            var collection = database.GetCollection<BsonDocument>("leaderboard");  // Use your collection name

            // Fetch all leaderboard entries from the collection
            var documents = collection.Find(new BsonDocument()).ToList();

            foreach (var doc in documents)
            {
                // Check if the necessary fields are present before accessing them
                string userId = doc.Contains("userId") ? doc["userId"].ToString() : "Unknown";
                string name = doc.Contains("name") ? doc["name"].ToString() : "Unknown";
                int score = doc.Contains("score") ? Convert.ToInt32(doc["score"]) : 0;
                int level = doc.Contains("level") ? Convert.ToInt32(doc["level"]) : 0;
                int rank = doc.Contains("rank") ? Convert.ToInt32(doc["rank"]) : 0;

                // Create a new leaderboard item and populate its text
                GameObject newItem = Instantiate(leaderboardItemPrefab, contentPanel);
                TextMeshProUGUI playerText = newItem.GetComponent<TextMeshProUGUI>();
                playerText.text = $"{rank}. {name} - Score: {score} - Level: {level}";
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error retrieving leaderboard data: " + ex.Message);
      
        }
    }

     public void OnBackButtonClick()
    {
        // Load the scene by name or build index.
        // Example: If you want to load a scene named "MainMenu"
        SceneManager.LoadScene("HomePage");  // Change "MainMenu" to the name of your desired scene
    }
}
