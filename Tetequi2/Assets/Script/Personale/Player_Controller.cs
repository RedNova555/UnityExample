using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    //Guarda el movimiento vertical y horizontal
    private float _horizontalMove;
    private float _verticalMove;

    //almasena el input del jugador
    private Vector3 _inputJugador;

    //poder mover el charactercontroller del personaje
    public CharacterController player;

    //velocidad de movimiento del jugador
    public float playerSpeed = 10;

    //saber hacia donde mira el punto de anclaje
    public Camera anclaje;

    //guardas las posiciones del punto de anclaje
    private Vector3 _anclajeDelante;
    private Vector3 _anclajeDerecha;

    //saber direccion a la que se mueve el personaje en relacion con el punto de anclaje
    private Vector3 _movePlayer;

    private void Awake()
    {
        player = GetComponent<CharacterController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //agarra y guarda el movimiento que el jugador este haciendo
        _horizontalMove = Input.GetAxis("Horizontal");
        _verticalMove = Input.GetAxis("Vertical");

        //guardamos los valores en el vector 3 (inputJugador), para despues estabilisarlo a que el maximo de movimiento sea 1
        _inputJugador = new Vector3(_horizontalMove, 0, _verticalMove);
        _inputJugador = Vector3.ClampMagnitude(_inputJugador, 1);

        //mandamos a llamar la dirreccion del punto de anclaje
        perDireccion();

        //seteamos la direccion del punto de anclaje
        _movePlayer = _inputJugador.x * _anclajeDerecha + _inputJugador.z * _anclajeDelante;

        player.transform.LookAt(player.transform.position + _movePlayer);

        //mandamos a llamar a Mover enviandole un vector 3
        Mover(_movePlayer);
    }

    //variable que ejecuta el movimiento
    void Mover(Vector3 movilidadJugador)
    {
        //actualizamos el movimiento del jugador
        player.Move(movilidadJugador * playerSpeed * Time.deltaTime);
    }

    //saber, asia donde mira el punto de anclaje para el movimiento del personaje
    void perDireccion()
    {
        //le ingresamos los datos del punto de anclaje
        _anclajeDelante = anclaje.transform.forward;
        _anclajeDerecha = anclaje.transform.right;

        //freseamos el movimiento en y
        _anclajeDelante.y = 0;
        _anclajeDerecha.y = 0;

        //normalizamos la direccion para que sea mas ajustada
        _anclajeDelante = _anclajeDelante.normalized;
        _anclajeDerecha = _anclajeDerecha.normalized;
    }
}
