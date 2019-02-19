using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Image))]
public class Drawable : MonoBehaviour {
    
    public int brushSize = 1;
    [Range(0f, 1f)] public float strength;

    private Texture mapTexture;
    public Sprite mapSprite;

    public Slider brushColorSlider;
    public Slider brushSizeSlider;

    private Vector2 inputPos;
    private Vector2 inputTranslator;
    private RectTransform rectTransform;

    private Vector2 brushBoxCoord1, brushBoxCoord2;
    private float tempBrushSize;

    // pixel covered by brush
    private struct pixelData
    {
        public int x, y;
        public Color color;

        public pixelData(int x ,int y, Color color)
        {
            this.x = x;
            this.y = y;
            this.color = color;
        }
    }

    private List<pixelData> pixels = new List<pixelData>();

    private void Awake()
    {
        GetComponent<Image>().material.mainTexture = mapSprite.texture; // Load target sprite
        rectTransform = GetComponent<RectTransform>();

        // resize to divide perfectly with texture resolution
        rectTransform.sizeDelta -= new Vector2(rectTransform.sizeDelta.x % mapSprite.texture.width, rectTransform.sizeDelta.y % mapSprite.texture.height);

        inputTranslator.x = rectTransform.sizeDelta.x / mapSprite.texture.width;
        inputTranslator.y = rectTransform.sizeDelta.y / mapSprite.texture.height;
        
        brushColorSlider.value = strength;
        brushSizeSlider.value = brushSize;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, Input.mousePosition, null, out inputPos); // Get mouse position in rect transform coords

            inputPos = inputPos / inputTranslator; // Convert to pixel coordinates
            inputPos.x = (int)inputPos.x;
            inputPos.y = (int)inputPos.y;

            //// If input is inside the map
            //if (inputPos.x > 0f && inputPos.x <= mapSprite.texture.width && inputPos.y > 0f && inputPos.y <= mapSprite.texture.height)
            //{
            //    mapSprite.texture.SetPixel((int)inputPos.x, (int)inputPos.y, new Color(strength, strength, strength));
            //    mapSprite.texture.Apply();
            //}

            GetPixels();
            ApplyPixels();
        }
    }

    private void GetPixels()
    {
        // Resize brush to fit better
        if (brushSize % 2 == 1)
            tempBrushSize = brushSize - 1;
        else
            tempBrushSize = brushSize;

        // Get oposite corners for the box the brush is inside
        brushBoxCoord1 = inputPos - Vector2.one * (tempBrushSize / 2);
        brushBoxCoord2 = inputPos + Vector2.one * (tempBrushSize / 2);

        // Itterate through all pixels in the box and add them to a list with a color based on distance from centre pixel
        for (int i = (int)brushBoxCoord1.x; i < (int)brushBoxCoord2.x; i++)
        {
            for (int j = (int)brushBoxCoord1.y; j < (int)brushBoxCoord2.y; j++)
            {
                pixels.Add(new pixelData(i, j, ColorByLength(i, j)));
            }
        }
    }

    private Color ColorByLength(int i, int j)
    {
        float distance = Vector2.Distance(new Vector2(i, j), inputPos);
        Color currentColor = mapSprite.texture.GetPixel(i, j);
        // If distance is greater than brush size, return color unchanged. Else, calculate new color based on distance
        if (distance <= tempBrushSize / 2)
        {
            float lerper = (distance / (brushSize / 2));
            float color = Mathf.Lerp(strength, currentColor.grayscale, lerper);
            return new Color(color, color, color);
        }
        else
        {
            return currentColor;
        }

    }

    private void ApplyPixels()
    {
        foreach (pixelData pixel in pixels)
        {
            // If input is inside the map
            if (pixel.x >= 0f && pixel.x <= mapSprite.texture.width && pixel.y >= 0f && pixel.y <= mapSprite.texture.height)
            {
                mapSprite.texture.SetPixel(pixel.x, pixel.y, pixel.color);
            }
        }
        mapSprite.texture.Apply();

        pixels.Clear();
    }

    public void SetColor(float color)
    {
        strength = 1 - color;
    }

    public void SetBrushSize(float size)
    {
        brushSize = (int)size;
    }

    // Closes the draw map scene
    public void Done()
    {
        if(MasterBehavior.instance != null)
        {
            MasterBehavior.instance.DoneEditingMap();
        }
        SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName("DrawMap"));
    }
}
