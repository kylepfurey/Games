using TMPro;
using UnityEngine;

public class TextScroll : MonoBehaviour
{
    public bool start = true;
    [SerializeField] private float delay = 0;

    [SerializeField] private string text;
    [SerializeField] private float textDelay = 0.1f;

    [SerializeField] private float destroyAfter = 3;
    [SerializeField] private float fadeOutSpeed = 3;

    private TextMeshProUGUI textUI;
    private TextMeshProUGUI textShadowUI;
    private float timer;
    private float textTimer;
    private int character;

    void Start()
    {
        timer = 0;
        character = 0;

        textUI = GetComponent<TextMeshProUGUI>();
        textShadowUI = transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        textUI.text = "";
        textShadowUI.text = "";
    }

    void Update()
    {
        if (timer > delay && start && textTimer <= timer && textUI.text != text)
        {
            textUI.text += text[character];
            textShadowUI.text = textUI.text;

            GameManager.audio.PlayOnce("Beep");

            textTimer = timer + textDelay;
            character++;
        }
        else if (textUI.text == text)
        {
            if (timer > destroyAfter + textTimer - textDelay)
            {
                textUI.color = new Vector4(textUI.color.r, textUI.color.g, textUI.color.b, textUI.color.a - fadeOutSpeed * Time.deltaTime);
                textShadowUI.color = new Vector4(textShadowUI.color.r, textShadowUI.color.g, textShadowUI.color.b, textUI.color.a);

                if (textUI.color.a <= 0)
                {
                    Destroy(gameObject);
                }
            }
        }


        // Timer
        timer += Time.deltaTime;
    }
}
