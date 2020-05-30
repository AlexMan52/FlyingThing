using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField] float yVelocity = 2f;
    [SerializeField] float shipRotationSpeed = 2f;

    Rigidbody rb_ship;
    

    // Start is called before the first frame update
    void Start()
    {
        rb_ship = GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        PlayBoostSound();
    }

    private void Movement()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rb_ship.AddRelativeForce(0f, yVelocity, 0f);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * shipRotationSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.back * shipRotationSpeed * Time.deltaTime);
        }
    }


    void PlayBoostSound()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetComponent<AudioSource>().Play();
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            GetComponent<AudioSource>().Stop();
        }
    }
}
