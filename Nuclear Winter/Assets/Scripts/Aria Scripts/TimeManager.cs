using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeManager : MonoBehaviour
{
    [SerializeField] float timeStopStart = 9;
    [SerializeField] float shockwaveStart = 6;
    [SerializeField] float heatwaveStart = 7;
    [SerializeField] float deathwaveStart = 8;
    [SerializeField] MeshRenderer snow;
    [SerializeField] float meltSpeed = 0.02f;
    public float timeScale = 0.025f;

    private GameObject[] treeObject;
    private MeshRenderer[] trees;
    private bool melt;


    private void Start()
    {
        GameManager.instance.shockwave.SetActive(false);
        GameManager.instance.heatwave.SetActive(false);
        GameManager.instance.deathwave.SetActive(false);
        StartCoroutine(StartShockwave());
        StartCoroutine(StartHeatwave());
        StartCoroutine(StartDeathwave());
        StartCoroutine(StopTime());

        int index = 0;
        foreach (GameObject Object in SceneManager.GetActiveScene().GetRootGameObjects())
        {
            if (Object.name.Contains("tree"))
            {
                GameObject[] oldArray = treeObject;
                treeObject = new GameObject[index + 1];

                for (int i = 0; i < treeObject.Length; i++)
                {
                    if (i != index)
                    {
                        treeObject[i] = oldArray[i];
                    }
                    else
                    {
                        treeObject[i] = Object;
                    }
                }

                index++;
            }
        }

        trees = new MeshRenderer[treeObject.Length];

        index = 0;
        foreach (GameObject Object in treeObject)
        {
            trees[index] = Object.GetComponent<MeshRenderer>();

            index++;
        }
    }
    private IEnumerator StartShockwave()
    {
        yield return new WaitForSeconds(shockwaveStart);
        GameManager.instance.shockwave.SetActive(true);
        GameManager.audio.Play("Rumble");
        GameManager.audio.Play("Wind");
        GameManager.audio.Play("Cooking");
    }
    private IEnumerator StartHeatwave()
    {
        yield return new WaitForSeconds(heatwaveStart);
        GameManager.instance.heatwave.SetActive(true);
        melt = true;
    }
    private IEnumerator StartDeathwave()
    {
        yield return new WaitForSeconds(deathwaveStart);
        GameManager.instance.deathwave.SetActive(true);
    }
    private IEnumerator StopTime()
    {
        yield return new WaitForSeconds(timeStopStart);
        GameManager.timeScale = timeScale;
        GameManager.audio.Stop("Rumble");
        GameManager.audio.Stop("Nuclear Warning");
        GameManager.audio.Play("Nuclear Warning 2", timeStopStart);
        GameManager.audio.PlayOnce("Shatter");
    }

    private void Update()
    {
        if (melt)
        {
            Material snowMaterial = snow.material;
            snowMaterial.SetFloat("_Transition", snowMaterial.GetFloat("_Transition") + meltSpeed * Time.unscaledDeltaTime);
            snow.material = snowMaterial;

            foreach (MeshRenderer tree in trees)
            {
                Material treeMaterial = tree.material;
                treeMaterial.SetFloat("_Transition", treeMaterial.GetFloat("_Transition") + meltSpeed * Time.unscaledDeltaTime / 3);
                tree.material = treeMaterial;
            }

            if (snow.material.GetFloat("_Transition") > 1 && snow.tag != "Stone")
            {
                snow.tag = "Stone";
            }
        }
    }
}
