using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using extOSC;

public class MyOSC : MonoBehaviour
{
    public extOSC.OSCReceiver oscReceiver;
    public GameObject myTarget;

    // Tableau pour stocker les valeurs re�ues (une valeur par index)
    public float[] values = new float[3]; // Tu peux ajuster la taille du tableau selon tes besoins

    public static float ScaleValue(float value, float inputMin, float inputMax, float outputMin, float outputMax)
    {
        return Mathf.Clamp(((value - inputMin) / (inputMax - inputMin) * (outputMax - outputMin) + outputMin), outputMin, outputMax);
    }

    void Start()
    {
        // Lier les adresses OSC aux m�thodes correspondantes
        oscReceiver.Bind("/first", message => TraiterMessageOSC(message, 0)); // Index 0 pour la premi�re valeur
        oscReceiver.Bind("/second", message => TraiterMessageOSC(message, 1)); // Index 1 pour la deuxi�me valeur
        oscReceiver.Bind("/third", message => TraiterMessageOSC(message, 2)); // Index 2 pour la troisi�me valeur
    }

    void TraiterMessageOSC(OSCMessage oscMessage, int index)
    {
        float value = 0;

        // V�rifie le type de valeur dans le message OSC
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
            return; // Quitte si la valeur n'est pas valide
        }

        // Stocker la valeur dans le tableau � l'index correspondant
        if (index >= 0 && index < values.Length)
        {
            values[index] = value;
        }

        Debug.Log($"Valeur re�ue � l'index {index}: {value}");

        // Appliquer la rotation au GameObject
        float rotation = ScaleValue(value, 0, 4095, 45, 315);
        myTarget.transform.eulerAngles = new Vector3(0, 0, rotation);
    }
}
