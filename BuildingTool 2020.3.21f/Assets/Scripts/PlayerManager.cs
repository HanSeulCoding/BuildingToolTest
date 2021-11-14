using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


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
    public Vector3 testPos;

    [HideInInspector]
    public Vector3 _clickNormal;

    [HideInInspector]
    public Mode mCurrentMode = Mode.Insert;

    [HideInInspector]
    public GameObject clickedBlock;

    [HideInInspector]
    public bool isPress;

    private float isPressTime;
    

    private Ray mCameraHitRay = new Ray();

  
    private void Awake()
    {
        instance = this;
    }
  
    private void Update()
    { 

        if (Input.GetKeyDown(KeyCode.Insert) == true)
        {
            RootCanvas.instance.PrintMode("Insert Mode");
            mCurrentMode = Mode.Insert;
        }
        if (Input.GetKey(KeyCode.LeftControl) == true)
            mCurrentMode = Mode.BlockSelect;
        if (Input.GetKeyDown(KeyCode.Delete) == true)
        {
            mCurrentMode = Mode.Delete;
            RootCanvas.instance.PrintMode("Delete Mode");
        }
        
        //if (Input.GetKeyDown(KeyCode.Delete) == true)
        //    mCurrentMode = Mode.Delete;


        BlockManageUpdate();
        BlockTypeSelect(); //블럭 타입 선택
        
    }
    private bool IsDragDecide()
    {
        Position mouseOnClickPos = TransPosition.instance.TransLocalPosition(Builder.Instance.mouseOnClickPosition);
        Position currentMousePos = TransPosition.instance.TransLocalPosition(Builder.Instance.currentMousePosition);
       // Builder.Instance.blockY += 0.5f;


        if (mouseOnClickPos.x == currentMousePos.x && mouseOnClickPos.y == currentMousePos.y
            && mouseOnClickPos.z == currentMousePos.z)
            return false;
        Debug.Log("World MouseOnClickPos" + Builder.Instance.mouseOnClickPosition);
        Debug.Log("World CurrentMousePos" + Builder.Instance.currentMousePosition);
        Debug.Log("Local mouseOnclickPos X : " + mouseOnClickPos.x + "Y : " + mouseOnClickPos.y + "Z :" + mouseOnClickPos.z);
        Debug.Log("Local currentMousePos X : " + currentMousePos.x + "Y : " + currentMousePos.y + "Z :" + currentMousePos.z);
        return true;

    }
    //public bool IsPress()
    //{
    //    isPressTime += Time.deltaTime;
    //    if (isPressTime > 0.7)
    //    {
    //        isPressTime = 0.0f;
    //        return true;
            
          
    //    }

    //    return false;
     
    //}
   
    private void BlockManageUpdate()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButtonDown(0) == true) //클릭 건물 건설 및 삭제
            {
                switch (mCurrentMode)
                {
                    case Mode.Insert:
                        Builder.Instance.BuildAndDelClick(true);
                        break;
                    case Mode.Delete:
                        Builder.Instance.BuildAndDelClick(false);
                        break;
                }

            }
            if (Input.GetMouseButton(0) == true) //Drag 시 작동
            {
                isPressTime += Time.deltaTime;

                if (isPressTime > 0.15)
                    isPress = true;
                else
                    isPress = false;
                Builder.Instance.PressClick();

                //if (isPress)
                //{ 
                //    Builder.Instance.AddVisibleBlock(hit);
                //}
            }
            if (Input.GetMouseButtonUp(0) == true) //건물 건설 및 삭제 
            {
                isPressTime = 0.0f;

                switch (mCurrentMode)
                {
                    case Mode.Insert:
                        WorldGenerator.Instance.DragBuildBlock();
                        break;

                    case Mode.Delete:
                        if (isPress)
                        {
                            Debug.Log("Del IsPress");
                            WorldGenerator.Instance.DragDeleteBlock();
                            WorldGenerator.Instance.VisibleBlockFalse();
                        }
                        break;
                }
                isPress = false;

                if (WorldGenerator.Instance.undoRedoKey <= 9)
                    WorldGenerator.Instance.undoRedoKey++;
            }
        }
        if(Input.GetKey(KeyCode.LeftArrow))
        {
            if(WorldGenerator.Instance.undoRedoKey >= 0)
                WorldGenerator.Instance.Undo();
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            if(WorldGenerator.Instance.undoRedoKey > 0)
                 WorldGenerator.Instance.undoRedoKey--;
        }
        if (Input.GetKey(KeyCode.RightArrow))
            WorldGenerator.Instance.Redo();

        if(Input.GetKeyUp(KeyCode.RightArrow))
            WorldGenerator.Instance.undoRedoKey++;

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
                        block.TranslateScale(Builder.Instance.mouseOnClickPosition, _clickNormal); //Block Scale 변경
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
    }
   
}
