using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MeshRenderer rend = GetComponent<MeshRenderer>();
        rend.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
