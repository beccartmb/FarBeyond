using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportNextScene : MonoBehaviour
{
    public string nextScene;

    void OnTriggerEnter2D(Collider2D other) //eso nos va a permitir detectar dentro de un empty vacio con un collider que genere un area. 
    {
        IllayPlayer player = other.GetComponent<IllayPlayer>();
        if (player != null)
        {
            SceneManager.LoadScene(nextScene); //este teleport nos sirve para todas las escenas dado que hemos asignado arriba que se pueda cargar un string (arrastrandolo porque es publico)
        }
    }
}
