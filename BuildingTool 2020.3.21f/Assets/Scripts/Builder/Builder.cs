using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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

    private Block prevBlock;
    private Ray mCameraHitRay = new Ray();

    [HideInInspector]
    public Vector3 mouseOnClickPosition;

    [HideInInspector]
    public Vector3 currentMousePosition;

    [HideInInspector]
    public int blockSelectIndex = 0;

    private Position prevPosition = new Position();
    private void InitSetting()
    {
   
    }
    private void Awake()
    {
        Instance = this;
        InitSetting();

       
    }

    public void AddBlockTypeSelect(int _index)
    {
        blockSelectIndex = _index;
        RootCanvas.instance.SelectBlock(_index);
    }
    public void OnClick()
    {
        RaycastHit hit;
        mCameraHitRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(mCameraHitRay, out hit) == true)
        {
            mouseOnClickPosition = hit.point;
        }
    }

    public void AddVisibleBlock()
    {
        Position currentPosition;
        RaycastHit hit;
        mCameraHitRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(mCameraHitRay, out hit) == true)
        {
            currentMousePosition.x = hit.point.x;
            currentMousePosition.z = hit.point.z;
            switch (hit.transform.gameObject.layer)
            { 
                case 7:
                    currentMousePosition.y = hit.transform.position.y + (WorldGenerator.Instance.YSize / 10.0f / 2.0f);
                    if(PlayerManager.instance.mCurrentMode == PlayerManager.Mode.Delete)
                    {
                        currentMousePosition.y = hit.transform.position.y;
                    }
                    break;
                case 6:
                    currentMousePosition.y = (WorldGenerator.Instance.YSize / 10.0f / 2.0f);
                    break;

            }
            currentPosition = Math.instance.TransLocalPosition(currentMousePosition);

            if (currentPosition.x != prevPosition.x || currentPosition.z != prevPosition.z || currentPosition.y != prevPosition.y)
                WorldGenerator.Instance.MousePosTranslate(blockSelectIndex, mouseOnClickPosition, currentMousePosition);

            prevPosition = Math.instance.TransLocalPosition(currentMousePosition);
        }
        //RootCanvas.Instance.SetWorkFlow(WorldGenerator.Instance.kCurrentWorkList, WorldGenerator.Instance.kCurrentFillWorkCount, WorldGenerator.Instance.kCurrentWorkNextIndex - 1);

    }
    public void DeleteBlock()
    {
        Position currentPosition;
        RaycastHit hit;
        mCameraHitRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(mCameraHitRay, out hit) == true)
        {
            currentMousePosition.x = hit.point.x;
            currentMousePosition.z = hit.point.z;
            switch (hit.transform.gameObject.layer)
            {
                case 7:
                    currentMousePosition.y = hit.transform.position.y + (WorldGenerator.Instance.YSize / 10.0f / 2.0f);
                    break;
            }


            currentPosition = Math.instance.TransLocalPosition(currentMousePosition);

            if (currentPosition.x != prevPosition.x || currentPosition.z != prevPosition.z || currentPosition.y != prevPosition.y)
                WorldGenerator.Instance.MousePosTranslate(blockSelectIndex, mouseOnClickPosition, currentMousePosition);

            prevPosition = Math.instance.TransLocalPosition(currentMousePosition);
        }
    }
    Vector2 rotation = Vector2.zero;

    public void BlockClick()
    {
        mCameraHitRay = Camera.main.ScreenPointToRay(Input.mousePosition);
  
        RaycastHit hit;

        if (prevBlock != null)
            prevBlock.isPrintUI = false;

        if (Physics.Raycast(mCameraHitRay, out hit) == true)
        {
             if (hit.transform.gameObject.layer == 7)
            {
                BlockSelectImg.instance.SetActive(true);
                Block block = hit.transform.gameObject.GetComponent<Block>();
                prevBlock = block;
                block.isPrintUI = true;            
                
                PlayerManager.instance.mCurrentMode = PlayerManager.Mode.BlockSelect;
                PlayerManager.instance.clickedBlock = hit.transform.gameObject;
                PlayerManager.instance._clickNormal = hit.normal;
                Debug.Log("hit");
            }
            else 
            {
                if(PlayerManager.instance.clickedBlock != null)  //block 외의 것 클릭 시 
                { 
                    PlayerManager.instance.clickedBlock.GetComponent<Block>().isPrintUI = false; //block Click UI 출력 X 
                    BlockSelectImg.instance.SetActive(false);  
                }
            }
        }
    }

    public void DragCreateBlock()
    {

    }

}
