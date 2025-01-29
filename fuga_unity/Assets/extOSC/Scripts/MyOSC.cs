using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using extOSC;
using Treegen; // Ajout du namespace pour acc�der aux classes de l'asset

public class MyOSC : MonoBehaviour
{
    public extOSC.OSCReceiver oscReceiver;
    public TreegenTreeGenerator treeGenerator; // R�f�rence au g�n�rateur d'arbre

    // Tableau pour stocker les valeurs re�ues (une valeur par index)
    public float[] values = new float[3]; // Ajuste la taille du tableau selon tes besoins

    public static float ScaleValue(float value, float inputMin, float inputMax, float outputMin, float outputMax)
    {
        return Mathf.Clamp(((value - inputMin) / (inputMax - inputMin) * (outputMax - outputMin) + outputMin), outputMin, outputMax);
    }

    void Start()
    {
        Debug.Log("Recherche de TreegenTreeGenerator...");
        var test = FindObjectOfType<TreegenTreeGenerator>();

        if (test != null)
            Debug.Log("TreegenTreeGenerator trouv� !");
        else
            Debug.LogError("TreegenTreeGenerator introuvable !");
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
    }

    void Update()
    {
        if (treeGenerator == null)
        {
            Debug.LogError("treeGenerator n'est pas assign� !");
            return;
        }

        // Modifier la courbure du tronc
        treeGenerator.TreeNoiseForce = values[0];

        // Modifier l'�paisseur du tronc
        treeGenerator.TrunkThickness.keys[1].value = Mathf.Clamp(values[1], 0.1f, 2.0f);

        // R�gler la taille des feuilles
        treeGenerator.LeavesScale = new Vector3(values[2], values[2], values[2]);

        // Rafra�chir la g�n�ration de l'arbre
        treeGenerator.NewGen();
    }
}
