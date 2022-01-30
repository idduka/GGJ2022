using UnityEngine;

public class SmokeScreen : MonoBehaviour
{
    private ParticleSystem _particleSystem;
    // Start is called before the first frame update
    public AudioClip SmokeSound;
    public AudioSource PowerUPSSound;

    void Start()
    {
        _particleSystem = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Deploy()
    {
        PowerUPSSound.clip = SmokeSound;
        PowerUPSSound.Play();
        _particleSystem.Play();
    }
}
