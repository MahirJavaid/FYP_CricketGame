using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScript : MonoBehaviour
{
    public Slider loadingBar;
    public Text Progress;
    public Text Info;
    public float progress = 0;
    

    // Start is called before the first frame update
    void Start()
    {
        Progress.text = "Loading: " + Mathf.Clamp((int)progress, 0, 100) + "%";
        loadingBar.value = progress;
    }

    // Update is called once per frame
    void Update()
    {
        progress += 0.1f;
        Progress.text = "Loading: " + Mathf.Clamp((int)progress, 0, 100) + "%";
        loadingBar.value = progress;

        if (progress >= 100)
        {
            SceneManager.LoadScene("HomePage");
        }

        if (progress <= 33)
        {
            Info.text = "Downloading Content...";
        }

        if (progress > 33 && progress <= 66)
        {
            Info.text = "Loading Data...";
        }

        if (progress > 66 && progress <= 99)
        {
            Info.text = "Preparing Matches...";
        }
    }
}
