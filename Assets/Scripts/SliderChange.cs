using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderChange : MonoBehaviour

    
{
    public Text valueText;
    public Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SliderUpdate(int TimeStepVal){
        slider.value = ((float)TimeStepVal);
    }

    public void OnSliderChanged(float value)
    {
    valueText.text = value.ToString();
    }
}
