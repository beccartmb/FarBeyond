using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FairyStates //recordamos de nuevo que el enum sirve para hacer estados.
{
    Patrol, Chase, Attack //si quieres mas estados, añade mas, podras acceder de uno en uno en ellos. 
}
public class FairyCode : MonoBehaviour
{


    public float chaseRange;
    public float attackRange;
    public float speedFairy = 8;
    bool hasAttackFinished = false; //ponemos que el ataque no esta acabado por ahora, para que se pueda activar con facilidad. 
    public Animator anim;
    public List<Transform> PatroPointsFairy = new List<Transform>(); //punto de patruyaje del hada. 
    int nextPatrolPointFairy = 0; //el contador empieza en 0 ¿recuerdas?
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
        while (true) //creamos un Ienumeretor por cada acción.
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

        MoveTowardsPoint(PatroPointsFairy[nextPatrolPointFairy].position); //para la sirena SOLO NECESITAMOS ESTA LINEA. 
        //si hemos llegado al punto, cambiar al siguiente punto para hacer patrullaje. 
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
        MoveTowardsPoint(IllayPlayer.Instance.transform.position);
        //esto permite perseguir al jugador mediante las velocidades impuestas en las variables de arriba.
        yield return null;
    }
    IEnumerator Attack()
    {
        hasAttackFinished = false;
        if (illayTransform.position.y < this.transform.position.y && bulletFairyPrefab.transform.position.y > -4.2) //este es el codigo para que el enemigo baje si la pelota esta por debajo de la posicion Y. 
        {
            this.transform.position += speedFairy * Time.deltaTime * Vector3.down;
        } //quiero que la bala vaya hacia el jugador. ¿estoy haciendo esto correctamente ?  (NO DISPARA LA BOLA)

        if (this.transform.localScale.x > 0) //si la tranformacion del local scale EN X es mayor que 0 inmediatamente se lee como derecha. Sino, es izquierda. NECESITARAS DOS IMAGENES DISTINTAS, bala derecha y bala izquierda. 
        {
            Instantiate(bulletFairyPrefab, this.transform.position + new Vector3(3f, -2f, 0f), Quaternion.identity); //crear una bala (BulletPlayer) en la posicion en la que esta el jugador.
        }
        else //como hemos designado arriba que la derecha es mayor que 0, <0 es inmediatamente izquierda. 
        {
            Instantiate(bulletLeftFairyPrefab, this.transform.position + new Vector3(-3, -2f, 0f), Quaternion.identity); //crear una bala (BulletPlayer) en la posicion en la que esta el jugador.
                                                                                                                         //hemos puesto que tenga un vector 3 porque la bala nos salia muy arriba, con esto la estamos desplazando un poco para que salga en donde nosotros consideramos. 
        }
        yield return new WaitForSeconds(1.0f);
        hasAttackFinished = true; //y se termina el ataque. 
        yield return null; //y volvemos a empezar. 
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

}

