using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Work
{
    public Dictionary<int,List<int>> removeBlockTypeList = new Dictionary<int, List<int>>();
    public Dictionary<int,List<Vector3>> removeBlockPosList = new Dictionary<int, List<Vector3>>();

    public Dictionary<int, List<Block>> removeBlock = new Dictionary<int, List<Block>>();
    public List<Block> addBlockList = new List<Block>();
     
}
public class WorldGenerator : MonoBehaviour
{
    public string Version = "1.0.0";

    public static WorldGenerator Instance;
    [Header("맵 가로 Size")]
    public int rowSize;
    [Header("맵 세로 Size")]
    public int columnSize;
    [Header("맵 Y축 Size")]
    public int YSize;
    public enum Command
    {
        None,
        Add,        //추가된
        Remove      //삭제된
    }

    [Header("생성 모델1")]
    public GameObject addBlock1;
    [Header("생성 모델2")]
    public GameObject addBlock2;
    [Header("생성 모델3")]
    public GameObject addBlock3;
    [Header("생성 모델4")]
    public GameObject addBlock4;
    [Header("생성 모델5")]
    public GameObject addBlock5;
    [Header("생성 모델6")]
    public GameObject addBlock6;
    [Header("생성 모델7")]
    public GameObject addBlock7;
    [Header("생성 모델8")]
    public GameObject addBlock8;
    [Header("생성 모델9")]
    public GameObject addBlock9;
    [Header("생성 모델10")]
    public GameObject addBlock10;

    private GameObject poolingBlock;
    private GameObject bottomFloor;
    private Transform addTransform;
    private Transform poolingBlockTransform;

    private List<Block> poolingBlockList = new List<Block>();

    [HideInInspector]
    public Work work;

    [HideInInspector]
    public List<GameObject> addBlockList = new List<GameObject>();
    [HideInInspector]
    public bool isBuild;
    [HideInInspector]
    public int undoRedoKey = 0;
    [HideInInspector]
    private int buildCount;
    [HideInInspector]
    public List<Material> materials;

    private void Awake()
    {
        Instance = this;
        addBlockList.Add(addBlock1);
        addBlockList.Add(addBlock2);
        addBlockList.Add(addBlock3);
        addBlockList.Add(addBlock4);

        addTransform = transform.Find("Add");
        bottomFloor = GameObject.Find("BottomFloor");
        bottomFloor.transform.localScale = new Vector3(rowSize, 1, columnSize);

        poolingBlock = Resources.Load<GameObject>("Blocks/PoolingBlock"); //pooling block 만들기
        poolingBlock.GetComponent<Block>().isPooling = true;
        poolingBlock.SetActive(false);
        poolingBlockTransform = GameObject.Find("PoolingBlock").transform;

        poolingBlock.gameObject.transform.position = new Vector3(0, 0, 0);
        for (int i = 0; i < 100; i++) //PoolingBlock Instantiate 
        {
            for (int j = 0; j < 100; j++)
            {
                poolingBlockList.Add(Instantiate(poolingBlock, poolingBlockTransform).GetComponent<Block>());
            }
        }

        work = new Work();
        for (int i = 0; i < 10; i++)
        {
            work.removeBlockPosList[i] = new List<Vector3>();
            work.removeBlockTypeList[i] = new List<int>();
            work.removeBlock[i] = new List<Block>();
        }
        undoRedoKey = 0;
        SetMaterials();
    }
    // Start is called before the first frame update
    private void SetMaterials()
    {
        for(int i=1;i<4;i++)
        {
            materials.Add(Resources.Load<Material>("Blocks/BlockMaterial/Block" + i + "Mat"));
        }
    }
    //PreBlock 정보 설정
    public void MousePosTranslate(int _blockSelect, Vector3 clickMousePos, Vector3 currentMousePos)
    {
        Debug.Log(currentMousePos);
        Position clickPos = Math.instance.TransLocalPosition(clickMousePos);
        Position currentpos = Math.instance.TransLocalPosition(currentMousePos);
        if (currentpos.x > 50 || currentpos.x < -50
            || currentpos.z > 50 || currentpos.z < -50)
            return;

        Debug.Log("current Pos X: " + currentpos.x + "Pos Y : " + currentpos.y + "Pos Z :" + currentpos.z);
        RectMake(clickPos, currentpos);
    }

