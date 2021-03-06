using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FairyStates //recordamos de nuevo que el enum sirve para hacer estados.
{
    Patrol, Chase, Attack //si quieres mas estados, a?ade mas, podras acceder de uno en uno en ellos. 
}
public class FairyCode : MonoBehaviour
{


    public float chaseRange;
    public float attackRange;
    public float speedFairy = 8;
    bool hasAttackFinished = false; //ponemos que el ataque no esta acabado por ahora, para que se pueda activar con facilidad. 
    public Animator anim;
    public List<Transform> PatroPointsFairy = new List<Transform>(); //punto de patruyaje del hada. 
    int nextPatrolPointFairy = 0; //el contador empieza en 0 ?recuerdas?
    public int FairyLife = 16;
    public GameObject bulletFairyPrefab;
    public GameObject bulletLeftFairyPrefab;
    public Transform illayTransform; //esto lo utilizaremos para que controle en que posicion esta el jugador. 

    public FairyStates currentStates = FairyStates.Patrol; //vamos a necesitar referenciar el enum poniendo .algo

    #region SINGLETON
    public static FairyCode Instance { get; private set; }

    private void Awake() //esto junto con el intance de arriba convierte nuestro personaje en un singleton (mucho mas comodo para juegos de un jugador) ya que nos permite acceder a este codigo desde otros codigos. 
    {
        Instance = this;
    }
    #endregion


    public void Start()
    {
        anim = GetComponent<Animator>(); //llamos aqui la animacion, sera necesaria, ya sabes. 
        StartCoroutine(FMSCoroutine()); //y aqui la courtina para que siempre ejecute los enum. 
    }

    IEnumerator FMSCoroutine()
    {
        while (true) //creamos un Ienumeretor por cada acci?n.
        {
            //ahora vamos a ejecutar los estados. 
            switch (currentStates)
            {
                case FairyStates.Patrol:
                    yield return Patrol();
                    break;
                case FairyStates.Chase:
                    yield return Chase();
                    break;
                case FairyStates.Attack:
                    yield return Attack();
                    break;
            }
            //ahora vamos a ejecutar las transiciones. 
            if (CurrentStatesIs(FairyStates.Patrol))
            {
                if (DistanceToPlayer() < chaseRange)
                {
                    SwitchStateTo(FairyStates.Chase);
                }
            }
            else if (CurrentStatesIs(FairyStates.Chase))
            {
                if (DistanceToPlayer() > chaseRange)
                {
                    SwitchStateTo(FairyStates.Patrol);
                }
                else if (DistanceToPlayer() < attackRange)
                {
                    SwitchStateTo(FairyStates.Attack);
                }
            }
            else if (CurrentStatesIs(FairyStates.Attack))
            {
                if (HasAttackFinished())
                {
                    SwitchStateTo(FairyStates.Chase);
                }
            }
        }
    }
    //ahora mismo tienes infinidad de errores, dont worry. Cuando termines de programar preocupate por ellos. 

