using System;
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
    private List<Vector2Int> _directions;
    private int _discsCount;

    private const float COLUMN_WIDTH = 0.9f;
    private const float ROW_HEIGHT = 0.8f;
    public const int COLUMNS = 7;
    public const int ROWS = 6;

    private void Start()
    {
        _discs = new Disc[ROWS * COLUMNS];
        for (int i = 0; i < ROWS * COLUMNS; i++)
        {
            _discs[i] = null;
        }

        _directions = new List<Vector2Int>()
        {
            new Vector2Int(1,0),
            new Vector2Int(-1,0),
            new Vector2Int(0, -1),
            new Vector2Int(1,1),
            new Vector2Int(1,-1),
            new Vector2Int(-1, 1),
            new Vector2Int(-1, -1)
        };
    }

    public bool CanAddDiscAtColumn(int col)
    {
        return GetDisc(col, 5) == null;
    }

    public void AddDiscAtColumn(int col, DiscColor color, Action<DiscColor, bool> completeCallback)
    {
        int row = NextRowAtColumn(col);
        var dropPosition = GetDropPosition(col, row);
        var targetPosition = GetBoardPosition(col, row);

        var disc = Instantiate(_discPrefab);
        disc.transform.position = dropPosition;
        disc.transform.SetParent(_discsContainer, false);
        disc.Initialize(color);
        SetDisc(col, row, disc);
        _discsCount++;

        float time = 0.07f * (dropPosition.y - targetPosition.y);
        LeanTween.move(disc.gameObject, targetPosition, time)
            .setEase(LeanTweenType.easeOutSine)
            .setOnComplete(() => completeCallback?.Invoke(color, CheckFourInARow(col, row)));
    }

    public int GetColumnWithMousePos(Vector3 mousePos)
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

    private Vector3 GetDropPosition(int col, int row)
    {
        return GetBoardPosition(col, ROWS - 1);
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

    public bool CheckFourInARow(int col, int row)
    {
        foreach (var dir in _directions)
        {
            if (CheckFourInARowWithDirection(col, row, dir.x, dir.y))
            {
                return true;
            }
        }
        return false;
    }

    // Checks horizontal match from (col, row) to the right
    bool CheckFourInARowWithDirection(int col, int row, int hDir, int vDir)
    {
        if (col + hDir * 3 >= COLUMNS || col + hDir * 3 < 0 || row + vDir * 3 >= ROWS || row + vDir * 3 < 0)
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
            disc = GetDisc(col + i * hDir, row + i * vDir);
            if (disc == null || disc.Color != color)
            {
                return false;
            }
        }
        Debug.Log("Match from (" + col + ", " + row + ") with direction (" + hDir + ", " + vDir + ")");
        return true;
    }

    public bool IsComplete()
    {
        return _discsCount == ROWS * COLUMNS;
    }
}
