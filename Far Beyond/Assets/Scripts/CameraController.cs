using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;
    public float minX; //ESTO LO CONTROLAREMOS DESDE EL EDITOR CADA VEZ QUE LO NECESIMOS. es decir, pondremos los valores de la camara desde la vista del juego. 
    public float maxX;
    public float minY;
    public float maxY; //hemos puesto aqui el maximo y el minimo tanto de la Y como de la "X" para que no pete, mueve la camara para ver cual es el maximo y el minimo.. 

    void Update()
    {
        transform.position = new Vector3(player.position.x + offset.x, player.position.y + offset.y, offset.z); // Camera follows the player with specified offset position
        if (transform.position.x < minX)
        {
            transform.position = new Vector3(minX, transform.position.y, transform.position.z); // el minimo de "X" es la cordenada que aparece cuando la camara esta lo mas a la izquierda posible.
        }

        if (transform.position.x > maxX)
        {
            transform.position = new Vector3(maxX, transform.position.y, transform.position.z); // el maximo de "X" es la cordenada que aparece cuando la camara esta lo mas a la derecha posible.
        }
        if (transform.position.y > minY)
        {
            transform.position = new Vector3(transform.position.x, minY, transform.position.z); //  el minimo de "Y" es la cordenada que aparece cuando la camara esta lo mas bajo que tu quieras. 
        }
        if (transform.position.y < maxY)
        {
            transform.position = new Vector3(transform.position.x, maxY, transform.position.z); // el maximo de "Y" es la cordenada que aparece cuando la camara esta lo mas alto que tu quieres asignar.
        }
    }
}


