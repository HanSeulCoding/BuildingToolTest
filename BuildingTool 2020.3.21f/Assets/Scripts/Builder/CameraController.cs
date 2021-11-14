using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called before the first frame update
    public static CameraController instance;

    [Header("Detail Grid �� Cam �Ÿ�")]
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
        int setWidth = 1920; // ����� ���� �ʺ�
        int setHeight = 1080; // ����� ���� ����

        int deviceWidth = Screen.width; // ��� �ʺ� ����
        int deviceHeight = Screen.height; // ��� ���� ����

        Screen.SetResolution(setWidth, (int)(((float)deviceHeight / deviceWidth) * setWidth), true); // SetResolution �Լ� ����� ����ϱ�

        if ((float)setWidth / setHeight < (float)deviceWidth / deviceHeight) // ����� �ػ� �� �� ū ���
        {
            float newWidth = ((float)setWidth / setHeight) / ((float)deviceWidth / deviceHeight); // ���ο� �ʺ�
            Camera.main.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f); // ���ο� Rect ����
        }
        else // ������ �ػ� �� �� ū ���
        {
            float newHeight = ((float)deviceWidth / deviceHeight) / ((float)setWidth / setHeight); // ���ο� ����
            Camera.main.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight); // ���ο� Rect ����
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
        if (transform.position.y <= bottomFloor.position.y + 5) //y���� floor ������ ���������� ����
            transform.position = new Vector3(transform.position.x,bottomFloor.position.y + 5,transform.position.z);

        if (Input.GetMouseButtonDown(1) == true) //���콺 ������ ��ư Ŭ���� ȸ�� ����
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

        if (Input.GetKey(KeyCode.Space)) //SpaceBar ���� ��� �̵�
        {
            float y;
            y = transform.position.y;
            y += Builder.Instance.camVerticalSpeed * Time.deltaTime;
            transform.position = new Vector3(transform.position.x, y, transform.position.z);
        }
        if (Input.GetKey(KeyCode.LeftShift)) //Shift ���� �ϰ� �̵�
        {
            float y;
            y = transform.position.y;
            y -= Builder.Instance.camVerticalSpeed * Time.deltaTime;
            transform.position = new Vector3(transform.position.x, y, transform.position.z);
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0) //�� Ȯ��
        {
            if( transform.position.y > bottomFloor.position.y + 5)
                transform.position += transform.forward * scrollSpeed * Time.deltaTime;
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0) //�� ���
                transform.position -= transform.forward * scrollSpeed * Time.deltaTime;

        if (detailGridDist + bottomFloor.position.y > transform.position.y) //detailGrild ��� ����
        {
            isRemoveDetailGrid = false;
            if(!isCreateDetailGrid)
                GridGenerator.instance.CreateDetailGrid();
        }
        else //detailGrid ��� X
        {
            isCreateDetailGrid = false;
            if(!isRemoveDetailGrid)
                GridGenerator.instance.RemoveDetailGrid();
        }

        if (isLookAtMove) //ī�޶� �� �̵�
        {
            Vector3 angles = transform.eulerAngles;
            angles.y += Input.GetAxis("Mouse X") * scrollSpeed * Time.deltaTime;
            angles.x -= Input.GetAxis("Mouse Y") * scrollSpeed * Time.deltaTime;
            transform.eulerAngles = angles;
        }
    }
}
