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
