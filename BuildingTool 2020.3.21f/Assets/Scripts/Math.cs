using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Position
{
    public int x;
    public int y;
    public int z;
    public bool isMinusZeroX;
    public bool isMinusZeroZ;
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
        tempPosition.x = (int)(position.x / (WorldGenerator.Instance.rowSize / 10.0f));
        tempPosition.y = (int)(position.y / (WorldGenerator.Instance.rowSize / 10.0f));
        tempPosition.z = (int)(position.z / (WorldGenerator.Instance.columnSize / 10.0f));

        return tempPosition;
    }
    
    public Vector3 TransWorldPosition(Position position)
    {
        float tempX = WorldGenerator.Instance.rowSize / 10.0f / 2.0f;
        float tempZ = WorldGenerator.Instance.columnSize / 10.0f / 2.0f;
        if (position.x < 0)
            tempX = -tempX;
        if (position.z < 0)
            tempZ = -tempZ;

        worldPosition = new Vector3((float)position.x + tempX, (float)(position.y + 0.5f), (float)(position.z + tempZ));
        return worldPosition;
    }

}
