using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable] //esto permite que la informacion se serialice, es decir, que se guarde de manera ordenada. 
public class SaveData
{
    //aqui vamos a guardar datos importantes, como nivel de experiencia, nivel propio... ETC. AHAY QUE QUITAR EL MONOBIEVIOUR.
    public int value;
    public float playerSpeed;
    //vida de cada personaje.
    //magia que le queda a cada personaje. 
    //etc. 
}
