using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    static private bool splitScreen = false;

    public MoveCamera firstScreen = null;

    public Camera cam = null;

    public float xPos = 0;

    public float yPos = 0;

    public float zPos = 0;

    public Transform camera = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSplitScreenDim()
    {
        cam.rect = new Rect(0.088f, 0.0f, 0.456f, 1.0f);
    }

    public void Move()
    {
        Debug.Log("moving.");
        camera.position = new Vector3(xPos, yPos, zPos);
        if (firstScreen != null && !splitScreen)
        {
            splitScreen = true;
            firstScreen.SetSplitScreenDim();
        }
    }
}
