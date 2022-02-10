using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class furtherScreenZone : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)//esto es para las colisiones EN AREA 2D. 
    {
        furtherScreenZone furtherZone = other.GetComponent<furtherScreenZone>(); //aqui hemos detectado los colisionadores que tengan el script indicado como deathzone y saveZone.
        if (furtherZone != null) //null significa algo que no existe //esto permite que al colisionar contra la pared derecha. vaya a la cordenada en X -9,4 apareciendo asi por el otro lado.
        {
            //cam.orthographicSize = orthographicSizeMax;
        }
    }
}
