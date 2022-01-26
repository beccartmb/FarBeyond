using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportEndGraveyard : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other) //eso nos va a permitir detectar dentro de un empty vacio con un collider que genere un area. 
    {
        IllayPlayer player = other.GetComponent<IllayPlayer>();
        if (player != null)
        {
            player.isInTeleportGraveyard = true;//¿está dentro del teleport? SI, pues me teletransporto. Sino nada.  
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        IllayPlayer player = other.GetComponent<IllayPlayer>();
        if (player != null)
        {
            player.isInTeleportGraveyard = false;//si no esta dentro del teleport, no me teletransporto. 
        }
    }
}
