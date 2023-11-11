using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WaterSystem.Data;
using WaterSystem;
public class Wavecontroller : MonoBehaviour
{

    public manager _manager;

    public string _received;

    public Water _water;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _received = _manager.received_message;

        // Try to convert _received to a float
        if (float.TryParse(_received, out float convertedFloat))
        {
            // Successfully converted to float
            Debug.Log("Converted float value: " + convertedFloat);

            // Now you can use the convertedFloat variable as needed
            // For example, you might want to use it in some calculations or conditions
        }
        else
        {
            // Handle the case where the conversion failed (invalid string)
            Debug.LogError("Unable to convert to float: " + _received);
        }

        if (convertedFloat == 50f)
            _water._waveHeight = 1;
        

        if (convertedFloat == 100f)
            _water._waveHeight = 2;

    }
}
