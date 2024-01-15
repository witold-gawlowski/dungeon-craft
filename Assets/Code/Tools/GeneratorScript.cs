using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class GeneratorScript : MonoBehaviour
{
    [SerializeField] private GameObject tilePrefab;

    [SerializeField] private MapGeneratorConfig config;

    [SerializeField] private Transform levelParent;

    private int width { get { return config.width; } }
    private int height { get { return config.height; } }
    private float initial { get { return config.initial; } }
    private int birthLimit { get { return config.birthLimit; } }
    private int deathLimit { get { return config.deathLimit; } }
    private int steps { get { return config.steps; } }


    private float midX;
    private float midY;
    
    private Dictionary<Vector2Int, int> tab;
    private Dictionary<Vector2Int, int> tab2;

    private List<GameObject> tiles;

    public void Run()
    {
        midX = width / 2;
        midY = height / 2;

        tab = new Dictionary<Vector2Int, int>();
        tab2 = new Dictionary<Vector2Int, int>();

        foreach (Vector2Int coord in ProduceCoords())
        {
            tab[coord] = Random.value > initial ? 1 : 0;
        }

        for (int i = 0; i < steps; i++)
        {
            foreach (Vector2Int coord in ProduceCoords())
            {
                int cellCount = 0;
                foreach (Vector2Int neighbourCoord in ProduceNeighbours(coord.x, coord.y))
                {
                    if (! IsIn(neighbourCoord) || tab[neighbourCoord] > 0)
                    {
                        cellCount++;
                    }
                }

                if (tab[coord] > 0 && cellCount < deathLimit)
                {
                    tab2[coord] = 0;
                }
                else if (cellCount > birthLimit)
                {
                    tab2[coord] = 1;
                }
                else
                {
                    tab2[coord] = tab[coord];
                }
            }

            tab = new Dictionary<Vector2Int, int>(tab2);
            tab2 = new Dictionary<Vector2Int, int>();
        }
        ClearLevel();
        CrateTiles();
    }


    public void CrateTiles()
    {
        if(tiles == null)
        {
            tiles = new List<GameObject> ();
        }

        foreach (Vector2Int coord in ProduceCoords())
        {
            if (tab[coord] == 0)
            {
                var newTile = GameObject.Instantiate(tilePrefab, new Vector3(coord.x - midX, coord.y - midY), quaternion.identity, levelParent);
                tiles.Add(newTile);
            }
        }
    }

    public void ClearLevel()
    {
        if(tiles == null)
        {
            return;
        }

        foreach(var tile in tiles)
        {
            DestroyImmediate(tile);
        }
    }

    private bool IsIn(Vector2Int coord)
    {
        return coord.x >= 0 && coord.y >= 0 && coord.x < width && coord.y < height;
    }

    private IEnumerable<Vector2Int> ProduceCoords()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                yield return new Vector2Int(i, j);
            }
        }
    }

    private IEnumerable<Vector2Int> ProduceNeighbours(int x, int y)
    {
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i != 0 || j != 0)
                {
                    yield return new Vector2Int(i + x, j + y); 
                }
            }
        }
    }
}
