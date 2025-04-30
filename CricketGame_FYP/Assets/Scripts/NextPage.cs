using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextPage : MonoBehaviour
{
    public GameObject loginPanel; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void goToLoadingPage()
    {
        SceneManager.LoadScene("LoadingPage");
    }

    public void loginAction()
    {
        loginPanel.SetActive(true);
    }

    public void cancelAction() 
    {
        loginPanel.SetActive(false);
    }

    public void ExitGame()
    {
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }
}
