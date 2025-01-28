using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using extOSC;

public class MyOSC : MonoBehaviour
{
    public extOSC.OSCReceiver oscReceiver;
    public GameObject myTarget;

    // Variables pour stocker les valeurs reçues (visibles dans l'inspecteur)
    public float first { get; private set; }
    public float second { get; private set; }
    public float third { get; private set; }

    public static float ScaleValue(float value, float inputMin, float inputMax, float outputMin, float outputMax)
    {
        return Mathf.Clamp(((value - inputMin) / (inputMax - inputMin) * (outputMax - outputMin) + outputMin), outputMin, outputMax);
    }

    void Start()
    {
        // Lier les adresses OSC aux méthodes correspondantes
        oscReceiver.Bind("/first", message => TraiterMessageOSC(message, "first"));
        oscReceiver.Bind("/second", message => TraiterMessageOSC(message, "second"));
        oscReceiver.Bind("/third", message => TraiterMessageOSC(message, "third"));
    }

    void TraiterMessageOSC(OSCMessage oscMessage, string messageType)
    {
        float value = 0;

        if (oscMessage.Values[0].Type == OSCValueType.Int)
        {
            value = oscMessage.Values[0].IntValue;
        }
        else if (oscMessage.Values[0].Type == OSCValueType.Float)
        {
            value = oscMessage.Values[0].FloatValue;
        }
        else
        {
            return; // Quitter si la valeur n'est pas valide
        }

        Debug.Log($"[{messageType}] Valeur reçue : {value}");

        // Stocker la valeur dans la bonne variable
        switch (messageType)
        {
            case "first":
                first = value;
                break;
            case "second":
                second = value;
                break;
            case "third":
                third = value;
                break;
        }

        // Appliquer la rotation au GameObject
        float rotation = ScaleValue(value, 0, 4095, 45, 315);
        myTarget.transform.eulerAngles = new Vector3(0, 0, rotation);
    }
}
