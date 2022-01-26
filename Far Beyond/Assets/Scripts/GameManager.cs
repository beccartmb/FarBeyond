using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    //a este game manager se le denomina PERSISTENTE. y significa que NO SE DESTRUYE. 
    //esto nos permite que el GameManager se CREE a si mismo siempre que lo necesite. ESTA ES LA MEJOR MANERA DE HACER UN GAME MANAGER.
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject("Game Manager").AddComponent<GameManager>();
                DontDestroyOnLoad(instance.gameObject); //aqui permitimos que si cambiamos de escena no se destruya ni se tenga que volver a hacer. 
            }
            return instance;
        }
    }

    public int playerLife = 3; //Y aqui hemos designado que el jugador tendra 3 vidas. 

    public SaveData currentSave = new SaveData(); //esto permite tener TODA la informacion de la partida GUARDADA. Esto permitirá poder acceder a ello en cualquier momento. 

    public void SaveGame() //esto nos permite meter en una caja toda la informacion, almacenandola en un archivo. PODEMOS TENER VARIOS SAVEGAMES CON DISTINTO NOMBRE, AL IGUAL QUE MAS LOADGAME.
    {
        BinaryFormatter bf = new BinaryFormatter();
        MemoryStream stream = new MemoryStream();
        bf.Serialize(stream, currentSave);
        File.WriteAllBytes("save", stream.GetBuffer()); //el "save" es el nombre del archivo que te crea. por ejemplo, si pones SaveGame 1, SaveGame2, esta carpeta debera llamarse
                                                        //"save1" y "save2".
    }
    public void LoadGame() //aqui nos permite abrir la caja nombrada anteriormente de informacion y cargarla. (lee todos los bytes de la caja)
    {
        MemoryStream stream = new MemoryStream(File.ReadAllBytes("save"));
        BinaryFormatter bf = new BinaryFormatter();
        currentSave = bf.Deserialize(stream) as SaveData;
    }

    private void Update()
    {
        if (playerLife < 0) //aqui hemos designado que si la vida del  jugador llega a 0, que reinicie el nivel en el que está. EN EL JUGADOR RESTAMOS UNA CADA VEZ QUE TOCAS LA DEATHZONE.
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name); //esto sirve para reinciar la escena EN LA QUE EL PERSONAJE DESAPAREZCA/muera.
            playerLife = 3;
        }
    }
}
//este GameManager solo aparece si le das al PLAY y lo usas. Si no lo usas NO APARECE.
//if (keyboard.current.sKey.wasPressedThisFrame ) //esto lo utilizamos para guardar al pulsar la S. Se debera meter en el codigo que queramos guardar.
//   {
//        GameManager.Instance.SaveGame
//   }

//if (keyboard.current.lKey.wasPressedThisFrame ) //esto lo utilizamos para cargar si pulsamos la tecla L. Se debera meter en el codigo que queramos guardar. 
//   {
//        GameManager.Instance.LoadGame
//   }
