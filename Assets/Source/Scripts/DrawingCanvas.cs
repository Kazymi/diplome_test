using UnityEngine;
using UnityEngine.UI;

public class DrawingCanvas : MonoBehaviour
{
    [Header("Canvas Settings")]
    [SerializeField] private int textureWidth = 512;
    [SerializeField] private int textureHeight = 512;
    [SerializeField] private Color backgroundColor = Color.white;
    [SerializeField] private Slider slider;

    [Header("Drawing Settings")]
    [SerializeField] private Color[] availableColors;
    [SerializeField] private float brushSize = 8f;
    [SerializeField] private BrushShape brushShape = BrushShape.Round;

    [Header("References")]
    [SerializeField] private RawImage drawingArea;
    [SerializeField] private PetStateMachine petStateMachine;
    [SerializeField] private Image previewImage;

    private Texture2D _texture;
    private Color _currentColor;
    private RectTransform _rectTransform;
    private bool _isErasing;

    private enum BrushShape
    {
        Round,
        Square
    }

    private void Start()
    {
        _texture = new Texture2D(textureWidth, textureHeight, TextureFormat.RGBA32, false);
        ClearCanvas();

        drawingArea.texture = _texture;
        _rectTransform = drawingArea.GetComponent<RectTransform>();

        _currentColor = availableColors.Length > 0 ? availableColors[0] : Color.black;
        ChangeBrushSize();
    }

    private void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        if (Input.GetMouseButton(0))
            DrawAtScreenPosition(Input.mousePosition);
#elif UNITY_ANDROID || UNITY_IOS
        if (Input.touchCount > 0)
        {
            foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
                    DrawAtScreenPosition(touch.position);
            }
        }
#endif
    }

    private void DrawAtScreenPosition(Vector2 screenPosition)
    {
        // Конвертируем позицию экрана в локальные координаты RawImage
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(_rectTransform, screenPosition, null, out Vector2 localPoint))
            return;

        // Проверяем, находится ли точка внутри зоны рисования
        Rect rect = _rectTransform.rect;
        if (localPoint.x < rect.xMin || localPoint.x > rect.xMax || localPoint.y < rect.yMin || localPoint.y > rect.yMax)
            return;

        // Переводим координаты в пиксели текстуры
        float px = (localPoint.x + rect.width / 2f) * (textureWidth / rect.width);
        float py = (localPoint.y + rect.height / 2f) * (textureHeight / rect.height);

        Color drawColor = _isErasing ? backgroundColor : _currentColor;

        for (int x = (int)-brushSize; x <= brushSize; x++)
        {
            for (int y = (int)-brushSize; y <= brushSize; y++)
            {
                if (brushShape == BrushShape.Round && (x * x + y * y > brushSize * brushSize))
                    continue;

                int drawX = Mathf.Clamp((int)px + x, 0, textureWidth - 1);
                int drawY = Mathf.Clamp((int)py + y, 0, textureHeight - 1);
                _texture.SetPixel(drawX, drawY, drawColor);
            }
        }

        _texture.Apply();
    }

    public void ChangeBrushSize()
    {
        // Можно подстроить множитель под желаемую чувствительность
        brushSize = 8f * (slider.value * 3);
    }

    public void SetColor(int index)
    {
        if (index >= 0 && index < availableColors.Length)
        {
            _isErasing = false;
            _currentColor = availableColors[index];
        }
    }

    public void SetBrushShape(int shapeIndex)
    {
        brushShape = (BrushShape)shapeIndex;
    }

    public void SetEraser()
    {
        _isErasing = !_isErasing;
    }

    public void ClearCanvas()
    {
        Color[] pixels = new Color[textureWidth * textureHeight];
        for (int i = 0; i < pixels.Length; i++)
            pixels[i] = backgroundColor;
        _texture.SetPixels(pixels);
        _texture.Apply();
    }

    public void CreateSprite()
    {
        Sprite newSprite = Sprite.Create(_texture, new Rect(0, 0, textureWidth, textureHeight),
            new Vector2(0.5f, 0.5f), 100f);

        var pet = Instantiate(petStateMachine);
        var player = FindAnyObjectByType<PlayerStateMachine>().transform;

        pet.transform.position = player.position;
        pet.GetComponentInChildren<SpriteRenderer>().sprite = newSprite;

        Destroy(gameObject);
    }
}
