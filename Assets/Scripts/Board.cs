using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

// Board creates and manages the discs.
// It also has checks for winning condition and a method to get the best move for the AI
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
    private const float COUNT_WEIGHT = 1;
    private const float PILING_WEIGHT = 0.75f;

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
            new Vector2Int(0, 1),
            new Vector2Int(1,1),
            new Vector2Int(1,-1)
        };
    }

    // Check if a disc can be added to a certain column.
    // If the top position is empty then a disc can be added
    public bool CanAddDiscAtColumn(int col)
    {
        return GetDisc(col, 5) == null;
    }

    // Creates a new disc and adds it to the board at a certain column
    // The disc appears with an animation
    public void AddDiscAtColumn(int col, DiscColor color, bool isAI, Action<DiscColor, bool, bool> completeCallback)
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
            .setOnComplete(() => completeCallback?.Invoke(color, isAI, CheckFourInARow(col, row)));
    }

    // Get a column from mouse position
    // If clicked outside the board, returns -1
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

    // Get the world position of a (col,row)
    private Vector3 GetBoardPosition(int col, int row)
    {
        float x = COLUMN_WIDTH * (col - COLUMNS / 2);
        float y = _bottomRightPoint.position.y + ROW_HEIGHT * (row + 0.5f);
        return new Vector3(x, y, 0);
    }

    // Get the position from where the discs are dropped
    private Vector3 GetDropPosition(int col, int row)
    {
        return GetBoardPosition(col, ROWS - 1);
    }

    private Disc GetDisc(int col, int row)
    {
        return _discs[col + row * COLUMNS];
    }

    private void SetDisc(int col, int row, Disc disc)
    {
        _discs[col + row * COLUMNS] = disc;
    }

    // Get the next empty row for a certain column
    // If the column is full return -1
    private int NextRowAtColumn(int col)
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

    // Check if a match was made that has (col, row)
    public bool CheckFourInARow(int col, int row)
    {
        var disc = GetDisc(col, row);
        foreach (var dir in _directions)
        {
            // Check in all directions if there are 4 discs with the same color as disc
            if (CheckFourInARowWithDirection(disc.Color, col, row, dir))
            {
                return true;
            }
        }
        return false;
    }

    // Checks if there are 3 discs with color <color> around (col,row) in direction <dir>
    private bool CheckFourInARowWithDirection(DiscColor color, int col, int row, Vector2Int dir)
    {
        int count = 0;
        count += CountInHalfDirection(color, col, row, dir);
        count += CountInHalfDirection(color, col, row, -dir);
        if (count == 3)
        {
            //Debug.Log("Match from (" + col + ", " + row + ") with direction: " + dir);
            return true;
        }
        return false;
    }

    // Get the amount of discs that are of color <color> from this (col, row) in a direction <dir>
    private int CountInHalfDirection(DiscColor color, int col, int row, Vector2Int dir)
    {
        int count = 0;
        for (int i = 1; i <= 3; i++)
        {
            var pos = new Vector2Int(col + i * dir.x, row + i * dir.y);
            if (!IsValidPosition(pos))
            {
                continue;
            }
            var disc = GetDisc(pos.x, pos.y);
            if (disc == null || disc.Color != color)
            {
                break;
            }
            else if (disc != null)
            {
                count++;
            }
        }
        return count;
    }

    // Check if the board is complete
    public bool IsComplete()
    {
        return _discsCount == ROWS * COLUMNS;
    }

    // Get a column where to play. Useful for AI to know where to play next
    public int GetGoodMove(DiscColor color)
    {
        // First check if there's any position to win
        int col = GetColumnForMatch(color);
        if (col != -1)
        {
            return col;
        }

        // Then check if there's any position to prevent losing
        col = GetColumnForMatch(color == DiscColor.Black ? DiscColor.Red : DiscColor.Black);
        if (col != -1)
        {
            return col;
        }

        // Last return the column that has the best potential placement
        return GetColumnForPotentialMatchInDirections(color, _directions);
    }

    // Check every column to see if there's a possible match
    private int GetColumnForMatch(DiscColor color)
    {
        for (int col = 0; col < COLUMNS; col++)
        {
            int row = NextRowAtColumn(col);
            if (row == -1)
            {
                continue;
            }
            foreach (var dir in _directions)
            {
                if (CheckFourInARowWithDirection(color, col, row, dir))
                {
                    return col;
                }
            }
        }
        return -1;
    }

    // Goes through every column and for each column goes through every empty row and checks what's the potential of making a match by adding
    // a disc in the empty slots. It adds the potential for each column to get the column with the highest potential.
    private int GetColumnForPotentialMatchInDirections(DiscColor color, List<Vector2Int> directions)
    {
        float maxPotential = -10;
        int maxPotentialCol = -1;
        for (int col = 0; col < COLUMNS; col++)
        {
            int nextRow = NextRowAtColumn(col);
            if (nextRow == -1)
            {
                continue;
            }
            for (int row = nextRow; row < ROWS; row++) // check all positions from this one up, to see if there are potential matches
            {
                float positivePotential = 0;
                float totalPotential = 0;
                // Get the potential for all directions for this position(positive potentials from different directions add up)
                foreach (var dir in directions)
                {
                    float potentialAdd = GetPotentialMatchInDirection(color, col, row, dir);
                    if (potentialAdd > 0)
                    {
                        positivePotential += potentialAdd;
                    }
                    totalPotential += potentialAdd;
                }
                float potential = positivePotential >= 0 ? positivePotential : totalPotential; // if there is at least one positive potential, I use that, if not I use the sum of all negative potentials
                if (potential > maxPotential || (potential == maxPotential && UnityEngine.Random.value < 0.5f)) // if same potential choose randomly
                {
                    maxPotential = potential;
                    maxPotentialCol = col;
                }
            }
        }

        return maxPotentialCol;
    }

    // Get the potential of a match in a certain direction
    // The potential is calculated by weightning the amount of discs already placed and the amount that needed to be piled up to place the needed ones    
    private float GetPotentialMatchInDirection(DiscColor color, int col, int row, Vector2Int dir)
    {
        int count = 0;
        int pilingNeeded = 0;
        var bestWindow = GetBestFourDiscWindow(color, col, row, dir);

        if (bestWindow == null)
        {
            return -10; // very low potential as this position cannot make a match
        }

        foreach (var pos in bestWindow)
        {
            var disc = GetDisc(pos.x, pos.y);
            if (disc != null)
            {
                count++;
            }
            else if (dir.x != 0) // do not count piling if piling vertically because it would pile onto the same color each turn
            {
                pilingNeeded += Mathf.Abs(pos.y - NextRowAtColumn(pos.x));
            }
        }

        if (pilingNeeded > 0)
        {
            //Debug.Log("pos:(" + col + ", " + row + "), dir: " + dir + ", count: " + count + ", pilingNeeded: " + pilingNeeded + " " + string.Join(",", bestWindow));
        }
        return 1 + count * COUNT_WEIGHT - pilingNeeded * PILING_WEIGHT;
    }

    // From 7 discs(3 to each side of the disc I'm looking at) take the best window of 4.
    // The best window is the one with most discs of the center disc's color and that does not have any discs of the other color
    // If non can be found, return null
    private List<Vector2Int> GetBestFourDiscWindow(DiscColor color, int col, int row, Vector2Int dir)
    {
        int bestOffset = 0; // best window offset
        int bestCount = -1; // count of discs of this color in the best window
        for (int offset = -3; offset <= 0; offset++)
        {
            int count = 0;
            int countPlusEmpty = 0;
            for (int i = 0; i < 4; i++)
            {
                var pos = new Vector2Int(col + (i + offset) * dir.x, row + (i + offset) * dir.y);
                if (!IsValidPosition(pos))
                {
                    break;
                }
                var disc = GetDisc(pos.x, pos.y);
                if (disc != null && disc.Color != color)
                {
                    break;
                }
                else
                {
                    countPlusEmpty++;
                    if (disc != null)
                    {
                        count++;
                    }
                }
            }
            if (countPlusEmpty >= 4) // if it can be a possible match
            {
                if (count > bestCount)
                {
                    bestCount = count;
                    bestOffset = offset;
                }
            }
        }

        if (bestCount == -1)
        {
            return null;
        }

        var bestWindow = new List<Vector2Int>();
        for (int i = 0; i < 4; i++)
        {
            bestWindow.Add(new Vector2Int(col + (i + bestOffset) * dir.x, row + (i + bestOffset) * dir.y));
        }

        return bestWindow;
    }

    // Return if a position is in bounds
    private bool IsValidPosition(Vector2Int pos)
    {
        return pos.x >= 0 && pos.y >= 0 && pos.x < COLUMNS && pos.y < ROWS;
    }
}
