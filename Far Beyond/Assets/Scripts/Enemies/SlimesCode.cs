using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyStates //un enum es un conjunto de valores del que solo podra elegir uno. Se escriben empezando por mayuscula
{
    Patrol, Chase, Attack //podrás elegir cualquiera de los estados que tu metas, PERO solo y exclusivamente uno. (por ejemplo, a mele y a distancia, serian dos distintos)
}

public class SlimesCode : MonoBehaviour
{
    public float chaseRange;
    public float attackRange;
    public float speedSlime = 2f;
    public List<Transform> PatrolPoints = new List<Transform>();
    int nextPatrolPoint = 0; //el contador empieza en 0, asi no nos dara problemas de movimiento.
    bool hasAttackFinished = false; //por defecto no ha acabado el ataque, asi nos sera mas facil programarlo.
    public Animator anim; //esto es por si queremos que el slime cambie de animación. Por ahora no está en uso.
    public int SlimeLife = 6;

    public EnemyStates currentState = EnemyStates.Patrol; //siempre que queramos referenciar un ENUM sera mediante .algo

    public void Start()
    {
        StartCoroutine(FMSCoroutine());
    }

    IEnumerator FMSCoroutine()
    {
        while (true) //creamos un Ienumeretor por cada accion. 
        {
            //ejecutar estados (enum)
            switch (currentState) //swich esta hecho para hacer funcionar dichos estados. 
            {
                case EnemyStates.Patrol: // si esto es patrol, ejecutamos todo lo que este dentro hasta el break. ¡
                    yield return Patrol();
                    break;
                case EnemyStates.Chase:
                    yield return Chase();
                    break;
                case EnemyStates.Attack:
                    yield return Attack();
                    break;
            }
            //ejecutar transiciones. 
            if (CurrentStateIs(EnemyStates.Patrol)) //si esta patruyando puede pasar a chase si cumple la condicion de distancia de jugador es menor que el rango de perseguir.
            {
                if (DistanceToPlayer() < chaseRange)
                {
                    SwitchStateTo(EnemyStates.Chase);
                }
            }
            else if (CurrentStateIs(EnemyStates.Chase)) //Si la distancia del jugador es mayor que el rango del perseguir, pasamos a patruyar. 
            {
                if (DistanceToPlayer() > chaseRange)
                {
                    SwitchStateTo(EnemyStates.Patrol);
                }
                else if (DistanceToPlayer() < attackRange)//si la distancia del jugador es MENOR que el rango de ataque, atacar. 
                {
                    SwitchStateTo(EnemyStates.Attack);
                }
            }
            else if (CurrentStateIs(EnemyStates.Attack))

                if (HasAttackFinished())
                {
                    SwitchStateTo(EnemyStates.Chase);
                }

        }
    }



    //si estoy en un estado, ejecuto dicho estado, y asi los tenemos totalmente separado de las demas cosas. 

    bool CurrentStateIs(EnemyStates stateToCheck) //esto chequea a cuanto esta del estado. 
    {
        return currentState == stateToCheck;
    }

    float DistanceToPlayer()
    {
        return Vector3.Distance(this.transform.position, IllayPlayer.Instance.transform.position);
    }

    void SwitchStateTo(EnemyStates newState) //esto hace un cambio de estado. nos servira para cambiar entre patruyar, atacar y perseguir. 
    {
        currentState = newState;
    }

    bool HasAttackFinished()
    {
        return hasAttackFinished;
    }