    //MouseClickposition에서 현재 마우스포지션의 안의 사각형 정보 추출 
    private void RectMake(Position clickPos, Position currentPos)
    {
        int xWidth = currentPos.x - clickPos.x;
        int zHeight = currentPos.z - clickPos.z;

        Vector2Int rectTopLeft = new Vector2Int();
        Vector2Int rectBottomRight = new Vector2Int();


        if (xWidth <= 0 && zHeight <= 0) //오른쪽 위쪽에서 왼쪽 아래쪽으로 드래그
        {
            //  rectBottomLeft = new Vector2Int(currentPos.x, currentPos.z);
            rectTopLeft = new Vector2Int((int)currentPos.x, (int)clickPos.z);
            rectBottomRight = new Vector2Int(clickPos.x, currentPos.z);
            // rectTopRight = new Vector2Int(clickPos.x, clickPos.z);
        }
        else if (xWidth <= 0 && zHeight >= 0) // 오른쪽 아래쪽에서 왼쪽 위쪽으로 드래그
        {
            // rectBottomLeft = new Vector2Int(currentPos.x, clickPos.z);
            rectTopLeft = new Vector2Int(currentPos.x, currentPos.z);
            rectBottomRight = new Vector2Int(clickPos.x, clickPos.z);
            // rectTopRight = new Vector2Int(clickPos.x, currentPos.z); 
        }
        else if (xWidth >= 0 && zHeight >= 0) // 왼쪽 위쪽에서 오른쪽 아래쪽으로 드래그
        {
            //rectBottomLeft = new Vector2Int(clickPos.x, clickPos.z); 
            rectTopLeft = new Vector2Int(clickPos.x, currentPos.z);
            rectBottomRight = new Vector2Int(currentPos.x, clickPos.z);
            //  rectTopRight = new Vector2Int(currentPos.x, currentPos.z);  
        }
        else if (xWidth >= 0 && zHeight <= 0) //왼쪽 아래쪽에서 오른쪽 위쪽으로 드래그
        {
            // rectBottomLeft = new Vector2Int(clickPos.x, currentPos.z); 
            rectTopLeft = new Vector2Int(clickPos.x, clickPos.z);
            rectBottomRight = new Vector2Int(currentPos.x, currentPos.z);
            // rectTopRight = new Vector2Int(currentPos.x, clickPos.z);  
        }

        //Debug.Log(rectTopLeft);
        //Debug.Log(rectBottomRight);

        MakeVisibleBlock(rectTopLeft, rectBottomRight, currentPos.y);
        RemoveVisibleBlock(rectTopLeft, rectBottomRight, currentPos.y);
    }
    //사각형 정보를 토대로 PreBlock 건설 
    private void MakeVisibleBlock(Vector2Int rectTopLeft, Vector2Int rectBottomRight, int yLine)
    {
        if (rectTopLeft.y == rectBottomRight.y && rectTopLeft.x == rectBottomRight.x)
        {
            for (int y = 0; y <= yLine; y++)
                SetVisiblePosition(rectBottomRight.x, y, rectBottomRight.y);
        }

        if (rectTopLeft.y == rectBottomRight.y) //세로가 같을 경우(일자로 쭉 세로로 건설할 경우)
        {
            for (int i = rectTopLeft.x; i < rectBottomRight.x; i++)
            {
                for (int y = 0; y <= yLine; y++)
                    SetVisiblePosition(i, y, rectTopLeft.y);
            }
            return;
        }
        if (rectTopLeft.x == rectBottomRight.x) // 가로가 같을 경우 (일자로 쭉 가로로 건설할 경우)
        {
            for (int i = rectBottomRight.y; i < rectTopLeft.y; i++)
            {
                for (int y = 0; y <= yLine; y++)
                    SetVisiblePosition(rectTopLeft.x, y, i);
            }
            return;
        }

        for (int i = rectTopLeft.x; i <= rectBottomRight.x; i++) //그 외 일반 사각형 건설
        {
            for (int z = rectBottomRight.y; z <= rectTopLeft.y; z++)
            {
                for (int y = 0; y <= yLine; y++)
                    SetVisiblePosition(i, y, z);
            }
        }
    }

