using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] float yRotationSpeed = 300f;
    [SerializeField] Vector3 movementVector;
    [SerializeField] float period = 2f; // устанавливаем время периода (т.е. за сколько пройдет 1 волна синусоиды) 

    //[Range(0,1)] [SerializeField] 
    float movementFactor; //делали для ручного движения объектов на отрезок

    Vector3 startingPos;
    

    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (period <= Mathf.Epsilon) { return; } //для float нужно проверять не 0, а Эпсилон, так точнее
        float cycles = Time.time / period; // привязываем время в игре к времени периода, чтобы при течении времени в игре изменялось значение от которого берётся синус
        const float tau = Mathf.PI * 2; //задаем угол для синуса, равное двум ПИ, чтобы всегда получить значение синуса от -1 до +1
        float rawSinWave = Mathf.Sin(cycles * tau); //получаем синус от 2П (т.е. значения от -1 до +1) 

        movementFactor = rawSinWave / 2f + 0.5f; // перводим значения синуса, чтобы получить промежуток от 0 до 1: делим на 2 - будет от -0.5 до +0.5 и прибавляем 0.5

        transform.Rotate(0f, yRotationSpeed * Time.deltaTime, 0f);

        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPos + offset;
    }
}
