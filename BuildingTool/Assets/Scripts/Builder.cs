using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Builder : MonoBehaviour
{
    public static Builder Instance;
    [Header("카메라 이동 속도")]
    public float camSpeed = 2f;
    [Header("카메라 추진속도")]
    public float camAccel;
    [Header("카메라 회전 속도")]
    public float camRotateSpeed = 10;
    [Header("카메라 줌 속도")]
    public float camZommSpeed = 10f;
    [Header("Vertical Speed")]
    public float camVerticalSpeed = 5f;

    private void InitSetting()
    {
   
    }
    private void Awake()
    {
        Instance = this;
        InitSetting();

       
    }

    Vector2 rotation = Vector2.zero;

}
