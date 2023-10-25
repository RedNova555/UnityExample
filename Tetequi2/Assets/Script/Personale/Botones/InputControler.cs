using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputControler : MonoBehaviour
{
    //detecta si se esta usando el joystick
    public bool isUsed = false;

    //guarda el movimiento horizontal y vertical del joystick
    public Joystick _joystickMove;

    //guarda el movimiento horizontal y vertical transformado en un flooat
    public float x;
    public float z;

    private void Update()
    {
        //tomamos el movimiento del joystick
        x = _joystickMove.Horizontal;
        z = _joystickMove.Vertical;

        //comprueba si se uso el joystic
        if (x != 0 || z != 0)
        {
            
            //indica que el botton se uso
            isUsed = true;
        }
        //si se deja de usar el joistick, comprobara si el personaje se esta moviendo
        else
        {
            //indicara que ya no se esta moviendo
            isUsed = false;
        }
    }
}