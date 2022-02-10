using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SirensStates//es un enum, recuerda que sirve para hacer estados, en nuestro caso seria, patruyar, atacar (que te quite vida) y perseguir.
{
    Patrol, Chase, Attack //podras elegir cualquiera de los estados, pero solo podras acceder de uno en uno. 
}

public class SirensCode : MonoBehaviour
{
    public float chaseRange;
    public float attackRange;
    public float speedSirens = 3f;
    bool hasAttackFinished = false; // de manera predeterminada siempre estara en false, hasta que ataque que se activara el true.
    public Animator anim;
    public Vector3 secureZoneSirens;


    public SirensStates currentState = SirensStates.Patrol; //siempre que queramos referenciar un enum sera mediante enum.algo

    public void Start()
    {
        anim = GetComponent<Animator>(); //para que se ejecuten todas las animaciones nada mas empezar debera de estar esto aqui. 
        StartCoroutine(FMSCoroutine());
    }

    IEnumerator FMSCoroutine()
    {
        //vamos a meter whiles y switch para llamar a todos los estados enum.
        while (true)
        {
            switch (currentState) //sin esto no podras llamar a ningun state. asi que por favor, ponlo. 
            {
                case SirensStates.Patrol:
                    yield return Patrol();
                    break; //utilizaremos break para romper dichos enum
                case SirensStates.Chase:
                    yield return Chase();
                    break;
                case SirensStates.Attack:
                    yield return Attack();
                    break;
            }
            //ahora vamos a ejecutar las transiciones, hasta aqui te saldra con error "patrol" "chase" y "attack" porque no estan designados en ningun lugar. Tranquila.

            if (CurrentStatesIs(SirensStates.Patrol))
            {
                if (DistanceToPlayer() < chaseRange && IllayPlayer.Instance.isInWater)
                {
                    SwitchStateTo(SirensStates.Chase);
                }
            }
            else if (CurrentStatesIs(SirensStates.Chase))
            {
                if (DistanceToPlayer() > chaseRange || !IllayPlayer.Instance.isInWater) //aqui hemos pedido que si illay no esta dentro del agua, QUE NO LE PERSIGA. 
                {
                    SwitchStateTo(SirensStates.Patrol);
                }
                else if (DistanceToPlayer() < attackRange)
                {
                    SwitchStateTo(SirensStates.Attack);
                }
            }
            else if (CurrentStatesIs(SirensStates.Attack))
            {
                if (HasAttackFinished())
                {
                    SwitchStateTo(SirensStates.Chase);
                }
            }
        }
    }

    //te va a seguir dando infinidad de errores CUANDO TERMINES DE PROGRAMAR PREOCUPATE POR ELLOS.

    bool CurrentStatesIs(SirensStates stateToCheck)
    {
        return currentState == stateToCheck;
    }
    float DistanceToPlayer()
    {
        return Vector3.Distance(this.transform.position, IllayPlayer.Instance.transform.position);
    }
    void SwitchStateTo(SirensStates newState) //esto hace el cambio de estado, nos servira para cambiar entre los distintos estados que tengamos (Patruyar, Perseguir y atacar.
    {
        currentState = newState;
    }
    bool HasAttackFinished()
    {
        return hasAttackFinished;
    }
    void MoveTowardsPoint(Vector3 target) //aqui agrupamos el movimiento de la sirena. Lo llamaremos posteriormente, no te preocupes. 
    {
        if (target.x > this.transform.position.x) //si el punto esta mas a la derecha que dicho personaje, la escala estara en positivo.
        {
            this.transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else
        {
            this.transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        this.transform.position = Vector3.MoveTowards(this.transform.position, target, speedSirens * Time.deltaTime);
    }
    void OnTriggerEnter2D(Collider2D other)//esto es para las colisiones EN AREA 2D. 
    {
        SecureZoneSirens nestSiren = other.GetComponent<SecureZoneSirens>(); //aqui hemos detectado los colisionadores que tengan el script indicado como SecureZoneSirens.

        if (nestSiren != null) //null significa algo que no existe //esto permite que al colisionar contra la pared derecha. vaya a la cordenada en X -9,4 apareciendo asi por el otro lado.
        {
            secureZoneSirens = nestSiren.transform.position; //hemos hecho una variable ARRIBA que me permite guardar la informacion del vector 3 del objeto. 
                                                             //cada vez que pase por un "saveZone" se guardará la nueva posicion.
        }
    }
    IEnumerator Patrol()
    {
        MoveTowardsPoint(secureZoneSirens);
        yield return null;
    }
    IEnumerator Chase()
    {
        MoveTowardsPoint(IllayPlayer.Instance.transform.position); //esto nos permitira seguir al jugador a su posicion.
        yield return null;
    }
    IEnumerator Attack()
    {
        hasAttackFinished = false; //aqui empieza el ataque.
        GameManager.Instance.playerLife = 0; //lo mata.
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




