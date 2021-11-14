using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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

    private int prevIndex;
    private Block prevBlock;
    private Ray mCameraHitRay = new Ray();

    [HideInInspector]
    public Vector3 mouseOnClickPosition;

    //[HideInInspector]
    //public Vector3 currentMousePosition;
    [HideInInspector]
    public Vector3 currentMousePosition = new Vector3();

    private Position prevPosition = new Position();
    private Transform blockTransform;
    private RaycastHit blockHit;
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
       
        GameManager.instance.selectBlockNum = _index;
        //if(GameManager.instance.materials[_index])
        //RootCanvas.instance.transform.Find("NotBlockType").gameObject.SetActive(true);
        PrintSelectImg(_index,prevIndex);
        prevIndex = _index;

    }
    private void PrintSelectImg(int index,int prevIndex)
    {
        if (GameManager.instance.materials[index] != null)
        {
            Inventory.instance.transform.GetChild(index).GetChild(1).gameObject.SetActive(true);
        }
        if(index != prevIndex)
            Inventory.instance.transform.GetChild(prevIndex).GetChild(1).gameObject.SetActive(false);

    }
    public void BuildAndDelClick(bool isBuild)
    {
        Vector3 _normal = new Vector3();
        RaycastHit hit;
        bool isBoxClick = false;
      
        mCameraHitRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(mCameraHitRay, out hit) == true)
        {
            mouseOnClickPosition = hit.point;
            
            Debug.Log("mouseOnClickPos" + mouseOnClickPosition);
            if (hit.transform.gameObject.layer == 7)
                _normal = hit.normal;
        }
        if(isBuild)
          WorldGenerator.Instance.ClickBuildBlock(_normal);
        if (!isBuild)
            WorldGenerator.Instance.ClickDeleteBlock();
    }
    public RaycastHit PressClick()
    {
        RaycastHit hit;

       
        mCameraHitRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        int layerMask = 1 << 7;

        if (Physics.Raycast(mCameraHitRay, out hit, Mathf.Infinity,layerMask) == true) //block�̶� pooling block �̶� ��ĥ �� ����ó�� ������� 
        {
            currentMousePosition.x = hit.point.x;
            currentMousePosition.z = hit.point.z;
            if (PlayerManager.instance.isPress)
            {
                Debug.Log("block click");
                AddVisibleBlock(hit);
                
            }
            blockTransform = hit.transform;
            return hit;
        }


        if (Physics.Raycast(mCameraHitRay, out hit) == true)
        {
            currentMousePosition.x = hit.point.x;
            currentMousePosition.z = hit.point.z;
            if (PlayerManager.instance.isPress)
            {
                AddVisibleBlock(hit);
            }
        }
        return hit;
    }
    public void AddVisibleBlock(RaycastHit hit) //���̴� �� ��ġ Drag �� ��� 
    {
        bool isBlockClick = false;
        Position currentPosition;
      
        switch (hit.transform.gameObject.layer)
        { 
            case 7:
                isBlockClick = true;
                currentMousePosition.y = hit.transform.position.y + (WorldGenerator.Instance.YSize / 10.0f / 2.0f);//��ġ�� ��� Ŭ�� ��,
                if (PlayerManager.instance.mCurrentMode == PlayerManager.Mode.Delete)
                {
                    currentMousePosition.y = hit.transform.position.y;
                }
                break;
     
                case 6:
                  currentMousePosition.y = (WorldGenerator.Instance.YSize / 10.0f / 2.0f); //pooling block Ŭ�� ��.
                    break;

                case 8:
                     currentMousePosition.y = (WorldGenerator.Instance.YSize / 10.0f / 2.0f);
                     break;
            default:
                return;
        }
        currentPosition = TransPosition.instance.TransLocalPosition(currentMousePosition);
        //Debug.Log("currentPos X :" + currentPosition.x + "Y : " + currentPosition.y + "Z :" + currentPosition.z);

        if (currentPosition.x != prevPosition.x || currentPosition.z != prevPosition.z || currentPosition.y != prevPosition.y)
            WorldGenerator.Instance.AddVisibleBlock(GameManager.instance.selectBlockNum, mouseOnClickPosition, currentMousePosition);

        prevPosition = currentPosition;
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

            currentPosition = TransPosition.instance.TransLocalPosition(currentMousePosition);

            if (currentPosition.x != prevPosition.x || currentPosition.z != prevPosition.z || currentPosition.y != prevPosition.y)
                WorldGenerator.Instance.AddVisibleBlock(GameManager.instance.selectBlockNum, mouseOnClickPosition, currentMousePosition);

            prevPosition = TransPosition.instance.TransLocalPosition(currentMousePosition);
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
                if(PlayerManager.instance.clickedBlock != null)  //block ���� �� Ŭ�� �� 
                { 
                    PlayerManager.instance.clickedBlock.GetComponent<Block>().isPrintUI = false; //block Click UI ��� X 
                    BlockSelectImg.instance.SetActive(false);  
                }
            }
        }
    }

    public void DragCreateBlock()
    {

    }

}
