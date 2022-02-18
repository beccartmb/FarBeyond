using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaFlameFillAmount : MonoBehaviour
{
    public Image barFlameImage; //si te sale error esque se necesita arriba el unsign UnityEugine.UI;

    public void Update()
    {
        barFlameImage.fillAmount = (float)GameManager.Instance.currentSave.stamina / (float)GameManager.Instance.maxStamina; //Esta linea de codigo es la que controlar� la barra de estamina de LLAMA.
        //este codigo se deber� poner en la imagen que se vaya a acortar(la estamina), necesitamos : fondo de la estamina, la estamina en cuestion y el reborde de adorno para la estamina. 
        //al poner la imagen, dentro del inspector, en "ImageTipe" habr� que poner el tipo "filled" y en horizontal para que vaya gestionando este codigo eso.
        //deber� ponerse como float al principio para que no lo lea como 0 o como 1, sino que lo lea progresivamente.
    }
}

