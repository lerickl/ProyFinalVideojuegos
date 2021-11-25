using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    // Start is called before the first frame update
   public void BotonStart()
    {
        SceneManager.LoadScene("scenaFrezer");
    }

    // Update is called once per frame
    public void BotonQuit()
    {
        Debug.Log("Quitamos la aplicacion");
        Application.Quit();
    }
    public void BotonComandos()
    {
        SceneManager.LoadScene("ayuda");
    }

    public void BotonregresarMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
