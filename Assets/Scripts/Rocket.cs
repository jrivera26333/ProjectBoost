using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    Rigidbody rigidbody;
    AudioSource audiosource;

    [SerializeField]
    float rcsThrust = 100f;

    [SerializeField]
    float mainThrust = 100f;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        audiosource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Thrust();
        Rotate();
    }

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space)) //Seperate if so we can thrust and rotate
        {
            rigidbody.AddRelativeForce(Vector3.up * (mainThrust * Time.deltaTime));
            if (!audiosource.isPlaying)
                audiosource.Play();
        }
        else
        {
            audiosource.Stop();
        }
    }

    private void Rotate()
    {
        rigidbody.freezeRotation = true; // Before we take manual control of rotation
        float rotationThisFrame = rcsThrust * Time.deltaTime; //Calculating our rotation this frame since all frame lengths are different

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.P))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }

        rigidbody.freezeRotation = false; // Resume physics control of rotation
    }
}
