using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable] //esto permite que la informacion se serialice, es decir, que se guarde de manera ordenada. 
public class SaveData
{    
    //---------------------------------------------------------------------------------------------------------------------------------
    //aqui vamos a guardar datos importantes, como nivel de experiencia, nivel propio... ETC. HAY QUE QUITAR EL MONOBIEVIOUR.
    //---------------------------------------------------------------------------------------------------------------------------------

    public string currentScene = "Hallway"; //aqui llamamos a la escena principal.
    public int playerHearts = 4;
    public int countdownLifes = 4; //aqui vamos a gestionar el contador de vidas para que, cuando muera 4 veces, se reinicie la escena.
    public float stamina = 0;
    public float staminaO2 = 0.1f; //esto lo he puesto como 0.1 para que no haya problemas de compatibilidad, sino siempre se detecta que estas muerto.
    public float staminaUpGrade = 0; 
    
    //en caso de querer quitar el SCRIPT de SAVEDATA necesitas quitar el currentSave DE TODOS LOS CODIGOs.
}
