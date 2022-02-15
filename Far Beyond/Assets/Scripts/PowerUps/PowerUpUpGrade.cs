using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpUpGrade : MonoBehaviour
{
    //el numero menos time.Deltatime. 
    public void Die()
    {
        Destroy(this.gameObject);
    }
    void OnTriggerEnter2D(Collider2D other) //eso nos va a permitir detectar dentro de un empty vacio con un collider que genere un area. 
    {
        IllayPlayer player = other.GetComponent<IllayPlayer>();
        if (player != null)
        {
            GameManager.Instance.staminaUpGrade += 6; //cuando se quiere añadir un numero mayor a 1 (lo que seria ++) hay que poner += X. TENDRAS 5 segundos para crecer de golpe.
        }
    }

    IEnumerator Respawn() //en vez de que muera, hacemos que se desactive y luego se active GRACIAS AL GAME MANAGER.
    {
        gameObject.SetActive(false); //esto permitira que el power up respauné.
        yield return new WaitForSeconds(12.0f);
        gameObject.SetActive(true);
    }
}
