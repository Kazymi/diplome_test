using System;
using UnityEngine;
using UnityEngine.UI;

public class TriggerZone : MonoBehaviour
{
    [Header("UI")] [SerializeField] private Image fillImage;

    [Header("Настройки")] [SerializeField] private float fillSpeed = 0.5f;
    [SerializeField] private float decreaseSpeed = 1f;

    public event Action OnTriggerEnterCompleted;
    private bool _isInside = false;
    private bool _eventInvoked = false;

    private void Update()
    {
        if (fillImage == null) return;

        if (_isInside)
        {
            fillImage.fillAmount += fillSpeed * Time.deltaTime;

            if (fillImage.fillAmount >= 1f)
            {
                fillImage.fillAmount = 1f;

                if (!_eventInvoked)
                {
                    _eventInvoked = true;
                    OnTriggerEnterCompleted?.Invoke();
                }
            }
        }
        else
        {
            fillImage.fillAmount -= decreaseSpeed * Time.deltaTime;
            fillImage.fillAmount = Mathf.Clamp01(fillImage.fillAmount);

            if (fillImage.fillAmount < 1f)
            {
                _eventInvoked = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            _isInside = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            _isInside = false;
    }
}