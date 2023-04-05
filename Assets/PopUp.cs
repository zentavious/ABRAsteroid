using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUp : MonoBehaviour
{

    public GameObject popUp;
    // Start is called before the first frame update
    void Start()
    {
        popUp.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowPopUp()
    {
        Debug.Log("showing");
        popUp.SetActive(true);
    }

    public void HidePopUp()
    {
        Debug.Log("hiding");
        popUp.SetActive(false);
    }
}
