using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageSwap : MonoBehaviour
{

    public Image image;
    public Sprite colorSprite;
    public Sprite graySprite;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeSprite(bool toggle)
    {
        Debug.Log(toggle);
        if (toggle)
        {
            image.sprite = colorSprite;
        }
        else
        {
            image.sprite = graySprite;
        }
    }
}
