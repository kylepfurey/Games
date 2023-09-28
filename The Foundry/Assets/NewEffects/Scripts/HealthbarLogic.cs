using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FPSGame;

public class HealthbarLogic : MonoBehaviour
{
    public Health myHealth;
    public Slider sliderMain;
    public Slider sliderLerp;

    public float lerprate = 5f;

    public GameObject visualsContainer;

    public bool showWhenDamaged;
    private float previousHealth;
    private float showTimer;
    public float showDelay = 1.5f;


    public bool dirtyToggler; //setting up a quick and dirty switch to toggle between enemy and player logics

    private void Start()
    {
        if (dirtyToggler == false)
        {
            myHealth = GetComponentInParent<Health>();

        }
        else
        {
            Player player = FindObjectOfType<Player>();
            myHealth = player.GetComponent<Health>();
        }


        myHealth.onDead += testFunction;
        previousHealth = myHealth.maxHealth;

        UpdateValues();
        sliderLerp.value = sliderMain.value;
    }

    private void Update()
    {
        if (previousHealth != myHealth.currentHealth)
        {
            showTimer = Time.time + showDelay;
            previousHealth = myHealth.currentHealth;

        }

        if (showWhenDamaged)
        {
            visualsContainer.SetActive(Time.time < showTimer);
        }

        //visualsContainer.SetActive(Time.time < showTimer);
        if (visualsContainer.activeInHierarchy)
        {
            UpdateValues();
        }
    }

    void UpdateValues()
    {
        sliderMain.maxValue = myHealth.maxHealth;
        sliderMain.value = myHealth.currentHealth;

        sliderLerp.maxValue = myHealth.maxHealth;
        sliderLerp.value = Mathf.Lerp(sliderLerp.value, sliderMain.value, Time.deltaTime * lerprate);
    }

    public void testFunction()
    {
        Debug.Log("LOL");
    }
}
