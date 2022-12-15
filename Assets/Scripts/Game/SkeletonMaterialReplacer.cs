using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class SkeletonMaterialReplacer : MonoBehaviour
{
    [SerializeField] Material ogMat, replaceMat;
    void Start()
    {
        GetComponent<SkeletonMecanim>().CustomMaterialOverride.Add(ogMat, replaceMat);
    }

}
