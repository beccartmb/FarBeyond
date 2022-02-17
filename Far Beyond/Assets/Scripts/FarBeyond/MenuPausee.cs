using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuPausee : MonoBehaviour
{
    public GameObject CanvasMenuPause;
    void start()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            CanvasMenuPause.SetActive(true);
        }
        else
        {
            CanvasMenuPause.SetActive(false);
        }
    }
    public void PauseButtonX()
    {

    }
    public void PauseButtonCredits()
    {

    }
    public void PauseButtonMenu()
    {

    }
    public void PauseButtonQuickGame()
    {

    }
    IEnumerator PauseOrPlay(GameObject go) //en vez de que muera, hacemos que se desactive y luego se active GRACIAS AL GAME MANAGER.
    {
        //creamos un nombre el gameobject y lo llamamos abajo, en este caso se llamara GO. ESto evitara problemas futuros.
        go.SetActive(true); //esto permitira que el power up respauné.
        yield return new WaitForSeconds(12.0f);
        go.SetActive(false);
    }
}
