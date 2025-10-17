using System;
using TMPro;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private CharacterParamSystem characterParamSystem;
    [SerializeField] private HealthBarUI healthBarUI;
    [SerializeField] private TMP_Text healthText;

    private int currentHealth;

    private void Awake()
    {
        currentHealth = characterParamSystem.Health;
        UpdateHealthBar();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        healthText.text = currentHealth.ToString() + "/" + characterParamSystem.Health;
        healthBarUI.UpdateHealth(currentHealth, characterParamSystem.Health);
    }
}