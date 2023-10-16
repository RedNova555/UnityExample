using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camara_punto_de_anclaje : MonoBehaviour
{
    //tomamos el transform del personaje
    public Transform personaje;

    //toma el transform actual de la camara
    private Transform camara;

    //posicion actualizada
    private Vector3 posicionActualizada;
    
    //posicion de distancia entre la camara y el jugador
    private float posicionX;
    private float posicionZ;

    void Awake()
    {
        //tomamos el transfom de la camara
        camara = GetComponent<Transform>();
    }

    void Start()
    {
        //medimos la diferencia de distancia entre el punto de anclaje y el personaje
        posicionX = personaje.position.x - camara.position.x;
        posicionZ = personaje.position.z - camara.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        //tomamos los datos de la nueva posicion actualizada
        posicionActualizada = new Vector3(personaje.position.x - posicionX, personaje.position.y, personaje.position.z - posicionZ);
    }

    private void LateUpdate()
    {
        //actualiza la posicion
        camara.position = posicionActualizada;
    }
}
