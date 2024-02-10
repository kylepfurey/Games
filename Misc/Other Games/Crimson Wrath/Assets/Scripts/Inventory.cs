using TMPro;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [Header("Item Variables")]
    [HideInInspector] public int bottleCaps;
    [HideInInspector] public bool pencil;
    [HideInInspector] public bool cinnamonStick;
    [HideInInspector] public bool paperClip;
    [HideInInspector] public bool pinwheel;
    [HideInInspector] public bool rubberBand;

    [Header("Item Object Variables")]
    [SerializeField] private GameObject[] bottleCapsObject;
    [SerializeField] private GameObject pencilObject;
    [SerializeField] private GameObject cinnamonStickObject;
    [SerializeField] private GameObject paperClipObject;
    [SerializeField] private GameObject rubberBandObject;
    [SerializeField] private GameObject pinwheelObject;
    [SerializeField] private GameObject rockObject;

    [Header("Mantis Dialogue Variables")]
    [SerializeField] private DialogueReader mantisDialogue;
    [SerializeField] private string middayDialogueStart;
    [SerializeField] private string middayDialogueEnd;
    [SerializeField] private string nightDialogueStart;
    [SerializeField] private string nightDialogueEnd;
    private bool talked;
    private bool talkedSecond;
    private bool go1;
    private bool go2;

    [Header("HUD Variables")]
    [SerializeField] private TextMeshProUGUI talkToMantis;
    [SerializeField] private TextMeshProUGUI bottleCapsText;
    [SerializeField] private TextMeshProUGUI bottleCapsTextShadow;
    [SerializeField] private TextMeshProUGUI pencilText;
    [SerializeField] private TextMeshProUGUI cinnamonStickText;
    [SerializeField] private TextMeshProUGUI paperClipText;
    [SerializeField] private TextMeshProUGUI rubberBandText;
    [SerializeField] private TextMeshProUGUI pinwheelText;
    [SerializeField] private TextMeshProUGUI explore;
    [SerializeField] private Color dontHaveColor;
    [SerializeField] private Color haveColor;

    private void Start()
    {
        for (int i = 0; i < bottleCapsObject.Length; i++)
        {
            bottleCapsObject[i].SetActive(false);
        }

        talkToMantis.transform.parent.gameObject.SetActive(true);
        bottleCapsText.transform.parent.gameObject.SetActive(false);
        pencilText.transform.parent.gameObject.SetActive(false);
        cinnamonStickText.transform.parent.gameObject.SetActive(false);
        paperClipText.transform.parent.gameObject.SetActive(false);
        rubberBandText.transform.parent.gameObject.SetActive(false);
        pinwheelText.transform.parent.gameObject.SetActive(false);
        explore.transform.parent.gameObject.SetActive(false);
    }

    private void Update()
    {
        for (int i = 0; i < bottleCaps; i++)
        {
            bottleCapsObject[i].SetActive(true);
        }

        pencilObject.SetActive(pencil);
        cinnamonStickObject.SetActive(cinnamonStick);
        paperClipObject.SetActive(paperClip);
        rubberBandObject.SetActive(rubberBand);
        pinwheelObject.SetActive(pinwheel);

        if (bottleCaps == 4 && pencil && cinnamonStick && paperClip && pinwheel && rubberBand && rockObject)
        {
            Destroy(rockObject);
        }

        if (!go1)
        {
            if (bottleCaps == 4 && pencil && cinnamonStick && talked)
            {
                go1 = true;
                mantisDialogue.dialogueName = middayDialogueStart;
                mantisDialogue.dialogueEnd = middayDialogueEnd;
                mantisDialogue.FindDialogue();
                mantisDialogue.hasSpoken = false;

                bottleCapsText.transform.parent.gameObject.SetActive(false);
                pencilText.transform.parent.gameObject.SetActive(false);
                cinnamonStickText.transform.parent.gameObject.SetActive(false);
                paperClipText.transform.parent.gameObject.SetActive(false);
                rubberBandText.transform.parent.gameObject.SetActive(false);
                pinwheelText.transform.parent.gameObject.SetActive(false);

                talkToMantis.transform.parent.gameObject.SetActive(true);
            }
        }

        if (!go2)
        {
            if (bottleCaps == 4 && pencil && cinnamonStick && paperClip && pinwheel && rubberBand && talked && talkedSecond)
            {
                go2 = true;
                mantisDialogue.dialogueName = nightDialogueStart;
                mantisDialogue.dialogueEnd = nightDialogueStart;
                mantisDialogue.FindDialogue();
                mantisDialogue.hasSpoken = false;

                bottleCapsText.transform.parent.gameObject.SetActive(false);
                pencilText.transform.parent.gameObject.SetActive(false);
                cinnamonStickText.transform.parent.gameObject.SetActive(false);
                paperClipText.transform.parent.gameObject.SetActive(false);
                rubberBandText.transform.parent.gameObject.SetActive(false);
                pinwheelText.transform.parent.gameObject.SetActive(false);

                talkToMantis.transform.parent.gameObject.SetActive(true);
            }
        }


        if (bottleCaps < 4)
        {
            bottleCapsTextShadow.text = "Find " + (4 - bottleCaps) + " Wheels";
            bottleCapsText.text = "Find " + (4 - bottleCaps) + " Wheels";

            bottleCapsText.color = dontHaveColor;
        }
        else
        {
            bottleCapsTextShadow.text = "Found " + 4 + " Wheels";
            bottleCapsText.text = "Found " + 4 + " Wheels";

            bottleCapsText.color = haveColor;
        }

        if (pencil)
        {
            pencilText.color = haveColor;
        }
        else
        {
            pencilText.color = dontHaveColor;
        }

        if (cinnamonStick)
        {
            cinnamonStickText.color = haveColor;
        }
        else
        {
            cinnamonStickText.color = dontHaveColor;
        }

        if (paperClip)
        {
            paperClipText.color = haveColor;
        }
        else
        {
            paperClipText.color = dontHaveColor;
        }

        if (rubberBand)
        {
            rubberBandText.color = haveColor;
        }
        else
        {
            rubberBandText.color = dontHaveColor;
        }

        if (pinwheel)
        {
            pinwheelText.color = haveColor;
        }
        else
        {
            pinwheelText.color = dontHaveColor;
        }
    }

    public void TalkToMantis(bool second)
    {
        talkToMantis.transform.parent.gameObject.SetActive(false);

        if (!second)
        {
            talked = true;
        }
        else
        {
            talkedSecond = true;
        }
    }

    public void Wheels()
    {
        if (!talkToMantis.transform.parent.gameObject.activeSelf)
        {
            bottleCapsText.transform.parent.gameObject.SetActive(true);
        }
    }

    public void Axle()
    {
        if (!talkToMantis.transform.parent.gameObject.activeSelf)
        {
            pencilText.transform.parent.gameObject.SetActive(true);
            cinnamonStickText.transform.parent.gameObject.SetActive(true);
        }
    }

    public void MiddayItems()
    {
        if (!talkToMantis.transform.parent.gameObject.activeSelf)
        {
            paperClipText.transform.parent.gameObject.SetActive(true);
            rubberBandText.transform.parent.gameObject.SetActive(true);
            pinwheelText.transform.parent.gameObject.SetActive(true);
        }
    }

    public void Explore()
    {
        explore.transform.parent.gameObject.SetActive(true);
    }
}
