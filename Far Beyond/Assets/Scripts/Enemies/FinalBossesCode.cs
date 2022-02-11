using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossesStates //recordamos de nuevo que el enum sirve para hacer estados.
{
    Patrol, Chase, Attack //si quieres mas estados, añade mas, podras acceder de uno en uno en ellos. 
}
public class FinalBossesCode : MonoBehaviour
{



    public float chaseRange;
    public float attackRange;
    public float speedBoss = 8f;
    public Vector3 secureZoneBoss;

    bool hasAttackFinished = false; //por defecto no ha acabado el ataque, asi nos sera mas facil programarlo.
    public Animator anim; //esto es por si queremos que el slime cambie de animación. Por ahora no está en uso.
    public int bossMaxLife = 30;
    public int bossLife = 30;


    public BossesStates currentState = BossesStates.Patrol; //siempre que queramos referenciar un enum sera mediante enum.algo

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
                case BossesStates.Patrol:
                    yield return Patrol();
                    break; //utilizaremos break para romper dichos enum
                case BossesStates.Chase:
                    yield return Chase();
                    break;
                case BossesStates.Attack:
                    yield return Attack();
                    break;
            }
            //ahora vamos a ejecutar las transiciones, hasta aqui te saldra con error "patrol" "chase" y "attack" porque no estan designados en ningun lugar. Tranquila.

            if (CurrentStatesIs(BossesStates.Patrol))
            {
                if (DistanceToPlayer() < chaseRange)
                {
                    SwitchStateTo(BossesStates.Chase);
                }
            }
            else if (CurrentStatesIs(BossesStates.Chase))
            {
                if (DistanceToPlayer() > chaseRange) //aqui hemos pedido que si illay no esta dentro del agua, QUE NO LE PERSIGA. 
                {
                    SwitchStateTo(BossesStates.Patrol);
                }
                else if (DistanceToPlayer() < attackRange)
                {
                    SwitchStateTo(BossesStates.Attack);
                }
            }
            else if (CurrentStatesIs(BossesStates.Attack))
            {
                if (HasAttackFinished())
                {
                    SwitchStateTo(BossesStates.Chase);
                }
            }
        }
    }

    //te va a seguir dando infinidad de errores CUANDO TERMINES DE PROGRAMAR PREOCUPATE POR ELLOS.

    bool CurrentStatesIs(BossesStates stateToCheck)
    {
        return currentState == stateToCheck;
    }
    float DistanceToPlayer()
    {
        return Vector3.Distance(this.transform.position, IllayPlayer.Instance.transform.position);
    }
    void SwitchStateTo(BossesStates newState) //esto hace el cambio de estado, nos servira para cambiar entre los distintos estados que tengamos (Patruyar, Perseguir y atacar.
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
        this.transform.position = Vector3.MoveTowards(this.transform.position, target, speedBoss * Time.deltaTime);
    }
    void OnTriggerEnter2D(Collider2D other)//esto es para las colisiones EN AREA 2D. 
    {
        secureZoneBoss nestBoss= other.GetComponent<secureZoneBoss>(); //aqui hemos detectado los colisionadores que tengan el script indicado como SecureZoneSirens.

        if (nestBoss != null) //null significa algo que no existe //esto permite que al colisionar contra la pared derecha. vaya a la cordenada en X -9,4 apareciendo asi por el otro lado.
        {
            secureZoneBoss = nestBoss.transform.position;                                              //cada vez que pase por un "saveZone" se guardará la nueva posicion.
        }
    }
    IEnumerator Patrol()
    {
        MoveTowardsPoint(secureZoneBoss);
        yield return null;
    }
    IEnumerator Chase()
    {
        /*yield return new WaitForSeconds(0.2f);*/
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
