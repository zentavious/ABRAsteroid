using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{

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

    public void Move()
    {
        Debug.Log("moving.");
        camera.position = new Vector3(xPos, yPos, zPos);
    }
}
