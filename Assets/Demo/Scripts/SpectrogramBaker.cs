using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpectrogramBaker : MonoBehaviour
{
    [SerializeField] RenderTexture src;
    [SerializeField] RenderTexture dst;
    [SerializeField] RenderTexture temp;

    [SerializeField] ComputeShader cs;
    int kernelID;
    int dispatchNum;


    [SerializeField] Material debug;
    void Start()
    {
        kernelID = cs.FindKernel("Update");
        //dispatchNum = (src.width * src.height) / 512;

        src.Release();
        dst.Release();
        temp.Release();
        src.enableRandomWrite = true;
        dst.enableRandomWrite = true;
        temp.enableRandomWrite = true;
        src.Create();
        dst.Create();
        temp.Create();

    }

    void Update()
    {
        cs.SetFloat("_res", src.width);
        cs.SetTexture(kernelID, "src", src);
        cs.SetTexture(kernelID, "dst", dst);
        cs.SetTexture(kernelID, "temp", temp);
        cs.Dispatch(kernelID, src.width/32, src.height /32, 1);


        debug.SetTexture("_MainTex", dst);
    }
}
