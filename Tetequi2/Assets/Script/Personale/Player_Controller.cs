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

    //saber hacia donde mira la camara
    public Camera camara;

    //guardas las posiciones de la camara
    private Vector3 _camaraForward;
    private Vector3 _camaraRight;

    //saber direccion a la que se mueve el personaje en relacion con la camara
    private Vector3 _movePlayer;

    //gravedad del personaje
    public float gravity = 20;

    //indica la velozidad de caida
    private float _fallVelozity;

    //indica si esta sobre alguna plataforma inclinada
    public bool _isOnSlope = false;

    //saca la Normal del objeto en el que esta parado
    private Vector3 _hitNormal;

    //velocidad a la que decendera de una pendiente
    public float sliteVelocity = 7;
    
    //aceleracion de caida cuando se encuentra en una pendiente
    public float slopeForceDown = -10;

    //Check para saber si en 
    private float _positionComfirm;

    //fuerza de salto
    //public float jumpForce = 10;


    private void Awake()
    {
        //tomamos el character controler del jugador
        player = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //detecta si se encuentra sobre una pendiente
        if (_isOnSlope)
        {
            //detecta si el personaje esta a la misma altura para ver si es permitido moverse o no
            //(esto es debido a un error que ocuria y el personaje se congelaba y era imposile moverse)
            if (player.transform.position.y != _positionComfirm)
            {
            }
            else
            {
                //agarra y guarda el movimiento que el jugador este haciendo
                _horizontalMove = Input.GetAxis("Horizontal");
                _verticalMove = Input.GetAxis("Vertical");
            }
        }
        else
        {
            //agarra y guarda el movimiento que el jugador este haciendo
            _horizontalMove = Input.GetAxis("Horizontal");
            _verticalMove = Input.GetAxis("Vertical");  
        }

        //guardamos los valores en el vector 3 (inputJugador), para despues estabilisarlo a que el maximo de movimiento sea 1
        _inputJugador = new Vector3(_horizontalMove, 0, _verticalMove);
        _inputJugador = Vector3.ClampMagnitude(_inputJugador, 1);

        //mandamos a llamar la dirreccion del la camara
        _CamDireccion();

        //seteamos la direccion de la camara
        _movePlayer = _inputJugador.x * _camaraRight + _inputJugador.z * _camaraForward;

        //modifica la velocidad del jugador (se hace de este modo para no afectar la gravedad 2 veces)
        _movePlayer = _movePlayer * playerSpeed;
        /*
        *Si no importa modificar la gravedad, se cambia en _Mover() esta linea asi player.Move(_movePlayer * player * Time.deltaTime);
        *y se elimina rastro del movimiento de la gravedad(variables, esta linea y el _SetGravity())
        */

        //actualiza para que siempre se mueba en relacion con la camara
        player.transform.LookAt(player.transform.position + _movePlayer);

        //mandamos a llamar a _SetGravity
        _SetGravity();

        //tomamos la altura del personaje antes de actualizarse
        _positionComfirm = player.transform.position.y;

        //mandamos a llamar a Mover()
        _Mover();

        //manda a llamara el salto
        //_SatoJugador();
    }

    //saber, asia donde mira la camara para el movimiento del personaje
    void _CamDireccion()
    {
        //le ingresamos los datos de la camara
        _camaraForward = camara.transform.forward;
        _camaraRight = camara.transform.right;

        //friseamos el movimiento en y
        _camaraForward.y = 0;
        _camaraRight.y = 0;

        //normalizamos la direccion para que sea mas ajustada
        _camaraForward = _camaraForward.normalized;
        _camaraRight = _camaraRight.normalized;
    }

    //variable que ejecuta el movimiento
    void _Mover()
    {
        //actualizamos el movimiento del jugador
        player.Move(_movePlayer * Time.deltaTime);
    }

    //ejecuta la grabedad
    void _SetGravity()
    {
        //saber si esta tocando el suelo el jugador para aumentar o estabilicisar la velocidad de caida
        if (player.isGrounded)
        {
            //Una vez toque el suelo, su velocidad de caida se estabilisa
            _fallVelozity = -gravity * Time.deltaTime;
        }
        else
        {
            //mientras no toque el suelo, su velocidad de caida acelerara.
            _fallVelozity -= gravity * Time.deltaTime;
        }
        //añadimos la velocidad de caida al jugador
        _movePlayer.y = _fallVelozity;

        //manda a llamar a _SlideDown()
        _SlideDown();
    }

    //Compara si nuestro jugador esta en una rampa y ejeser las fuerzas nesesarias
    void _SlideDown()
    {
        //detecta si estamos sobre una pendiente mayor de la quel CharacterControler pueda subir 
        _isOnSlope = Vector3.Angle(Vector3.up, _hitNormal) >= player.slopeLimit;

        //en caso de estar en una pendiente mayor, hara que se resbale
        if (_isOnSlope)
        { 
            //mueve al jugador en el eje X y Z en direccion a la pendiente por la velocidad de deslizamiento
            //tambien caclcula que tan inclinada esta la pendiente y en consecuencia, se desliza mas o menos rapido
            _movePlayer.x += ((1f - _hitNormal.y) * _hitNormal.x) * sliteVelocity;
            _movePlayer.z += ((1f - _hitNormal.y) * _hitNormal.z) * sliteVelocity;

            //para evitar que de "saltitos" le mandamos una fuerza en Y
            _movePlayer.y += slopeForceDown;
        }
    }

    //detecta cuando el CharacterControler toca algo (Mientras se mueve), pero no detecta cuando algun otro objeto choca con el CharacterControler
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //almacena la normal
        _hitNormal = hit.normal;
    }

    //hara que el jugador salte
    /*
    void _SatoJugador()
    {
        //comprueba si esta en el suelo y se presiona la tecla para saltar
        if (player.isGrounded && Input.GetButtonDown("Jump"))
        {
            //se agrega a _fallVelozit debido a que debe actualizarse la elebacion, para despues calcular la gravedad en _SetGravity()
            _fallVelozity = jumpForce;

            //agrega el impulso al jugador
            _movePlayer.y = _fallVelozity;
        }
    }
    */
}
