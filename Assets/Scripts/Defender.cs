using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Defender : MonoBehaviour
{
    public bool IsPlayer1 = true;
    public Planet HomePlanet;
    public float RadialSpeed = 180;
    public Projectile ProjectilePrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Application.isEditor && !Application.isPlaying)
        {
            // Control and update defender properties while in Editor mode...
            if (HomePlanet != null)
            {
                transform.localPosition = new Vector3(0.0f, HomePlanet.Radius, 0.0f);
            }
        }
        else
        {
            var horizontalAxisName = IsPlayer1 ? "P1Horizontal" : "P2Horizontal";
            var fireAxisName = IsPlayer1 ? "P1Fire" : "P2Fire";

            var horizontalAxisValue = Input.GetAxis(horizontalAxisName);
            if (horizontalAxisValue != 0)
            {
                var rotation = RadialSpeed * Time.deltaTime * horizontalAxisValue;
                transform.RotateAround(HomePlanet.transform.position, Vector3.back, rotation);
            }
            if (Input.GetButtonDown(fireAxisName))
            {
                Fire();
            }
        }
    }

    public void Fire()
    {
        AudioSource firesound = GetComponent <AudioSource>();
        firesound.Play(0);
        var projectile = Instantiate(ProjectilePrefab, transform.position, transform.rotation);
        //projectile.Direction = (transform.position - HomePlanet.transform.position).normalized;
    }
}
