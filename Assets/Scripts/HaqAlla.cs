using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaqAlla : MonoBehaviour
{
    public float Speed = 10.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Time.deltaTime * Input.GetAxis("Horizontal") * Speed, 0, Time.deltaTime * Input.GetAxis("Vertical") * Speed);
    }
}
