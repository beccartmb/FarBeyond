using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
   
    public void LoadSceneWithVibrate(string nextScene) //PARA HACER ESTE SE NECESITA QUE EL CANVAS ESTE PUESTO COMO WORLD SPACE.
    {
        StartCoroutine(ShakeAndLoadScene(nextScene));
    }
    public void QuitMenu()
    {
        Application.Quit(); //esto sirve para que dentro del build sitting puedas quitar el videojuego pulsando la tecla "escape".
    }
    public void SocialMedia(string newUrl) //si pones dentro de los parentesis "string" y lo que quieras peter, te permitira meterlo desde botones. 
    {
        Application.OpenURL(newUrl); //esto sirve para que nos abra los url identificados mediante el apartado OnClick de los botones. Copia y pega el url que quieres visitar.
    }
    IEnumerator ShakeAndLoadScene(string nextScene) //esto es un coroutine que sirve para esperar X tiempo. ES COMO UN CONTADOR.
    {
        CameraController.Instance.shakeTime = 0.65f;
        CameraController.Instance.maxShakeTime = 0.65f;
        CameraController.Instance.shakeAmount = 0.015f;

        yield return new WaitForSeconds(0.8f); //esto es para esperar un tiempo determinado, en este caso, 0.1f segundos.
        // yield return null; // Espera 1 frame
        SceneManager.LoadScene(nextScene); //esto sirve para que cargue la escena que tu designes dentro de los botones. Tendras que escribir a la perfeccion dicho nombre. 
    }
    public void LoadScene(string nextScene) //hemos hecho dos metodos distintos para cambar de escena porque con la vibracion SE NECESITA EL CANVAS EN WORLD SPACE. para evitar problemas, usa esto.
    {
        SceneManager.LoadScene(nextScene);//esto sirve para que cargue la escena que tu designes dentro de los botones. Tendras que escribir a la perfeccion dicho nombre. 
    }


}
