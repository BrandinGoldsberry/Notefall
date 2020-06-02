using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class VHSPostRender : MonoBehaviour
{

    public Material _material;

    void Awake()
    {
        _material = new Material(Shader.Find("Custom/VHSeffect"));
        //_material.SetTexture("_SecondaryTex", Resources.Load("Textures/tvnoise2") as Texture);
        _material.SetFloat("_OffsetPosY", 0f);
        _material.SetFloat("_OffsetColor", 0f);
        _material.SetFloat("_OffsetDistortion", 1000f);
        _material.SetFloat("_Intensity", 0.04f);
    }

    public void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        // TV noise
        _material.SetFloat("_OffsetNoiseX", Random.Range(0f, 0.1f));
        float offsetNoise = _material.GetFloat("_OffsetNoiseY");
        _material.SetFloat("_OffsetNoiseY", offsetNoise + Random.Range(-0.009f, 0.009f));

        // Vertical shift
        float offsetPosY = _material.GetFloat("_OffsetPosY");
        if (offsetPosY > 0.0f)
        {
            _material.SetFloat("_OffsetPosY", offsetPosY - Random.Range(0f, offsetPosY));
        }
        else if (offsetPosY < 0.0f)
        {
            _material.SetFloat("_OffsetPosY", offsetPosY + Random.Range(0f, -offsetPosY));
        }
        else if (Random.Range(0, 500) == 1)
        {
            _material.SetFloat("_OffsetPosY", Random.Range(-0.05f, 0.05f));
        }

        // Channel color shift
        float offsetColor = _material.GetFloat("_OffsetColor");
        if (offsetColor > 0.003f)
        {
            _material.SetFloat("_OffsetColor", offsetColor - 0.001f);
        }
        else if (Random.Range(0, 1000) == 1)
        {
            _material.SetFloat("_OffsetColor", Random.Range(0.001f, 0.03f));
        }

        // Distortion
        if (Random.Range(0, 15) == 1)
        {
            _material.SetFloat("_OffsetDistortion", Random.Range(500f, 1250f));
        }
        else
        {
            _material.SetFloat("_OffsetDistortion", 750f);
        }

        Graphics.Blit(source, destination, _material);
    }
}