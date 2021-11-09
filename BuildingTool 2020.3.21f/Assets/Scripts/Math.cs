using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Position
{
     public int x;
     public int y;
     public int z;
}
public class Math : MonoBehaviour
{
    // Start is called before the first frame update
    public Position position = new Position();
    //public Position tempPosition = new Position();
    public Vector3 worldPosition = new Vector3();
    public List<Position> positionList = new List<Position>();
    public static Math instance;
    private void Awake()
    {
        instance = this;
        for(int i=0;i<100;i++)
        {

        }
    }
   
    public Position TransLocalPosition(Vector3 position)
    {
        Position tempPosition = new Position();
        tempPosition.x = (int)(position.x / 1.0f);
        tempPosition.y = (int)(position.y / 1.0f);
        tempPosition.z = (int)(position.z / 1.0f);
        
        return tempPosition;
    }

    public Vector3 TransWorldPosition(Position position)
    {
        float tempX = 0.5f;
        float tempY = 0.5f;
        if (position.x < 0)
            tempX = -0.5f;
        if (position.x > 0)
            tempX = 0.5f;
        if (position.z < 0)
            tempY = -0.5f;
        if (position.z > 0)
            tempY = 0.5f;

        worldPosition = new Vector3((float)position.x + tempX, (float)(position.y + 0.5f), (float)(position.z + tempY));
        return worldPosition;

        //Vector3 pos = position;
        //int xInt = (int)pos.x;
        //float xFloat = pos.x - xInt;

        //if (xFloat > 0)
        //    xFloat = 0.5f;
        //if (xFloat < 0)
        //    xFloat = -0.5f;

        //int zInt = (int)pos.z;
        //float zFloat = pos.z - zInt;

        //if (zFloat > 0)
        //    zFloat = 0.5f;
        //if (zFloat < 0)
        //    zFloat = -0.5f;

        //pos.x = xInt + xFloat;
        //pos.z = zInt + zFloat;

        //return pos + Vector3.up * 0.5f;
    }

}