    void MoveTowardsPoint(Vector3 target) //aqui hemos agrupado el codigo de movimiento que utilizaremos para movernos, inclusive en instance del jugador. para no andar repitiendo codigo lo metemos en un metodo
    {
        if (target.x > this.transform.position.x) //si el punto esta mas a la derecha del personaje, la escala esta normal. 
        {
            this.transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else //sino, si el target esta a la izquierda de este, el enemigo se voltea en X.
        {
            this.transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        this.transform.position = Vector3.MoveTowards(this.transform.position, target, speedSlime * Time.deltaTime);
    }

    IEnumerator Patrol()
    {
        MoveTowardsPoint(PatrolPoints[nextPatrolPoint].position); //para la sirena SOLO NECESITAMOS ESTA LINEA. 
        //si hemos llegado al punto, cambiar al siguiente punto para hacer patrullaje. 
        if (Vector3.Distance(this.transform.position, PatrolPoints[nextPatrolPoint].position) < 1.0f) //que sea menor de 1 metro. habra que ajustar esta distancia para lo de patruyaje.

        {
            nextPatrolPoint++;
            if (nextPatrolPoint >= PatrolPoints.Count)
            {
                nextPatrolPoint = 0; //en caso de no haber ningun punto de patrullaje nuevo, vuelvo al del principio.
                yield return null;
            }

        }
        //aqui tendremos que asignar que si no esta dentro del rango de chase, volver a una zona de patrullaje. 
    }
    IEnumerator Chase()
    {
        MoveTowardsPoint(IllayPlayer.Instance.transform.position);
        //esto permite perseguir al jugador, mediante la velocidad asignada en las variables de arriba y luego esperar un fotograma.
        yield return null; //si el siguiente fotograma sigue en chase el jugador, perseguir. 
    }
    IEnumerator Attack()
    {
        hasAttackFinished = false;
        GetComponent<SpriteRenderer>().color = Color.green; // AQUI METEREMOS LAS ANIMACIONES por ahora cambiamos a verde
        yield return new WaitForSeconds(2.0f);
        GetComponent<SpriteRenderer>().color = Color.white; //cuando termine el ataque, vuelve a blanco (color predeterminado)
        while (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 0.98f)
        {
            yield return null;
        }
        hasAttackFinished = true;
        yield return null;
    }

    void OnTriggerEnter2D(Collider2D other) //eso nos va a permitir detectar dentro de un empty vacio con un collider que genere un area. 
    {
        IllayPlayer player = other.GetComponent<IllayPlayer>();
        IllayBullet illayBullet = other.GetComponent<IllayBullet>();
        IllayFlame illayFlame = other.GetComponent<IllayFlame>();
        if (player != null)
        {
            //this.transform.position = new Vector3(transform.position.x - 1, this.transform.position.y, this.transform.position.z); //esto hace que se mueva EL SLIME uno hacia atras cuando COLISIONAN. 
            //debera cambiarse en vez de player, bola de fuego.
            Vector3 direction = (player.transform.position - this.transform.position).normalized; //esto nos va a permitir empujar AL JUGADOR si choca contra el slime.
            player.transform.position += direction * 1.0f;//el 1.0 es la distancia que empuja al jugador, cuanto mas grande, mas le va a empujar. 
            StartCoroutine(FlashColor(player.GetComponent<SpriteRenderer>()));//aqui lo llamos como si fuese un metodo dentro de un coroutine.
                                                                              //si el jugador entra dentro del daño, el jugador se pone rojo.
        }
        if (illayBullet != null)
        {

            SlimeLife--; //si le toca la bala, -1 de vida.
            Vector3 direction = (player.transform.position - this.transform.position).normalized;
            this.gameObject.transform.position += direction * 1.0f;//el 1.0 es la distancia que empuja al jugador, cuanto mas grande, mas le va a empujar. 
            StartCoroutine(FlashColor(this.GetComponent<SpriteRenderer>()));//aqui lo llamos como si fuese un metodo dentro de un coroutine.
                                                                              //si la bala choca contra el slime, este se pondra rojo??.


            illayBullet.anim.Play("Bullet_die"); //Esto es para llamar el anim desde la bala. Como es una animacion creada en bullet la tenemos que llamar desde allí.
            Destroy(illayBullet.gameObject, 0.6f); //Esto es para que la bala deje de exixtir.
                                                   //Para que le de tiempo a hacerse la animación ponemos ese tiempo de espera antes de que muera.

            //Para que se pueda llamar las animaciones hay que arrastrar el animator de unity al script del cual llamamos por ejm aqui en el de bala o flame.
        }
        if (illayFlame != null)
        {
            SlimeLife -= 3; //si le toca la llamarada, menos 3 de vida. 
            Vector3 direction = (player.transform.position - this.transform.position).normalized;
            this.gameObject.transform.position += direction * 1.0f;//el 1.0 es la distancia que empuja al jugador, cuanto mas grande, mas le va a empujar. 
            StartCoroutine(FlashColor(this.GetComponent<SpriteRenderer>()));//aqui lo llamos como si fuese un metodo dentro de un coroutine.
                                                                            //si la bala choca contra el slime, este se pondra rojo??.

            illayFlame.anim.Play("Flame_die"); //Esto es para llamar el anim desde la bala. Como es una animacion creada en bullet la tenemos que llamar desde allí.
            Destroy(illayFlame.gameObject, 1.0f); //Esto es para que la bala deje de exixtir.
                                                   //Para que le de tiempo a hacerse la animación ponemos ese tiempo de espera antes de que muera.


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
        if (SlimeLife <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
