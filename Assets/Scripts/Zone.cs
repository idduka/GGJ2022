using UnityEngine;

public class Zone : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Destroy(other.gameObject);
    }
}
