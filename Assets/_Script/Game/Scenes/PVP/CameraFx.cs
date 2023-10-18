using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraFx : MonoBehaviour
{
    public Material material;
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (material == null)
        {
            Graphics.Blit(source, destination);
            return;
        }
        Graphics.Blit(source, destination, material);
    }

    public void ToGrayScale()
    {
        StartCoroutine(LerpMaterialProperty("_Ratio", 1, .2f));
    }
    private IEnumerator LerpMaterialProperty(string propertyName, float targetValue, float duration)
    {
        float startTime = Time.time;
        float startValue = material.GetFloat(propertyName);

        while (Time.time - startTime < duration)
        {
            float t = (Time.time - startTime) / duration;
            float lerpedValue = Mathf.Lerp(startValue, targetValue, t);
            material.SetFloat(propertyName, lerpedValue);
            yield return null; // Wait for the next frame
        }
        material.SetFloat(propertyName, targetValue);
        
    }
    public void ToColor()
    {
        StartCoroutine(LerpMaterialProperty("_Ratio", 0, .2f));
    }
}
