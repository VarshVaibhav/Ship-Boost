using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{
    // lectures for 30, 31


    [SerializeField] Vector3 movementVector = new Vector3(10f, 10f, 10f);

    [SerializeField] float period = 2f;
    // todo remove from inspector later

    [Range(0,1)]
    [SerializeField] float movementFactor; //between 0 and 1

    Vector3 startingPos;

    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // protect against period = zero


        // set movementfactor
        if(period <= Mathf.Epsilon)
        {
            return;
        }
        float cycles = Time.time / period;

        const float tau = Mathf.PI * 2f;
        float rawSinWave = Mathf.Sin(cycles * tau);

        //print(rawSinWave);
        movementFactor = rawSinWave / 2f + 0.5f; // mainnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnn

        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPos + offset;
    }
}
