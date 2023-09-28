using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class wlwdwdw : MonoBehaviour
{
    public int FileCounter = 0;

    public Cubemap cubemap;
    public bool shshsh;
    private void LateUpdate()
    {
        if (shshsh)
        {
            shshsh = false;
            CamCapture();
        }
    }

    void CamCapture()
    {
        Camera Cam = GetComponent<Camera>();

        RenderTexture currentRT = RenderTexture.active;
        RenderTexture.active = Cam.targetTexture;

        Cam.Render();

        Texture2D Image = new Texture2D(Cam.targetTexture.width, Cam.targetTexture.height);
        Image.ReadPixels(new Rect(0, 0, Cam.targetTexture.width, Cam.targetTexture.height), 0, 0);
        Image.Apply();
        RenderTexture.active = currentRT;

        //cubemap.set
        var Bytes = Image.EncodeToPNG();
        Destroy(Image);

        File.WriteAllBytes(Application.dataPath + "/Backgrounds/" + FileCounter + ".png", Bytes);
        FileCounter++;
    }
}
