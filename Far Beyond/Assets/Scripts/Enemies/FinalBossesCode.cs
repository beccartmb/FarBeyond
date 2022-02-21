using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public void Update()
    {
        if (bossLife <= 5)
        {
            SceneManager.LoadScene("EndMenu"); //si le metes de hostias hasta que el boss llegue a 5 de vida, te lleva al "EndMenu".
        }
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
            this.transform.localScale = new Vector3(-2f, 2f, 1f);
        }
        else
        {
            this.transform.localScale = new Vector3(2f, 2f, 1f);
        }
        this.transform.position = Vector3.MoveTowards(this.transform.position, target, speedBoss * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision) //Esto lo ponemos aquí para que el jugador se ponga rojo cuando COLISIONE y para que muera.
    {
        IllayPlayer player = collision.collider.GetComponent<IllayPlayer>();
        IllayBullet illayBullet = collision.collider.GetComponent<IllayBullet>(); //hemos tenido que meter tambien las balas porque sino empujaban al enemigo.

        if (illayBullet != null)
        {

            bossLife--; //si le toca la bala, -1 de vida. //recuerda que en vector 3 hay que cambiar quien choca con el. 
            Vector3 direction = (illayBullet.transform.position - this.transform.position).normalized;
            this.gameObject.transform.position += direction * 1.0f;//el 1.0 es la distancia que empuja al jugador, cuanto mas grande, mas le va a empujar. 
            StartCoroutine(FlashColor(this.GetComponent<SpriteRenderer>()));//aqui lo llamos como si fuese un metodo dentro de un coroutine.
                                                                            //si la bala choca contra el slime, este se pondra rojo??.


            illayBullet.anim.Play("Bullet_die"); //Esto es para llamar el anim desde la bala. Como es una animacion creada en bullet la tenemos que llamar desde allí.
            Destroy(illayBullet.gameObject, 0.6f); //Esto es para que la bala deje de exixtir.
                                                   //Para que le de tiempo a hacerse la animación ponemos ese tiempo de espera antes de que muera.
            Die();

            //Para que se pueda llamar las animaciones hay que arrastrar el animator de unity al script del cual llamamos por ejm aqui en el de bala o flame.
        }
        if (player != null)
        {
            //this.transform.position = new Vector3(transform.position.x - 1, this.transform.position.y, this.transform.position.z); //esto hace que se mueva EL SLIME uno hacia atras cuando COLISIONAN. 
            //debera cambiarse en vez de player, bola de fuego.
            Vector3 direction = (player.transform.position - this.transform.position).normalized; //esto nos va a permitir empujar AL JUGADOR si choca contra el slime.
            player.transform.position += direction * 1.0f;//el 1.0 es la distancia que empuja al jugador, cuanto mas grande, mas le va a empujar. 
            StartCoroutine(FlashColor(player.GetComponent<SpriteRenderer>()));//aqui lo llamos como si fuese un metodo dentro de un coroutine.
                                                                              //si el jugador entra dentro del daño, el jugador se pone rojo.


            GameManager.Instance.currentSave.playerHearts--; //FIJATE EN LA LINEA DE ABAJO.
            //-------------------------------------------------------------------------------------------------------------
            //EN CASO DE QUERER QUE DEJE DE GUARDARSE EL CODIGO COMO SAVE DATA Y TE SALE ERROR, QUITA EL CURRENTSAVE. REVISA TAMBIEN EL GAME MANAGER.
            //----------------------------------------------------------------------------------------------------------------------------

        }
    }
    void OnTriggerEnter2D(Collider2D other)//esto es para las colisiones EN AREA 2D. 
    {
        secureZoneBoss nestBoss = other.GetComponent<secureZoneBoss>(); //aqui hemos detectado los colisionadores que tengan el script indicado como SecureZoneSirens.
        IllayFlame illayFlame = other.GetComponent<IllayFlame>();
        if (nestBoss != null) //null significa algo que no existe //esto permite que al colisionar contra la pared derecha. vaya a la cordenada en X -9,4 apareciendo asi por el otro lado.
        {
            secureZoneBoss = nestBoss.transform.position;                                              //cada vez que pase por un "saveZone" se guardará la nueva posicion.
        }

        
        if (illayFlame != null)
        {
            bossLife -= 3; //si le toca la llamarada, menos 3 de vida. 
            Vector3 direction = (illayFlame.transform.position - this.transform.position).normalized; //he cambiado para que la LLAMARADA lo mate. 
            this.gameObject.transform.position += direction * 1.0f;//el 1.0 es la distancia que empuja al jugador, cuanto mas grande, mas le va a empujar. 
            StartCoroutine(FlashColor(this.GetComponent<SpriteRenderer>()));//aqui lo llamos como si fuese un metodo dentro de un coroutine.
                                                                            //si la bala choca contra el slime, este se pondra rojo??.

            illayFlame.anim.Play("Flame_die"); //Esto es para llamar el anim desde la bala. Como es una animacion creada en bullet la tenemos que llamar desde allí.
            Destroy(illayFlame.gameObject, 1.0f); //Esto es para que la bala deje de exixtir.
                                                  //Para que le de tiempo a hacerse la animación ponemos ese tiempo de espera antes de que muera.
            Die();


        }
    }

    IEnumerator Patrol()
    {
        if (Vector3.Distance(this.transform.position, secureZoneBoss) > 1.0f)// que se mueva a la zona segura siempre y cuando esté a una distancia de 1 metro.
        {                                                                    // de esta forma hacemos que la zona de seguridad tenga un metro más de margen al detectar la zona segura. 
            MoveTowardsPoint(secureZoneBoss);
        }
        yield return null;
    }
    IEnumerator Chase()
    {
        if (GameManager.Instance.currentSave.playerHearts > 0)//Si ponemos esto aquí hacemos que cuando Illay esté muerto no nos ataquen y nos dejen morir sin desplazarnos.
        {
            /*yield return new WaitForSeconds(0.2f);*/
            MoveTowardsPoint(IllayPlayer.Instance.transform.position); //esto nos permitira seguir al jugador a su posicion.
            yield return null;
        }
           
    }
    IEnumerator Attack()
    {
        if (GameManager.Instance.currentSave.playerHearts > 0)//Si ponemos esto aquí hacemos que cuando Illay esté muerto no nos ataquen y nos dejen morir sin desplazarnos.
        {
            hasAttackFinished = false; //aqui empieza el ataque.
            anim.Play("Final_boss_charge");
            //GameManager.Instance.playerLife = 0; //si ponemos esto nos mata de un toque.
            hasAttackFinished = true; //y se termina el ataque.
        }

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

    public void Die()
    {
        if (bossLife <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
