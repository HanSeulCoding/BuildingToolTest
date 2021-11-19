using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BlockInfo
{
    public float[] pos = new float[3];
    public int dataType;

}
[System.Serializable]
public class BlockDatas
{
    public int dataNum;
    public List<BlockInfo> blockInfos = new List<BlockInfo>();
}
public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static GameManager instance;

    BlockDatas blockData = new BlockDatas();
    string path;
    [HideInInspector]
    public bool isLoad1;

    [Header("undoSize")]
    public int undoSize;

    [HideInInspector]
    public int undoPushCount;
    int count;
    [HideInInspector]
    public List<Block>[] undoRedoList;
    

    private void Awake()
    {
        instance = this;
        path = Application.dataPath + "/blockData" + ".dat";
        undoRedoList = new List<Block>[5];
        for(int i=0;i<5;i++)
        {
            undoRedoList[i] = new List<Block>();
        }
    }
    void Start()
    {


    }
    private void SetMaterials()
    {
        //for (int i = 1; i < 4; i++)
        //{
        //    materials.Add(Resources.Load<Material>("Blocks/BlockMaterial/Block" + i + "Mat"));
        //}
    }
    // Update is called once per frame
    public void Save()
    {
        SaveBlock();
        DataManager.BinarySerialize<BlockDatas>(blockData, path);
    }
    private void SaveBlock()
    {
        List<Block> addBlockList = Builder.instance.work.addBlockList;
       
 
        for(int i = addBlockList.Count - 1; i>=0; i--)
        {
            BlockInfo blockInfo = new BlockInfo();
            blockInfo.pos[0] = addBlockList[i].transform.position.x;
            blockInfo.pos[1] = addBlockList[i].transform.position.y;
            blockInfo.pos[2] = addBlockList[i].transform.position.z;
            blockInfo.dataType = addBlockList[i].blockType;

            blockData.blockInfos.Add(blockInfo);
        }
        blockData.dataNum = blockData.blockInfos.Count - 1;
    }  
    public void UndoRedoInit()
    {
        for(int i=4;i>0;i--)
        {
            undoRedoList[i - 1] = undoRedoList[i];
        }
    }
    public void UndoPush(Block block)
    {
        undoPushCount = undoPushCount % 5;
        undoRedoList[undoPushCount].Add(block);
        if (undoPushCount == 4)
            UndoRedoInit();
    }
    public void Load()
    {
        Builder.instance.work.addBlockList.Clear();
      


        blockData = DataManager.BinaryDeserialize<BlockDatas>(path);

        for (int i = 0; i < blockData.dataNum; i++)
        {
            Vector3 position = new Vector3(blockData.blockInfos[i].pos[0], blockData.blockInfos[i].pos[1],
                blockData.blockInfos[i].pos[2]);
            Builder.instance.work.addBlockList.Add(Builder.instance.AddBlock(blockData.blockInfos[i].dataType, position));
        }


        //foreach (Block block in Builder.instance.work.addBlockList) //ÃÊ±âÈ­ 
        //    Destroy(block);
        //for (int i = 0; i < 3000; i++)
        //{

        //    Vector3 position = new Vector3(blockData.blockInfos[i].pos[0], blockData.blockInfos[i].pos[1],
        //        blockData.blockInfos[i].pos[2]);
        //    Builder.instance.work.addBlockList.Add(Builder.instance.AddBlock(blockData.blockInfos[i].dataType, position));
        //}
        //for (int i = 3000; i < 6000; i++)
        //{
        //    Vector3 position = new Vector3(blockData.blockInfos[i].pos[0], blockData.blockInfos[i].pos[1],
        //       blockData.blockInfos[i].pos[2]);
        //    Builder.instance.work.addBlockList.Add(Builder.instance.AddBlock(blockData.blockInfos[i].dataType, position));
        //}
        //for (int i = 6000; i < 9000; i++)
        //{
        //    Vector3 position = new Vector3(blockData.blockInfos[i].pos[0], blockData.blockInfos[i].pos[1],
        //      blockData.blockInfos[i].pos[2]);
        //    Builder.instance.work.addBlockList.Add(Builder.instance.AddBlock(blockData.blockInfos[i].dataType, position));
        //}
        //for(int i=9000;i<12000;i++)
        //{
        //    Vector3 position = new Vector3(blockData.blockInfos[i].pos[0], blockData.blockInfos[i].pos[1],
        //      blockData.blockInfos[i].pos[2]);
        //    Builder.instance.work.addBlockList.Add(Builder.instance.AddBlock(blockData.blockInfos[i].dataType, position));
        //}
        //for(int i=12000;i<15000;i++)        {
        //    Vector3 position = new Vector3(blockData.blockInfos[i].pos[0], blockData.blockInfos[i].pos[1],
        //      blockData.blockInfos[i].pos[2]);
        //    Builder.instance.work.addBlockList.Add(Builder.instance.AddBlock(blockData.blockInfos[i].dataType, position));
        //}
        //for(int i=15000;i<18000;i++)
        //{
        //    Vector3 position = new Vector3(blockData.blockInfos[i].pos[0], blockData.blockInfos[i].pos[1],
        //     blockData.blockInfos[i].pos[2]);
        //    Builder.instance.work.addBlockList.Add(Builder.instance.AddBlock(blockData.blockInfos[i].dataType, position));     
        //}
        //for (int i = 18000; i < 21000; i++)
        //{
        //    Vector3 position = new Vector3(blockData.blockInfos[i].pos[0], blockData.blockInfos[i].pos[1],
        //     blockData.blockInfos[i].pos[2]);
        //    Builder.instance.work.addBlockList.Add(Builder.instance.AddBlock(blockData.blockInfos[i].dataType, position));
        //}
        //for (int i = 21000; i < 24000; i++)
        //{
        //    Vector3 position = new Vector3(blockData.blockInfos[i].pos[0], blockData.blockInfos[i].pos[1],
        //    blockData.blockInfos[i].pos[2]);
        //    Builder.instance.work.addBlockList.Add(Builder.instance.AddBlock(blockData.blockInfos[i].dataType, position));
        //}
        //for (int i = 24000; i < 27000; i++)
        //{
        //    Vector3 position = new Vector3(blockData.blockInfos[i].pos[0], blockData.blockInfos[i].pos[1],
        //    blockData.blockInfos[i].pos[2]);
        //    Builder.instance.work.addBlockList.Add(Builder.instance.AddBlock(blockData.blockInfos[i].dataType, position));
        //}
        //for (int i = 27000; i < 30000; i++)
        //{
        //    Vector3 position = new Vector3(blockData.blockInfos[i].pos[0], blockData.blockInfos[i].pos[1],
        //    blockData.blockInfos[i].pos[2]);
        //    Builder.instance.work.addBlockList.Add(Builder.instance.AddBlock(blockData.blockInfos[i].dataType, position));
        //}
        //for (int i = 30000; i < 33000; i++)
        //{
        //    Vector3 position = new Vector3(blockData.blockInfos[i].pos[0], blockData.blockInfos[i].pos[1],
        //    blockData.blockInfos[i].pos[2]);
        //    Builder.instance.work.addBlockList.Add(Builder.instance.AddBlock(blockData.blockInfos[i].dataType, position));
        //}
        //    for (int i = 33000; i < 36000; i++)
        //    {
        //        Vector3 position = new Vector3(blockData.blockInfos[i].pos[0], blockData.blockInfos[i].pos[1],
        //        blockData.blockInfos[i].pos[2]);
        //        Builder.instance.work.addBlockList.Add(Builder.instance.AddBlock(blockData.blockInfos[i].dataType, position));
        //    }
        //for (int i = 36000; i < 39000; i++)
        //{
        //    Vector3 position = new Vector3(blockData.blockInfos[i].pos[0], blockData.blockInfos[i].pos[1],
        //    blockData.blockInfos[i].pos[2]);
        //    Builder.instance.work.addBlockList.Add(Builder.instance.AddBlock(blockData.blockInfos[i].dataType, position));
        //}
        //for (int i = 39000; i < 41000; i++)
        //{
        //    Vector3 position = new Vector3(blockData.blockInfos[i].pos[0], blockData.blockInfos[i].pos[1],
        //    blockData.blockInfos[i].pos[2]);
        //    Builder.instance.work.addBlockList.Add(Builder.instance.AddBlock(blockData.blockInfos[i].dataType, position));
        //}
        //for (int i = 41000; i < 44000; i++)
        //{
        //    Vector3 position = new Vector3(blockData.blockInfos[i].pos[0], blockData.blockInfos[i].pos[1],
        //    blockData.blockInfos[i].pos[2]);
        //    Builder.instance.work.addBlockList.Add(Builder.instance.AddBlock(blockData.blockInfos[i].dataType, position));
        //}
        //for (int i = 44000; i < 47000; i++)
        //{
        //    Vector3 position = new Vector3(blockData.blockInfos[i].pos[0], blockData.blockInfos[i].pos[1],
        //    blockData.blockInfos[i].pos[2]);
        //    Builder.instance.work.addBlockList.Add(Builder.instance.AddBlock(blockData.blockInfos[i].dataType, position));
        //}
        //for (int i = 47000; i < 50000; i++)
        //{
        //    Vector3 position = new Vector3(blockData.blockInfos[i].pos[0], blockData.blockInfos[i].pos[1],
        //    blockData.blockInfos[i].pos[2]);
        //    Builder.instance.work.addBlockList.Add(Builder.instance.AddBlock(blockData.blockInfos[i].dataType, position));
        //}
        //for (int i = 50000; i < 53000; i++)
        //{
        //    Vector3 position = new Vector3(blockData.blockInfos[i].pos[0], blockData.blockInfos[i].pos[1],
        //    blockData.blockInfos[i].pos[2]);
        //    Builder.instance.work.addBlockList.Add(Builder.instance.AddBlock(blockData.blockInfos[i].dataType, position));
        //}
        //for (int i = 53000; i < 56000; i++)
        //{
        //    Vector3 position = new Vector3(blockData.blockInfos[i].pos[0], blockData.blockInfos[i].pos[1],
        //    blockData.blockInfos[i].pos[2]);
        //    Builder.instance.work.addBlockList.Add(Builder.instance.AddBlock(blockData.blockInfos[i].dataType, position));
        //}
        //for (int i = 56000; i < 59000; i++)
        //{
        //    Vector3 position = new Vector3(blockData.blockInfos[i].pos[0], blockData.blockInfos[i].pos[1],
        //    blockData.blockInfos[i].pos[2]);
        //    Builder.instance.work.addBlockList.Add(Builder.instance.AddBlock(blockData.blockInfos[i].dataType, position));
        //}
        //for (int i = 59000; i < 62000; i++)
        //{
        //    Vector3 position = new Vector3(blockData.blockInfos[i].pos[0], blockData.blockInfos[i].pos[1],
        //    blockData.blockInfos[i].pos[2]);
        //    Builder.instance.work.addBlockList.Add(Builder.instance.AddBlock(blockData.blockInfos[i].dataType, position));
        //}
        //for (int i = 62000; i < 65000; i++)
        //{
        //    Vector3 position = new Vector3(blockData.blockInfos[i].pos[0], blockData.blockInfos[i].pos[1],
        //    blockData.blockInfos[i].pos[2]);
        //    Builder.instance.work.addBlockList.Add(Builder.instance.AddBlock(blockData.blockInfos[i].dataType, position));
        //}
        //for (int i = 65000; i < 68000; i++)
        //{
        //    Vector3 position = new Vector3(blockData.blockInfos[i].pos[0], blockData.blockInfos[i].pos[1],
        //    blockData.blockInfos[i].pos[2]);
        //    Builder.instance.work.addBlockList.Add(Builder.instance.AddBlock(blockData.blockInfos[i].dataType, position));
        //}
        //for (int i = 68000; i < 71000; i++)
        //{
        //    Vector3 position = new Vector3(blockData.blockInfos[i].pos[0], blockData.blockInfos[i].pos[1],
        //    blockData.blockInfos[i].pos[2]);
        //    Builder.instance.work.addBlockList.Add(Builder.instance.AddBlock(blockData.blockInfos[i].dataType, position));
        //}
        //for (int i = 71000; i < 74000; i++)
        //{
        //    Vector3 position = new Vector3(blockData.blockInfos[i].pos[0], blockData.blockInfos[i].pos[1],
        //    blockData.blockInfos[i].pos[2]);
        //    Builder.instance.work.addBlockList.Add(Builder.instance.AddBlock(blockData.blockInfos[i].dataType, position));
        //}
        //for (int i = 74000; i < 77000; i++)
        //{
        //    Vector3 position = new Vector3(blockData.blockInfos[i].pos[0], blockData.blockInfos[i].pos[1],
        //    blockData.blockInfos[i].pos[2]);
        //    Builder.instance.work.addBlockList.Add(Builder.instance.AddBlock(blockData.blockInfos[i].dataType, position));
        //}
        //for (int i = 77000; i < 80000; i++)
        //{
        //    Vector3 position = new Vector3(blockData.blockInfos[i].pos[0], blockData.blockInfos[i].pos[1],
        //    blockData.blockInfos[i].pos[2]);
        //    Builder.instance.work.addBlockList.Add(Builder.instance.AddBlock(blockData.blockInfos[i].dataType, position));
        //}
    }
private IEnumerator Load2Start()
    {
        yield return new WaitForSeconds(2.0f);
       
    }
    public void Load2()
    {
        for (int i = 21000; i < blockData.blockInfos.Count - 1; i++)
        {
            Vector3 position = new Vector3(blockData.blockInfos[i].pos[0], blockData.blockInfos[i].pos[1],
                blockData.blockInfos[i].pos[2]);
            Builder.instance.work.addBlockList.Add(Builder.instance.AddBlock(blockData.blockInfos[i].dataType, position));
        }
    }

  
    void Update()
    {
        
    }
}
