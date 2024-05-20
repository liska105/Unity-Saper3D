using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    // Tile object
    public GameObject objTile;
    // Tile object Renderer
    private Renderer rdrTile;

    public GameManager gameManager;
    public GameObject boom;
    public GameObject flag;
    public GameObject greenFlag;

    public AudioSource source;
    public AudioClip clickSound;
    public AudioClip explosionSound;

    public bool flagged = false;
    public bool active = true;
    public bool isMine = false;
    public int mineCount = 0;

    GameObject placedFlag;

    void Start()
    {
        rdrTile = objTile.GetComponent<Renderer>();

        rdrTile.material.SetTexture("_MainTex", gameManager.resources["Uncovered"]);
    }

    
    public void OnMouseOver()
    {
        if (active)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (isMine && !flagged)
                {
                    source.PlayOneShot(explosionSound);
                }

                else if (!flagged)
                {
                    source.PlayOneShot(clickSound);
                }
                ClickedTile();
            }
            else if (Input.GetMouseButtonDown(1))
            {
                flagged = !flagged;
                if (flagged)
                {
                    //rdrTile.material.SetTexture("_MainTex", gameManager.resources["Flag"]);
                    placedFlag = Instantiate(flag, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
                    placedFlag.transform.Rotate(0, 90, 0);
                    source.PlayOneShot(clickSound);

                }
                else
                {
                    rdrTile.material.SetTexture("_MainTex", gameManager.resources["Uncovered"]);
                    Destroy(placedFlag);
                    source.PlayOneShot(clickSound);
                }
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                gameManager.ExpandIfFlagged(this);
                source.PlayOneShot(clickSound);
            }
        }
    }

    
    public void ClickedTile()
    {
        //Bo nie możemy kliknąć lewym na flage
        if (active & !flagged)
        {
            active = false;
            if (isMine)
            {
                rdrTile.material.SetTexture("_MainTex", gameManager.resources["RedMine"]);
                source.PlayOneShot(explosionSound);
                gameManager.GameOver();
                Instantiate(boom, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
            }
            else
            {
                rdrTile.material.SetTexture("_MainTex", gameManager.resources[mineCount.ToString()]);
                if (mineCount == 0)
                {
                    gameManager.ClickNeighbours(this);
                }
                gameManager.CheckGameOver();
            }
        }
    }
    

    public void ShowGameOverState()
    {
        if (active)
        {
            active = false;
            if (isMine & !flagged)
            {
                rdrTile.material.SetTexture("_MainTex", gameManager.resources["Mine"]);
                //wybuch w tym miejscu
                source.PlayOneShot(explosionSound);
                Instantiate(boom, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
            }
            else if (flagged & !isMine)
            {
                rdrTile.material.SetTexture("_MainTex", gameManager.resources["WrongMine"]);
            }
        }
    }
    

    public void SetFlaggedIfMine()
    {
        if (isMine)
        {
            flagged = true;
            //rdrTile.material.SetTexture("_MainTex", gameManager.resources["ValidFlag"]);
            Destroy(placedFlag);
            placedFlag = Instantiate(greenFlag, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
            placedFlag.transform.Rotate(0, 90, 0);
        }
    }
    
}
