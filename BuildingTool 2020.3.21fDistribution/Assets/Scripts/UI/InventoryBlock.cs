using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryBlock : MonoBehaviour
{
    // Start is called before the first frame update
    public void SelectBlock()
    {
        this.transform.GetChild(1).gameObject.SetActive(true);
    }
}
