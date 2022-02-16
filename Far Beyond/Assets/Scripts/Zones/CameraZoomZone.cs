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
            anim.Play(nameAnimationBig);
            CameraController.Instance.isZoomedOut = true; //como hay un bool en la camara que dice si esta dentro o fuera, Aqui activamos todo lo relacionado con el zoom OUT.
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        IllayPlayer player = other.GetComponent<IllayPlayer>();

        if (player != null)
        {
            anim.Play(nameAnimationSmall);
            CameraController.Instance.isZoomedOut = false; //como hay un bool de si esta dentro del area de zoom out aqui decimos que no. Que ha salido y por ende cogemos los parametros fuera del zoom out. 
        }
    }
}



