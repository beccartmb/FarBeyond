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


    //public int playerHearts = 4; //Y aqui hemos designado que el jugador tendra 3 vidas. 
    public int maxPlayerHearts = 4;
    public float maxStamina = 4f; //ESTE ES EL MAXIMO DE ESTAMINA QUE DEBERA TENER PARA LANZAR LA LLAMARADA. SE PONDRA COMO FLOAT PARA QUE PUEDAS RESTARLE EL TIEMPO.
    //public float stamina = 0;
    public float maxStaminaO2 = 10.0f; //esto debera ser decimal para que baje con el tiempo, es decir, time.deltatime. 
    //public float staminaO2 = 0.1f;
    //public int countdownLifes = 4; 
    public int countdownLifesMax = 4;
    //public float staminaO2 = 0.1;
    public float maxPowerUpGrade = 4f; //esto va a ser aquello que llamemos desde el powerUpGRADE que te permitira crecer por 6 segundos.
                                       //public float staminaUpGrade = 0;
                                       //public float staminaUpGrade = 0;


    Animator anim;
    bool dieCoroutineInExecution; //Esto es para crear la corrutina para esperar un tiempo determinado.

    public SaveData currentSave = new SaveData(); //esto permite tener TODA la informacion de la partida GUARDADA. Esto permitirá poder acceder a ello en cualquier momento. 

    private void Start()
    {
        //rbody = GetComponent<Rigidbody2D>(); //esto permite que te detecte el RIGIDBODY para detectar suelos y paredes desde el minuto 0.
    }

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
        //aqui deberiamos llamar para que al cargar la escena, el menu de pause aparezca desactivado.
        //MenuPausee.Instance.canvasMenuPause.SetActive(false); //esto hara que no este activado el canvas si no se pulsa la letra ESCAPE.
        //Time.timeScale = 1;  lo veria como una solucion pero no me la hace.
    }

    private void Update()
    {
        if (GameManager.Instance.currentSave.playerHearts <= 0) //aqui hemos designado que si la vida del  jugador llega a 0, que reinicie el nivel en el que está. EN EL JUGADOR RESTAMOS UNA CADA VEZ QUE TOCAS LA DEATHZONE.
        {
            StartCoroutine(WaitingDeath());
        }
        if (GameManager.Instance.currentSave.playerHearts > maxPlayerHearts)
        {
            GameManager.Instance.currentSave.playerHearts = maxPlayerHearts;
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
            IllayDie();
            yield return new WaitForSeconds(2.0f);
            GameManager.Instance.currentSave.countdownLifes--; //aqui le estoy quitando una vida al contador de vidas(con un maximo de 4 para asi poder tener reinicio de escena y reaparicion en los save points.
            GameManager.Instance.currentSave.playerHearts = 4;
            if (GameManager.Instance.currentSave.countdownLifes <= 0)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name); //esto sirve para reinciar la escena EN LA QUE EL PERSONAJE DESAPAREZCA/muera.
            }
            else
            {
                IllayPlayer.Instance.transform.position = IllayPlayer.Instance.newSaveZone; //si toca una deathzone que tenga metido el script, te llevara a la posicion del ultimo SaveZone guardado por la variable de arriba.
                IllayPlayer.Instance.rbody.velocity = Vector2.zero; //asi quitamos la deceleracion para que no se te cuele por el mapa.
            }
            dieCoroutineInExecution = false;
        }
    }

    public void SaveGameScene()
    {
        GameManager.Instance.currentSave.currentScene = SceneManager.GetActiveScene().name; //esto me permite guardar la escena en la que el jugador esté.
        SaveGame(); //esto llama al metodo save game.
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
