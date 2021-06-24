using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The background has an animated effect using a shader
public class Background : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(ScheduleAnimateMaterial());
    }

    private IEnumerator ScheduleAnimateMaterial()
    {
        yield return new WaitForSeconds(4);
        AnimateMaterial();
        while (true)
        {
            yield return new WaitForSeconds(10);
            AnimateMaterial();
        }
    }

    private void AnimateMaterial()
    {
        LeanTween.value(0, 0.5f, 1f).setOnUpdate(OnTweenUpdate);
    }

    private void OnTweenUpdate(float t)
    {
        var material = GetComponent<SpriteRenderer>().sharedMaterial;
        material.SetFloat("_Radius", t);
        GetComponent<SpriteRenderer>().sharedMaterial = material;
    }
}
