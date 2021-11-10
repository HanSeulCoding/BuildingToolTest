using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Work
{
    public List<int> removeBlockTypeList;
    public List<Vector3> removeBlockPosList;

    public List<Block> addBlockList;
}
public class WorldGenerator : MonoBehaviour
{
    public string Version = "1.0.0";

    public static WorldGenerator Instance;
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
    public GameObject addBlock1;
    [Header("���� ��2")]
    public GameObject addBlock2;
    [Header("���� ��3")]
    public GameObject addBlock3;
    [Header("���� ��4")]
    public GameObject addBlock4;
    [Header("���� ��5")]
    public GameObject addBlock5;
    [Header("���� ��6")]
    public GameObject addBlock6;
    [Header("���� ��7")]
    public GameObject addBlock7;
    [Header("���� ��8")]
    public GameObject addBlock8;
    [Header("���� ��9")]
    public GameObject addBlock9;
    [Header("���� ��10")]
    public GameObject addBlock10;

    private GameObject poolingBlock;
    private GameObject bottomFloor;
    private Transform addTransform;
    private Transform poolingBlockTransform;

    private List<Block> poolingBlockList = new List<Block>();
    private List<Block> falsePoolingBlock = new List<Block>();
    private Work work;

    [HideInInspector]
    public List<GameObject> addBlockList = new List<GameObject>();
    [HideInInspector]
    public bool isBuild;
    [HideInInspector]
    public float visibleBlockY;

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

        poolingBlock = Resources.Load<GameObject>("Blocks/PoolingBlock"); //pooling block �����
        poolingBlock.SetActive(false);
        poolingBlockTransform = GameObject.Find("PoolingBlock").transform;

        poolingBlock.gameObject.transform.position = new Vector3(0, 0, 0);
        for (int i=0;i<100;i++) //PoolingBlock Instantiate 
        {
            for(int j =0;j<100;j++)
            {
                poolingBlockList.Add(Instantiate(poolingBlock, poolingBlockTransform).GetComponent<Block>());
            }
        }

        work = new Work();
        work.addBlockList = new List<Block>();
        work.removeBlockPosList = new List<Vector3>();
        work.removeBlockTypeList = new List<int>();
    }
    // Start is called before the first frame update

    //PreBlock ���� ����
    public void VisibleAddBlock(int _blockSelect, Vector3 clickMousePos, Vector3 currentMousePos)
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
    //MouseClickposition���� ���� ���콺�������� ���� �簢�� ���� ���� 
    private void RectMake(Position clickPos, Position currentPos)
    {
        int xWidth = currentPos.x - clickPos.x;
        int zHeight = currentPos.z - clickPos.z;

        Vector2Int rectTopLeft = new Vector2Int();
        Vector2Int rectBottomRight = new Vector2Int();
 

        if (xWidth <= 0 && zHeight <= 0) //������ ���ʿ��� ���� �Ʒ������� �巡��
        {
          //  rectBottomLeft = new Vector2Int(currentPos.x, currentPos.z);
            rectTopLeft = new Vector2Int((int)currentPos.x, (int)clickPos.z);
            rectBottomRight = new Vector2Int(clickPos.x, currentPos.z);
           // rectTopRight = new Vector2Int(clickPos.x, clickPos.z);
        }
        else if( xWidth <= 0 && zHeight >= 0) // ������ �Ʒ��ʿ��� ���� �������� �巡��
        {
           // rectBottomLeft = new Vector2Int(currentPos.x, clickPos.z);
            rectTopLeft = new Vector2Int(currentPos.x, currentPos.z); 
            rectBottomRight = new Vector2Int(clickPos.x, clickPos.z);
           // rectTopRight = new Vector2Int(clickPos.x, currentPos.z); 
        }
        else if( xWidth >= 0 && zHeight >= 0) // ���� ���ʿ��� ������ �Ʒ������� �巡��
        {
            //rectBottomLeft = new Vector2Int(clickPos.x, clickPos.z); 
            rectTopLeft = new Vector2Int(clickPos.x, currentPos.z);  
            rectBottomRight = new Vector2Int(currentPos.x, clickPos.z); 
          //  rectTopRight = new Vector2Int(currentPos.x, currentPos.z);  
        }
        else if (xWidth >= 0 && zHeight <= 0) //���� �Ʒ��ʿ��� ������ �������� �巡��
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
    //�簢�� ������ ���� PreBlock �Ǽ� 
    private void MakeVisibleBlock(Vector2Int rectTopLeft, Vector2Int rectBottomRight, int yLine)
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
                    SetVisiblePosition(rectTopLeft.x,y, i);
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
    public void BuildBlock()
    {
        foreach(Block poolingBlock in poolingBlockList)
        {
            if (!poolingBlock.isVisible)
                continue;
            //if (OverlapBlockBuild(poolingBlock))
             //   continue;

            if(poolingBlock.gameObject.activeSelf == true)
            {
                GameObject blockGo = Instantiate(addBlockList[Builder.Instance.blockSelectIndex], addTransform) as GameObject;
                blockGo.transform.position = poolingBlock.transform.position; 
                poolingBlock.gameObject.SetActive(false);
                poolingBlock.isVisible = false;
                
                if(blockGo.GetComponent<Block>() != null)
                    work.addBlockList.Add(blockGo.GetComponent<Block>());
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
        //*�ǵ�����(Undo)�� �ٽý���(Redo)�� �ƴϸ� �� �۾����� �ν����� ����

        GameObject blockGo = Instantiate(addBlockList[_blockSelect], addTransform) as GameObject;
        blockGo.transform.rotation = Quaternion.identity;

        switch (_layer)
        {
            //�� ���� ����� ��
            case 7:
                if (isVisible)
                    break;
                blockGo.transform.position = _pos + _normal;
                break;
            //�ٴڿ� ����� ��
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
