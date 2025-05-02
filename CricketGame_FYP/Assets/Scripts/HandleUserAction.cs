using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HandleUserAction : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void goToLoginPage()
    {
        SceneManager.LoadScene("Login");
    }

    public void goToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
