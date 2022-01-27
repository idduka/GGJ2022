using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaqAlla : MonoBehaviour
{
    public float Speed = 10.0f;
    public bool IsPlayer1 = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var horizontalAxisName = IsPlayer1 ? "P1Horizontal" : "P2Horizontal";
        var fireAxisName = IsPlayer1 ? "P1Fire" : "P2Fire";

        var direction = new Vector3(Input.GetAxis(horizontalAxisName), 0.0f, 0.0f);
        direction.Normalize();
        transform.Translate(direction * Speed * Time.deltaTime);
    }
}
