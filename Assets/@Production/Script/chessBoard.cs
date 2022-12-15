using System;
using UnityEngine;

public class chessBoard : MonoBehaviour
{
    [Header("Art")]
    [SerializeField] private Material tileMaterial;

    #region Logic
    private const int TILE_COUNT_X = 8;
    private const int TILE_COUNT_Y = 8;
    private GameObject[,] tiles;
    private Camera currentCamera;
    private Vector2Int currentHover;

    public void Awake()
    {
        GenerateAllTiles(1, TILE_COUNT_X, TILE_COUNT_Y);
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
        if (Physics.Raycast(ray, out info, 100, LayerMask.GetMask("Tile","Hover")))
        {
            Vector2Int hitPosition = LookupTileIndex(info.transform.gameObject);

            if (currentHover == -Vector2Int.one)
            {
                currentHover = hitPosition;
                tiles[hitPosition.x, hitPosition.y].layer = LayerMask.NameToLayer("Hover");
            }

            if (currentHover != hitPosition)
            {
                tiles[currentHover.x, currentHover.y].layer = LayerMask.NameToLayer("Tile");
                currentHover = hitPosition;
                tiles[hitPosition.x, hitPosition.y].layer = LayerMask.NameToLayer("Hover");
            }
        }
        else 
        {
            if (currentHover != -Vector2Int.one) 
            {
                tiles[currentHover.x, currentHover.y].layer = LayerMask.NameToLayer("Tile");
                currentHover = -Vector2Int.one;
            }
        }
    }

    #endregion

    #region Generate Tiles 
    private void GenerateAllTiles(float tileSize, int tilecountX, int tilecountY)
    {
        tiles = new GameObject[tilecountX, tilecountY];
        for (int x = 0; x < tilecountX; x++)
        {
            for (int y = 0; y < tilecountY; y++)
            {
                tiles[x, y] = GenerateSingleTiles(tileSize, x, y);
            }
        }
    }
    private GameObject GenerateSingleTiles(float tileSize, int x, int y)
    {
        GameObject tileObject = new GameObject(string.Format("x:{0},Y{1}", x, y));
        tileObject.transform.parent = transform;

        Mesh mesh = new Mesh();
        tileObject.AddComponent<MeshFilter>().mesh = mesh;
        tileObject.AddComponent<MeshRenderer>().material = tileMaterial;

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
    #endregion

    #region Operations
    private Vector2Int LookupTileIndex(GameObject hitInfo) 
    {
        for (int x = 0; x < TILE_COUNT_X; x++) {
            for (int y = 0; y < TILE_COUNT_Y; y++) {
                if (tiles[x, y] == hitInfo)
                {
                    return new Vector2Int(x, y);
                }
            }
        }
        return -Vector2Int.one;
    }
    #endregion
}


