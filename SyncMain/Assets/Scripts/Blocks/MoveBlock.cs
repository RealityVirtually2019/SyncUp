using UnityEngine;
using System.Collections;

public class MoveBlock : MonoBehaviour
{
    public float boundary = 5;
    public float speed = 2f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Destroy object if it moves out of bounds
       // if (gameObject.transform.position.y >= boundary) {
       //       Destroy(gameObject);
     //   }
        // Move the object forward along its z axis 1 unit/second.
        transform.Translate(-Vector3.forward * speed * Time.deltaTime);
    }

}
