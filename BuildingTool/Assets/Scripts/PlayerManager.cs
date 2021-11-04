using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    enum Mode
    {
        Insert, //추가 모드
        Delete  //삭제 모드
    }
    private Mode mCurrentMode = Mode.Insert;
    private bool mIsLookAtMove;

    private void Start()
    {
        mIsLookAtMove = false;
    }
    private void Update()
    {
        //BuilderMoveUpdate();

        if (Input.GetKeyDown(KeyCode.Insert) == true)
            mCurrentMode = Mode.Insert;
        if (Input.GetKeyDown(KeyCode.Delete) == true)
            mCurrentMode = Mode.Delete;


        BlockTypeSelect(); //블럭 타입 선택
    }

    private void BlockAddAndRemoveUpdate()
    {
        if (Input.GetMouseButtonDown(0) == true)
        {
            switch (mCurrentMode)
            {
                //case Mode.Delete:
                //    {
                //        Builder.Instance.RemoveBlock();
                //    }
                //    break;
                case Mode.Insert:
                    {
                        Builder.Instance.AddBlock();
                    }
                    break;
            }
        }
    }
    private void BlockTypeSelect()
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
