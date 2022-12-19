using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PostProcessToggler : MonoBehaviour
{
    public Camera overlayCam;
    public GameObject postFx1, postFX2;
    public Light2D globalLight;

    private void Start()
    {
        OnStart();
        //Invoke(nameof(OnStart), 0.1f);
    }
    private void OnStart()
    {
        GameManager.instance.overlayCam = overlayCam;
        GameManager.instance.postFx1 = postFx1;
        GameManager.instance.postFX2 = postFX2;

        globalLight = GetComponentInChildren<Light2D>();
    }
    public void ToggleHackerFX(bool enable)
    {
        overlayCam.enabled = enable;
        postFx1.SetActive(!enable);
        postFX2.SetActive(enable);

        if (enable)
            globalLight.intensity = 1;
        else
            globalLight.intensity = 0.43f;

    }
}
