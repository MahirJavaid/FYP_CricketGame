using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScreen : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(LoadLoginSceneAfterDelay());
    }

    private IEnumerator LoadLoginSceneAfterDelay()
    {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("Login");
    }
}
