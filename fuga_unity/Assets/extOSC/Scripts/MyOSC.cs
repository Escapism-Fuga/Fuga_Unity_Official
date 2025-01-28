using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using extOSC;


public class MyOSC : MonoBehaviour
{
    public extOSC.OSCReceiver oscReceiver;
    public extOSC.OSCTransmitter oscTransmitter;
    public GameObject myTarget;
    // Variable utilisée pour contrôler la vitesse d'envoi des messages :
    float myChronoStart;



    public static float ScaleValue(float value, float inputMin, float inputMax, float outputMin, float outputMax)
    {
        return Mathf.Clamp(((value - inputMin) / (inputMax - inputMin) * (outputMax - outputMin) + outputMin), outputMin, outputMax);
    }

    void TraiterMessageOSC(OSCMessage oscMessage)
    {
        // Récupérer une valeur numérique en tant que float
        // même si elle est de type float ou int :
        float value;
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
            // Si la valeur n'est ni un foat ou int, on quitte la méthode :
            return;
        }

        // Changer l'échelle de la valeur pour l'appliquer à la rotation :
        float rotation = ScaleValue(value, 0, 4095, 45, 315);
        // Appliquer la rotation au GameObject ciblé :
        myTarget.transform.eulerAngles = new Vector3(0, 0, rotation);
    }


    // Start is called before the first frame update
    void Start()
    {
        // Mettre cette ligne dans la méthode start()
        oscReceiver.Bind("/adresse", TraiterMessageOSC);
    }

    // LateUpdate is called once per frame after Update
    void LateUpdate()
    {
        // Si 50 millisecondes se sont écoulées depuis le dernier envoi :
        if (Time.realtimeSinceStartup - myChronoStart >= 0.05f ) 
        {
            myChronoStart = Time.realtimeSinceStartup;

            // Créer le message
            var myOscMessage = new OSCMessage("/adresse");

        
            // Aller chercher une valeur:
            float myPositionX = myTarget.transform.position.x;
            // Changer l'échelle de la valeur:
            float myScaledPositionX = ScaleValue(myPositionX, -7, 7, 0, 255);

            // Ajouter la valeur au message
            myOscMessage.AddValue(OSCValue.Int( (int) myScaledPositionX) ); // Le (int) entre parenthèses convertit le type.

            // Envoyer le message
            oscTransmitter.Send(myOscMessage);
        }
  
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
