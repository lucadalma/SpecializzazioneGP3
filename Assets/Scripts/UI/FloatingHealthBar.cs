using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Script per aggiornare una barra della vita (Slider) sopra un nemico o giocatore
public class FloatingHealthBar : MonoBehaviour
{
    // Riferimento al componente Slider della UI
    [SerializeField] private Slider slider;

    // Aggiorna la barra della vita in base ai valori attuali e massimi
    public void UpdateHealthBar(float currentValue, float maxValue)
    {
        // Imposta il valore del slider come percentuale della vita rimanente
        // Slider.value deve essere compreso tra 0 e 1
        slider.value = currentValue / maxValue;
    }
}
