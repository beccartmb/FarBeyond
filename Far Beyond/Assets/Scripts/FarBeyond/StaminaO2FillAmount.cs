using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaO2FillAmount : MonoBehaviour
{
    public Image barO2Image; //si te sale error esque se necesita arriba el unsign UnityEugine.UI;

    public void Update()
    {
        barO2Image.fillAmount = (float)GameManager.Instance.currentSave.staminaO2 / (float)GameManager.Instance.maxStaminaO2; //Esta linea de codigo es la que controlará la barra de estamina de LLAMA.
        //este codigo se deberá poner en la imagen que se vaya a acortar(la estamina), necesitamos : fondo de la estamina, la estamina en cuestion y el reborde de adorno para la estamina. 
        //al poner la imagen, dentro del inspector, en "ImageTipe" habrá que poner el tipo "filled" y en horizontal para que vaya gestionando este codigo eso.
        //deberá ponerse como float al principio para que no lo lea como 0 o como 1, sino que lo lea progresivamente.
    }
}
