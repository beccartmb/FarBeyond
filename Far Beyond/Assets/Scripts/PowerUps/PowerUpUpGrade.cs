using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpUpGrade : MonoBehaviour
{
    public void Die()
    {
        Destroy(this.gameObject);
    }
    void OnTriggerEnter2D(Collider2D other) //eso nos va a permitir detectar dentro de un empty vacio con un collider que genere un area. 
    {
        IllayPlayer player = other.GetComponent<IllayPlayer>();
        if (player != null) //si player choca contra este power up, este se desactiva porque comienza la courtina. 
        {
            //ponemos this.gameobject para que no de problemas.
            GameManager.Instance.StartCoroutine(Respawn(this.gameObject)); //ESTO SERA IMPORTANTE PARA QUE RESPAUNÉ ES DECIR, SIN ESTO, NUNCA DESAPARECE EL POWER UP NI TAMPOCO SE REGENERA.
            GameManager.Instance.currentSave.staminaUpGrade += 4; //esto añade 4 a la estamina, cuando la gastes volvera a 0. 
            //cuando se quiere añadir un numero mayor a 1 (lo que seria ++) hay que poner += X. TENDRAS 5 segundos para crecer de golpe.    MIRA LA LINEA DE ABAJO.
            //------------------------------------------------------------------------
            //EN CASO DE ERROR, QUITA DEL CODIGO DE ARRIBA "currentSave" ESTO SE DEBE A QUE DICHA INFORMACION SE ESTÁ ALMACENANDO EN EL SCRIPT SaveData". REVISA TAMBIEN EL GAME MANAGER.
            //------------------------------------------------------------------------
        }
    }

    IEnumerator Respawn(GameObject go) //en vez de que muera, hacemos que se desactive y luego se active GRACIAS AL GAME MANAGER.
    {
        //creamos un nombre el gameobject y lo llamamos abajo, en este caso se llamara GO. ESto evitara problemas futuros.
        go.SetActive(false); //esto permitira que el power up respauné. es decir, esto permite que se desactive el objeto (POR EL SetActive)
        yield return new WaitForSeconds(12.0f);//pasan 12 segundos.
        go.SetActive(true); //y se activa. es decir, NUNCA SE BORRA DICHO OBJETO.
        //si quisiesemos hacer que empiece desactivado y cuando pases por X sitio se active, debera estar en el visor como desactivado, y al activar X sitio, activar el spawn de dicho elemento.
    }
}
