using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Questo script fa sì che un Canvas 3D (World Space) guardi sempre verso la camera principale
public class CanvasUI : MonoBehaviour
{
    // Riferimento alla camera principale
    private Camera mainCamera;

    void Start()
    {
        // Otteniamo la camera principale all'avvio
        mainCamera = Camera.main;
    }

    void LateUpdate()
    {
        // Controllo di sicurezza: se non c'è camera principale, non fare nulla
        if (mainCamera == null) return;

        // Imposta la direzione del canvas verso la camera (billboarding)
        transform.forward = mainCamera.transform.forward;
    }
}
