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
            //ponemos this.gameobject para que no de problemas.
            GameManager.Instance.StartCoroutine(Respawn(this.gameObject)); //como en la linea de abajo (en la courtine) desactivamos las cosas, vamos a pedir al game manager que nos reactive esto, evitando problemas.
            GameManager.Instance.currentSave.staminaO2 += 6; //cuando se quiere añadir un numero mayor a 1 (lo que seria ++) hay que poner += X. TENDRAS 5 segundos para nadar a toda pastilla .     MIRA LA LINEA DE ABAJO.
            //------------------------------------------------------------------------
            //EN CASO DE ERROR, QUITA DEL CODIGO DE ARRIBA "currentSave" ESTO SE DEBE A QUE DICHA INFORMACION SE ESTÁ ALMACENANDO EN EL SCRIPT SaveData". REVISA TAMBIEN EL GAME MANAGER.
            //------------------------------------------------------------------------
        }
    }

    IEnumerator Respawn(GameObject go) //en vez de que muera, hacemos que se desactive y luego se active GRACIAS AL GAME MANAGER.
    {
        //creamos un nombre el gameobject y lo llamamos abajo, en este caso se llamara GO. ESto evitara problemas futuros.
        go.SetActive(false);
        yield return new WaitForSeconds(12.0f);
        go.SetActive(true);

    }
}
