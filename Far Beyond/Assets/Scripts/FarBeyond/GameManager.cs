using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.InputSystem;
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

    //--------------------------------------------------------------------------------------
    //todo lo comentado está comentado es porque hemos guardado la informacion en SaveDATA. en caso de no tener dicho escript, borra la linea de codigo "GameManager.Instance.currentSave." de todos los objetos que lo tengan y quita el comentario de las variables.
    //--------------------------------------------------------------------------------------


    //public int playerLife = 4; //Y aqui hemos designado que el jugador tendra 3 vidas. 
    public float maxStamina = 4f; //ESTE ES EL MAXIMO DE ESTAMINA QUE DEBERA TENER PARA LANZAR LA LLAMARADA. SE PONDRA COMO FLOAT PARA QUE PUEDAS RESTARLE EL TIEMPO.
    //public float stamina = 0;
    public float maxStaminaO2 = 10.0f; //esto debera ser decimal para que baje con el tiempo, es decir, time.deltatime. 
    //public float staminaO2 = 0;
    public float maxPowerUpGrade = 4f; //esto va a ser aquello que llamemos desde el powerUpGRADE que te permitira crecer por 6 segundos.
    //public float staminaUpGrade = 0;
    Animator anim;
    bool dieCoroutineInExecution; //Esto es para crear la corrutina para esperar un tiempo determinado.

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

    public void Start()
    {
        anim = GetComponent<Animator>(); //SIEMPRE HAY QUE PONER ESTO EN LOS ESTAR SIEMPRE Y CUANDO HAYA ANIMACIONES DE POR MEDIO.

    }

    private void Update()
    {
        if (GameManager.Instance.currentSave.playerLife <= 0) //aqui hemos designado que si la vida del  jugador llega a 0, que reinicie el nivel en el que está. EN EL JUGADOR RESTAMOS UNA CADA VEZ QUE TOCAS LA DEATHZONE.
        {
            IllayDie();
            StartCoroutine(WaitingDeath());
        }
        if (GameManager.Instance.currentSave.stamina > maxStamina) //esto permite sumar estamina hasta llegar al maximo. Si llega al maximo de estamina SIEMPRE SERA EL MAXIMO.
        {
            GameManager.Instance.currentSave.stamina = maxStamina;
        }
        if (GameManager.Instance.currentSave.staminaO2 > maxStaminaO2) // esto permite sumar estamina hasta llegar al maximo.Si llega al maximo de estamina SIEMPRE SERA EL MAXIMO.
        {
            GameManager.Instance.currentSave.staminaO2 = maxStaminaO2;
        }
        if (GameManager.Instance.currentSave.staminaUpGrade > maxPowerUpGrade) //esto, al igual que todas las estaminas las gestionamos desde illay y el script de power up.
        {
            GameManager.Instance.currentSave.staminaUpGrade = maxPowerUpGrade; //si toca varios iguales, la estamina sera la misma que la maxima, es decir, 6, no se acumula.
        }

        /*if (Keyboard.current.rightCtrlKey.wasPressedThisFrame) //si el control derecho de abajo se pulsa, la escena se guarda. ESTO NO SERA NECESARIO SI NO TENEMOS CODIGO DE SAVEDATA.
        {
            GameManager.Instance.currentSave.currentScene = SceneManager.GetActiveScene().name; //esto me permite guardar la escena en la que el jugador esté.
            SaveGame();
        }*/
    }
    public void IllayDie()

    {
        IllayPlayer.Instance.anim.Play("Illay_die");
       

    }
    IEnumerator WaitingDeath() //esto es una corrutina que nos permite reactivar los collider en 0,5 segundos. LUEGO HAY QUE LLAMARLA AL TERMINAR LO DEL FLOOR WITH EFFECTOR
    {
        if (!dieCoroutineInExecution)
        {
            dieCoroutineInExecution = true; //A veces las corrutinas se repiten de forma ilimitada. Con esto hacemos que solo se repita una vez. Ya que cuando entra en este if hace el true y se frena al llegar al false.
            yield return new WaitForSeconds(2.0f);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name); //esto sirve para reinciar la escena EN LA QUE EL PERSONAJE DESAPAREZCA/muera.
            GameManager.Instance.currentSave.playerLife = 4;
            dieCoroutineInExecution = false;
        }
    }

    public void saveGameScene()
    {
        GameManager.Instance.currentSave.currentScene = SceneManager.GetActiveScene().name; //esto me permite guardar la escena en la que el jugador esté.
        SaveGame();
    }
}


//if (keyboard.current.sKey.wasPressedThisFrame ) //esto lo utilizamos para guardar al pulsar la S. Se debera meter en el codigo que queramos guardar.
//   {
//        GameManager.Instance.SaveGame
//   }

//if (keyboard.current.lKey.wasPressedThisFrame ) //esto lo utilizamos para cargar si pulsamos la tecla L. Se debera meter en el codigo que queramos guardar. 
//   {
//        GameManager.Instance.LoadGame
//   }
