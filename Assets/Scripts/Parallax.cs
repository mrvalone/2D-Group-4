using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float length;
    private float startPos;

    public GameObject cam;

    public float parallaxEffect; // Changed in inspector for effect

    void Start()
    {
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x; // Grabbing the size of the background
    }

    void FixedUpdate()
    {
        float distance = (cam.transform.position.x * parallaxEffect);

        transform.position = new Vector3(startPos + distance, transform.position.y, transform.position.z);
    }
}