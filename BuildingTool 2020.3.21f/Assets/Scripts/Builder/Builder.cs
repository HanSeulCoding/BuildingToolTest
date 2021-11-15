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
public class Builder : MonoBehaviour
{
    public string Version = "1.0.0";

    public Material[] materials;

    public static Builder instance;
    [Header("�� ���� Size")]
    public int rowSize;
    [Header("�� ���� Size")]
    public int columnSize;
    [Header("�� Y�� Size")]
    public int YSize;
    public enum Command
    {
        None,
        Add,        //�߰���
        Remove      //������
    }

    [Header("���� ��1")]
    public GameObject blockModel1;

    private GameObject poolingBlock;
    //private GameObject poolingBlockParent;
    
    private Transform addTransform;
    private Transform poolingBlockTransform;

    private List<Block> poolingBlockList = new List<Block>();

    [HideInInspector]
    public Work work;

    [HideInInspector]
    public GameObject blockModel;

    [HideInInspector]
    public GameObject bottomFloor;

    [HideInInspector]
    public bool isBuild;

    [HideInInspector]
    public int undoRedoKey = 0;

    [HideInInspector]
    private int buildCount;

    [HideInInspector]
    public int blockType;
  

    private void Awake()
    {
        instance = this;
        InitSetting();
        
    }
    private void InitSetting()
    {
        blockModel = Resources.Load<GameObject>("Blocks/Block");

        addTransform = transform.Find("Add");
        bottomFloor = GameObject.Find("BottomFloor");
        bottomFloor.transform.localScale = new Vector3(rowSize, 1, columnSize);

        poolingBlock = Resources.Load<GameObject>("Blocks/PoolingBlock"); //pooling block �����
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
        SetMaterial();
    }
    private void SetMaterial()
    {
        materials = new Material[3];
        materials[0] = Resources.Load<Material>("Blocks/BlockMaterial/Block" + 1 + "Mat");
    }
    // Start is called before the first frame update
    
    //PreBlock ���� ����
    public void AddVisibleBlock(int _blockSelect, Vector3 clickMousePos, Vector3 currentMousePos)
    {
        Debug.Log(currentMousePos);
        Position clickPos = TransPosition.instance.TransLocalPosition(clickMousePos);
        Position currentpos = TransPosition.instance.TransLocalPosition(currentMousePos);
        if (currentpos.x > 50 || currentpos.x < -50
            || currentpos.z > 50 || currentpos.z < -50)
            return;

        Debug.Log("current Pos X: " + currentpos.x + "Pos Y : " + currentpos.y + "Pos Z :" + currentpos.z);
        RectMake(clickPos, currentpos);
    }

