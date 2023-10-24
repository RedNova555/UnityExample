using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camara_folower : MonoBehaviour
{
    //tomamos el transform del personaje
    private Transform _personaje;

    //toma el transform actual de la camara
    private Transform _camara;

    //distancia entre camara y jugador
    private Vector3 _distancia;

    //simulador de lag en frames
    [Range (0, 1)]public float velocidadMovimiento = 0.006f;

    void Awake()
    {
        //tomamos el transfom de la camara
        _camara = GetComponent<Transform>();

        //hacemos que tome el transforme del personaje
        _personaje = GameObject.Find("Player").transform;
    }

    void Start()
    {
        //actualizamos los valores de la distancia que existe entre la camara y el jugador
        _distancia = new Vector3(_camara.position.x - _personaje.position.x, _camara.position.y - _personaje.position.y, _camara.position.z - _personaje.position.z);
    }

    private void LateUpdate()
    {
        //para que mire al jugador
        transform.LookAt(_personaje);

        //actualizamos la posicion de la camara
        transform.position = Vector3.Lerp(transform.position, _personaje.position + _distancia, velocidadMovimiento);
    }
}
