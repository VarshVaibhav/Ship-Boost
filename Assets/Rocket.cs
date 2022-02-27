using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;
public class Rocket : MonoBehaviour
{

    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;
    [SerializeField] float levelLoadDelay = 2f;

    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip success;
    [SerializeField] AudioClip death;

    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem deathParticles;

    Rigidbody rigidBody;
    AudioSource audioSource;

    enum State { Alive, Dying,Transcending}
    State state = State.Alive;


    bool collisionAreEnabled = true;
    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.Alive)
        {
            if (CrossPlatformInputManager.GetButton("Jump"))
            {
                Fly();
            }
            RespondToThrust();
            RespondToRotate();
        }

        // todo only if debug is on
        if (Debug.isDebugBuild) // in build setting, development build should be off
        {
            RespondToDebugKeys();
        }
    }

    private void RespondToDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextScene();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            // toggle collision
            collisionAreEnabled = !collisionAreEnabled; // toggle 
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(state != State.Alive || !collisionAreEnabled)
        {
            return;
        }
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                StartSuccesSequence();
                break;
            default:
                break;
        }
    }

    private void StartSuccesSequence()
    {
        state = State.Transcending;
        audioSource.Stop();
        audioSource.PlayOneShot(success);
        successParticles.Play();
        Invoke("LoadNextScene", levelLoadDelay);
    }
    private void StartDeathSequence()
    {
        state = State.Dying;
        audioSource.Stop();
        audioSource.PlayOneShot(death);
        deathParticles.Play();
        Invoke("LoadFirstLevel", levelLoadDelay);
    }

    private void LoadNextScene()
    {

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if(nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }
    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }

    public void Fly()
    {
        float speed = mainThrust * Time.deltaTime;
        rigidBody.AddRelativeForce(Vector3.up * speed);
    }
    private void RespondToThrust()
    {

        float speed = mainThrust * Time.deltaTime;
        if (Input.GetKey(KeyCode.Space))
        {

            rigidBody.AddRelativeForce(Vector3.up * speed);


            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(mainEngine);
            }
            mainEngineParticles.Play();

        }
        else
        {
            audioSource.Stop();
            mainEngineParticles.Stop();
        }
    }

    private void ApplyThurst(float speed)
    {
        rigidBody.AddRelativeForce(0.1f* speed, 0.1f * speed, 0);

        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
        }
        mainEngineParticles.Play();
    }

    private void RespondToRotate()
    {
        rigidBody.freezeRotation = true; // take manual control of roatation 


        float rotationThisFrame =  rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            Debug.Log("left");
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {

            Debug.Log("right");
            transform.Rotate(-Vector3.forward*rotationThisFrame);
        }

        rigidBody.freezeRotation = false; // resume physics control of rotation

    }

    /*public void TouchInput()
    {
        if (Input.GetMouseButton(0))
        {
            if(Input.mousePosition.x <= Screen.width / 2)
            {
                Debug.Log("left");
                ApplyThurst(mainThrust * Time.deltaTime);
            }
            if(Input.mousePosition.x >= Screen.width / 2)
            {
                Debug.Log("right");
                ApplyThurst(mainThrust * Time.deltaTime);
            }
        }
    }*/

}
