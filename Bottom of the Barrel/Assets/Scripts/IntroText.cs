using UnityEngine;

public class IntroText : MonoBehaviour
{
    [SerializeField] private float lerpSpeed = 2;
    [SerializeField] private GameObject target = null;
    [SerializeField] private GameObject target2 = null;
    [SerializeField] private float distance = 1;
    [SerializeField] private float delay = 1;
    private float timer = 0;
    private bool reachedTarget = false;

    private void Update()
    {
        if (!reachedTarget)
        {
            transform.position = Vector3.Lerp(transform.position, target.transform.position, lerpSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, target.transform.position) <= distance)
            {
                reachedTarget = true;
            }
        }
        else
        {
            if (timer < delay)
            {
                timer += Time.deltaTime;

                transform.position = Vector3.Lerp(transform.position, target.transform.position, lerpSpeed * Time.deltaTime);
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, target2.transform.position, lerpSpeed * Time.deltaTime);
            }
        }
    }
}
