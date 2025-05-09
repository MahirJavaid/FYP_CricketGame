using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using TMPro;


public class SettingsScript : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown dropdown;
    public AudioMixer audioMixer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setVolume(float volume)
    {
        // Debug.Log(volume);
        audioMixer.SetFloat("volume", volume);
    }

    public void getDropdownValue()
    {
        int pickedEntryIndex = dropdown.value;

        Debug.Log(pickedEntryIndex);
    }

    public void setQuality()
    {
        int qualityIndex = dropdown.value;
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void goBack()
    {
        SceneManager.LoadScene("HomePage");
    }
}
