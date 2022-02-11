using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class IllayPlayer : MonoBehaviour
{

    private Rigidbody2D rbody;
    public float speed = 3.0f;
    public float jumpSpeed = 5.0f;
    public float jumpSpeedWater = 2.0f;
    public GameObject IllayPrefab;
    public GameObject bulletIllayPrefab;
    public GameObject bulletLeftIllayPrefab; //esto será necesario para mover la bala del jugador hacia la izquierda. 
    public GameObject flameBulletPrefab;
    public GameObject flameObject;
    public Vector3 newSaveZone;
    Animator anim; //ESTO NOS VA A PERMITIR METER Y MODIFICAR ANIMACIONES.
    bool isInCameraZoomZone;

    #region SINGLETON
    public static IllayPlayer Instance { get; private set; }

    private void Awake() //esto junto con el intance de arriba convierte nuestro personaje en un singleton (mucho mas comodo para juegos de un jugador) ya que nos permite acceder a este codigo desde otros codigos. 
    {
        Instance = this;
    }
    #endregion

    private List<Collider2D> floors = new List<Collider2D>(); //necesaria para detectar los suelos. 
    private List<Collider2D> wallOnRight = new List<Collider2D>(); //necesaria para detectar las paredes a la derecha. 
    private List<Collider2D> wallOnLeft = new List<Collider2D>(); //necesaria para detectar las paredes a la izquierda. 

    public bool isSlidingRight; //esto me permite saber cuando esta dentro del area de deslizamiento para moverse constantemente a la derecha. 
    public bool isInWater; //aqui permite hace un codigo de booleana que detecte si esta en el agua o no. 
    public bool isInTeleportGraveyard; //aqui designamos los teleport para otras escenas. 
    public bool passedSaveZone;


    private void Start()
    {
        rbody = GetComponent<Rigidbody2D>(); //esto permite que te detecte el RIGIDBODY para detectar suelos y paredes desde el minuto 0.
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        if (isInWater) //si esta en el agua, que haga este movimiento. 
        {
            MovementIllayWater(); //AQUI SE LLAMA SI ESTA EN EL AGUA.


        }
        else //sino, que haga el movimiento que hace cuando esta en suelo.
        {
            MovementIllay();
            DigThroughFloor();
        }
        Animate();
        //aqui dentro ira el metodo de movimiento.
        Shoot();
        if (isInCameraZoomZone)
        {
            //mete la animacion grande.
            //CameraZoomZone.anim.Play("Big_camera_water");
        }
        else
        {
            //mete la animacion hacia la camara pequeña.
        }
    }

    void Animate() //Vamos a crear un void separado para configurar las animaciones. Debe ser llamado en el Update
    {
        anim = gameObject.GetComponent<Animator>(); //esto es para cambiar las animaciones, sin embargo tienes que tenerlas metidas en windows> animation >animation para que el personaje tenga todo.
                                                    //El orden es importante. Lo que vaya arriba va a tener prioridad sobre lo que va debajo.
        if (isInWater)
        {
            anim.Play("Illay_swim");
        }
        if (Mathf.Abs(rbody.velocity.y) > 0.001f) // si la velocidad en y es mayor que 0.1 significa que está saltando. 
        {
            //anim.Play("Illay_jump"); Lo he puesto en el movimiento del jugador porque como cuando nada no había suelo ambas animaciones se hacían a la vez.
            //El salto tiene prioridad porque si se está desplazando en y es siempre porque está saltando.
        }
        else if (Mathf.Abs(rbody.velocity.x) > 0.001f)
        {
            anim.Play("Illay_running");
        }
        else
        {
            anim.Play("Illay_idle"); //aqui designaremos la animacion a la que quiere cambiar. RECUERDA QUE LA VARIABLE SE TIENE QUE DECLARAR ARRIBA.
        }
    }
    #region MovementIllay


    public GameObject Illay_jump; // Aquí ponemos el sonido como un gameObject para poder meterselo en unity. En unity hemos hecho un empty al que hemos metido el sonido.
                                  // Quitar el awake en el sonido de unity porque eso hace que se ejecute nada más empezar. Hay que hacer un código en un script para matar el sonido del hierarchy.

    public void MovementIllay()
    {
        Vector2 velocity = rbody.velocity; //aqui determinamos la velocidad del movimiento. 
        rbody.gravityScale = 2.0f; //aqui designamos LA GRAVEDAD EN TIERRA. (para que no nos de problema saltar en el agua. 

        //movimiento horizontal
        if (Keyboard.current.rightArrowKey.isPressed && wallOnRight.Count == 0 || isSlidingRight) //esto me permite no volver a saltar una segunda vez, ademas, si esta dentro del area de desliz,
                                                                                                  //deslizara solo, como si estuviese pulsando siempre la tecla derecha. 
        {
            this.transform.localScale = new Vector3(1f, transform.localScale.y, transform.localScale.z); //esto lo ponemos para que gira de derecha a izquierda el vector, volteandolo.
            velocity.x = speed; //esto me permite MOVERME en 2D, dado que NO ES IGUAL que en 3D. 

        }
        else if (Keyboard.current.leftArrowKey.isPressed && wallOnLeft.Count == 0) //esto me permite no volver a saltar una segunda vez. 
        {
            this.transform.localScale = new Vector3(-1f, transform.localScale.y, transform.localScale.z);
            velocity.x = -speed; //necesitamos que sea negativo para que te muevas hacia la izquierda, es la manera de moverte en 2D. 
        }

        else
        {
            velocity.x *= 0.9f; //esto hace que cada frame que no se mueve, se deslice menos. Si el numero es mayor (el 0.9) el deslizamiento es MAYOR.
        }


        //movimiento del salto. 
        if (Keyboard.current.spaceKey.wasPressedThisFrame && floors.Count > 0) //cuando pulse la tecla espacio y el numero de suelos sea MAYOR que 0, SALTA!
        {
            velocity.y = jumpSpeed;
            Instantiate(Illay_jump); //Esto es para que cuando se pulse el salto se reproduzca el sonido.
            anim.Play("Illay_jump");
        }

        rbody.velocity = velocity; //esto lo utilizamos para RESETEAR el movimiento, es decir, que si no estas pulsando las flechas, que no se mueva. 


    }
    #endregion
    public void MovementIllayWater()
    {
        rbody.gravityScale = 0.099f; //esto significa que dentro del agua NO VA A HABER GRAVEDAD. 
        Vector2 velocity = rbody.velocity; //aqui volvemos a determinar la velocidad para el movimiento. 
        if (GameManager.Instance.staminaO2 <= 0) //si ILLAY entra dentro del agua y la estamina esta a 0, muere y le manda a la zona segura. 
        {
            //ESTO ME PERMITIRA RESTARLE UNO DE VIDA Y 
            GameManager.Instance.playerLife--;
            transform.position = newSaveZone;
            rbody.velocity = Vector2.zero;
            GameManager.Instance.staminaO2 = 0.1f; //esto hara que vuelvas a tener estamina. hara que no mueran 18 veces seguidas. 
        }
        else
        {
            GameManager.Instance.staminaO2 -= Time.deltaTime;
            if (Keyboard.current.rightArrowKey.isPressed) //esto me permite no volver a saltar una segunda vez. 
            {
                this.transform.localScale = new Vector3(1f, transform.localScale.y, transform.localScale.z);
                velocity.x = speed; //esto me permite MOVERME en 2D, dado que NO ES IGUAL que en 3D. 
            }
            else if (Keyboard.current.leftArrowKey.isPressed) //esto me permite no volver a saltar una segunda vez. 
            {
                this.transform.localScale = new Vector3(-1f, transform.localScale.y, transform.localScale.z); //Esto es para que se gire.
                velocity.x = -speed; //necesitamos que sea negativo para que te muevas hacia la izquierda, es la manera de moverte en 2D. 

            }
            else if (Keyboard.current.upArrowKey.isPressed) //esto me permite no volver a saltar una segunda vez. 
            {
                velocity.y = speed; //necesitamos que sea negativo para que te muevas hacia la izquierda, es la manera de moverte en 2D. 
            }
            else
            {
                velocity.x *= 0.99f; //esto hace que cada frame que no se mueve, se deslice menos. Si el numero es mayor (el 0.9) el deslizamiento es MAYOR.
            }

            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                velocity.y = jumpSpeedWater;
            }
            rbody.velocity = velocity;
        }
    }
    #region GoThrought //CODIGO PARA ATRAVESAR PLATAFORMAS. MEDIANTE "PlatforEffector2D"
    public void DigThroughFloor() //para que pueda atravesar ciertos suelos, como por ejemplo, escaleras una detras de otras. 
    {
        if (Keyboard.current.downArrowKey.wasPressedThisFrame)
        {
            List<Collider2D> floorWithEffectors = new List<Collider2D>();
            foreach (Collider2D floor in floors)
            {
                if (floor.GetComponent<PlatformEffector2D>() != null)//aqui esta buscando todos los suelos que tengan el efecto "plataformEffector2D" y los añade a la lista nueva.
                {
                    floorWithEffectors.Add(floor);
                }
            }
            foreach (Collider2D floorWithEffector in floorWithEffectors)
            {
                floorWithEffector.enabled = false; //esto sirve para DESACTIVAR EL COLLIDER
                StartCoroutine(ReactivateFloorCollider(floorWithEffector)); //esto, como esta designado en IENUMERATOR, nos permite restablecer el collider en 0,25 segundos.
            }
        }
    }
    IEnumerator ReactivateFloorCollider(Collider2D collider) //esto es una cortina que nos permite reactivar los collider en 0,5 segundos. LUEGO HAY QUE LLAMARLA AL TERMINAR LO DEL FLOOR WITH EFFECTOR
    {
        yield return new WaitForSeconds(0.5f);
        collider.enabled = true; //esto sirve PARA ACTIVAR EL COLLIDER.
    }
    #endregion 

    private void OnCollisionEnter2D(Collision2D collision) //esto me va a permitir AÑADIR A LA LISTA cuando el jugador TOQUE y pase lo que hay dentro del IF.
    {
        if (collision.GetContact(0).normal.y > 0.5f) //ponemos "0" al lado de contact porque recuerda que en programacion se empieza a contar desde 0. 
        {
            floors.Add(collision.collider);
        }
        if (collision.GetContact(0).normal.x < -0.5f && collision.collider.GetComponent<PlatformEffector2D>() == null) //Aqui usamos normal.x porque determinamos que es la pared DERECHA mediante el rebote que haria al chocar con ella. 
        {
            wallOnRight.Add(collision.collider); //como es la derecha, rebotaria hacia la izquierda, por eso, el 0.5 ESTA EN NEGATIVO. 
        }
        if (collision.GetContact(0).normal.x > +0.5f && collision.collider.GetComponent<PlatformEffector2D>() == null) //Aqui usamos normal.x porque determinamos que es la pared IZQUIERDA mediante el rebote que haria al chocar con ella. SI NO TIENE EFFECTOR PLATAFORM SE CONSIDERA SUELO O PARED
        {
            wallOnLeft.Add(collision.collider); //como es la izquierda, rebotaria hacia la derecha, por eso, el 0.5 ESTA EN POSITIVO. 
        }
    }

    #region SaveZone y DeathZone
    void OnTriggerEnter2D(Collider2D other)//esto es para las colisiones EN AREA 2D. 
    {
        SaveZone saveZone = other.GetComponent<SaveZone>(); //aqui hemos detectado los colisionadores que tengan el script indicado como deathzone y saveZone.
        DeathZone deathZone = other.GetComponent<DeathZone>();
        InWater inWater = other.GetComponent<InWater>();
        CameraZoomZone cameraZoomOut = other.GetComponent<CameraZoomZone>();


        if (saveZone != null) //null significa algo que no existe //esto permite que al colisionar contra la pared derecha. vaya a la cordenada en X -9,4 apareciendo asi por el otro lado.
        {
            newSaveZone = saveZone.transform.position; //hemos hecho una variable ARRIBA que me permite guardar la informacion del vector 3 del objeto. 
            //cada vez que pase por un "saveZone" se guardará la nueva posicion.
        }
        if (deathZone != null)
        {
            GameManager.Instance.playerLife--; //aqui  hemos designado mediante el GAME MANAGER que si toca la deathzone, te reste uno de vida.
            transform.position = newSaveZone; //si toca una deathzone que tenga metido el script, te llevara a la posicion del ultimo SaveZone guardado por la variable de arriba.
            rbody.velocity = Vector2.zero; //asi quitamos la deceleracion para que no se te cuele por el mapa.
        }
    }
    //LA FALTA DE ESTAMINA DENTRO DEL AGUA ESTÁ DENTRO DEL MOVIMIENTO EN EL AGUA DE ILLAY. 


    #endregion
    //ontrigger sirve para cuando los objetos se van a atravesar y oncollision cuando colisionan y se empujan. (ambos deberan tener collision)


    private void OnCollisionExit2D(Collision2D collision) //esto permite que cuando NO ESTES CHOCANDO CONTRA DICHO OBJETO, se quite de la lista. 
    {
        floors.Remove(collision.collider);
        wallOnRight.Remove(collision.collider);
        wallOnLeft.Remove(collision.collider);
    }
    void Shoot()
    { //si se echa para atras el jugador al disparar, mueve la bala, el collider esta haciendo que se mueva. 
        if (Keyboard.current.wKey.wasPressedThisFrame) //PARA DISPARAR TECLA W.
        {
            if (this.transform.localScale.x > 0) //si la tranformacion del local scale EN X es mayor que 0 inmediatamente se lee como derecha. Sino, es izquierda. NECESITARAS DOS IMAGENES DISTINTAS, bala derecha y bala izquierda. 
            {
                Instantiate(bulletIllayPrefab, this.transform.position + new Vector3(0f, 0f, 0f), Quaternion.identity); //crear una bala (BulletPlayer) en la posicion en la que esta el jugador.
            }
            else //como hemos designado arriba que la derecha es mayor que 0, <0 es inmediatamente izquierda. 
            {
                Instantiate(bulletLeftIllayPrefab, this.transform.position + new Vector3(0, 0f, 0f), Quaternion.identity); //crear una bala (BulletPlayer) en la posicion en la que esta el jugador.
                                                                                                                           //hemos puesto que tenga un vector 3 porque la bala nos salia muy arriba, con esto la estamos desplazando un poco para que salga en donde nosotros consideramos. 
            }

        }
        //las rotaciones se hablan con Quaternion, el identity que le sigue es la rotación por defecto.

        if (Keyboard.current.eKey.isPressed && GameManager.Instance.stamina > 0) //si la municion de la estamina es 1, disparar MANTENIENDO PULSADO PARA GASTARSE. 
        {
            //FlameShoot();

            flameObject.SetActive(true); //Esto nos sirve para que no salga de dentro sino que active la animacion mediante una imagen ya impuesta. 
            GameManager.Instance.stamina -= Time.deltaTime; //que al disparar se reste una. QUIERO PONERLE TIEMPO A DICHO DISPARO
        }
        else
        {
            flameObject.SetActive(false); //cuando no tienes estamina ni tampoco mantienes pulsada la E cuando la tienes, la animacion para. 
        }
    }
    void FlameShoot()
    {
        Instantiate(flameBulletPrefab, this.transform.position + new Vector3(6f, -4f, 0f), Quaternion.identity); //designamos el rayo como bala en RayShoot.
        //aqui hemos designado, al igual que en bala, la posicion de la llamarada dado que nos salia muy arriba. 

    }

    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); //
    }
    //animator.play para acceder a las animaciones. 
}
