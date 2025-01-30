
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Rendering;
using extOSC;
using UnityEditor;
using Treegen;


public class MyOSC : MonoBehaviour
{
    // public TreegenTreeGenerator treegenTreeGenerator;
    public extOSC.OSCReceiver oscReceiver;

    public Treegen.TreegenTreeGenerator treegenTreeGenerator;
    public float[] values = new float[3];

    public static float ScaleValue(float value, float inputMin, float inputMax, float outputMin, float outputMax)
    {
        return Mathf.Clamp(((value - inputMin) / (inputMax - inputMin) * (outputMax - outputMin) + outputMin), outputMin, outputMax);
    }

    void Start()
    {
        oscReceiver.Bind("/first", message => TraiterMessageOSC(message, 0));
        oscReceiver.Bind("/second", message => TraiterMessageOSC(message, 1));
        oscReceiver.Bind("/third", message => TraiterMessageOSC(message, 2));
    }

    void TraiterMessageOSC(OSCMessage oscMessage, int index)
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
            return;
        }


        if (index >= 0 && index < values.Length)
        {
            values[index] = value;
        }

        Debug.Log($"Valeur reçue à l'index {index}: {value}");
    }
    void Update()
    {
        treegenTreeGenerator.TreeNoiseForce = values[0];
        treegenTreeGenerator.TrunkThickness.keys[1].value = Mathf.Clamp(values[1], 0.1f, 2.0f);
        treegenTreeGenerator.LeavesScale = new Vector3(values[2], values[2], values[2]);
        treegenTreeGenerator.NewGen();

    }
}
