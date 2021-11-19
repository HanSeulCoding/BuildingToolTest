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
    int count;

    private void Awake()
    {
        instance = this;
        path = Application.dataPath + "/blockData" + ".dat";
       
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
    public void Load()
    {
        Builder.instance.work.addBlockList.Clear();
        foreach (Block block in Builder.instance.work.addBlockList) //√ ±‚»≠ 
            Destroy(block);

        blockData = DataManager.BinaryDeserialize<BlockDatas>(path);
        
        for (int i = 0; i < blockData.dataNum; i++)
        {
            Vector3 position = new Vector3(blockData.blockInfos[i].pos[0], blockData.blockInfos[i].pos[1],
                blockData.blockInfos[i].pos[2]);
            Builder.instance.work.addBlockList.Add(Builder.instance.AddBlock(blockData.blockInfos[i].dataType, position));
        }

    }
    private IEnumerator Load2Start()
    {
        yield return new WaitForSeconds(5.0f);
        Load2();
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
