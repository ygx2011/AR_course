using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarioShadowYgx : MonoBehaviour
{
    public GameObject plane;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var vv = plane.transform.position;
        var v = transform.position;
        v.y = vv.y;
        transform.position = v;
    }
}
