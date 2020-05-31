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
    } // движение вверх, повороты по оси Z

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
    } // фоновая музыка

    private void OnCollisionEnter(Collision collision) // проверка на прикосновение с объектом с определенным тэгом
    {
        switch (collision.gameObject.tag)
        {
            case "Friendly":

                break;
            case "Fuel":
                Destroy(collision.gameObject);
                AddFuel();
                break;
            default:
                Death();
                break;
        }
    }   

    void Death() // смерть корабля
    {
        Destroy(gameObject);
    }

    void AddFuel() // сбор топлива
    {
        
    }
}
