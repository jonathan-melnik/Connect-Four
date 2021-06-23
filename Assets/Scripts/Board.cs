using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] private Transform _topLeftPoint;
    [SerializeField] private Transform _bottomRightPoint;
    [SerializeField] private Disc _discPrefab;
    [SerializeField] private Transform _discsContainer;

    private Disc[] _discs;
    private bool _redPlays = true;

    private const float COLUMN_WIDTH = 0.9f;
    private const float ROW_HEIGHT = 0.8f;
    private const int COLUMNS = 7;
    private const int ROWS = 6;

    private void Start()
    {
        _discs = new Disc[ROWS * COLUMNS];
        for (int i = 0; i < ROWS * COLUMNS; i++)
        {
            _discs[i] = null;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            int col = GetColumnWithMousePos(Input.mousePosition);
            if (col != -1)
            {
                AddDiscAtColumn(col);
            }
        }
    }

    private void AddDiscAtColumn(int col)
    {
        if (GetDisc(col, 5) != null)
        {
            return;
        }
        var disc = Instantiate(_discPrefab);
        int row = NextRowAtColumn(col);
        disc.transform.position = GetBoardPosition(col, row);
        disc.transform.SetParent(_discsContainer, false);
        disc.Initialize(color: _redPlays ? DiscColor.Red : DiscColor.Black);
        SetDisc(col, row, disc);

        _redPlays = !_redPlays;

        CheckFourInARow();
    }

    private int GetColumnWithMousePos(Vector3 mousePos)
    {
        var pos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, -Camera.main.transform.position.z));

        var topLeft = _topLeftPoint.position;
        var bottomRight = _bottomRightPoint.position;
        if (pos.x < topLeft.x || pos.y > topLeft.y || pos.x > bottomRight.x || pos.y < bottomRight.y)
        {
            return -1;
        }
        return Mathf.Min(6, Mathf.FloorToInt(7 * (pos.x - topLeft.x) / (bottomRight.x - topLeft.x)));
    }

    private Vector3 GetBoardPosition(int col, int row)
    {
        float x = COLUMN_WIDTH * (col - COLUMNS / 2);
        float y = _bottomRightPoint.position.y + ROW_HEIGHT * (row + 0.5f);
        return new Vector3(x, y, 0);
    }

    Disc GetDisc(int col, int row)
    {
        return _discs[col + row * COLUMNS];
    }

    void SetDisc(int col, int row, Disc disc)
    {
        _discs[col + row * COLUMNS] = disc;
    }

    int NextRowAtColumn(int col)
    {
        for (int row = 0; row < ROWS; row++)
        {
            if (GetDisc(col, row) == null)
            {
                return row;
            }
        }
        return -1;
    }

    public bool CheckFourInARow()
    {
        for (int col = 0; col < COLUMNS; col++)
        {
            for (int row = 0; row < ROWS; row++)
            {
                if (CheckHorizontal(col, row) || CheckVertical(col, row)
                    || CheckDiagonalUp(col, row) || CheckDiagonalDown(col, row))
                {
                    return true;
                }
            }
        }
        return false;
    }

    // Checks horizontal match from (col, row) to the right
    bool CheckHorizontal(int col, int row)
    {
        if (col > COLUMNS - 4)
        {
            return false;
        }

        var disc = GetDisc(col, row);
        if (disc == null)
        {
            return false;
        }
        DiscColor color = disc.Color;
        for (int i = 1; i < 4; i++)
        {
            disc = GetDisc(col + i, row);
            if (disc == null || disc.Color != color)
            {
                return false;
            }
        }
        Debug.Log("Horizontal match from (" + col + ", " + row + ")");
        return true;
    }

    // Checks vertical match from (col, row) up
    bool CheckVertical(int col, int row)
    {
        if (row > ROWS - 4)
        {
            return false;
        }

        var disc = GetDisc(col, row);
        if (disc == null)
        {
            return false;
        }
        DiscColor color = disc.Color;
        for (int i = 1; i < 4; i++)
        {
            disc = GetDisc(col, row + i);
            if (disc == null || disc.Color != color)
            {
                return false;
            }
        }
        Debug.Log("Vertical match from (" + col + ", " + row + ")");
        return true;
    }

    // Checks diagonal match from (col, row) diagonal up-right
    bool CheckDiagonalUp(int col, int row)
    {
        if (row > ROWS - 4)
        {
            return false;
        }

        var disc = GetDisc(col, row);
        if (disc == null)
        {
            return false;
        }
        DiscColor color = disc.Color;
        for (int i = 1; i < 4; i++)
        {
            disc = GetDisc(col + i, row + i);
            if (disc == null || disc.Color != color)
            {
                return false;
            }
        }
        Debug.Log("Diagonal-up match from (" + col + ", " + row + ")");
        return true;
    }

    // Checks diagonal match from (col, row) diagonal down-right
    bool CheckDiagonalDown(int col, int row)
    {
        if (row < 4)
        {
            return false;
        }

        var disc = GetDisc(col, row);
        if (disc == null)
        {
            return false;
        }
        DiscColor color = disc.Color;
        for (int i = 0; i < 4; i++)
        {
            disc = GetDisc(col - i, row - i);
            if (disc == null || disc.Color != color)
            {
                return false;
            }
        }
        Debug.Log("Diagonal-down match from (" + col + ", " + row + ")");
        return true;
    }
}
