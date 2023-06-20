using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeImageTitle : MonoBehaviour
{
    [SerializeField] Image _image;


    public void Revert()
    {
        _image.color = Color.white;
        
    }

    public void Fade()
    {
        StartCoroutine(FadeCoroutine());
    }

    IEnumerator FadeCoroutine()
    {
        while (true)
        {
            var newColor = _image.color;
            newColor.a -=0.4f * Time.deltaTime;
            _image.color = newColor;
            if(newColor.a <= 0.1 ) {
                _image.color = new Color { a = 0.0f };
                this.gameObject.SetActive( false );
                yield break;
            }
            yield return null;
        }
    }

    private void OnDisable()
    {

    }
}
