using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    [SerializeField] GameObject pauseCanvas;

    [SerializeField] GameObject healthCanvas;


    public void StartApplication()
    {
        SceneManager.LoadScene("TestScene", LoadSceneMode.Additive);
    }


    public void QuitApplication()
    {
        Application.Quit();
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            if (pauseCanvas.activeSelf) 
            {
                Time.timeScale = 1;
                pauseCanvas.SetActive(false);
                healthCanvas.SetActive(true);
            }
            else
            {
                Time.timeScale = 0;
                pauseCanvas.SetActive(true);
                healthCanvas.SetActive(false);
            }
        }
    }

}
