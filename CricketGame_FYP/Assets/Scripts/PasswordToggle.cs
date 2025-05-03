using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class PasswordToggle : MonoBehaviour
{
    public TMP_InputField passwordField;
    public Image eyeIcon;
    public Sprite eyeOpenIcon;
    public Sprite eyeClosedIcon;

    private bool isPasswordHidden = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TogglePasswordVisibility()
    {
        isPasswordHidden = !isPasswordHidden;
        passwordField.contentType = isPasswordHidden ? TMP_InputField.ContentType.Password : TMP_InputField.ContentType.Standard;
        passwordField.ForceLabelUpdate();

        if (eyeIcon != null)
            eyeIcon.sprite = isPasswordHidden ? eyeOpenIcon : eyeClosedIcon;
    }
}