    bool CurrentStatesIs(FairyStates stateToCheck)
    {
        return currentStates == stateToCheck;
    }
    float DistanceToPlayer()
    {
        return Vector3.Distance(this.transform.position, IllayPlayer.Instance.transform.position);
    }
    void SwitchStateTo(FairyStates newState)
    {
        currentStates = newState;
    }
    bool HasAttackFinished()
    {
        return hasAttackFinished;
    }
    void MoveTowardsPoint(Vector3 target) //aqui agrupamos el codigo del movimiento del hada. 
    {

        if (target.x > this.transform.position.x) //si el punto esta mas a la derecha del personaje, la escala esta normal. 
        {
            this.transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else //sino, si el target esta a la izquierda de este, el enemigo se voltea en X.
        {
            this.transform.localScale = new Vector3(1f, 1f, 1f);
        }
        this.transform.position = Vector3.MoveTowards(this.transform.position, target, speedFairy * Time.deltaTime);
    }

    IEnumerator Patrol()
    {

        MoveTowardsPoint(PatroPointsFairy[nextPatrolPointFairy].position);

        if (Vector3.Distance(this.transform.position, PatroPointsFairy[nextPatrolPointFairy].position) < 1.0f) //que sea menor de 1 metro. habra que ajustar esta distancia para lo de patruyaje.

        {
            nextPatrolPointFairy++;
            if (nextPatrolPointFairy >= PatroPointsFairy.Count)
            {
                nextPatrolPointFairy = 0; //en caso de no haber ningun punto de patrullaje nuevo, vuelvo al del principio.
                yield return null;
            }

        }
        //aqui tendremos que asignar que si no esta dentro del rango de chase, volver a una zona de patrullaje. 
    }
    IEnumerator Chase()
    {
        if (Vector3.Distance(this.transform.position, IllayPlayer.Instance.transform.position) > 2) //aqui hemos pedido que la distancia entre PLAYER y Fairy sea al menos de 2 metros para poder hacer el perseguir
        {
            MoveTowardsPoint(IllayPlayer.Instance.transform.position);
            //esto permite perseguir al jugador mediante las velocidades impuestas en las variables de arriba.
            yield return null;
        }
    }
    IEnumerator Attack()
    {
        hasAttackFinished = false;

        if (this.transform.localScale.x > 0) //si la tranformacion del local scale EN X es mayor que 0 inmediatamente se lee como derecha. Sino, es izquierda. NECESITARAS DOS IMAGENES DISTINTAS, bala derecha y bala izquierda. 
        {
            Instantiate(bulletFairyPrefab, this.transform.position + new Vector3(0f, 0f, 0f), Quaternion.identity); //crear una bala (BulletPlayer) en la posicion en la que esta el jugador.
        }
        else //como hemos designado arriba que la derecha es mayor que 0, <0 es inmediatamente izquierda. 
        {
            Instantiate(bulletLeftFairyPrefab, this.transform.position + new Vector3(0f, 0f, 0f), Quaternion.identity); //crear una bala (BulletPlayer) en la posicion en la que esta el jugador.
             //CUIDADO CON EL NOMBRE DE LAS BALAS QUE ASIGNAMOS A CADA LADO                                            //hemos puesto que tenga un vector 3 porque la bala nos salia muy arriba, con esto la estamos desplazando un poco para que salga en donde nosotros consideramos. 
        }


        yield return new WaitForSeconds(Random.Range(2f, 6f)); //esto hara que las balas salgan aleatoriamente entre 2 y 6 segundos
        hasAttackFinished = true; //y se termina el ataque. 
        yield return null; //y volvemos a empezar. 
    }

    void OnTriggerEnter2D(Collider2D other) //eso nos va a permitir detectar dentro de un empty vacio con un collider que genere un area. 
    {
        IllayPlayer player = other.GetComponent<IllayPlayer>();
        IllayFlame illayFlame = other.GetComponent<IllayFlame>();

       
        if (illayFlame != null)
        {
            FairyLife -= 3; //si le toca la llamarada, menos 3 de vida. 
            Vector3 direction = (illayFlame.transform.position - this.transform.position).normalized; //he cambiado para que la LLAMARADA lo mate. 
            this.gameObject.transform.position += direction * 1.0f;//el 1.0 es la distancia que empuja al jugador, cuanto mas grande, mas le va a empujar. 
            StartCoroutine(FlashColor(this.GetComponent<SpriteRenderer>()));//aqui lo llamos como si fuese un metodo dentro de un coroutine.
                                                                            //si la bala choca contra el slime, este se pondra rojo??.

            illayFlame.anim.Play("Flame_die"); //Esto es para llamar el anim desde la bala. Como es una animacion creada en bullet la tenemos que llamar desde all?.
            Destroy(illayFlame.gameObject, 1.0f); //Esto es para que la bala deje de exixtir.
                                                  //Para que le de tiempo a hacerse la animaci?n ponemos ese tiempo de espera antes de que muera.
            Die();


        }
    }
    void OnCollisionEnter2D(Collision2D collision) //hemos tenido que poner LAS BALAS en collision porque tenian rigidbody y sino lo empujaba.
    {
        IllayBullet illayBullet = collision.collider.GetComponent<IllayBullet>();
        if (illayBullet != null)
        {

            FairyLife--; //si le toca la bala, -1 de vida. //recuerda que en vector 3 hay que cambiar quien choca con el. 
            Vector3 direction = (illayBullet.transform.position - this.transform.position).normalized;
            this.gameObject.transform.position += direction * 1.0f;//el 1.0 es la distancia que empuja al jugador, cuanto mas grande, mas le va a empujar. 
            StartCoroutine(FlashColor(this.GetComponent<SpriteRenderer>()));//aqui lo llamos como si fuese un metodo dentro de un coroutine.
                                                                            //si la bala choca contra el slime, este se pondra rojo??.


            illayBullet.anim.Play("Bullet_die"); //Esto es para llamar el anim desde la bala. Como es una animacion creada en bullet la tenemos que llamar desde all?.
            Destroy(illayBullet.gameObject, 0.6f); //Esto es para que la bala deje de exixtir.
                                                   //Para que le de tiempo a hacerse la animaci?n ponemos ese tiempo de espera antes de que muera.
            Die();

            //Para que se pueda llamar las animaciones hay que arrastrar el animator de unity al script del cual llamamos por ejm aqui en el de bala o flame.
        }
    }

    IEnumerator FlashColor(SpriteRenderer spriteRender) //esto es un coroutine que sirve para esperar X tiempo. ES COMO UN CONTADOR.
    {
        spriteRender.color = Color.red;
        yield return new WaitForSeconds(0.1f); //esto es para esperar un tiempo determinado, en este caso, 0.1f segundos.
                                               // yield return null; // Espera 1 frame
        spriteRender.color = Color.white;
    }

    private void OnDrawGizmos() //esto nos hace ayudas visuales para el rango de ataque y el rango de perseguir del enemigo. SOLO SIRVEN COMO AYUDA VISUAL Y NO SE VEN EN LA PANTALLA DE GAME.
    {
        Gizmos.color = new Color(1f, 0f, 0f, 0.3f); //esto es el R G B A (rojo semi transparente)
        Gizmos.DrawSphere(this.transform.position, attackRange);
        Gizmos.color = new Color(1f, 0f, 1f, 0.3f); // y esto es una especie de morado.
        Gizmos.DrawSphere(this.transform.position, chaseRange);

    }
    public void Die()
    {
        if (FairyLife < 0)
        {
            Destroy(this.gameObject);
        }
    }

}

