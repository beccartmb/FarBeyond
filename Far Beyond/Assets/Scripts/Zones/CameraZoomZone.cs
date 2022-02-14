using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoomZone : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;
    public float minXZoom; //ESTO LO CONTROLAREMOS DESDE EL EDITOR CADA VEZ QUE LO NECESIMOS. es decir, pondremos los valores de la camara desde la vista del juego. 
    public float maxXZoom;
    public float minYZoom;
    public float maxYZoom; //hemos puesto aqui el maximo y el minimo tanto de la Y como de la "X" para que no pete, mueve la camara para ver cual es el maximo y el minimo.. 

    public string nameAnimationSmall;
    public string nameAnimationBig;
    public Animator anim;

    void OnTriggerEnter2D(Collider2D other)
    {
        IllayPlayer player = other.GetComponent<IllayPlayer>();
        CameraController camera = other.GetComponent<CameraController>();

        if (player != null)
        {
            //Offsetcamera();
            anim.Play(nameAnimationBig);
            CameraController.Instance.minY = 21f;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        IllayPlayer player = other.GetComponent<IllayPlayer>();

        if (player != null)
        {
            //Offsetcamera();
            anim.Play(nameAnimationSmall);
        }
    }
    /*void Offsetcamera()     //COMO NO FUNCIONA POR AHORA NO LO METO.
    {
        transform.position = new Vector3(player.position.x + offset.x, player.position.y + offset.y, offset.z); // Camera follows the player with specified offset position
        if (transform.position.x < minXZoom)
        {
            transform.position = new Vector3(minXZoom, transform.position.y, transform.position.z); // el minimo de "X" es la cordenada que aparece cuando la camara esta lo mas a la izquierda posible.
        }

        if (transform.position.x > maxXZoom)
        {
            transform.position = new Vector3(maxXZoom, transform.position.y, transform.position.z); // el maximo de "X" es la cordenada que aparece cuando la camara esta lo mas a la derecha posible.
        }
        if (transform.position.y > minYZoom)
        {
            transform.position = new Vector3(transform.position.x, minYZoom, transform.position.z); //  el minimo de "Y" es la cordenada que aparece cuando la camara esta lo mas bajo que tu quieras. 
        }
        if (transform.position.y < maxYZoom)
        {
            transform.position = new Vector3(transform.position.x, maxYZoom, transform.position.z); // el maximo de "Y" es la cordenada que aparece cuando la camara esta lo mas alto que tu quieres asignar.
        }
    */
}



