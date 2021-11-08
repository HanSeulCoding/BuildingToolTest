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
    public Position tempPosition = new Position();
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

        tempPosition.x = (int)(position.x / 1.0f);
        tempPosition.y = (int)(position.y / 1.0f);
        tempPosition.z = (int)(position.z / 1.0f);
        
        return tempPosition;
    }

    public Vector3 TransWorldPosition(Position position)
    {
        float temp = 0.5f;
        if (position.x < 0)
            temp = -0.5f;
        if (position.z < 0)
            temp = -0.5f;
        worldPosition = new Vector3((float)position.x + temp, (float)(position.y + 0.5f), (float)(position.z + temp));
        return worldPosition;
    }

}
