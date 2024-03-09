using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class Static : MonoBehaviour
{
    public Image image;
    public Image image2;
    public float time = 0.1f;
    public float time2 = 0.75f;
    private bool phase;
    public TextMeshProUGUI text;

    void Awake()
    {
        image = transform.GetChild(0).GetComponent<Image>();
        image2 = GetComponent<Image>();
        Invoke("UpdateStatic", time);
        Invoke("UpdateCamera", time2);
    }

    private void Update()
    {
        if (GameManager.lightSwitches > 0)
        {
            text.text = GameManager.lightSwitches + "";
            text.color = Color.white;
        }
        else
        {
            text.text = "ESCAPE";
            text.color = Color.red;
        }
    }

    void UpdateStatic()
    {
        Invoke("UpdateStatic", time);

        image.transform.localScale = new Vector3(image.transform.localScale.x * (phase ? -1 : 1), image.transform.localScale.y * (phase ? 1 : -1), 1);

        phase = !phase;
    }

    void UpdateCamera()
    {
        Invoke("UpdateCamera", time2);
        image2.color = new Vector4(image2.color.r, image2.color.g, image2.color.b, image2.color.a == 1 ? 0 : 1);
    }
}
