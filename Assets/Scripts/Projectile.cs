using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float Speed = 25.0f;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Speed * Vector3.up * Time.deltaTime);
    }
}
