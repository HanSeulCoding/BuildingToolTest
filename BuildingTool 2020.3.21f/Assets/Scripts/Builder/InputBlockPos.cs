using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputBlockPos : MonoBehaviour
{
    public static InputBlockPos Instance;
  
    [HideInInspector]
    public Vector3 mouseOnClickPosition;

    private Vector3 _normal = new Vector3();
    private Ray mCameraHitRay = new Ray();
    private Position prevPosition = new Position();
    private Transform blockTransform;
    private void InitSetting()
    {
   
    }
    private void Awake()
    {
        Instance = this;
        InitSetting();
    }
    public Vector3 GetClickMousePos()
    {
        RaycastHit hit;
        bool isBoxClick = false;
        Vector3 _normal = new Vector3();

        mCameraHitRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(mCameraHitRay, out hit) == true)
        {
            mouseOnClickPosition = hit.point;

            //Debug.Log("mouseOnClickPos" + mouseOnClickPosition);
            if (hit.transform.gameObject.layer == 7)
                _normal = hit.normal;
        }
        return _normal;
    }
    public void BuildAndDelClick(bool isBuild,Vector3 _normal)
    {
        if (isBuild)
        {
            Builder.instance.ClickBuildBlock(_normal);
        }
        if (!isBuild)
          Builder.instance.ClickDeleteBlock();
    }
    public RaycastHit PressClick()
    {
        RaycastHit hit;
        Vector3 currentMousePosition = new Vector3();
        mCameraHitRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        int layerMask = 1 << 7;

        if (Physics.Raycast(mCameraHitRay, out hit, Mathf.Infinity,layerMask) == true) //block�̶� pooling block �̶� ��ĥ �� ����ó�� ������� 
        {
            currentMousePosition.x = hit.point.x;
            currentMousePosition.z = hit.point.z;
            if (PlayerInputManager.instance.isPress)
            {
               // Debug.Log("block click");
                InputPos_VisibleBlock(currentMousePosition,hit);
                Debug.Log("BuildBlock");
                
            }
            blockTransform = hit.transform;
            return hit;
        }

        if (Physics.Raycast(mCameraHitRay, out hit,Mathf.Infinity, 1<<6))
        {
            currentMousePosition.x = hit.point.x;
            currentMousePosition.z = hit.point.z;
            if (PlayerInputManager.instance.isPress)
            {
                InputPos_VisibleBlock(currentMousePosition,hit);
            }
        }
        return hit;
    }
    public void InputPos_VisibleBlock(Vector3 currentMousePosition, RaycastHit hit) //���̴� �� ��ġ Drag �� ��� 
    {
        bool isBlockClick = false;
        Position currentPosition;
      
        switch (hit.transform.gameObject.layer)
        { 
            case 7:
                isBlockClick = true;
                currentMousePosition.y = hit.transform.position.y + (Builder.instance.YSize / 10.0f / 2.0f);//��ġ�� ��� Ŭ�� ��,
                if (PlayerInputManager.instance.mCurrentMode == PlayerInputManager.Mode.Delete)
                {
                    currentMousePosition.y = hit.transform.position.y;
                }
                break;
     
                case 6:
                 // currentMousePosition.y = (Builder.instance.YSize / 10.0f / 2.0f); //pooling block Ŭ�� ��.
                    break;

                case 8:
                     currentMousePosition.y = (Builder.instance.YSize / 10.0f / 2.0f);
                     break;
            default:
                return;
        }
        currentPosition = TransPosition.instance.TransLocalPosition(currentMousePosition);


       // if (currentPosition.x != prevPosition.x || currentPosition.z != prevPosition.z || currentPosition.y != prevPosition.y)
        Builder.instance.AddVisibleBlock(Builder.instance.blockType, mouseOnClickPosition, currentMousePosition);

        prevPosition = currentPosition;
    }
    //public void InputPos_DeleteBlock()
    //{
    //    Position currentPosition;
    //    RaycastHit hit;
    //    mCameraHitRay = Camera.main.ScreenPointToRay(Input.mousePosition);
    //    if (Physics.Raycast(mCameraHitRay, out hit) == true)
    //    {
    //        currentMousePosition.x = hit.point.x;
    //        currentMousePosition.z = hit.point.z;
    //        switch (hit.transform.gameObject.layer)
    //        {
    //            case 7:
    //                currentMousePosition.y = hit.transform.position.y + (WorldGenerator.Instance.YSize / 10.0f / 2.0f);
    //                break;
    //        }

    //        currentPosition = TransPosition.instance.TransLocalPosition(currentMousePosition);

    //        if (currentPosition.x != prevPosition.x || currentPosition.z != prevPosition.z || currentPosition.y != prevPosition.y)
    //            WorldGenerator.Instance.AddVisibleBlock(GameManager.instance.selectBlockNum, mouseOnClickPosition, currentMousePosition);

    //        prevPosition = TransPosition.instance.TransLocalPosition(currentMousePosition);
    //    }
    //}
    //Vector2 rotation = Vector2.zero;

    //public void BlockClick()
    //{
    //    mCameraHitRay = Camera.main.ScreenPointToRay(Input.mousePosition);
  
    //    RaycastHit hit;

    //    if (prevBlock != null)
    //        prevBlock.isPrintUI = false;

    //    if (Physics.Raycast(mCameraHitRay, out hit) == true)
    //    {
    //         if (hit.transform.gameObject.layer == 7)
    //        {
    //            BlockSelectImg.instance.SetActive(true);
    //            Block block = hit.transform.gameObject.GetComponent<Block>();
    //            prevBlock = block;
    //            block.isPrintUI = true;            
                
    //            PlayerManager.instance.mCurrentMode = PlayerManager.Mode.BlockSelect;
    //            PlayerManager.instance.clickedBlock = hit.transform.gameObject;
    //            PlayerManager.instance._clickNormal = hit.normal;
    //            Debug.Log("hit");
    //        }
    //        else 
    //        {
    //            if(PlayerManager.instance.clickedBlock != null)  //block ���� �� Ŭ�� �� 
    //            { 
    //                PlayerManager.instance.clickedBlock.GetComponent<Block>().isPrintUI = false; //block Click UI ��� X 
    //                BlockSelectImg.instance.SetActive(false);  
    //            }
    //        }
    //    }
    //}

}
