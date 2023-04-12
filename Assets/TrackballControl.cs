using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackballControl : MonoBehaviour
{

    public GameObject camera;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            camera.transform.Translate(0, 10, 0);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            camera.transform.Translate(0, -10, 0);
        }
    }

   // public void 
}
