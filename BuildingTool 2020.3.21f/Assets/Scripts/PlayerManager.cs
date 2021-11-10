using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    public enum Mode
    {
        Insert, //추가 모드
        Delete,
        BlockSelect,
        VisibleAdd
    }
    [HideInInspector]
    public bool isTranslateScale;

    [HideInInspector]
    public Vector3 mouseOnClickPosition;

    public Vector3 testPos;

    [HideInInspector]
    public Vector3 currentMousePosition;

    [HideInInspector]
    public Vector3 _clickNormal;

    [HideInInspector]
    public Mode mCurrentMode = Mode.Insert;

    [HideInInspector]
    public GameObject clickedBlock;
    private Position prevPosition = new Position();

    private bool mIsLookAtMove;

    private Ray mCameraHitRay = new Ray();
    private void Awake()
    {
        instance = this;
    }
  
    private void Update()
    {
        //BuilderMoveUpdate();

        if (Input.GetKeyDown(KeyCode.Insert) == true)
            mCurrentMode = Mode.Insert;
        if (Input.GetKey(KeyCode.LeftControl) == true)
            mCurrentMode = Mode.BlockSelect;
        
        //if (Input.GetKeyDown(KeyCode.Delete) == true)
        //    mCurrentMode = Mode.Delete;


        BlockManageUpdate();
        BlockTypeSelect(); //블럭 타입 선택
        
    }

    //private Vector3 GetMousePos()
    //{
    //    Vector3 temp;
    //    mCameraHitRay = Camera.main.ScreenPointToRay(Input.mousePosition);

    //    RaycastHit hit;
    //    if (Physics.Raycast(mCameraHitRay, out hit) == true)
    //    {
    //        temp = hit.transform.position;
    //    }
    //    return temp;
    //}

    private void BlockManageUpdate()
    {

        Position currentPosition;
        RaycastHit hit;
        mCameraHitRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButtonDown(0) == true)
        {
            if (Physics.Raycast(mCameraHitRay, out hit) == true)
            {
                mouseOnClickPosition = hit.point;
            }
        }
        if (Input.GetMouseButton(0) == true)
        {
            if (Physics.Raycast(mCameraHitRay, out hit) == true)
            {
                currentMousePosition = hit.point;
                switch (hit.transform.gameObject.layer)
                {
                    case 7:
                        currentMousePosition.y = hit.transform.position.y + (WorldGenerator.Instance.YSize / 10.0f / 2.0f);
                        break;
                    default:
                        currentMousePosition.y = (WorldGenerator.Instance.YSize / 10.0f / 2.0f);
                        break;

                }

                
                currentPosition = Math.instance.TransLocalPosition(currentMousePosition);

               // if (currentPosition.x != prevPosition.x || currentPosition.z != prevPosition.z)
                WorldGenerator.Instance.VisibleAddBlock(Builder.Instance.blockSelectIndex, mouseOnClickPosition, currentMousePosition);

               // prevPosition = Math.instance.TransLocalPosition(currentMousePosition);
            }
        }
        if (Input.GetMouseButtonUp(0) == true)
        {
            WorldGenerator.Instance.BuildBlock();
        }

        //if (Input.GetMouseButtonDown(0) == true)
        //{
        //    switch (mCurrentMode)
        //    {
        //        //case Mode.Delete:
        //        //    {
        //        //        Builder.Instance.RemoveBlock();
        //        //    }
        //        //    break;
        //        case Mode.Insert:
        //            {

        //                Builder.Instance.AddBlock();

        //            }
        //            break;
        //    }

        //}
        //if(Input.GetMouseButton(0) == true)
        //{

        //}

        if (mCurrentMode == Mode.Insert)
            {
                RootCanvas.instance.PrintMode("Insert Mode");
                if (clickedBlock != null) //기존에 보이던 Select Img 없애야함 
                {
                    Block block = clickedBlock.GetComponent<Block>();
                    block.isPrintUI = false;
                    BlockSelectImg.instance.SetActive(false);
                }
            }

            if (mCurrentMode == Mode.BlockSelect) //block Click 시
            {
                RootCanvas.instance.PrintMode("Scale Modify Mode");

                if (Input.GetMouseButton(0) == true)
                {
                    Builder.Instance.BlockClick();
                }
                if (isTranslateScale) //Scale 변경 가능 시 
                {
                    if (clickedBlock != null)
                    {
                        Block block = clickedBlock.GetComponent<Block>();
                        block.TranslateScale(mouseOnClickPosition, _clickNormal); //Block Scale 변경
                    }
                }

            }
       }
    
    
    public void BlockTypeSelect()
    {
        if (Input.GetKey(KeyCode.Alpha1) == true)
            Builder.Instance.AddBlockTypeSelect(0);
        if (Input.GetKey(KeyCode.Alpha2) == true)
            Builder.Instance.AddBlockTypeSelect(1);
        if (Input.GetKey(KeyCode.Alpha3) == true)
            Builder.Instance.AddBlockTypeSelect(2);
        if (Input.GetKey(KeyCode.Alpha4) == true)
            Builder.Instance.AddBlockTypeSelect(3);
        if (Input.GetKey(KeyCode.Alpha5) == true)
            Builder.Instance.AddBlockTypeSelect(4);
        if (Input.GetKey(KeyCode.Alpha6) == true)
            Builder.Instance.AddBlockTypeSelect(5);
        if (Input.GetKey(KeyCode.Alpha7) == true)
            Builder.Instance.AddBlockTypeSelect(6);
        if (Input.GetKey(KeyCode.Alpha8) == true)
            Builder.Instance.AddBlockTypeSelect(7);
        if (Input.GetKey(KeyCode.Alpha9) == true)
            Builder.Instance.AddBlockTypeSelect(8);
        if (Input.GetKey(KeyCode.Alpha0) == true)
            Builder.Instance.AddBlockTypeSelect(9);
    }
   
}
