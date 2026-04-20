using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FragmentShrink : MonoBehaviour
{
    public Transform target;
    public void StartShrink(float lifetime, float shrinkDuration, Transform objectAsTarget)
    {
        target = GameObject.FindWithTag("Player").transform;
        StartCoroutine(ShrinkAndDestroy(lifetime, shrinkDuration));
    }

    IEnumerator ShrinkAndDestroy(float lifetime, float shrinkDuration)
    {
        Destroy(gameObject, 2.8f);
        // Tunggu dulu
        yield return new WaitForSeconds(lifetime);

        Vector3 originalScale = transform.localScale;
        float elapsed = 0f;

        while (elapsed <= shrinkDuration)
        {
            float tRatio = elapsed / shrinkDuration;
            transform.position = Vector3.Lerp(this.transform.position, target.position, tRatio);
            transform.localScale = Vector3.Lerp(originalScale, Vector3.zero, tRatio);
            elapsed += Time.deltaTime;
            yield return null;
        }
    }
}