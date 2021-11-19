using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager instance;
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
    public Vector3 _clickNormal;

    [HideInInspector]
    public Mode mCurrentMode = Mode.Insert;

    [HideInInspector]
    public GameObject clickedBlock;

    [HideInInspector]
    public bool isPress;

    private int undoCount;
    private bool isDelete;
    private float isPressTime;
    private int prevIndex = 0;
    private Ray mCameraHitRay = new Ray();
    private Vector3 _normal;
    private void Awake()
    {
        instance = this;
    }
  
    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl) == true)
            mCurrentMode = Mode.BlockSelect;
        
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            RootCanvas.instance.QuitPannelEnter();
        }
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            isDelete = !isDelete;
            if (isDelete)
            {
                mCurrentMode = Mode.Delete;
                RootCanvas.instance.PrintMode("Delete Mode");
            }
            if (!isDelete)
            {
                mCurrentMode = Mode.Insert;
                RootCanvas.instance.PrintMode("Insert Mode");
            }
        }
        if(Input.GetKey(KeyCode.LeftControl))
        {
            if(Input.GetKeyDown(KeyCode.Z))
            {
                undoCount %= 5;
                Builder.instance.Undo(undoCount);
                undoCount++;
            }
        }
        BuildBlock_Click();
        BuildVisibleBlock_Drag();
        BuildBlock_DragComplete();

        BlockManageUpdate();
        BlockTypeSelect(); //블럭 타입 선택
        
    }
    //private bool IsDragDecide()
    //{
    //    Position mouseOnClickPos = TransPosition.instance.TransLocalPosition(Builder.Instance.mouseOnClickPosition);
    //    Position currentMousePos = TransPosition.instance.TransLocalPosition(Builder.Instance.currentMousePosition);
    //   // Builder.Instance.blockY += 0.5f;


    //    if (mouseOnClickPos.x == currentMousePos.x && mouseOnClickPos.y == currentMousePos.y
    //        && mouseOnClickPos.z == currentMousePos.z)
    //        return false;
    //    Debug.Log("World MouseOnClickPos" + Builder.Instance.mouseOnClickPosition);
    //    Debug.Log("World CurrentMousePos" + Builder.Instance.currentMousePosition);
    //    Debug.Log("Local mouseOnclickPos X : " + mouseOnClickPos.x + "Y : " + mouseOnClickPos.y + "Z :" + mouseOnClickPos.z);
    //    Debug.Log("Local currentMousePos X : " + currentMousePos.x + "Y : " + currentMousePos.y + "Z :" + currentMousePos.z);
    //    return true;

    //}
    public void BuildBlock_Click()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButtonDown(0) == true) //클릭 건물 건설 및 삭제
            {
                _normal = InputBlockPos.Instance.GetClickMousePos();
            }
        }
    }
    private void BuildVisibleBlock_Drag()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButton(0) == true) //Drag 시 작동
            {
                isPressTime += Time.deltaTime;

                if (isPressTime > 0.15)
                    isPress = true;
                else
                    isPress = false;
                InputBlockPos.Instance.PressClick();
            }
        }
    }
    private void BuildBlock_DragComplete()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
           // if (Inventory.instance.IsActiveTrue_BoxImg(Builder.instance.blockType)) //inventory boxImg additional option
               // return;

            if (Input.GetMouseButtonUp(0) == true) //건물 건설 및 삭제 
            {
                isPressTime = 0.0f;

                switch (mCurrentMode)
                {
                    case Mode.Insert:
                        if (!isPress)
                            InputBlockPos.Instance.BuildAndDelClick(true,_normal);
                        Builder.instance.DragBuildBlock();
                        break;

                    case Mode.Delete:

                        if (!isPress)
                            InputBlockPos.Instance.BuildAndDelClick(false,_normal);
                        if (isPress)
                        {
                            Builder.instance.DragDeleteBlock();
                            Builder.instance.VisibleBlockFalse();
                        }
                        break;
                }
                isPress = false;

                if (Builder.instance.undoRedoKey <= 9)
                    Builder.instance.undoRedoKey++;
            }
        }
    }
    private void BlockManageUpdate()
    {
        //if (!EventSystem.current.IsPointerOverGameObject())
        //{
        //    if (Input.GetMouseButtonDown(0) == true) //클릭 건물 건설 및 삭제
        //    {
        //        switch (mCurrentMode)
        //        {
        //            case Mode.Insert:
        //                Builder.Instance.BuildAndDelClick(true);
        //                break;
        //            case Mode.Delete:
        //                Builder.Instance.BuildAndDelClick(false);
        //                break;
        //        }

        //    }
        //    if (Input.GetMouseButton(0) == true) //Drag 시 작동
        //    {
        //        isPressTime += Time.deltaTime;

        //        if (isPressTime > 0.15)
        //            isPress = true;
        //        else
        //            isPress = false;
        //        Builder.Instance.PressClick();
        //    }
        //    if (Input.GetMouseButtonUp(0) == true) //건물 건설 및 삭제 
        //    {
        //        isPressTime = 0.0f;

        //        switch (mCurrentMode)
        //        {
        //            case Mode.Insert:
        //                WorldGenerator.Instance.DragBuildBlock();
        //                break;

        //            case Mode.Delete:
        //                if (isPress)
        //                {
        //                    Debug.Log("Del IsPress");
        //                    WorldGenerator.Instance.DragDeleteBlock();
        //                    WorldGenerator.Instance.VisibleBlockFalse();
        //                }
        //                break;
        //        }
        //        isPress = false;

        //        if (WorldGenerator.Instance.undoRedoKey <= 9)
        //            WorldGenerator.Instance.undoRedoKey++;
        //    }
        //}
  

        if(Input.GetKeyUp(KeyCode.RightArrow))
            Builder.instance.undoRedoKey++;

            if (mCurrentMode == Mode.Insert)
            {
                //RootCanvas.instance.PrintMode("Insert Mode");
                if (clickedBlock != null) //기존에 보이던 Select Img 없애야함 
                {
                    Block block = clickedBlock.GetComponent<Block>();
                    block.isPrintUI = false;
                    BlockSelectImg.instance.SetActive(false);
                }
            }

            if (mCurrentMode == Mode.BlockSelect) //block Click 시
            {
                //RootCanvas.instance.PrintMode("Scale Modify Mode");

                if (Input.GetMouseButton(0) == true)
                { 
                    //Builder.Instance.BlockClick();
                }
                if (isTranslateScale) //Scale 변경 가능 시 
                {
                    if (clickedBlock != null)
                    {
                        Block block = clickedBlock.GetComponent<Block>();
                        block.TranslateScale(InputBlockPos.Instance.mouseOnClickPosition, _clickNormal); //Block Scale 변경
                    }
                }
            }
       }
    public void BlockTypeSelect()
    {
        if (Input.GetKey(KeyCode.Alpha1) == true)
            AddBlockTypeSelect(0);
        if (Input.GetKey(KeyCode.Alpha2) == true)
           AddBlockTypeSelect(1);
        if (Input.GetKey(KeyCode.Alpha3) == true)
            AddBlockTypeSelect(2);
        if (Input.GetKey(KeyCode.Alpha4) == true)
            AddBlockTypeSelect(3);
        if (Input.GetKey(KeyCode.Alpha5) == true)
            AddBlockTypeSelect(4);
    }
    public void AddBlockTypeSelect(int _index)
    {

        Builder.instance.blockType = _index;
        //IsInventoryImg(_index);
       // PrintSelectImg(_index, prevIndex);
        
        prevIndex = _index;

    }
    private void PrintSelectImg(int index, int prevIndex) //SelectImg active setting
    {
        if (Builder.instance.materials[index] != null)
        {
            Inventory.instance.SetActiveTrue_SelectImg(index);
        }
        if (index != prevIndex)
            Inventory.instance.SetActiveFalse_SelectImg(prevIndex);
    }
    private void IsInventoryImg(int index)
    {
        Inventory.instance.IsActiveTrue_BoxImg(index);
    }
}
