using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaFlameFillAmount : MonoBehaviour
{
    private Image barFlameImage; //si te sale error esque se necesita arriba el unsign UnityEugine.UI;

    public void Update()
    {
        barFlameImage.fillAmount = GameManager.Instance.currentSave.stamina / GameManager.Instance.maxStamina; //Esta linea de codigo es la que controlará la barra de estamina de LLAMA.
        //este codigo se deberá poner en la imagen que se vaya a acortar(la estamina), necesitamos : fondo de la estamina, la estamina en cuestion y el reborde de adorno para la estamina. 
        //al poner la imagen, dentro del inspector, en "ImageTipe" habrá que poner el tipo "filled" y en horizontal para que vaya gestionando este codigo eso.
    }
}

