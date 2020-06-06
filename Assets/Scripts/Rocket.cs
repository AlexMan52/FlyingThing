using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Rocket : MonoBehaviour
{
    [SerializeField] float yVelocity = 2f;
    [SerializeField] float shipRotationSpeed = 2f;
    [SerializeField] float timeToWaitDeath = 2f;
    [SerializeField] float timeToWaitNextLvl = 2f;

    [SerializeField] AudioClip shipEngine;
    [SerializeField] AudioClip shipExplosion;
    [SerializeField] AudioClip levelWin;

    [SerializeField] GameObject shipEngineVFXGameobject;
    [SerializeField] GameObject shipExplosionVFXGameobject;
    [SerializeField] GameObject levelWinVFXGameobject;


    Rigidbody rb_ship;

    enum State { Alive, Dying, Transcending };
    State state = State.Alive;

    bool collisionsAreEnabled = true;

    // Start is called before the first frame update
    void Start()
    {
        rb_ship = GetComponent<Rigidbody>();
        shipEngineVFXGameobject.SetActive(false);
        shipExplosionVFXGameobject.SetActive(false);
        levelWinVFXGameobject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.Alive)
        {
            Movement();
            PlayEngineSFXandVFX();
        }
        else if (state == State.Dying)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
                rb_ship.freezeRotation = true;
                rb_ship.constraints = RigidbodyConstraints.FreezePosition;
            }
        else
            {
                rb_ship.constraints = RigidbodyConstraints.FreezePosition;
            }
        if (Debug.isDebugBuild)
        {
            if (Input.GetKeyDown(KeyCode.C)) // FOR DEBUG AND TESTING
            {
                collisionsAreEnabled = !collisionsAreEnabled;
            }
        }
    }

    private void Movement() // движение вверх, повороты по оси Z
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rb_ship.AddRelativeForce(0f, yVelocity * Time.deltaTime, 0f);
        }
        rb_ship.angularVelocity = Vector3.zero; // убираем вращение по законам физики, чтобы при столкновении не закручивало сильно
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * shipRotationSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.back * shipRotationSpeed * Time.deltaTime);
        }
    }

    private void PlayEngineSFXandVFX() // звук и огонь двигателя
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetComponent<AudioSource>().PlayOneShot(shipEngine);
            shipEngineVFXGameobject.SetActive(true);
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            StopEngineSFXandVFX();
        }
    }
    private void StopEngineSFXandVFX() //остановить звук и огонь двигателя
    {
        GetComponent<AudioSource>().Stop();
        shipEngineVFXGameobject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision) // действия при соприкосновении
    {
        if (state != State.Alive || !collisionsAreEnabled) { return; } // проверка на то, что корабль жив
        switch (collision.gameObject.tag) // проверка с чем соприкасается корабль
        {
            case "Friendly":
                break;
            case "Finish pad":
                state = State.Transcending;
                StartCoroutine(NextLevelWaitForTime());
                break;
            default:
                state = State.Dying;
                StartCoroutine(DeathWithDelay());
                break;
        }
    }
    IEnumerator DeathWithDelay() // процедура по уничтожению корабля
    {
        StopEngineSFXandVFX();
        DestroyShipBodyParts();
        GetComponent<AudioSource>().PlayOneShot(shipExplosion);
        shipExplosionVFXGameobject.SetActive(true);
        yield return new WaitForSecondsRealtime(timeToWaitDeath);
        Destroy(gameObject);
        FindObjectOfType<LevelLoader>().RestartScene();
    } 
    private static void DestroyShipBodyParts() // убрать детали корабля, чтоб после взрыва его не было видно
    {
        var shipDestroyables = GameObject.FindGameObjectsWithTag("ShipDestroyables");
        for (int i = 0; i < shipDestroyables.Length; i++)
        {
            Destroy(shipDestroyables[i]);
        }
    }
    IEnumerator NextLevelWaitForTime() // go to next level
    {
        GetComponent<AudioSource>().PlayOneShot(levelWin);
        levelWinVFXGameobject.SetActive(true);
        yield return new WaitForSecondsRealtime(timeToWaitNextLvl);
        FindObjectOfType<LevelLoader>().LoadNextScene();
    }
}

