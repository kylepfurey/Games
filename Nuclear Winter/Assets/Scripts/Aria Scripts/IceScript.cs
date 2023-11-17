using System.Collections;
using UnityEngine;

public class IceScript : MonoBehaviour
{
    [SerializeField] private float speed = 5;
    [SerializeField] private float destroySize = 1;
    [SerializeField] private float scale = 1;
    private float startingScale;

    private void Start()
    {
        startingScale = transform.localScale.y;
    }
    public void Melt()
    {
        StartCoroutine(MeltCoroutine());
    }

    private IEnumerator MeltCoroutine()
    {
        while (transform.lossyScale.y > destroySize)
        {
            float value = transform.localScale.y * 0.01f * speed * Time.unscaledDeltaTime;
            transform.localScale -= new Vector3(0, value, 0);
            transform.localPosition -= new Vector3(0, value / startingScale * scale, 0);

            yield return new WaitForEndOfFrame();
        }
        Destroy(this.gameObject);
    }
}
