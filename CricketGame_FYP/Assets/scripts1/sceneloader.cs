using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadMatchScene()
    {
        SceneManager.LoadScene("SampleScene");  // Use your actual match scene name
    }
}
