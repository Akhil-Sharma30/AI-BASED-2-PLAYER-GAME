using System;
using UnityEngine;

public class chessBoard : MonoBehaviour
{
    [Header("Art Stuff")]
    [SerializeField] private Material tileMaterial;
    [SerializeField] private Material tile_Odd_Material;
    [SerializeField] private Material tile_Even_Material;


    private const int Tile_Count_X = 4;
    private const int Tile_Count_Y = 4;
    private GameObject[,] tiles;
    private Camera currentCamera;
    private Vector2Int currentHover;

    private void Start()
    {
        GenereateAllTiles(1, Tile_Count_X, Tile_Count_Y);
        
    }

    private void Update()
    {
        if (!currentCamera)
        {
            currentCamera = Camera.main;
            return;
        }

        RaycastHit info;
        Ray ray = currentCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out info, 100, LayerMask.GetMask("Tile")))
        {
            // get the indexes of the tile i've hit 
            Vector2Int hitPosition = LookupTileIndex(info.transform.gameObject);

            //if we are hovering a tile after not hovering any tiles
            if (currentHover == -Vector2Int.one)
            {
                // first time hovering 
                currentHover = hitPosition;
                tiles[hitPosition.x, hitPosition.y].layer = LayerMask.NameToLayer("Hover");
            }

            // if we were already hovering a tile ,change the previous one
            if (currentHover != hitPosition)
            {
                // first time hovering 
                tiles[currentHover.x, currentHover.y].layer = LayerMask.NameToLayer("Tile");
                currentHover = hitPosition;
                tiles[hitPosition.x, hitPosition.y].layer = LayerMask.NameToLayer("Hover");
            }
        }
        else
        {
            if(currentHover != -Vector2Int.one)
            {
                tiles[currentHover.x, currentHover.y].layer = LayerMask.NameToLayer("Tile");
                currentHover = -Vector2Int.one;
            }
        }

    }

    // Generate Board
    private void GenereateAllTiles(float tileSize , int tileCountX , int tileCountY)
    {
        tiles = new GameObject[tileCountX, tileCountY];
        for(int x = 0; x < tileCountX; x++)
        {
            for(int y = 0; y < tileCountY; y++)
            {
               // Debug.Log("x+y = " + x + y + "---------");
                if ((x+y)%2 == 0)
                {
                   // Debug.Log("x+y = "+ x+y + ": Even");
                    tiles[x, y] = GenerateSingleTileWithColor(tileSize, x, y,tile_Even_Material);
                }
                else if ((x+y)%2 != 0)
                {
                    Debug.Log("x+y = " + x + y +": Odd");
                    tiles[x, y] = GenerateSingleTileWithColor(tileSize, x, y, tile_Odd_Material);
                }
                //tiles[x, y] = GenerateSingleTile(tileSize, x, y);
            }
        }
        SpawnKnight();
    }    
    private GameObject GenerateSingleTile(float tileSize, int x , int y)
    {
        GameObject tileObject = new GameObject(string.Format("X:{0},Y:{1}", x, y));
        tileObject.transform.parent = transform;

        Mesh mesh = new Mesh();
        tileObject.AddComponent<MeshFilter>().mesh= mesh;
        tileObject.AddComponent<MeshRenderer>().material = tileMaterial;

        Vector3[] vertices = new Vector3[4];
        vertices[0] = new Vector3(x * tileSize,0,y*tileSize);
        vertices[1] = new Vector3(x * tileSize, 0,(y+1) * tileSize);
        vertices[2] = new Vector3((x+1) * tileSize, 0, y * tileSize);
        vertices[3] = new Vector3((x+1) * tileSize, 0, (y+1) * tileSize);

        int[] tris = new int[] { 0, 1, 2, 1, 3, 2 };

        mesh.vertices = vertices;
        mesh.triangles = tris;

        mesh.RecalculateNormals();

        tileObject.layer = LayerMask.NameToLayer("Tile");
        tileObject.AddComponent<BoxCollider>();
        return tileObject;
    }

    void SpawnKnight()
    {
        Debug.Log("First Child Name: " + gameObject.transform.GetChild(0));
        Debug.Log("Last Child Name: " + gameObject.transform.GetChild(gameObject.transform.GetChildCount() - 1));
    }

    private GameObject GenerateSingleTileWithColor(float tileSize, int x, int y, Material tileColorMat)
    {
        GameObject tileObject = new GameObject(string.Format("X:{0},Y:{1}", x, y));
        tileObject.transform.parent = transform;

        Mesh mesh = new Mesh();
        tileObject.AddComponent<MeshFilter>().mesh = mesh;
        tileObject.AddComponent<MeshRenderer>().material = tileColorMat;

        Vector3[] vertices = new Vector3[4];
        vertices[0] = new Vector3(x * tileSize, 0, y * tileSize);
        vertices[1] = new Vector3(x * tileSize, 0, (y + 1) * tileSize);
        vertices[2] = new Vector3((x + 1) * tileSize, 0, y * tileSize);
        vertices[3] = new Vector3((x + 1) * tileSize, 0, (y + 1) * tileSize);

        int[] tris = new int[] { 0, 1, 2, 1, 3, 2 };

        mesh.vertices = vertices;
        mesh.triangles = tris;

        mesh.RecalculateNormals();

        tileObject.layer = LayerMask.NameToLayer("Tile");
        tileObject.AddComponent<BoxCollider>();
        return tileObject;
    }


    // Operation
    private Vector2Int LookupTileIndex(GameObject hitinfo)
    {
        for(int x = 0; x < Tile_Count_X; x++)
            for(int y= 0; y < Tile_Count_Y; y++)
                if (tiles[x, y] == hitinfo)
                    return new Vector2Int(x, y);
               
        return -Vector2Int.one;  //invalid 
    }
}
