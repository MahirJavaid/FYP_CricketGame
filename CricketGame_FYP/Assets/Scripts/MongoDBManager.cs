using MongoDB.Driver;
using UnityEngine;
using System;
using MongoDB.Bson;

public class MongoDBTestConnection : MonoBehaviour
{
    // Replace with your MongoDB connection string
    private string connectionString = "mongodb+srv://cric_admin:o5VCAoVJqEf2J7QA@fyp-cricket-db.vze2z.mongodb.net/?retryWrites=true&w=majority&appName=FYP-Cricket-DB";
    private string dbName = "cricket_game";
    private string collectionName = "player_cards";

    void Start()
    {
        TestMongoDBConnection();
        InsertData();
        //GetData();
    }

    // Test the MongoDB connection
   void TestMongoDBConnection()
{
    try
    {
        var settings = MongoClientSettings.FromConnectionString(connectionString);
        settings.ServerSelectionTimeout = TimeSpan.FromSeconds(60);  // Increase timeout to 60 seconds

        var client = new MongoClient(settings);
        var database = client.GetDatabase(dbName);
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

   void InsertData()
    {
        try
        {
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(dbName);
            var collection = database.GetCollection<BsonDocument>(collectionName);

            // var document = new BsonDocument
            // {
            //     { "name", "John Doe" },
            //     { "score", 100 },
            //     { "team", "Team A" },
            //     { "level", 1 }
            // };

            // collection.InsertOne(document);
            // Debug.Log("Document inserted successfully!");
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error inserting data: " + ex.Message);
        }
    }

    void GetData()
    {
        try
        {
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(dbName);
            var collection = database.GetCollection<BsonDocument>(collectionName);

            var documents = collection.Find(new BsonDocument()).ToList();

            foreach (var doc in documents)
            {
                Debug.Log("Document: " + doc.ToString());
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error retrieving data: " + ex.Message);
        }
    }
}










// using MongoDB.Bson;
// using MongoDB.Driver;
// using System.Collections;
// using UnityEngine;

// public class MongoDBConnector : MonoBehaviour
// {
//     private IMongoDatabase database;
//     private IMongoCollection<BsonDocument> collection;

//     // Replace with your MongoDB Atlas connection string
//     private string connectionString = "mongodb+srv://cric_admin:o5VCAoVJqEf2J7QA@cluster0.mongodb.net/test?retryWrites=true&w=majority";
//     private string dbName = "cricket_game";
//     private string collectionName = "inventory";

//     void Start()
//     {
//         ConnectToDatabase();
//         InsertDocument();
//         FetchDocuments();
//     }

//     // Connect to MongoDB
//     void ConnectToDatabase()
//     {
//         var client = new MongoClient(connectionString);
//         database = client.GetDatabase(dbName);
//         collection = database.GetCollection<BsonDocument>(collectionName);
//         Debug.Log("Connected to MongoDB.");
//     }

//     // Insert a document into MongoDB
//     void InsertDocument()
//     {
//         var document = new BsonDocument
//         {
//             { "inventory_id", "inv002" },
//             { "user_id", "u002" },
//             { "card_id", "card002" },
//             { "coins", "1000"}
//         };

//         collection.InsertOne(document);
//         Debug.Log("Document inserted.");
//     }

//     // Fetch and display documents from MongoDB
//     void FetchDocuments()
//     {
//         var documents = collection.Find(new BsonDocument()).ToList();
//         foreach (var doc in documents)
//         {
//             Debug.Log("Document: " + doc.ToString());
//         }
//     }
// }