    public void SetVisiblePosition(int x, int y, int z)
    {
        foreach (Block poolingBlock in poolingBlockList)
        {
            if (poolingBlock.gameObject.activeSelf == true)
            {
                if (poolingBlock.position.x == x && poolingBlock.position.z == z && poolingBlock.position.y == y) //중복검사(같은 곳 block 생성 X)
                    break;
            }
            if (poolingBlock.gameObject.activeSelf == false)
            {
                poolingBlock.gameObject.SetActive(true);

                poolingBlock.isVisible = true; //isVisible을 해주어야 Active 여부 및 Position이 설정 여부를 알 수 있다. 
                poolingBlock.transform.position = new Vector3(x, y, z); //pooling Box의 Position 결정
                poolingBlock.blockType = Builder.Instance.blockSelectIndex;
                poolingBlock.position.x = x;
                poolingBlock.position.z = z;
                poolingBlock.position.y = y;
                poolingBlock.TransWorldPosition();

                break;
            }
        }
    }

    public void RemoveVisibleBlock(Vector2Int rectTopLeft, Vector2Int rectBottomRight, int yLine)
    {
        foreach (Block poolingBlock in poolingBlockList)
        {
            if (poolingBlock.isVisible)
            {
                if (poolingBlock.gameObject.activeSelf == true)
                {
                    if (poolingBlock.position.x < rectTopLeft.x || poolingBlock.position.x > rectBottomRight.x
                         || poolingBlock.position.z < rectBottomRight.y || poolingBlock.position.z > rectTopLeft.y
                         || poolingBlock.position.y > yLine)
                    //사각형 안에 들어오지 않는경우
                    {
                        //Debug.Log("erase" + "X: " + poolingBlock.position.x + "Z: " + poolingBlock.position.z);
                        poolingBlock.gameObject.SetActive(false); // Active 꺼주기
                        poolingBlock.isVisible = false;

                    }
                }
            }
        }
    }
    public void ClickBuildBlock()
    {
        Block blockGo = AddBlock(Builder.Instance.blockSelectIndex, poolingBlock.transform.position);
        
        blockGo.transform.position = Math.instance.TranslatePosition(Builder.Instance.mouseOnClickPosition);
    }
    public void DragBuildBlock()
    {
        foreach (Block poolingBlock in poolingBlockList)
        {
            if (!poolingBlock.isVisible)
                continue;
            //if (OverlapBlockBuild(poolingBlock))
            //   continue;

            if (poolingBlock.gameObject.activeSelf == true)
            {
                Block blockGo = AddBlock(Builder.Instance.blockSelectIndex, poolingBlock.transform.position);

                poolingBlock.gameObject.SetActive(false);
                poolingBlock.isVisible = false;


               // UndoInfoSave(blockGo);

                if (blockGo.GetComponent<Block>() != null)
                    work.addBlockList.Add(blockGo.GetComponent<Block>());
            }
        }
        UndoPush(buildCount);
        buildCount++;
    }

