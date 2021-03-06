﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    Rigidbody rigidbody;
    AudioSource audiosource;
    bool collisionsDisabled;

    [SerializeField] float levelLoadDelay = 2f;
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip success;
    [SerializeField] AudioClip death;

    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem deathParticles;

    bool isTransitioning;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        audiosource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Debug.isDebugBuild)
        {
            RespondToDebugKeys();
        }

        //TODO stop sound on death
        if (!isTransitioning)
        {
            RespondToThrustInput();
            Rotate();
        }
    }

    private void RespondToDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            collisionsDisabled = !collisionsDisabled;
        }
    }

    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space)) //Seperate if so we can thrust and rotate
        {
            ApplyThrust();
        }
        else
        {
            StopApplyingThrust();
        }
    }

    private void StopApplyingThrust()
    {
        audiosource.Stop();
        mainEngineParticles.Stop();
    }

    private void ApplyThrust()
    {
        rigidbody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
        if (!audiosource.isPlaying)
        {
            audiosource.PlayOneShot(mainEngine); //Plays an audio from start to finish not through audio source
            mainEngineParticles.Play();
        }
    }

    private void Rotate()
    {
        rigidbody.angularVelocity = Vector3.zero; //Set any velcoity due to the physics engine make it to zero

        //rigidbody.freezeRotation = true; // Before we take manual control of rotation

        float rotationThisFrame = rcsThrust * Time.deltaTime; //Calculating our rotation this frame since all frame lengths are different
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.P))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }

        //rigidbody.freezeRotation = false; // Resume physics control of rotation
    }

    void OnCollisionEnter(Collision collision)
    {
        if(isTransitioning || collisionsDisabled) { return; } // Clean as heck to make sure multiple collisions arent happening once you finish or die it stops
        switch(collision.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartDeathSequence();
                break;
        }
    }

    private void StartDeathSequence()
    {
        isTransitioning = true;
        audiosource.Stop(); //We play this to stop the thrusting sound
        audiosource.PlayOneShot(death);
        deathParticles.Play();
        Invoke("LoadFirstLevel", levelLoadDelay); // paramterise time
    }

    private void StartSuccessSequence()
    {
        isTransitioning = true;
        audiosource.Stop(); //We play this to stop the thrusting sound
        audiosource.PlayOneShot(success);
        successParticles.Play();
        Invoke("LoadNextLevel", levelLoadDelay); // paramterise time

    }

    private void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        int totalNumberOfScenes = SceneManager.sceneCountInBuildSettings;

        if (nextSceneIndex == totalNumberOfScenes)
            nextSceneIndex = 0;
        else
            SceneManager.LoadScene(nextSceneIndex);
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }
}