    //MouseClickposition���� ���� ���콺�������� ���� �簢�� ���� ���� 
    private void RectMake(Position clickPos, Position currentPos)
    {
        int xWidth = currentPos.x - clickPos.x;
        int zHeight = currentPos.z - clickPos.z;

        Vector2Int rectTopLeft = new Vector2Int();
        Vector2Int rectBottomRight = new Vector2Int();


        if (xWidth <= 0 && zHeight <= 0) //������ ���ʿ��� ���� �Ʒ������� �巡��
        {
            rectTopLeft = new Vector2Int((int)currentPos.x, (int)clickPos.z);
            rectBottomRight = new Vector2Int(clickPos.x, currentPos.z);
        }
        else if (xWidth <= 0 && zHeight >= 0) // ������ �Ʒ��ʿ��� ���� �������� �巡��
        {
            rectTopLeft = new Vector2Int(currentPos.x, currentPos.z);
            rectBottomRight = new Vector2Int(clickPos.x, clickPos.z);
        }
        else if (xWidth >= 0 && zHeight >= 0) // ���� ���ʿ��� ������ �Ʒ������� �巡��
        {
            rectTopLeft = new Vector2Int(clickPos.x, currentPos.z);
            rectBottomRight = new Vector2Int(currentPos.x, clickPos.z);
        }
        else if (xWidth >= 0 && zHeight <= 0) //���� �Ʒ��ʿ��� ������ �������� �巡��
        {
            rectTopLeft = new Vector2Int(clickPos.x, clickPos.z);
            rectBottomRight = new Vector2Int(currentPos.x, currentPos.z);
        }

        Debug.Log("RectLeft : " + rectTopLeft);
        Debug.Log("RectRight : " +rectBottomRight);

        DragVisibleBlock(rectTopLeft, rectBottomRight, currentPos.y);
        RemoveVisibleBlock(rectTopLeft, rectBottomRight, currentPos.y);
    }
    //�簢�� ������ ���� PreBlock �Ǽ� 
    private void DragVisibleBlock(Vector2Int rectTopLeft, Vector2Int rectBottomRight, int yLine)
    {
        if (rectTopLeft.y == rectBottomRight.y && rectTopLeft.x == rectBottomRight.x)
        {
            for (int y = 0; y <= yLine; y++)
                SetVisiblePosition(rectBottomRight.x, y, rectBottomRight.y);
        }

        if (rectTopLeft.y == rectBottomRight.y) //���ΰ� ���� ���(���ڷ� �� ���η� �Ǽ��� ���)
        {
            for (int i = rectTopLeft.x; i < rectBottomRight.x; i++)
            {
                for (int y = 0; y <= yLine; y++)
                    SetVisiblePosition(i, y, rectTopLeft.y);
            }
            return;
        }
        if (rectTopLeft.x == rectBottomRight.x) // ���ΰ� ���� ��� (���ڷ� �� ���η� �Ǽ��� ���)
        {
            for (int i = rectBottomRight.y; i < rectTopLeft.y; i++)
            {
                for (int y = 0; y <= yLine; y++)
                    SetVisiblePosition(rectTopLeft.x, y, i);
            }
            return;
        }

        for (int i = rectTopLeft.x; i <= rectBottomRight.x; i++) //�� �� �Ϲ� �簢�� �Ǽ�
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
                if (poolingBlock.position.x == x && poolingBlock.position.z == z && poolingBlock.position.y == y) //�ߺ��˻�(���� �� block ���� X)
                    break;
            }
            if (poolingBlock.gameObject.activeSelf == false)
            {
                poolingBlock.gameObject.SetActive(true);

                poolingBlock.isVisible = true; //isVisible�� ���־�� Active ���� �� Position�� ���� ���θ� �� �� �ִ�. 
                poolingBlock.transform.position = new Vector3(x, y, z); //pooling Box�� Position ����
                poolingBlock.blockType = blockType;
                poolingBlock.position.x = x;
                poolingBlock.position.z = z;
                poolingBlock.position.y = y;
                //poolingBlock.TransWorldPosition();

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
                    //�簢�� �ȿ� ������ �ʴ°��
                    {
                        //Debug.Log("erase" + "X: " + poolingBlock.position.x + "Z: " + poolingBlock.position.z);
                        poolingBlock.gameObject.SetActive(false); // Active ���ֱ�
                        poolingBlock.isVisible = false;

                    }
                }
            }
        }
    }
    public void VisibleBlockFalse() //�����ֱ� �� Block ���� ����
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
    public void ClickBuildBlock(Vector3 _normal)
    {
        Block blockGo;
        Vector3 tempMousepos = InputBlockPos.Instance.mouseOnClickPosition;
        int tempY;

        if (_normal.y == 1.0f)
            _normal.y = 0;
        
        if (_normal.x == 1.0f)
            _normal.x = 0.1f;
        if (_normal.z == 1.0f)
            _normal.z = 0.1f;

        blockGo = AddBlock(blockType, poolingBlock.transform.position);

        tempY = (int)InputBlockPos.Instance.mouseOnClickPosition.y; //0.0������ Local����� Local Pos �� -1�� �Ǵ� �̻��� �������� ���� 0�ϋ��� ������                                                //0�� �ǰ� ���� 
        if (tempY == 0)                                       //0�� �ǰ� ���� 
            tempMousepos.y = 0.0f;

        blockGo.transform.position = TransPosition.instance.TranslatePosition(tempMousepos + _normal); 

        work.addBlockList.Add(blockGo); //workList�� �߰�
    }

    public void DragBuildBlock()
    {
        for(int i = poolingBlockList.Count - 1; i >= 0; i--)
        {
            if (!poolingBlockList[i].isVisible)
                continue;
            if(poolingBlockList[i].gameObject.activeSelf == true)
            {
                Block blockGo = AddBlock(blockType, poolingBlockList[i].transform.position); ;

                poolingBlockList[i].gameObject.SetActive(false);
                poolingBlockList[i].isVisible = false;

                if (blockGo.GetComponent<Block>() != null)
                   work.addBlockList.Add(blockGo.GetComponent<Block>());
            }
        }
        UndoPush(buildCount);
        buildCount++;
    }
    public Block AddBlock(int blockType, Vector3 pos)
    {
        if (blockType > 2)
            blockType = 2;
        GameObject blockGo = Instantiate(blockModel, addTransform) as GameObject;
        blockGo.transform.position = pos;
        Block block = blockGo.GetComponent<Block>();
        block.blockType = blockType;

        return block;
    }

    public void DragDeleteBlock()
    {
        //����Ž������ ����ȭ �ؾ���
        // int count = 0;
        foreach (Block poolingBlock in poolingBlockList)
        {
            if (!poolingBlock.gameObject.activeSelf)
                continue;

            if (!poolingBlock.isVisible) //���̴� PoolingBlock �ƴ� ���
                continue;

            if (poolingBlock.gameObject.activeSelf == true)
            {
                for (int i = work.addBlockList.Count - 1; i >= 0; i--)
                {
                    Block block = work.addBlockList[i];
                    if (poolingBlock.transform.position == block.transform.position)
                    {
                        work.addBlockList[i].isDestroy = true;
                        work.addBlockList.Remove(work.addBlockList[i]);

                        poolingBlock.gameObject.SetActive(false);
                        poolingBlock.isVisible = false;

                    }
                }
            }
        }
    }
    public void ClickDeleteBlock()
    {
        RaycastHit hit;
        Ray mCameraHitRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        int layerMask = 1 << 7;
        if (Physics.Raycast(mCameraHitRay, out hit) == true)
        {
            for (int i = work.addBlockList.Count - 1; i >= 0; i--)
            {
                if(work.addBlockList[i].transform.position == hit.transform.position)
                {
                    work.addBlockList[i].isDestroy = true;
                    work.addBlockList.Remove(work.addBlockList[i]);
                }
            }
        }
    }
    public void UndoInfoSave(Block block)
    {
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
        //����Ž������
        //����ȭ �ؾ��Ѵ�.

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
}
