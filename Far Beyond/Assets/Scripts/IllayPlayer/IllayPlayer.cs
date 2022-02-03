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
    public Vector3 newSaveZone;
    private Animation anim; //ESTO NOS VA A PERMITIR METER Y MODIFICAR ANIMACIONES.

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
    }
    private void Update()
    {
        if (isInWater) //si esta en el agua, que haga este movimiento. 
        {
            MovementIllayWater();
        }
        else //sino, que haga el movimiento que hace cuando esta en suelo.
        {
            MovementIllay();
            DigThroughFloor();
        }
        //aqui dentro ira el metodo de movimiento. 
    }
    #region MovementIllay
    public void MovementIllay()
    {
        Vector2 velocity = rbody.velocity; //aqui determinamos la velocidad del movimiento. 
        rbody.gravityScale = 2.0f; //aqui designamos LA GRAVEDAD EN TIERRA. (para que no nos de problema saltar en el agua. 

        
        if (!Keyboard.current.rightArrowKey.isPressed && !Keyboard.current.leftArrowKey.isPressed) //cuando ponemos la EXCLAMACION antes de algo, estamos poniendo que no se haga dicha cosa.
        {
            anim = gameObject.GetComponent<Animation>(); //esto es para cambiar las animaciones, sin embargo tienes que tenerlas metidas en windows> animation >animation para que el personaje tenga todo.
            anim.Play("Fire_movement"); //aqui designaremos la animacion a la que quiere cambiar. RECUERDA QUE LA VARIABLE SE TIENE QUE DECLARAR ARRIBA.
        }

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
        }
        rbody.velocity = velocity; //esto lo utilizamos para RESETEAR el movimiento, es decir, que si no estas pulsando las flechas, que no se mueva. 


    }
    #endregion
    public void MovementIllayWater()
    {
        rbody.gravityScale = 0.099f; //esto significa que dentro del agua NO VA A HABER GRAVEDAD. 
        Vector2 velocity = rbody.velocity; //aqui volvemos a determinar la velocidad para el movimiento. 

        if (Keyboard.current.rightArrowKey.isPressed) //esto me permite no volver a saltar una segunda vez. 
        {
            velocity.x = speed; //esto me permite MOVERME en 2D, dado que NO ES IGUAL que en 3D. 
        }
        else if (Keyboard.current.leftArrowKey.isPressed) //esto me permite no volver a saltar una segunda vez. 
        {
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

    #endregion
    //ontrigger sirve para cuando los objetos se van a atravesar y oncollision cuando colisionan y se empujan. (ambos deberan tener collision)


    private void OnCollisionExit2D(Collision2D collision) //esto permite que cuando NO ESTES CHOCANDO CONTRA DICHO OBJETO, se quite de la lista. 
    {
        floors.Remove(collision.collider);
        wallOnRight.Remove(collision.collider);
        wallOnLeft.Remove(collision.collider);
    }

    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); //
    }
    //animator.play para acceder a las animaciones. 
}
