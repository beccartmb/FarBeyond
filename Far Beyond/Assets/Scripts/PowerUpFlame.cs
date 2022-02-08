using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpFlame : MonoBehaviour
{
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
            GameManager.Instance.stamine++; //cuando el powe up muere, stamine + 1 con la posibilidad de llegar a 4. 
        }
    }
}

