using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpO2 : MonoBehaviour
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
            Die();
            GameManager.Instance.staminaO2+=6; //cuando se quiere añadir un numero mayor a 1 (lo que seria ++) hay que poner += X. TENDRAS 5 segundos para nadar a toda pastilla .
        }
    }
}
