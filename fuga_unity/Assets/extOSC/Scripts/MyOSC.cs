using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using extOSC;
using Treegen; // Ajout du namespace pour accéder aux classes de l'asset

public class MyOSC : MonoBehaviour
{
    public extOSC.OSCReceiver oscReceiver;
    public TreegenTreeGenerator treeGenerator; // Référence au générateur d'arbre

    // Tableau pour stocker les valeurs reçues (une valeur par index)
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
            Debug.Log("TreegenTreeGenerator trouvé !");
        else
            Debug.LogError("TreegenTreeGenerator introuvable !");
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

    void Update()
    {
        if (treeGenerator == null)
        {
            Debug.LogError("treeGenerator n'est pas assigné !");
            return;
        }

        // Modifier la courbure du tronc
        treeGenerator.TreeNoiseForce = values[0];

        // Modifier l'épaisseur du tronc
        treeGenerator.TrunkThickness.keys[1].value = Mathf.Clamp(values[1], 0.1f, 2.0f);

        // Régler la taille des feuilles
        treeGenerator.LeavesScale = new Vector3(values[2], values[2], values[2]);

        // Rafraîchir la génération de l'arbre
        treeGenerator.NewGen();
    }
}
