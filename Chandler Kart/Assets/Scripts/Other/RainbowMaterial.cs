using UnityEngine;

public class RainbowMaterial : MonoBehaviour
{
    public Material Material;
    public bool baseColor;
    public bool emissiveColor;

    public int mode = 0;
    public float speed;

    public Color color;
    public float red = 255;
    public float green = 0;
    public float blue = 0;

    void FixedUpdate()
    {
        // Red Overflow
        if (red < 0)
        {
            red = 0;
        }
        else if (red > 255)
        {
            red = 255;
        }

        // Green Overflow
        if (green < 0)
        {
            green = 0;
        }
        else if (green > 255)
        {
            green = 255;
        }

        // Blue Overflow
        if (blue < 0)
        {
            blue = 0;
        }
        else if (blue > 255)
        {
            blue = 255;
        }


        // Subtract Green
        if (red == 255 && green == 0 && blue == 0)
        {
            mode = 0;
        }

        // Add Blue
        if (red == 255 && green == 0 && blue == 255)
        {
            mode = 1;
        }

        // Subtract Red
        if (red == 0 && green == 0 && blue == 255)
        {
            mode = 2;
        }

        // Add Green
        if (red == 0 && green == 255 && blue == 255)
        {
            mode = 3;
        }

        // Subtract Blue
        if (red == 0 && green == 255 && blue == 0)
        {
            mode = 4;
        }

        // Add Red
        if (red == 255 && green == 255 && blue == 0)
        {
            mode = 5;
        }


        // Which values are being modified
        switch (mode)
        {
            case 0:
                red += 0;
                green += 0;
                blue += speed;
                break;

            case 1:
                red -= speed;
                green += 0;
                blue += 0;
                break;

            case 2:
                red += 0;
                green += speed;
                blue += 0;
                break;

            case 3:
                red += 0;
                green += 0;
                blue -= speed;
                break;

            case 4:
                red += speed;
                green += 0;
                blue += 0;
                break;

            case 5:
                red += 0;
                green -= speed;
                blue += 0;
                break;
        }


        // Set color
        color = new Color(red / 255f, green / 255f, blue / 255f);

        if (baseColor)
        {
            Material.SetColor("_Color", color);
        }

        if (emissiveColor)
        {
            Material.SetColor("_EmissionColor", color);
        }
    }
}
