using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  // Required for SceneManager

public class LoadingPage : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        string username = PlayerPrefs.GetString("Username");
    Debug.Log("Username on Loading Page: " + username);

    // Any loading process can be handled here. For example, wait a few seconds or do some background tasks.
    // After everything is done, transition to the HomePage scene.

    // For demonstration, let's load the HomePage after a delay
    StartCoroutine(LoadHomePageAfterDelay());
        
    }
    
private IEnumerator LoadHomePageAfterDelay()
{
    // Simulate loading delay (can be your actual loading process here)
    yield return new WaitForSeconds(2);  // Wait 2 seconds (you can adjust the time as needed)

    // Now, transition to the HomePage
    SceneManager.LoadScene("HomePage");
}

    // Update is called once per frame
}
