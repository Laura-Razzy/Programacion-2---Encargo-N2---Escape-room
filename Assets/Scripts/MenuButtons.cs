using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour // Este script esta metido en un gameobject random de la UI para que puedan referenciarlo los botones, da lo mismo donde esta con que no se destruya (en mi caso lo puse en el crosshair)
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Debug.Log("This button quits the game. If you are in the editor, this will not work.");
        Application.Quit();
    }
}
