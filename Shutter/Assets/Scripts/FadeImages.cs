
// Image Fade Script
// by Kyle Furey

using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

// The fading of one or more images on the screen.
public class FadeImages : MonoBehaviour
{
    [Header("The fading of one or more images on the screen.")]

    [Header("FADING IN / OUT")]

    [Header("The images we are fading in and out:")]
    [SerializeField] private List<Image> fadeImages = null;

    [Header("The speed to fade in and out (scaled with delta time):")]
    [SerializeField] private float fadeSpeed = 300;

    // Whether the screen is currently fading in or out
    public FadingState isFading = FadingState.NotFading;

    // The current fading state
    public enum FadingState { NotFading, FadingIn, FadingOut };

    // Fade the image in and out over time
    private void Update()
    {
        // Check if we are fading
        if (isFading != FadingState.NotFading)
        {
            // Fading in
            if (isFading == FadingState.FadingIn)
            {
                for (int i = 0; i < fadeImages.Count; i++)
                {
                    fadeImages[i].color = new Vector4(fadeImages[i].color.r, fadeImages[i].color.g, fadeImages[i].color.b, fadeImages[i].color.a + (fadeSpeed * Time.deltaTime / 255));
                }

                if (fadeImages[0].color.a >= 1)
                {
                    isFading = FadingState.FadingOut;
                }
            }
            // Fading out
            else if (isFading == FadingState.FadingOut)
            {
                for (int i = 0; i < fadeImages.Count; i++)
                {
                    fadeImages[i].color = new Vector4(fadeImages[i].color.r, fadeImages[i].color.g, fadeImages[i].color.b, fadeImages[i].color.a - (fadeSpeed * Time.deltaTime / 255));
                }

                if (fadeImages[0].color.a <= 0)
                {
                    isFading = FadingState.NotFading;
                }
            }
        }
    }

    // Begin fading in and out the screen
    public void Fade()
    {
        isFading = FadingState.FadingIn;
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
