using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    //Guarda el movimiento vertical y horizontal
    private float horizontalMove;
    private float verticalMove;

    //almasena el input del jugador
    private Vector3 inputJugador;

    //poder mover el charactercontroller del personaje
    public CharacterController player;

    //velocidad de movimiento del jugador
    public float playerSpeed = 10;

    //saber hacia donde mira el punto de anclaje
    public Camera anclaje;

    //guardas las posiciones del punto de anclaje
    private Vector3 anclajeDelante;
    private Vector3 anclajeDerecha;

    //saber direccion a la que se mueve el personaje en relacion con el punto de anclaje
    private Vector3 movePlayer;

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
        horizontalMove = Input.GetAxis("Horizontal");
        verticalMove = Input.GetAxis("Vertical");

        //guardamos los valores en el vector 3 (inputJugador), para despues estabilisarlo a que el maximo de movimiento sea 1
        inputJugador = new Vector3(horizontalMove, 0, verticalMove);
        inputJugador = Vector3.ClampMagnitude(inputJugador, 1);

        //mandamos a llamar la dirreccion del punto de anclaje
        perDireccion();

        //seteamos la direccion del punto de anclaje
        movePlayer = inputJugador.x * anclajeDerecha + inputJugador.z * anclajeDelante;

        player.transform.LookAt(player.transform.position + movePlayer);

        //mandamos a llamar a Mover enviandole un vector 3
        Mover(movePlayer);
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
        anclajeDelante = anclaje.transform.forward;
        anclajeDerecha = anclaje.transform.right;

        //freseamos el movimiento en y
        anclajeDelante.y = 0;
        anclajeDerecha.y = 0;

        //normalizamos la direccion para que sea mas ajustada
        anclajeDelante = anclajeDelante.normalized;
        anclajeDerecha = anclajeDerecha.normalized;
    }
}
