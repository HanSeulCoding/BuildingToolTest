using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public bool isPrintUI;

    // Start is called before the first frame update

    private void Update()
    {
        if (isPrintUI)
        {
            CreatePointUI();
        }
       

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

    public void CreatePointUI() //block 클릭 후 8개의 Point UI 생성 
    {
        List<Vector3> blocksPos = new List<Vector3>();
        Vector3 blockPos = transform.position;
        Vector3 pos1 = new Vector3(transform.position.x - (float)0.5, transform.position.y - (float)0.5, transform.position.z + (float)0.5);
        Vector3 pos2 = new Vector3(transform.position.x + (float)0.5, transform.position.y - (float)0.5, transform.position.z + (float)0.5);
        Vector3 pos3 = new Vector3(transform.position.x - (float)0.5, transform.position.y - (float)0.5, transform.position.z - (float)0.5);
        Vector3 pos4 = new Vector3(transform.position.x + (float)0.5, transform.position.y - (float)0.5, transform.position.z - (float)0.5);
        Vector3 pos5 = new Vector3(transform.position.x - (float)0.5, transform.position.y + (float)0.5, transform.position.z + (float)0.5);
        Vector3 pos6 = new Vector3(transform.position.x + (float)0.5, transform.position.y + (float)0.5, transform.position.z + (float)0.5);
        Vector3 pos7 = new Vector3(transform.position.x - (float)0.5, transform.position.y + (float)0.5, transform.position.z - (float)0.5);
        Vector3 pos8 = new Vector3(transform.position.x + (float)0.5, transform.position.y + (float)0.5, transform.position.z - (float)0.5);

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
    public int GetTypeValue()
    {
        if (gameObject.name.Equals(WorldGenerator.Instance.addBlock1.name) == true)
            return 0;
        else if (gameObject.name.Equals(WorldGenerator.Instance.addBlock2.name) == true)
            return 1;
        else if (gameObject.name.Equals(WorldGenerator.Instance.addBlock3.name) == true)
            return 2;
        else if (gameObject.name.Equals(WorldGenerator.Instance.addBlock4.name) == true)
            return 3;
        else if (gameObject.name.Equals(WorldGenerator.Instance.addBlock5.name) == true)
            return 4;
        else if (gameObject.name.Equals(WorldGenerator.Instance.addBlock6.name) == true)
            return 5;
        else if (gameObject.name.Equals(WorldGenerator.Instance.addBlock7.name) == true)
            return 6;
        else if (gameObject.name.Equals(WorldGenerator.Instance.addBlock8.name) == true)
            return 7;
        else if (gameObject.name.Equals(WorldGenerator.Instance.addBlock9.name) == true)
            return 8;

        return -1;
    }

    private void TranslateScale()
    {
        if(PlayerManager.instance.isTranslateScale)
        {
            Vector3 currentMouseP = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,
                -Camera.main.transform.position.z));// Input.mousePosition;
            float distance = Vector3.Distance(PlayerManager.instance.mouseOnClickPosition, currentMouseP);
            Debug.Log(currentMouseP);
            if(distance > 0.5)
            {
                
            }
        }
    }
}
