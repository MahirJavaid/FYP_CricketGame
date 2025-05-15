using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Fader : MonoBehaviour
{
    private void OnEnable()
    {
        var text = GetComponent<TextMeshProUGUI>();

        if (text != null)
        {
            text.DOFade(0, 2f).OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
        }

        var iamge = GetComponent<Image>();
        
        if (iamge != null)
        {
            Invoke("DisableIMage", 2f);
        }

    }

    void DisableIMage()
    {
        gameObject.SetActive(false);
    }
}
