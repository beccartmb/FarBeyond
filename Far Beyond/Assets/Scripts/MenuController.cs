using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
   

    public void LoadScene(string nextScene)
    {
        SceneManager.LoadScene(nextScene); //esto sirve para que cargue la escena que tu designes dentro de los botones. Tendras que escribir a la perfeccion dicho nombre. 
    }
    public void QuitMenu()
    {
        Application.Quit(); //esto sirve para que dentro del build sitting puedas quitar el videojuego pulsando la tecla "escape".
    }
    public void SocialMedia(string newUrl) //si pones dentro de los parentesis "string" y lo que quieras peter, te permitira meterlo desde botones. 
    {
        Application.OpenURL(newUrl); //esto sirve para que nos abra los url identificados mediante el apartado OnClick de los botones. Copia y pega el url que quieres visitar.
    }
}
