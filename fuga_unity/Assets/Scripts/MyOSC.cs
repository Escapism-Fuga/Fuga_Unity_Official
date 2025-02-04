
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
        //oscReceiver.Bind("/first", message => MessageVolume(message));
        //oscReceiver.Bind("/second", message => MessagePremValeur(message));
        //oscReceiver.Bind("/third", message => TraiterMessageOSC(message, 2));
    }

    void MessageVolume(OSCMessage oscMessage)
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

        Debug.Log("Volume: " + value);

        treegenTreeGenerator.TreeNoiseForce = value;
        //treegenTreeGenerator.TrunkThickness.keys[1].value = Mathf.Clamp(value, 0.1f, 2.0f);
        //treegenTreeGenerator.LeavesScale = new Vector3(value, 50, 10);

         treegenTreeGenerator.NewGen();  

    }
    void MessagePremValeur(OSCMessage oscMessage)
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

        Debug.Log("Autre Valeur: " + value);

        //treegenTreeGenerator.TreeNoiseForce = value;
        treegenTreeGenerator.TrunkThickness.keys[1].value = Mathf.Clamp(value, 0.1f, 2.0f);
        //treegenTreeGenerator.LeavesScale = new Vector3(value, 50, 10);


    }
    void Update()
    {
        

    }
}
