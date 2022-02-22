using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuPausee : MonoBehaviour
{
    #region SINGLETON
    public static MenuPausee Instance { get; private set; }

    private void Awake() //esto junto con el intance de arriba convierte nuestro personaje en un singleton (mucho mas comodo para juegos de un jugador) ya que nos permite acceder a este codigo desde otros codigos. 
    {
        Instance = this;
    }
    #endregion
    public GameObject canvasMenuPause;
    public GameObject canvasCredits;
    public GameObject canvasControls;

    void Update() //para que esto no se quede desactivado, crea un empty vacio al que llamaras (por ejemplo) PauseManager, ahi meterás este codigo, para que se active y desactive sin problemas.
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame) //aqui conseguimos que si lo pulsa una vez, aparezca, y si pulsa otra vez escape, desaparezca. es otra solucion de cerrar el menú.
        {
            if (canvasMenuPause.activeSelf)
            {
                canvasMenuPause.SetActive(false); //esto hara que no este activado el canvas si no se pulsa la letra ESCAPE.
                Time.timeScale = 1;
            }
            else
            {
                canvasMenuPause.SetActive(true);
                Time.timeScale = 0; //con esto paramos el juego de fuera, permitiendo entrar al menu.
            }

        }
    }
    public void PauseButtonX()
    {
        canvasMenuPause.SetActive(false); //esto hara que no este activado el canvas si no se pulsa la letra ESCAPE.
        Time.timeScale = 1; //si le das a la X, el videojuego se retoma.
    }
    public void PauseButtonCredits()
    {
        if (canvasCredits.activeSelf) //que si pulsas el boton CREDITS se active.
        {
            canvasCredits.SetActive(false);
        }
        else
        {
            canvasCredits.SetActive(true); 
        }
    }
    public void PauseButtonControls()
    {
        if (canvasControls.activeSelf) //que si pulsas el boton CREDITS se active.
        {
            canvasControls.SetActive(false);
        }
        else
        {
            canvasControls.SetActive(true);
        }
    }
    public void PauseButtonMenu()
    {
        SceneManager.LoadScene("StartMenu");//esto sirve para que pueda ir a la pestaña menú si tienes metido el script . 
    }
    public void PauseButtonQuickGame()
    {
        Application.Quit(); //esto sirve para que dentro del build sitting puedas quitar el videojuego pulsando la tecla "escape".
    }
    public void SaveGame()
    {
        GameManager.Instance.SaveGameScene(); //aqui he llamado al guardar partida puesto en el GameManager. nos permitirá guardar la ubicacion del jugador y crear una escena de guardado.
    }
}
