using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FlashingTextEffect : MonoBehaviour
{
    public TextMeshProUGUI tmpText;

    // Parameters to control the flashing effect
    public float dilationAmplitude = 0.1f; // Maximum dilation change
    public float softnessAmplitude = 0.1f; // Maximum softness change
    public float frequency = 2.0f;         // Speed of the flashing effect

    // Clamp values to prevent negative or overly large values
    public float minDilation = 0.0f;
    public float maxDilation = 1.0f;
    public float minSoftness = 0.0f;
    public float maxSoftness = 1.0f;

    // Initial values to oscillate around
    private float initialDilation;
    private float initialSoftness;

    void Start()
    {
        // Make sure the TextMeshPro component is assigned
        if (tmpText == null)
        {
            tmpText = GetComponent<TextMeshProUGUI>();
        }

        // Get the initial values for dilation and softness
        Material mat = tmpText.fontMaterial;
        initialDilation = mat.GetFloat(ShaderUtilities.ID_FaceDilate);
        initialSoftness = mat.GetFloat(ShaderUtilities.ID_OutlineSoftness);
    }

    void Update()
    {
        // Calculate the oscillating dilation and softness values
        float dilation = initialDilation + Mathf.Sin(Time.time * frequency) * dilationAmplitude;
        float softness = initialSoftness + Mathf.Cos(Time.time * frequency) * softnessAmplitude;

        // Clamp the values to prevent them from going negative or exceeding the desired range
        dilation = Mathf.Clamp(dilation, minDilation, maxDilation);
        softness = Mathf.Clamp(softness, minSoftness, maxSoftness);

        // Update the shader properties
        UpdateTextShaderProperties(dilation, softness);
    }

    void UpdateTextShaderProperties(float dilation, float softness)
    {
        // Access the material of the TextMeshPro component
        Material mat = tmpText.fontMaterial;

        // Set the dilation and softness with clamped values
        mat.SetFloat(ShaderUtilities.ID_FaceDilate, dilation);
        mat.SetFloat(ShaderUtilities.ID_OutlineSoftness, softness);

        // Apply the modified material back to the TextMeshPro component
        tmpText.fontMaterial = mat;
    }
}
