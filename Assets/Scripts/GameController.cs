using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public int[,] mBoard = new int[15, 15];

    // game object: stones, ban move representations, etc.
    public GameObject mBlackStone;
    public GameObject mWhiteStone;
    public float offsetDistance = 1.5f;

    public static bool isGameOver = false;
    public static bool isBlackTurn = true;
    private Vector3 cameraForward;
    private int defaultStoneCapacity = 113; // last one is tmp stone
    private int maxSize = 226;

    private float mBoardStartX = -3.53f;
    private float mBoardStartY = 2.94f;
    private float mBoardGap = 0.5f;

    private int mBlackStoneCount = 0;
    private int mWhiteStoneCount = 0;

    private IObjectPool<GameObject> mBlackStonePool { get; set; }
    private IObjectPool<GameObject> mWhiteStonePool { get; set; }

    void Start()
    {
        isGameOver = false;
        // set mode and rule
        string mode = PlayerPrefs.GetString("mode");
        string rule = PlayerPrefs.GetString("rule");

        cameraForward = Camera.main.transform.forward;

        // if mode is vs ai, call ResetAI
        for (int i = 0; i < 15; i++)
        {
            for (int j = 0; j < 15; j++)
            {
                mBoard[i, j] = 0;
            }
        }

        mBlackStonePool = new ObjectPool<GameObject>(CreateBlackStone, OnTakeFromPool, OnReturnToPool, OnDestroyStone, true, defaultStoneCapacity);
        mWhiteStonePool = new ObjectPool<GameObject>(CreateWhiteStone, OnTakeFromPool, OnReturnToPool, OnDestroyStone, true, defaultStoneCapacity);

        for (int i = 0; i < maxSize; i++)
        {
            GameObject tmpBlackStone = CreateBlackStone();
            GameObject tmpWhiteStone = CreateWhiteStone();
            mBlackStonePool.Release(tmpBlackStone.gameObject);
            mWhiteStonePool.Release(tmpWhiteStone.gameObject);
        }
    }

    private GameObject CreateBlackStone()
    {
        GameObject stone = Instantiate(mBlackStone);
        stone.SetActive(false);
        return stone;
    }

    private GameObject CreateWhiteStone()
    {
        GameObject stone = Instantiate(mWhiteStone);
        stone.SetActive(false);
        return stone;
    }

    private void OnTakeFromPool(GameObject stone)
    {
        stone.SetActive(true);
    }

    private void OnReturnToPool(GameObject stone)
    {
        stone.SetActive(false);
    }

    private void OnDestroyStone(GameObject stone)
    {
        Destroy(stone);
    }

    void Update()
    {
        if (isGameOver == true)
        {
            PlayerPrefs.SetInt("isGameOver", 1);
            SceneManager.LoadScene("SelectScene");
            return;
        }
        Debug.Log(PlayerPrefs.GetInt("isGameOver"));
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Debug.Log(mousePosition);
            mousePosition.z += offsetDistance;

            if (mousePosition.x < mBoardStartX 
                || mousePosition.x > mBoardStartX + 14 * mBoardGap
                || mousePosition.y > mBoardStartY
                || mousePosition.y < mBoardStartY - 14 * mBoardGap)
            {
                return;
            }

            for (int i = 0; i < 15; i++)
            {
                if (Mathf.Abs(mousePosition.x - (mBoardStartX + i * mBoardGap)) < 0.25f)
                {
                    mousePosition.x = mBoardStartX + i * mBoardGap;
                }
                if (Mathf.Abs(mousePosition.y - (mBoardStartY - i * mBoardGap)) < 0.25f)
                {
                    mousePosition.y = mBoardStartY - i * mBoardGap;
                }
            }

            int x = (int)Mathf.Abs((mousePosition.x - mBoardStartX) / 0.5f);
            int y = (int)Mathf.Abs((mousePosition.y - mBoardStartY) / 0.5f);

            if (mBoard[x, y] == 0)
            {
                if (isBlackTurn)
                {
                    var tmpBlackStone = mBlackStonePool.Get();
                    tmpBlackStone.transform.Translate(mousePosition);
                    mBoard[x, y] = 1;
                    mBlackStoneCount++;
                    isBlackTurn = false;
                }
                else
                {
                    var tmpWhiteStone = mWhiteStonePool.Get();
                    tmpWhiteStone.transform.Translate(mousePosition);
                    mBoard[x, y] = 2;
                    mWhiteStoneCount++;
                    isBlackTurn = true;
                }
                IsGameEnd();
            }
        }
    }
    private void IsGameEnd()
    {
        int[,] directions = new int[8, 2] { { 1, 0 }, { -1, 0 }, { 0, 1 }, { 0, -1 }, { 1, 1 }, { -1, -1 }, { 1, -1 }, { -1, 1 } };

        // Check if the board is a draw

        if (mBlackStoneCount + mWhiteStoneCount == 225)
        {
            isGameOver = true;
            PlayerPrefs.SetString("winner", "Draw");
            return;
        }
        // Check if the game is finished
        for (int y = 0; y < 15; ++y)
        {
            for (int x = 0; x < 15; ++x)
            {
                if (mBoard[y, x] != 0)
                {
                    int color = mBoard[y, x];
                    for (int i = 0; i < directions.GetLength(0); i++)
                    {
                        int count = 1;

                        int dx = directions[i, 0], dy = directions[i, 1];
                        int nx = x + dx, ny = y + dy;

                        while (nx >= 0 && nx < 15 
                                && ny >= 0 && ny < 15 
                                && mBoard[ny, nx] == color)
                        {
                            ++count;
                            nx += dx;
                            ny += dy;
                        }
                        if (count >= 5 && CheckWin(count, color))
                            return;
                    }
                }
            }
        }
        return;
    }
    private bool CheckWin(int count, int color)
    {
        if (count >= 5)
        {
            isGameOver = true;
            if (color == 1)
            {
                PlayerPrefs.SetString("winner", "Black");
            }
            else
            {
                PlayerPrefs.SetString("winner", "White");
            }
        }
        return true;
    }
    public void ResetGame()
    {

        for (int i = 0; i < 15; i++)
        {
            for (int j = 0; j < 15; j++)
            {
                mBoard[i, j] = 0;
            }
        }
        // if mode is vs ai, call ResetAI
    }

    public void SetMode(string mode)
    {
        PlayerPrefs.SetString("mode", mode);
    }

    public void SetRule(string rule)
    {
        PlayerPrefs.SetString("rule", rule);
    }

    
}
