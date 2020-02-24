using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    Rigidbody rigidbody;
    AudioSource audiosource;

    [SerializeField]
    float rcsThrust = 100f;

    [SerializeField]
    float mainThrust = 100f;

    enum State { Alive, Dying, Transcending};
    State state = State.Alive;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        audiosource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //TODO stop sound on death
        if (state == State.Alive)
        {
            Thrust();
            Rotate();
        }
    }

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space)) //Seperate if so we can thrust and rotate
        {
            rigidbody.AddRelativeForce(Vector3.up * mainThrust);
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

    void OnCollisionEnter(Collision collision)
    {
        if(state != State.Alive) { return; } // Clean as heck to make sure multiple collisions arent happening once you finish or die it stops
        switch(collision.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                state = State.Transcending;
                Invoke("LoadNextLevel", 1f); // paramterise time. Life keeps going on while we wait the 1f second
                break;
            default:
                print("Hit something deadly");
                state = State.Dying;
                Invoke("LoadFirstLevel", 1f); // paramterise time
                SceneManager.LoadScene(0);
                break;
        }
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(1);
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }
}
