using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable] //esto permite que la informacion se serialice, es decir, que se guarde de manera ordenada. 
public class SaveData
{
    public string currentScene = "Hallway";

    //aqui vamos a guardar datos importantes, como nivel de experiencia, nivel propio... ETC. HAY QUE QUITAR EL MONOBIEVIOUR.
    public int playerLife = 4;
    public float stamina = 0;
    public float staminaO2 = 0.1f; //esto lo he puesto como 0.1 para que no haya problemas de compatibilidad, sino siempre se detecta que estas muerto.
    public float staminaUpGrade = 0;
}
