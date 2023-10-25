using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Push_RiguidBody : MonoBehaviour
{
    //fuerza con la que empujara a los objetos
    public float pushPower = 2f;

    //detecta el nivel de masa del ojeto para saber como lo va a recorrer
    private float _targetMass;

    //en el momento en que toque un objeto, se activara esta funcion
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //guarda el Rigidbody del objeto donde colisiono
        Rigidbody body = hit.collider.attachedRigidbody;

        //detecta si el objeto con el que coliciono tiene Rigidbody
        //en caso de que no, o el riguidbody es Kinematic, no hara nada
        if (body == null || body.isKinematic)
        {
            //al no cumplirse la condicion, acaba esta funcion (OnControllerColliderHit(ControllerColliderHit hit))
            return;
        }

        //si tocamos un objeto mientras nosotros nos movemos asia abajo, no hara nada
        if (hit.moveDirection.y < -0.3)
        {
            //al no cumplirse la condicion, acaba esta funcion (OnControllerColliderHit(ControllerColliderHit hit))
            return;
        }

        //tomamos la masa del obeto
        _targetMass = body.mass;

        //guardamos la direccion a la que estamos empujando un objeto
        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

        //mueve el objeto dependiendo de su masa
        body.velocity = pushDir * pushPower / _targetMass;
    }
}
