using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockSpin : MonoBehaviour
{
    Vector3 position;
    public float speed = 10f;

    // Start is called before the first frame update
    void Start()
    {
        position = new Vector3(Random.Range(-10.0f, 10.0f), Random.Range(-10.0f, 10.0f), Random.Range(-10.0f, 10.0f));
    }
    
    void Update ()
    {
        transform.Rotate(position, speed * Time.deltaTime);
    }
    // Update is called once per frame
}
