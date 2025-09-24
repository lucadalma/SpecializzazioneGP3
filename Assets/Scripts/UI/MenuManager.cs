using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Gestisce il menu di pausa, la fine del gioco e il reset della scena
public class MenuManager : MonoBehaviour
{
    // Canvas per il menu di pausa
    [SerializeField] GameObject pauseCanvas;

    // Canvas della barra della vita
    [SerializeField] GameObject healthCanvas;

    // Canvas mostrato alla fine del gioco
    [SerializeField] GameObject endGameCanvas;

    // Liste di variabili da resettare al reload della scena
    [SerializeField] List<IntVariable> intVariables;
    [SerializeField] List<FloatVariable> floatVariables;

    // Riferimenti alla vita del giocatore e del nemico
    [SerializeField] Health playerHealth;
    [SerializeField] Health enemyHealth;

    // Iscrive la funzione EndGame agli eventi OnDie di player e nemico
    private void OnEnable()
    {
        if (playerHealth && enemyHealth)
        {
            playerHealth.OnDie += EndGame;
            enemyHealth.OnDie += EndGame;
        }
    }

    // Rimuove la registrazione agli eventi per sicurezza
    private void OnDisable()
    {
        if (playerHealth && enemyHealth)
        {
            playerHealth.OnDie -= EndGame;
            enemyHealth.OnDie -= EndGame;
        }
    }

    // Carica la scena principale all'avvio del gioco
    public void StartApplication()
    {
        SceneManager.LoadScene("TestScene", LoadSceneMode.Single);
    }

    // Chiude l'applicazione (funziona solo nel build, non in editor)
    public void QuitApplication()
    {
        Application.Quit();
    }

    private void Update()
    {
        // Se il gioco non è finito
        if (endGameCanvas.activeSelf == false)
        {
            // Controlla input tasto ESC per pausa
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (pauseCanvas.activeSelf)
                {
                    // Se il menu è aperto, chiudilo
                    Time.timeScale = 1;          // Riprende il gioco
                    pauseCanvas.SetActive(false);
                    healthCanvas.SetActive(true); // Mostra barra vita
                }
                else
                {
                    // Se il menu è chiuso, aprilo
                    Time.timeScale = 0;          // Ferma il gioco
                    pauseCanvas.SetActive(true);
                    healthCanvas.SetActive(false);
                }
            }
        }
    }

    // Ricarica la scena resettando tutte le variabili
    public void ReloadScene()
    {
        foreach (IntVariable variable in intVariables)
        {
            variable.Value = 0; // Reset variabili intere
        }

        foreach (FloatVariable variable in floatVariables)
        {
            variable.Value = 0f; // Reset variabili float
        }

        SceneManager.LoadScene("TestScene", LoadSceneMode.Single);
    }

    // Mostra il canvas di fine gioco
    public void EndGame()
    {
        Cursor.lockState = CursorLockMode.None; // Sblocca il cursore
        Cursor.visible = true;                  // Rende il cursore visibile

        endGameCanvas.SetActive(true);  // Mostra il canvas di fine gioco
        pauseCanvas.SetActive(false);   // Nasconde menu pausa
        healthCanvas.SetActive(false);  // Nasconde barra vita
    }
}
