using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Rocket : MonoBehaviour
{
    [SerializeField] float yVelocity = 2f;
    [SerializeField] float shipRotationSpeed = 2f;
    [SerializeField] float timeToWait = 2f;

    [SerializeField] AudioClip shipEngine;
    [SerializeField] AudioClip shipExplosion;
    [SerializeField] AudioClip levelWin;

    [SerializeField] GameObject shipEngineVFXGameobject;
    [SerializeField] GameObject shipExplosionVFXGameobject;
    [SerializeField] GameObject levelWinVFXGameobject;


    Rigidbody rb_ship;

    enum State {Alive, Dying, Transcending };
    State state = State.Alive;

    

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
            PlayShipEngineSound();
            PlayVFX();
        }
            
    }

    private void Movement() // движение вверх, повороты по оси Z
    {
        if (Input.GetKey(KeyCode.Space))
            {
                rb_ship.AddRelativeForce(0f, yVelocity * Time.deltaTime, 0f);
                
            }
        rb_ship.freezeRotation = true; // убираем вращение (ручное управление), чтобы при столкновении не закручивало сильно
        if (Input.GetKey(KeyCode.A))
            {
                transform.Rotate(Vector3.forward * shipRotationSpeed * Time.deltaTime);
            }
        else if (Input.GetKey(KeyCode.D))
            {
                transform.Rotate(Vector3.back * shipRotationSpeed * Time.deltaTime);
            }
        rb_ship.freezeRotation = false; // возвращаем вращение (рассчитывает физика)
        
    } 

    void PlayVFX()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            TriggerEngineVFX();
        }
    }
    void PlayShipEngineSound() // звук двигателя
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetComponent<AudioSource>().PlayOneShot(shipEngine);
            shipEngineVFXGameobject.SetActive(true);
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            GetComponent<AudioSource>().Stop();
            shipEngineVFXGameobject.SetActive(false);

        }
    } 

    private void OnCollisionEnter(Collision collision) // проверка на прикосновение с объектом с определенным тэгом
    {
        if (state != State.Alive) { return; } // проверка на то, что корабль жив
        switch (collision.gameObject.tag)
        {
            case "Friendly":

                break;
            case "Fuel":
                Destroy(collision.gameObject);
                AddFuel();
                break;
            case "Finish pad":
                state = State.Transcending;
                StartSuccessProcedure();
                break;
            default:
                state = State.Dying;
                StartDeathProcedure();
                break;
        }
    }
    void StartDeathProcedure() // смерть корабля
    {
        GetComponent<AudioSource>().Stop();
        shipEngineVFXGameobject.SetActive(false);
        StartCoroutine(DeadlyWaitForTime());
        
    }
    IEnumerator DeadlyWaitForTime()
    {
        var shipDestroyables = GameObject.FindGameObjectsWithTag("ShipDestroyables");
        for (int i = 0; i < shipDestroyables.Length; i++)
        {
            Destroy(shipDestroyables[i]);
        }

        GetComponent<AudioSource>().PlayOneShot(shipExplosion);
        shipExplosionVFXGameobject.SetActive(true);
        // TODO Сделать корабль невидимым
        yield return new WaitForSecondsRealtime(timeToWait);
        Destroy(gameObject);
        FindObjectOfType<LevelLoader>().RestartScene();
    }
    
    void StartSuccessProcedure()
    {
        StartCoroutine(NextLevelWaitForTime());
    }
    IEnumerator NextLevelWaitForTime()
    {
        GetComponent<AudioSource>().PlayOneShot(levelWin);
        levelWinVFXGameobject.SetActive(true);
        yield return new WaitForSecondsRealtime(timeToWait);
        FindObjectOfType<LevelLoader>().LoadNextScene();

    }


    private void TriggerEngineVFX() //todo исправить уничтожение vfx
    {
        //if (!engineVFX) { return; } // проверка, что у объекта есть частицы
        //GameObject engineVFXObject = Instantiate(engineVFX, transform.position, transform.rotation); // создем объект с частицами, привязанный к позиции объекта, к оторому привязан скрипт Health
        //Destroy(engineVFXObject, 1f); // убиваем частицы через 1 сек
    }




    void AddFuel() // сбор топлива
    {
        
    }
}
