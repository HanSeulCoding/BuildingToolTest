using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    public enum Mode
    {
        Insert, //�߰� ���
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

    private float isPressTime;

    private int prevIndex;

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
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            RootCanvas.instance.QuitPannelEnter();
        }
        BuildBlock_Click();
        BuildVisibleBlock_Drag();
        BuildBlock_DragComplete();

        BlockManageUpdate();
        BlockTypeSelect(); //�� Ÿ�� ����
        
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
            if (Input.GetMouseButtonDown(0) == true) //Ŭ�� �ǹ� �Ǽ� �� ����
            {
                switch (mCurrentMode)
                {
                    case Mode.Insert:
                        InputBlockPos.Instance.BuildAndDelClick(true);
                        break;
                    case Mode.Delete:
                        InputBlockPos.Instance.BuildAndDelClick(false);
                        break;
                }

            }
        }
    }
    private void BuildVisibleBlock_Drag()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButton(0) == true) //Drag �� �۵�
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
        if (Input.GetMouseButtonUp(0) == true) //�ǹ� �Ǽ� �� ���� 
        {
            isPressTime = 0.0f;

            switch (mCurrentMode)
            {
                case Mode.Insert:
                    Builder.instance.DragBuildBlock();
                    break;

                case Mode.Delete:
                    if (isPress)
                    {
                        Debug.Log("Del IsPress");
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
    private void BlockManageUpdate()
    {
        //if (!EventSystem.current.IsPointerOverGameObject())
        //{
        //    if (Input.GetMouseButtonDown(0) == true) //Ŭ�� �ǹ� �Ǽ� �� ����
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
        //    if (Input.GetMouseButton(0) == true) //Drag �� �۵�
        //    {
        //        isPressTime += Time.deltaTime;

        //        if (isPressTime > 0.15)
        //            isPress = true;
        //        else
        //            isPress = false;
        //        Builder.Instance.PressClick();
        //    }
        //    if (Input.GetMouseButtonUp(0) == true) //�ǹ� �Ǽ� �� ���� 
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
        if(Input.GetKey(KeyCode.LeftArrow))
        {
            if(Builder.instance.undoRedoKey >= 0)
                Builder.instance.Undo();
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            if(Builder.instance.undoRedoKey > 0)
                 Builder.instance.undoRedoKey--;
        }
        if (Input.GetKey(KeyCode.RightArrow))
            Builder.instance.Redo();

        if(Input.GetKeyUp(KeyCode.RightArrow))
            Builder.instance.undoRedoKey++;

            if (mCurrentMode == Mode.Insert)
            {
                RootCanvas.instance.PrintMode("Insert Mode");
                if (clickedBlock != null) //������ ���̴� Select Img ���־��� 
                {
                    Block block = clickedBlock.GetComponent<Block>();
                    block.isPrintUI = false;
                    BlockSelectImg.instance.SetActive(false);
                }
            }

            if (mCurrentMode == Mode.BlockSelect) //block Click ��
            {
                RootCanvas.instance.PrintMode("Scale Modify Mode");

                if (Input.GetMouseButton(0) == true)
                { 
                    //Builder.Instance.BlockClick();
                }
                if (isTranslateScale) //Scale ���� ���� �� 
                {
                    if (clickedBlock != null)
                    {
                        Block block = clickedBlock.GetComponent<Block>();
                        block.TranslateScale(InputBlockPos.Instance.mouseOnClickPosition, _clickNormal); //Block Scale ����
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
    }
    public void AddBlockTypeSelect(int _index)
    {

        Builder.instance.blockType = _index;
        PrintSelectImg(_index, prevIndex);
        prevIndex = _index;

    }
    private void PrintSelectImg(int index, int prevIndex)
    {
        if (Builder.instance.materials[index] != null)
        {
            Inventory.instance.transform.GetChild(index).GetChild(1).gameObject.SetActive(true);
        }
        if (index != prevIndex)
            Inventory.instance.transform.GetChild(prevIndex).GetChild(1).gameObject.SetActive(false);

    }
}
