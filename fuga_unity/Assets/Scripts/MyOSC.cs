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
        oscReceiver.Bind("/vkb_midi/9/note/36", message => MessageVolume(message));
        //oscReceiver.Bind("/second", message => TraiterMessageOSC(message, 1));
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
        float lightMath = Mathf.Lerp(0f, 5f, value / 1000f);

        treegenTreeGenerator.TreeNoiseForce = lightMath;

        treegenTreeGenerator.NewGen();

    }
    void Update()
    {


    }
}