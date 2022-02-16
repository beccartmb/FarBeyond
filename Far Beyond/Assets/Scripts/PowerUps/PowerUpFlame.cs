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
            GameManager.Instance.currentSave.stamina++; //cuando el powe up muere, stamine + 1 con la posibilidad de llegar a 4. //MIRA LA LINEA DE ABAJO.
            //-----------------------------------------------------------------------
            //EN CASO DE ERROR, POR FAVOR, QUITA DEL CODIGO DE ARRIBA EL "currentSave", ESTO ES PORQUE SE ESTA ALMACENANDO LA INFORMACION EN UN SCRIPT LLAMADO "SaveData" QUE TE PERMITE CARGAR ESCENAS. REVISA TAMBIEN EL GAME MANAGER.
            //---------------------------------------------------------------------
        }
    }
}


