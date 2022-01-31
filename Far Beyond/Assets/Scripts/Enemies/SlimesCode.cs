using System.Collections;
using UnityEngine;

public enum EnemyStates //un enum es un conjunto de valores del que solo podra elegir uno. Se escriben empezando por mayuscula
{
    Patrol,Chase,Attack //podrás elegir cualquiera de los estados que tu metas, PERO solo y exclusivamente uno. (por ejemplo, a mele y a distancia, serian dos distintos)
}

public class SlimesCode : MonoBehaviour
{
    public float chaseRange;
    public float attackRange;

    public EnemyStates currentState = EnemyStates.Patrol; //siempre que queramos referenciar un ENUM sera mediante .algo

    void OnTriggerEnter2D(Collider2D other) //eso nos va a permitir detectar dentro de un empty vacio con un collider que genere un area. 
    {
        IllayPlayer player = other.GetComponent<IllayPlayer>();
        IllayBullet illayBullet = other.GetComponent<IllayBullet>();
        if (player != null)
        {
            this.transform.position = new Vector3(transform.position.x-1, this.transform.position.y, this.transform.position.z); //esto hace que se mueva uno hacia atras cuando COLISIONAN. 
                                                                                                                                 //debera cambiarse en vez de player, bola de fuego.
            StartCoroutine(FlashColor(player.GetComponent<SpriteRenderer>()));//aqui lo llamos como si fuese un metodo dentro de un coroutine.
            //si el jugador entra dentro del daño, el jugador se pone rojo.
        }
        if (illayBullet != null)
        {
            StartCoroutine(FlashColor(this.GetComponent<SpriteRenderer>()));
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
}
