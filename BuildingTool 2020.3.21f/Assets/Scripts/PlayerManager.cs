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

    

    public Vector3 testPos;

    

    [HideInInspector]
    public Vector3 _clickNormal;

    [HideInInspector]
    public Mode mCurrentMode = Mode.Insert;

    [HideInInspector]
    public GameObject clickedBlock;


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
        if (Input.GetKeyDown(KeyCode.Delete) == true)
            mCurrentMode = Mode.Delete;
        
        //if (Input.GetKeyDown(KeyCode.Delete) == true)
        //    mCurrentMode = Mode.Delete;


        BlockManageUpdate();
        BlockTypeSelect(); //블럭 타입 선택
        
    }

    private void BlockManageUpdate()
    {
        
        if (Input.GetMouseButtonDown(0) == true)
        {
            if (WorldGenerator.Instance.undoRedoKey > 9)
            {
                WorldGenerator.Instance.undoRedoKey = 9;
                if(WorldGenerator.Instance.work.removeBlock[0] != null)
                    WorldGenerator.Instance.work.removeBlock[0].Clear();
            }
           

            
            Builder.Instance.OnClick();
            switch (mCurrentMode)
            {
                case Mode.Insert:
                    break;
                case Mode.Delete:
                    break;
            }
        }
        if(Input.GetMouseButton(0) == true)
        {
            Builder.Instance.AddVisibleBlock();
        }
        if (Input.GetMouseButtonUp(0) == true)
        {
            switch (mCurrentMode)
            {
                case Mode.Insert:
                    WorldGenerator.Instance.BuildBlock();
                    break;
                case Mode.Delete:
                    WorldGenerator.Instance.DeleteBlock();
                    WorldGenerator.Instance.VisibleBlockFalse();
                    break;
            }

            if(WorldGenerator.Instance.undoRedoKey <= 9)
                WorldGenerator.Instance.undoRedoKey++;
            // WorldGenerator.Instance.undoRedoKey = 9


            //  WorldGenerator.Instance.

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