    public void DragDeleteBlock()
    {
        //이진탐색으로 최적화 해야함
        // int count = 0;
        foreach (Block poolingBlock in poolingBlockList)
        {
            if (!poolingBlock.gameObject.activeSelf)
                continue;

            if (!poolingBlock.isVisible) //보이는 PoolingBlock 아닐 경우
                continue;

            if (poolingBlock.gameObject.activeSelf == true)
            {
                for (int i = work.addBlockList.Count - 1; i >= 0; i--)
                {
                    Block block = work.addBlockList[i];
                    if (poolingBlock.transform.position == block.transform.position)
                    {
                       // UndoInfoSave(block);
                        //UndoInfoSave(poolingBlock.transform.position, block.blockType);

                        work.addBlockList[i].isDestroy = true;
                        work.addBlockList.Remove(work.addBlockList[i]);

                        poolingBlock.gameObject.SetActive(false);
                        poolingBlock.isVisible = false;

                    }
                    //count++;
                }
            }
        }
    }
    public void ClickDeleteBlock()
    {
        for (int i = work.addBlockList.Count - 1; i >= 0; i--)
        {
            Block block = work.addBlockList[i];
            if (block.transform.position == Builder.Instance.mouseOnClickPosition)
            {
                work.addBlockList[i].isDestroy = true;
                work.addBlockList.Remove(work.addBlockList[i]);
            }
        }
    }
    public void UndoInfoSave(Block block)
    {
        //block.isUndo = true;
        work.removeBlock[undoRedoKey].Add(block);

    }
    public void UndoPop()
    {

    }
    public void UndoPush(int index)
    {
        if (index <= 9)
        {
            undoRedoKey = index;
        }
        if(index > 9)
        {
            for(int i = 9; i < 0; i--)
            {
                work.removeBlock[i - 1] = work.removeBlock[i];
            }
        }
    }
    public void UndoInfoSave(Vector3 position, int type)
    {
        //work.removeBlockPosList[undoRedoKey] = new List<Vector3>();
        //work.removeBlockTypeList[undoRedoKey] = new List<int>();

        work.removeBlockPosList[undoRedoKey].Add(position);
        work.removeBlockTypeList[undoRedoKey].Add(type);
    }
    public void Undo()
    {
        //이진탐색으로
        //최적화 해야한다.

        //Debug.Log("undoRedoKey : " + undoRedoKey);
        //foreach (Vector3 position in work.removeBlockPosList[undoRedoKey])
        //{
        //    //foreach(Block block in work.addBlockList)
        //    for (int i = work.addBlockList.Count - 1; i >= 0; i--)
        //    {
        //        if (position == work.addBlockList[i].transform.position)
        //        {
        //            work.addBlockList[i].isDestroy = true;
        //            work.addBlockList.Remove(work.addBlockList[i]);
        //        }
        //        //count++;
        //    }
        //}
        if (work.removeBlock.Count != 0 )
        {
            foreach (Block block in work.removeBlock[undoRedoKey - 1])
            {
                for (int i = work.addBlockList.Count - 1; i >= 0; i--)
                {
                    if (work.addBlockList[i] == block)
                    {
                        work.addBlockList[i].isDestroy = true;
                        work.addBlockList.Remove(work.addBlockList[i]);

                    }
                    //count++;
                }

            }
        }
        work.removeBlock[undoRedoKey - 1].Clear();
        //undoRedoKey--;

        //}
    }
    public void Redo()
    {
        List<int> typeList = work.removeBlockTypeList[undoRedoKey];
        List<Vector3> posList = work.removeBlockPosList[undoRedoKey];
        

        for(int i=0;i <= posList.Count-1;i++)
        {
            AddBlock(typeList[i], posList[i]);
        }
    }
    public Block AddBlock(int blockType, Vector3 pos)
    {
        if (blockType > 3)
            blockType = 3;
        GameObject blockGo = Instantiate(addBlockList[blockType-1], addTransform) as GameObject;
        blockGo.transform.position = pos;
        Block block = blockGo.GetComponent<Block>();
        block.blockType = blockType;

        return block;
    }
    public void VisibleBlockFalse()
    {
        foreach (Block poolingBlock in poolingBlockList)
        {
            if (!poolingBlock.gameObject.activeSelf)
                continue;

            if (poolingBlock.gameObject.activeSelf == true)
            {
                poolingBlock.isVisible = false;
                poolingBlock.gameObject.SetActive(false);
            }
        }
    }
    private bool OverlapBlockBuild(Block block)
    {
        foreach(Block addBlock in work.addBlockList)
        {
            if(addBlock.transform.position == block.transform.position)
            {
                return true;
            }
        }
        return false;
    }
    public void AddBlock(int _blockSelect, int _layer, Vector3 _pos, Vector3 _normal, bool _isAnimation, bool isVisible)
    {
        //*되돌리기(Undo)나 다시실행(Redo)이 아니면 새 작업으로 인식하지 않음

        GameObject blockGo = Instantiate(addBlockList[_blockSelect], addTransform) as GameObject;
        blockGo.transform.rotation = Quaternion.identity;

        switch (_layer)
        {
            //블럭 옆에 닿았을 때
            case 7:
                if (isVisible)
                    break;
                blockGo.transform.position = _pos + _normal;
                break;
            //바닥에 닿았을 때
            case 6:
                {
               

                    //Debug.Log(pos);
                
                    Block block = blockGo.GetComponent<Block>();
                    block.TranslatePosition();
                    block.TransWorldPosition();
                }
                break;
        }

        blockGo.name = blockGo.name.Replace("(Clone)", "");

        if (_isAnimation == true)
        {
            Block block = blockGo.GetComponent<Block>();
            block.CreateAnimation();
        }

        //work.addBlockList.Add(blockGo);
    }
    public void LookBlockCreate(int blockType)
    {
        GameObject blockObj = addBlockList[blockType];
        
    }
}
