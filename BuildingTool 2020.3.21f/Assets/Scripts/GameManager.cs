using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static GameManager instance;
    [HideInInspector]
    public int selectBlockNum;
    [HideInInspector]
   // public List<Material> materials;
    public Material[] materials;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        materials = new Material[3];
        // materials.Add(Resources.Load<Material>("Blocks/BlockMaterial/Block" + 1 + "Mat"));
        materials[0] = Resources.Load<Material>("Blocks/BlockMaterial/Block" + 1 + "Mat");
       // SetMaterials();
    }
    private void SetMaterials()
    {
        //for (int i = 1; i < 4; i++)
        //{
        //    materials.Add(Resources.Load<Material>("Blocks/BlockMaterial/Block" + i + "Mat"));
        //}
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
