using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    public float moveSpeed = 0.01f;
    private Vector3 moveVector = new Vector3(0.01f, 0, 0);
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < -8
            || transform.position.x > 8)
        {
            moveSpeed *= -1;
        }
        transform.Translate(new Vector3(0.01f,0,0) * moveSpeed);
    }
}
