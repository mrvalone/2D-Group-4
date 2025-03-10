using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player; //reference to the player
    
    private Vector3 offset; //stores offset between camera and player inital position
    
    void Start()
    {
        offset = player.transform.position - gameObject.transform.position; //allows the camera to be offset in unity editor and still track the player
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10.0f);  //moves the camera with the player (according to the offset) every frame
    }
}
