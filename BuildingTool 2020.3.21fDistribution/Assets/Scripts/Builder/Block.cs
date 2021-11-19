using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    [HideInInspector]
    public bool isPrintUI;

    [HideInInspector]
    public int blockType = 0;

    [HideInInspector]
    public bool isUndo;

    [HideInInspector]
    public bool isVisible = false;
    [HideInInspector]
    public Position position = new Position();
    [HideInInspector]
    public bool isDestroy;

    [HideInInspector]
    public bool isPooling;

    [HideInInspector]
    private GameObject block;
    // Start is called before the first frame update

    private void Start()
    {
        block = this.transform.GetChild(0).gameObject; 
        SelectMaterial();
        position = new Position();
    }
    private void SelectMaterial()
    {
        if (!isPooling)
        {
            Material[] mesh = block.GetComponent<MeshRenderer>().materials;
            
            switch (blockType)
            {
                case 0:
                    mesh[0] = Builder.instance.materials[0];
                    if (mesh[0] == null)
                    {
                        //RootCanvas.instance.transform.Find("NotBlockType").gameObject.SetActive(true);
                        Builder.instance.work.addBlockList.Remove(this);
                        Destroy(this.gameObject);
                        return;
                    }
                    //
                    block.GetComponent<MeshRenderer>().materials = mesh; //null ���� �Ǵ�
                    break;
                case 1:
                    mesh[0] = Builder.instance.materials[1];
                    if (mesh[0] == null)
                    {
                        
                        Builder.instance.work.addBlockList.Remove(this);
                        Destroy(this.gameObject);
                        return;
                    }

                    block.GetComponent<MeshRenderer>().materials = mesh;
                    break;
                case 2:
                    mesh[0] = Builder.instance.materials[2];
                    if (mesh[0] == null)
                    {
                        //RootCanvas.instance.transform.Find("NotBlockType").gameObject.SetActive(true);
                        Builder.instance.work.addBlockList.Remove(this);
                        Destroy(block);
                        return;
                    }
                    block.GetComponent<MeshRenderer>().materials = mesh;
                    break;
                case 3:
                    mesh[0] = Builder.instance.materials[3];
                    if (mesh[0] == null)
                    {
                        //RootCanvas.instance.transform.Find("NotBlockType").gameObject.SetActive(true);
                        Builder.instance.work.addBlockList.Remove(this);
                        Destroy(block);
                        return;
                    }
                    block.GetComponent<MeshRenderer>().materials = mesh;
                    break;
                case 4:
                    mesh[0] = Builder.instance.materials[4];
                    if (mesh[0] == null)
                    {
                        //RootCanvas.instance.transform.Find("NotBlockType").gameObject.SetActive(true);
                        Builder.instance.work.addBlockList.Remove(this);
                        Destroy(block);
                        return;
                    }
                    block.GetComponent<MeshRenderer>().materials = mesh;
                    break;
            }
        }
    }
    private void Update()
    {
        //if (isPrintUI)
        //{
        //    CreatePointUI();
        //}
        //if (isDestroy)
        //    Destroy(this.gameObject);
    }

    public void CreateAnimation()
    {
        StartCoroutine(CorCreatAnimation());
    }
    public IEnumerator CorCreatAnimation()
    {
        float duration = 0f;
        float totalTime = 0.25f;
        Vector3 scale = transform.localScale;
        transform.localScale = Vector3.zero;

        while (duration <= totalTime)
        {
            duration += Time.deltaTime;
            transform.localScale = Vector3.Lerp(transform.localScale, scale, duration / totalTime);
            yield return null;
        }
    }

    public void CreatePointUI() //
    {
        List<Vector3> blocksPos = new List<Vector3>();
        Vector3 temp = new Vector3(transform.localScale.x/2.0f, transform.localScale.y/2.0f,transform.localScale.z/2.0f);
        Vector3 divide = new Vector3(temp.x / 100.0f, temp.y / 100.0f, temp.z / 100.0f);
        Vector3 blockPos = transform.position;
        Vector3 pos1 = new Vector3(transform.position.x - (float)divide.x, transform.position.y - (float)divide.y, transform.position.z + (float)divide.z) ;
        Vector3 pos2 = new Vector3(transform.position.x + (float)divide.x, transform.position.y - (float)divide.y, transform.position.z + (float)divide.z);
        Vector3 pos3 = new Vector3(transform.position.x - (float)divide.x, transform.position.y - (float)divide.y, transform.position.z - (float)divide.z);
        Vector3 pos4 = new Vector3(transform.position.x + (float)divide.x, transform.position.y - (float)divide.y, transform.position.z - (float)divide.z);
        Vector3 pos5 = new Vector3(transform.position.x - (float)divide.x, transform.position.y + (float)divide.y, transform.position.z + (float)divide.z);
        Vector3 pos6 = new Vector3(transform.position.x + (float)divide.x, transform.position.y + (float)divide.y, transform.position.z + (float)divide.z);
        Vector3 pos7 = new Vector3(transform.position.x - (float)divide.x, transform.position.y + (float)divide.y, transform.position.z - (float)divide.z);
        Vector3 pos8 = new Vector3(transform.position.x + (float)divide.x, transform.position.y + (float)divide.y, transform.position.z - (float)divide.z);

        blocksPos.Add(Camera.main.WorldToScreenPoint(pos1));
        blocksPos.Add(Camera.main.WorldToScreenPoint(pos2));
        blocksPos.Add(Camera.main.WorldToScreenPoint(pos3));
        blocksPos.Add(Camera.main.WorldToScreenPoint(pos4));
        blocksPos.Add(Camera.main.WorldToScreenPoint(pos5));
        blocksPos.Add(Camera.main.WorldToScreenPoint(pos6));
        blocksPos.Add(Camera.main.WorldToScreenPoint(pos7));
        blocksPos.Add(Camera.main.WorldToScreenPoint(pos8));

        BlockSelectImg.instance.SetPosition(blocksPos);
    }
    //public int GetTypeValue()
    //{
    //    if (gameObject.name.Equals(WorldGenerator.Instance.addBlock1.name) == true)
    //        return 0;
    //    else if (gameObject.name.Equals(WorldGenerator.Instance.addBlock2.name) == true)
    //        return 1;
    //    else if (gameObject.name.Equals(WorldGenerator.Instance.addBlock3.name) == true)
    //        return 2;
    //    else if (gameObject.name.Equals(WorldGenerator.Instance.addBlock4.name) == true)
    //        return 3;
    //    else if (gameObject.name.Equals(WorldGenerator.Instance.addBlock5.name) == true)
    //        return 4;
    //    else if (gameObject.name.Equals(WorldGenerator.Instance.addBlock6.name) == true)
    //        return 5;
    //    else if (gameObject.name.Equals(WorldGenerator.Instance.addBlock7.name) == true)
    //        return 6;
    //    else if (gameObject.name.Equals(WorldGenerator.Instance.addBlock8.name) == true)
    //        return 7;
    //    else if (gameObject.name.Equals(WorldGenerator.Instance.addBlock9.name) == true)
    //        return 8;

    //    return -1;
    //}

    public void TranslateScale(Vector3 clickMousePosition, Vector3 _normal)
    {
        if(PlayerInputManager.instance.isTranslateScale)
        {
            Vector3 currentMouseP = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,
                -Camera.main.transform.position.z));
            float distance = Vector3.Distance(InputBlockPos.Instance.mouseOnClickPosition, currentMouseP);
 
            if(distance > 1.0)
            {
                Vector3 addScale = new Vector3((float)2, (float)2, (float)2);
                Vector3 transScale = Vector3.Scale(addScale, _normal);
                Vector3 addDist = new Vector3(transform.localScale.x + distance, transform.localScale.y + distance, transform.localScale.z + distance);
                transform.localScale = addDist;
            }
        }
    }
    public void TranslatePosition()
    {
        position = TransPosition.instance.TransLocalPosition(transform.position);
    }
    public void TransWorldPosition()
    {
        transform.position = TransPosition.instance.TransWorldPosition(position);
    }
}
