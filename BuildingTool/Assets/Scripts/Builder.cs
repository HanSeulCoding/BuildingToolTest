using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Builder : MonoBehaviour
{
    public static Builder Instance;
    [Header("ī�޶� �̵� �ӵ�")]
    public float camSpeed = 2f;
    [Header("ī�޶� �����ӵ�")]
    public float camAccel;
    [Header("ī�޶� ȸ�� �ӵ�")]
    public float camRotateSpeed = 10;
    [Header("ī�޶� �� �ӵ�")]
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
