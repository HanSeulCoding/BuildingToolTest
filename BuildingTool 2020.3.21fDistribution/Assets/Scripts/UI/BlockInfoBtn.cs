using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockInfoBtn : MonoBehaviour
{
    // Start is called before the first frame update
     public void ClickBlockBtn(int blockType)
    {
        int selectNum = Inventory.instance.selectBlockNum % 3;
 
        Image img = Inventory.instance.transform.GetChild(selectNum).GetChild(0).GetComponent<Image>();

        if (img.transform.gameObject.activeSelf == false)
            img.transform.gameObject.SetActive(true);

        img.sprite = Resources.Load<Sprite>("BlockImg/Block (" + blockType + ")");
        Builder.instance.materials[selectNum] = Resources.Load<Material>("Blocks/BlockMaterial/Block" + (blockType+1) + "Mat");

        Inventory.instance.selectBlockNum++;


    }
    public void UpdateBtn_Click()
    {
        RootCanvas.instance.transform.Find("UpdateRequiredPanel").gameObject.SetActive(true);
    }
}
