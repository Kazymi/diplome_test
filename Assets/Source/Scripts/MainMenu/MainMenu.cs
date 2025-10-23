using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private CameraController2D _cameraController;
    [SerializeField] private GameObject _mainCanvas;
    [SerializeField] private CanvasGroup _mainMenuCanvasGroup;
    [SerializeField] private CanvasGroup _mainMenuCanvasGroupMainGame;
    [SerializeField] private Button _startGame;
    [SerializeField] private GameManager _gameManager;

    private void Awake()
    {
        _startGame.onClick.AddListener(() =>
        {
            var sequence = DOTween.Sequence();
            sequence.Append(_mainMenuCanvasGroup.DOFade(0, 1f));
            float valFloat = _mainCamera.orthographicSize;
            sequence.Append(DOTween.To(() => valFloat, x => valFloat = x, 7, 1f).OnUpdate(() =>
            {
                _mainCamera.orthographicSize = valFloat;
            }));
            sequence.AppendCallback(() =>
            {
                _mainMenuCanvasGroup.gameObject.SetActive(false);
                _mainCanvas.gameObject.SetActive(true);
                _cameraController.enabled = true;
            });
            sequence.Append(_mainMenuCanvasGroupMainGame.DOFade(1, 0.6f).OnComplete(() => { _gameManager.StartGame(); }));
            
        });
    }
}