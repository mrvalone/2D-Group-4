using System.Collections;
using System.Linq;
// using VisualScripting;
using UnityEngine;

public class swingController : MonoBehaviour
{
    
    private IEnumerator deleter;
    
    void Start()
    {
        deleter = (Deleter(0.15f)); //sets the coroutine and calls it with a tiny delay.
        StartCoroutine(deleter);
    }

    private void Update()
    {
        gameObject.transform.Rotate(new Vector3(0,0,-360.0f)*Time.deltaTime*3);
    }

    private IEnumerator Deleter(float Wait) //deletes the object quickly
    {
        yield return new WaitForSeconds(Wait);
        Destroy(gameObject);
    }

}
