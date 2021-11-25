using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game_pausa : MonoBehaviour
{
    public GameObject pausaMenu;
    public void pauseButton()
    {
        pausaMenu.SetActive(true);
        Time.timeScale = 0;
    }
    public void playButton()
    {
        pausaMenu.SetActive(false);
        Time.timeScale=1;
    }
    public void botonMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            pauseButton();
        }
    }
}
