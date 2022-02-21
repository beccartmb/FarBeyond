using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFairy : MonoBehaviour
{
    //si la bala sale de los limites, que se destruya. 
    public float speedBullet = 8.0f;
    public float lifeTime = 3.0f;
    public Animator anim;
    Vector3 direction;

    void Start()
    {
        direction = (IllayPlayer.Instance.transform.position - this.transform.position).normalized;

    }


    void Update()
    {

        this.transform.position += direction * speedBullet * Time.deltaTime;
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
        {
            anim.Play("Bullet_die");
            Destroy(this.gameObject, 0.6f);
        }
    }

    public void Die()
    {
        Destroy(this.gameObject); //esto nos permite destruir la bala. 

    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        IllayPlayer player = other.GetComponent<IllayPlayer>();
        if (player != null)
        {
            Vector3 direction = (player.transform.position - this.transform.position).normalized; //esto nos va a permitir empujar AL JUGADOR si choca contra el slime.
            player.transform.position += direction * 1.0f;//el 1.0 es la distancia que empuja al jugador, cuanto mas grande, mas le va a empujar. 
            StartCoroutine(FlashColor(player.GetComponent<SpriteRenderer>()));//aqui lo llamos como si fuese un metodo dentro de un coroutine.
                                                                              //si el jugador entra dentro del daño, el jugador se pone rojo.


            GameManager.Instance.currentSave.playerHearts--; //FIJATE EN LA LINEA DE ABAJO.

            //----------------------------------------------------------------------------------------------
            //EN CASO DE QUE TE DE ERROR, QUITA EL CURRENTSAVE, SIGNIFICA QUE SE ESTA GUARDANDO DICHA INFORMACION EN SAVE DATA. REVISA TAMBIEN EL GAME MANAGER.
            //----------------------------------------------------------------------------------------
        }
    }

    IEnumerator FlashColor(SpriteRenderer spriteRender) //esto es un coroutine que sirve para esperar X tiempo. ES COMO UN CONTADOR.
    {
        spriteRender.color = Color.red;
        yield return new WaitForSeconds(0.1f); //esto es para esperar un tiempo determinado, en este caso, 0.1f segundos.
                                               // yield return null; // Espera 1 frame
        spriteRender.color = Color.white;
    }
}
