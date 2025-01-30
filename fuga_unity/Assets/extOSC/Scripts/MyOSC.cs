using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using extOSC;

public class MyOSC : MonoBehaviour
{
    public extOSC.OSCReceiver oscReceiver;

    // Tableau pour stocker les valeurs reçues (une valeur par index)
    public float[] values = new float[3]; // Ajuste la taille du tableau selon tes besoins

    public static float ScaleValue(float value, float inputMin, float inputMax, float outputMin, float outputMax)
    {
        return Mathf.Clamp(((value - inputMin) / (inputMax - inputMin) * (outputMax - outputMin) + outputMin), outputMin, outputMax);
    }

    void Start()
    {
        Debug.Log("Recherche de TreegenTreeGenerator...");

        
        // Lier les adresses OSC aux méthodes correspondantes
        oscReceiver.Bind("/first", message => TraiterMessageOSC(message, 0)); // Index 0 pour la première valeur
        oscReceiver.Bind("/second", message => TraiterMessageOSC(message, 1)); // Index 1 pour la deuxième valeur
        oscReceiver.Bind("/third", message => TraiterMessageOSC(message, 2)); // Index 2 pour la troisième valeur
    }

    void TraiterMessageOSC(OSCMessage oscMessage, int index)
    {
        float value = 0;

        // Vérifie le type de valeur dans le message OSC
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

        // Stocker la valeur dans le tableau à l'index correspondant
        if (index >= 0 && index < values.Length)
        {
            values[index] = value;
        }

        Debug.Log($"Valeur reçue à l'index {index}: {value}");
    }

   
}
