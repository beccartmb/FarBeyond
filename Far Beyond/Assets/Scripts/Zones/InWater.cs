using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InWater : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other) //eso nos va a permitir detectar dentro de un empty vacio con un collider que genere un area. 
    {
        IllayPlayer player = other.GetComponent<IllayPlayer>();
        if (player != null)
        {
            player.isInWater = true;//si esta dentro del agua. 
            player.GetComponent<Rigidbody2D>().velocity /= 5.0f; //aqui detectamos que si entra al agua desde un lugar muy alto, que la velocidad se divida entre 5.
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        IllayPlayer player = other.GetComponent<IllayPlayer>();
        if (player != null)
        {
            player.isInWater = false;//si no esta en el agua.
            player.GetComponent<Rigidbody2D>().velocity = Vector2.up * 10.0f; //aqui detectamos que cuando este fuera del agua en el movimiento del agua, que su salto se multiplique por 10. 
        }
    }
}
