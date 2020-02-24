using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{
    [SerializeField] Vector3 movementVector = new Vector3(10f,10f,10f);
    [SerializeField] float period = 2f; //Time to complete a cycle

    //TODO remove from inspector later
    [Range(0,1)][SerializeField]float movementFactor; // 0 for not moved, 1 for fully moved

    Vector3 startingPos;

    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float cycles = Time.time / period; //We are timing our cycle here because we only complete a cycle when we have a whole number.
        const float tau = Mathf.PI * 2f; //A complete circle for our Sin
        float rawSinWave = Mathf.Sin(cycles * tau); //Every cycle * tau is a complete circle then it restarts.

        print(rawSinWave);

        movementFactor = rawSinWave / 2f + .5f; //By dividing by 2 you get -.5 to .5 then we add .5 which gives us 0 to 1
        var offSet = movementFactor * movementVector; //Dot Product or Scaler Vector Multiplication
        transform.position = startingPos + offSet;
    }
}
