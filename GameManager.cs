using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform tilePrefab;
    [SerializeField] private Transform explosionPrefab;
    [SerializeField] private Transform gameHolder;

    public GameObject EndGameCanvas;
    public TMP_Text czyWygrana;

    public GameObject TimerCanvas;



    private int width;
    private int height;
    private int numberOfMines;


    public AudioSource source;
    public AudioClip winnerSound;
    

    private float spacing = 1f;
    private float tileSize = 10f;

    public int x;
    public int y;
    public int z;

    public Dictionary<string, Texture> resources = new();

    private List<Tile> tiles = new();


    public void CreateGameBoard(int width, int height, int numberOfMines)
    {
        this.width = width;
        this.height = height;
        this.numberOfMines = numberOfMines;

        //obiekt, na którym będą płytki
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.localScale = new Vector3((tileSize + spacing + 10) * width, 0.4f, (tileSize + spacing + 10) * height);
        cube.transform.position = new Vector3(tilePrefab.transform.position.x, tilePrefab.transform.position.y - 0.2f, tilePrefab.transform.position.z);

        for (int row = 0; row < height; row++)
        {
            for (int column = 0; column < width; column++)
            {
                Transform tileTransform = Instantiate(tilePrefab);
                tileTransform.parent = gameHolder;
                float xId = column - ((width - 1) / 2.0f);
                float zId = row - ((height - 1) / 2.0f);
                tileTransform.localPosition = new Vector3(xId * tileSize + column * spacing, 0, zId * tileSize + row * spacing);
                //Debug.Log(gameHolder);
                Tile tile = tileTransform.GetChild(0).GetComponent<Tile>();
                tiles.Add(tile);
                tile.gameManager = this;

            }
        }
    }



    private List<int> GetNeighbours(int position)
    {
        List<int> neighbours = new();
        int row = position / width;
        int column = position % width;

        if (row < (height - 1))
        {
            neighbours.Add(position + width); //dolny sąsiad
            if (column > 0)
            {
                neighbours.Add(position + width - 1); //prawy dolny
            }
            if (column < (width - 1))
            {
                neighbours.Add(position + width + 1); //lewy dolny
            }
        }

        if (column > 0)
        {
            neighbours.Add(position - 1); //lewy 
        }

        if (column < (width - 1))
        {
            neighbours.Add(position + 1); //prawy
        }

        if (row > 0)
        {
            neighbours.Add(position - width); //górny
            if (column > 0)
            {
                neighbours.Add(position - width - 1); //lewy górny
            }
            if (column < (width - 1))
            {
                neighbours.Add(position - width + 1); //prawy gorny
            }
        }
        return neighbours;
    }


    private int HowManyMines(int location)
    {
        int count = 0;
        foreach (int position in GetNeighbours(location))
        {
            if (tiles[position].isMine)
            {
                count++;
            }
        }
        return count;
    }

    private void ResetGameState()
    {
        int[] minePositions = Enumerable.Range(0, tiles.Count).OrderBy(x => Random.Range(0.0f, 1.0f)).ToArray();

        for (int i = 0; i < numberOfMines; i++)
        {
            int position = minePositions[i];
            tiles[position].isMine = true;
        }

        for (int i = 0; i < tiles.Count; i++)
        {
            tiles[i].mineCount = HowManyMines(i); 
        }
    }

    private void LoadResources()
    {
        for (int i = 0; i <=8; i++)
        {
            resources.Add(i.ToString(), Resources.Load<Texture>("Sprites/" + i.ToString()));
        }

        resources.Add("Flag", Resources.Load<Texture>("Sprites/Flag"));
        resources.Add("Mine", Resources.Load<Texture>("Sprites/Mine"));
        resources.Add("RedMine", Resources.Load<Texture>("Sprites/RedMine"));
        resources.Add("Uncovered", Resources.Load<Texture>("Sprites/Uncovered"));
        resources.Add("ValidFlag", Resources.Load<Texture>("Sprites/ValidFlag"));
        resources.Add("WrongMine", Resources.Load<Texture>("Sprites/WrongMine"));

    }

    
    public void ClickNeighbours(Tile tile)
    {
        int location = tiles.IndexOf(tile);
        foreach (int pos in GetNeighbours(location))
        {
            tiles[pos].ClickedTile();
        }
    }


    public void GameOver()
    {
        foreach (Tile tile in tiles)
        {
            tile.ShowGameOverState();
        }
        czyWygrana.text = "Game Over";
        TimerCanvas.SetActive(false);
        EndGameCanvas.SetActive(true);


    }


    public void CheckGameOver()
    {
        int count = 0;
        foreach (Tile tile in tiles)
        {
            if (tile.active)
            {
                count++;
            }
        }
        if (count == numberOfMines)
        {
            //Debug.Log("wygrana");
            czyWygrana.text = "You Won!";
            source.PlayOneShot(winnerSound);
            //TimerCanvas.SetActive(false);
            EndGameCanvas.SetActive(true);
            foreach (Tile tile in tiles)
            {
                tile.active = false;
                tile.SetFlaggedIfMine();
            }
        }
        
    }
    
    public void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ExpandIfFlagged(Tile tile)
    {
        int location = tiles.IndexOf(tile);
        int flagCount = 0;

        foreach (int pos in GetNeighbours(location))
        {
            if (tiles[pos].flagged)
            {
                flagCount++;
            }
        }
        Debug.Log(flagCount);
        if (flagCount == tile.mineCount)
        {
            Debug.Log(flagCount);
            ClickNeighbours(tile); 
        }
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu");
    }


    private void Start()
    {
        
         LoadResources();
         CreateGameBoard(x, y, z);
         ResetGameState();
        
    }

    
}



