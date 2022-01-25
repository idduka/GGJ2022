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
        var direction = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
        direction.Normalize();
        transform.Translate(direction * Speed * Time.deltaTime);
    }
}
