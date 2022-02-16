using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;
    public Vector3 offsetZoomOut;
    public bool isZoomedOut;
    public float minX; //ESTO LO CONTROLAREMOS DESDE EL EDITOR CADA VEZ QUE LO NECESIMOS. es decir, pondremos los valores de la camara desde la vista del juego. 
    public float maxX;
    public float minY;
    public float maxY; //hemos puesto aqui el maximo y el minimo tanto de la Y como de la "X" para que no pete, mueve la camara para ver cual es el maximo y el minimo.. 
    public float minXZoomOut; //ESTO LO CONTROLAREMOS DESDE EL EDITOR CADA VEZ QUE LO NECESIMOS. es decir, pondremos los valores de la camara desde la vista del juego. 
    public float maxXZoomOut;
    public float minYZoomOut;
    public float maxYZoomOut;

    void Update()
    {
        if (player != null) //en caso de no haber jugador, tambien hara el temblor. Esto permite que haya o no haya o no haya jugador, se permita esto
        {
            if (isZoomedOut) //esto hara que la camara siga al jugador si esta dentro del ZOOM OUT. el Lerp es para que haga las animaciones mas lentas. 
            {
                transform.position = Vector3.Lerp(transform.position, player.position + offsetZoomOut, Time.deltaTime * 2.0f); //si quieres que la animacion vaya mas rapido, pon el *2.0 mas alto. 
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, player.position + offset, Time.deltaTime * 1.5f);
                //esto sirve para que la camara vaya mas lenta al entrar y salir de ciertas zonas. Da un estilo al juego que mola, si no quieres esto, tambien se puede hacer as:
                //transform.position = player.position + offset;      Esto es mucho mas corto y práctivo para que siga al jugador. 
            }
            if (isZoomedOut) //aqui asignaremos los limites dentro del area de zoomOut. La altitud de la camara y todo se controlara desde el inspector. 
            {
                if (transform.position.x < minXZoomOut)
                {
                    transform.position = new Vector3(minXZoomOut, transform.position.y, transform.position.z); // el minimo de "X" es la cordenada que aparece cuando la camara esta lo mas a la izquierda posible.
                }

                if (transform.position.x > maxXZoomOut)
                {
                    transform.position = new Vector3(maxXZoomOut, transform.position.y, transform.position.z); // el maximo de "X" es la cordenada que aparece cuando la camara esta lo mas a la derecha posible.
                }
                if (transform.position.y > minYZoomOut)
                {
                    transform.position = new Vector3(transform.position.x, minYZoomOut, transform.position.z); //  el minimo de "Y" es la cordenada que aparece cuando la camara esta lo mas bajo que tu quieras. 
                }
                if (transform.position.y < maxYZoomOut)
                {
                    transform.position = new Vector3(transform.position.x, maxYZoomOut, transform.position.z); // el maximo de "Y" es la cordenada que aparece cuando la camara esta lo mas alto que tu quieres asignar.
                }
            }
            else //estos terminos seran designados cuando ESTE FUERA DEL ZOOM OUT. 
            {
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

        // esta es la orden para sacudir la cámara.
        shakeTime -= Time.deltaTime;
        if (shakeTime > 0f)
        {
            this.transform.position += Vector3.right
                * Mathf.Sin(Time.time * 80.0f) * shakeAmount;
        }
    }
    //Hacer temblar la cámara

    public float shakeTime = 0f;
    public float shakeAmount = 0f;
    public float maxShakeTime = 0f;

    #region Singleton
    public static CameraController Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
    #endregion



}


