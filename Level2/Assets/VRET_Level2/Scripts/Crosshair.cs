using UnityEngine;

public class Crosshair : MonoBehaviour
{
    void OnGUI()
    {
        float size = 5f;
        float thickness = 2f;
        Color green = new Color(0f, 1f, 0f, 1f); // #00FF00

        float centerX = Screen.width / 2f;
        float centerY = Screen.height / 2f;

        // Draw horizontal line
        DrawRect(centerX - size, centerY - thickness / 2, size * 2, thickness, green);

        // Draw vertical line
        DrawRect(centerX - thickness / 2, centerY - size, thickness, size * 2, green);
    }

    void DrawRect(float x, float y, float width, float height, Color color)
    {
        Texture2D texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, color);
        texture.Apply();
        GUI.DrawTexture(new Rect(x, y, width, height), texture);
    }
}