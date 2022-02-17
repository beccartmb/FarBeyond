using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuPausee : MonoBehaviour
{
    public GameObject CanvasMenuPause;
    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (CanvasMenuPause.activeSelf)
            {
                CanvasMenuPause.SetActive(false); //esto hara que no este activado el canvas si no se pulsa la letra ESCAPE.
                Time.timeScale = 1;
            }
            else
            {
                CanvasMenuPause.SetActive(true); //¿POR QUE NO FUNCIOOOOONAAAAAA? 
                Time.timeScale = 0; //con esto paramos el juego de fuera, permitiendo entrar al menu.
            }

        }
    }
    public void PauseButtonX()
    {
        CanvasMenuPause.SetActive(false); //esto hara que no este activado el canvas si no se pulsa la letra ESCAPE.
        Time.timeScale = 1; //si le das a la X, el videojuego se retoma.
    }
    public void PauseButtonCredits()
    {
        //cuando funcione la parte en la que sale el canvas, hago esto.
    }
    public void PauseButtonMenu()
    {
        SceneManager.LoadScene("MenuStart");//esto sirve para que pueda ir a la pestaña menú si tienes metido el script . 
    }
    public void PauseButtonQuickGame()
    {
        Application.Quit(); //esto sirve para que dentro del build sitting puedas quitar el videojuego pulsando la tecla "escape".
    }
    public void SaveGame()
    {
        GameManager.Instance.saveGameScene(); //aqui he llamado al guardar partida puesto en el GameManager. nos permitirá guardar la ubicacion del jugador y crear una escena de guardado.
    }
}
