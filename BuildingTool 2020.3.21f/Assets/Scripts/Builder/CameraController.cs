using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called before the first frame update
    public static CameraController instance;

    [Header("Detail Grid 와 Cam 거리")]
    public float detailGridDist;

    private float moveSpeed;
    private float prevMoveSpeed;
    private float scrollSpeed;
    private float zoomSpeed;
  

    [HideInInspector]
    public bool isLookDetailGrid;
    [HideInInspector]
    public bool isCreateDetailGrid;
    [HideInInspector]
    public bool isRemoveDetailGrid;

    private bool isLookAtMove = false;

    
    private Transform bottomFloor;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        bottomFloor = GameObject.Find("BottomFloor").transform;
        moveSpeed = Builder.Instance.camSpeed;
        scrollSpeed = Builder.Instance.camRotateSpeed;
        zoomSpeed = Builder.Instance.camZommSpeed;

        SetSolution();
    }

    private void SetSolution()
    {
        int setWidth = 1920; // 사용자 설정 너비
        int setHeight = 1080; // 사용자 설정 높이

        int deviceWidth = Screen.width; // 기기 너비 저장
        int deviceHeight = Screen.height; // 기기 높이 저장

        Screen.SetResolution(setWidth, (int)(((float)deviceHeight / deviceWidth) * setWidth), true); // SetResolution 함수 제대로 사용하기

        if ((float)setWidth / setHeight < (float)deviceWidth / deviceHeight) // 기기의 해상도 비가 더 큰 경우
        {
            float newWidth = ((float)setWidth / setHeight) / ((float)deviceWidth / deviceHeight); // 새로운 너비
            Camera.main.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f); // 새로운 Rect 적용
        }
        else // 게임의 해상도 비가 더 큰 경우
        {
            float newHeight = ((float)deviceWidth / deviceHeight) / ((float)setWidth / setHeight); // 새로운 높이
            Camera.main.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight); // 새로운 Rect 적용
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            moveSpeed = Builder.Instance.camAccel;
            prevMoveSpeed = moveSpeed;
        }

        if (Input.GetKeyUp(KeyCode.LeftControl))
            moveSpeed = prevMoveSpeed;

        CameraCtrl();


    }
    private void CameraCtrl() //camera control
    {
        if (transform.position.y <= bottomFloor.position.y + 5) //y축이 floor 밑으로 못내려가게 만듬
            transform.position = new Vector3(transform.position.x,bottomFloor.position.y + 5,transform.position.z);

        if (Input.GetMouseButtonDown(1) == true) //마우스 오른쪽 버튼 클릭시 회전 여부
            isLookAtMove = true;
        if (Input.GetMouseButtonUp(1) == true)
            isLookAtMove = false;

        if (Input.GetKey(KeyCode.W) == true)
            transform.position += moveSpeed * transform.forward * Time.deltaTime;

        if (Input.GetKey(KeyCode.S) == true)
            transform.position -= moveSpeed * transform.forward * Time.deltaTime;

        if (Input.GetKey(KeyCode.D) == true)
            transform.position += moveSpeed * transform.right * Time.deltaTime;
        if (Input.GetKey(KeyCode.A) == true)
            transform.position -= moveSpeed * transform.right * Time.deltaTime;

        if (Input.GetKey(KeyCode.Space)) //SpaceBar 수직 상승 이동
        {
            float y;
            y = transform.position.y;
            y += Builder.Instance.camVerticalSpeed * Time.deltaTime;
            transform.position = new Vector3(transform.position.x, y, transform.position.z);
        }
        if (Input.GetKey(KeyCode.LeftShift)) //Shift 수직 하강 이동
        {
            float y;
            y = transform.position.y;
            y -= Builder.Instance.camVerticalSpeed * Time.deltaTime;
            transform.position = new Vector3(transform.position.x, y, transform.position.z);
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0) //휠 확대
        {
            if( transform.position.y > bottomFloor.position.y + 5)
                transform.position += transform.forward * scrollSpeed * Time.deltaTime;
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0) //휠 축소
                transform.position -= transform.forward * scrollSpeed * Time.deltaTime;

        if (detailGridDist + bottomFloor.position.y > transform.position.y) //detailGrild 출력 범위
        {
            isRemoveDetailGrid = false;
            if(!isCreateDetailGrid)
                GridGenerator.instance.CreateDetailGrid();
        }
        else //detailGrid 출력 X
        {
            isCreateDetailGrid = false;
            if(!isRemoveDetailGrid)
                GridGenerator.instance.RemoveDetailGrid();
        }

        if (isLookAtMove) //카메라 축 이동
        {
            Vector3 angles = transform.eulerAngles;
            angles.y += Input.GetAxis("Mouse X") * scrollSpeed * Time.deltaTime;
            angles.x -= Input.GetAxis("Mouse Y") * scrollSpeed * Time.deltaTime;
            transform.eulerAngles = angles;
        }
    }
}
