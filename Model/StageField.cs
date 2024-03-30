using UnityEngine;

namespace u1w_2024_3.Src.Model
{
    /// <summary>
    /// ステージ全体の情報
    /// </summary>
    public sealed class StageField
    {
        /// <summary>
        /// マップ端(全て座標を含んだ範囲)
        /// </summary>
        public int UpperBoundGrid { get; private set; }

        public int LowerBoundGrid { get; private set; }
        public int LeftBoundGrid { get; private set; }
        public int RightBoundGrid { get; private set; }

        public float UpperBoundFloat => UpperBoundGrid;
        public float LowerBoundFloat => LowerBoundGrid - 1;
        public float LeftBoundFloat => LeftBoundGrid;
        public float RightBoundFloat => RightBoundGrid + 1;


        public int Width => RightBoundGrid - LeftBoundGrid + 1;
        public int Height => UpperBoundGrid - LowerBoundGrid + 1;

        private const float JudgementMargin = 0.01f;

        public void SetBoundsGrid(int topBound, int bottomBound, int leftBound, int rightBound)
        {
            UpperBoundGrid = topBound;
            LowerBoundGrid = bottomBound;
            LeftBoundGrid = leftBound;
            RightBoundGrid = rightBound;
            
            Debug.Log($"StageField(Grid): {LeftBoundGrid}, {RightBoundGrid}, {LowerBoundGrid}, {UpperBoundGrid}");
            Debug.Log($"StageField(Float): {LeftBoundFloat}, {RightBoundFloat}, {LowerBoundFloat}, {UpperBoundFloat}");
        }

        public void Move(Vector2Int move)
        {
            UpperBoundGrid += move.y;
            LowerBoundGrid += move.y;
            LeftBoundGrid += move.x;
            RightBoundGrid += move.x;
        }

        public bool IsInBoundsGrid(Vector2Int position)
        {
            return position.x >= LeftBoundGrid && position.x <= RightBoundGrid &&
                   position.y >= LowerBoundGrid && position.y <= UpperBoundGrid;
        }

        public bool IsInBoundsFloat(Vector2 position)
        {
            return position.x >= LeftBoundFloat && position.x <= RightBoundFloat &&
                   position.y >= LowerBoundFloat && position.y <= UpperBoundFloat;
        }
        
        public bool IsInBoundsWithPlayerSize(float x, float y, float playerSize)
        {
            var margin = playerSize / 2;
            return x >= LeftBoundFloat + margin && x <= RightBoundFloat - margin &&
                   y >= LowerBoundFloat + margin && y <= UpperBoundFloat - margin;
        }
        
        public bool IsInBoundsWithPlayerSize(Vector2 position, float playerSize)
        {
            return IsInBoundsWithPlayerSize(position.x, position.y, playerSize);
        }

        /// <summary>
        /// playerのサイズを考慮して範囲外か判定（見た目の改善用）
        /// </summary>
        /// <remarks>
        /// playerは正方形と仮定
        /// </remarks>
        /// <returns></returns>
        public bool IsOutOfBoundsWithPlayerSize(float x, float y, float playerSize)
        {
            var margin = playerSize / 2;
            return x < LeftBoundFloat + margin || x > RightBoundFloat - margin ||
                   y < LowerBoundFloat + margin || y > UpperBoundFloat - margin;
        }

        public bool IsOutOfBoundsWithPlayerSize(Vector2 position, float playerSize)
        {
            return IsOutOfBoundsWithPlayerSize(position.x, position.y, playerSize);
        }

        private bool IsOutOfBoundsFloat(float x, float y)
        {
            return x < LeftBoundFloat - JudgementMargin || x > RightBoundFloat + JudgementMargin ||
                   y < LowerBoundFloat - JudgementMargin || y > UpperBoundFloat + JudgementMargin;
        }

        public bool IsOutOfBoundsFloat(Vector2 position)
        {
            return IsOutOfBoundsFloat(position.x, position.y);
        }

        public bool IsOutOfBoundsGrid(Vector2Int position)
        {
            var x = position.x;
            var y = position.y;
            return x < LeftBoundGrid || x > RightBoundGrid || y < LowerBoundGrid || y > UpperBoundGrid;
        }

        /// <summary>
        /// 逆側の座標を取得
        /// </summary>
        /// <returns></returns>
        public Vector2 GetInversePosition(Vector2 position)
        {
            if (IsInBoundsFloat(position))
            {
                Debug.LogError("Position is in bounds.");
                return position;
            }

            var x = position.x;
            var y = position.y;

            if (x < LeftBoundFloat)
            {
                x += Width;
            }
            else if (x > RightBoundFloat)
            {
                x -= Width;
            }

            if (y < LowerBoundFloat)
            {
                y += Height;
            }
            else if (y > UpperBoundFloat)
            {
                y -= Height;
            }

            return new Vector2(x, y);
        }

        public Vector2 GetInversePositionWithPlayerMargin(Vector2 position, float playerSize)
        {
            var margin = playerSize / 2;

            var x = position.x;
            var y = position.y;

            if (x <= LeftBoundFloat + margin)
            {
                x += Width;
            }
            else if (x >= RightBoundFloat - margin)
            {
                x -= Width;
            }

            if (y <= LowerBoundFloat + margin)
            {
                y += Height;
            }
            else if (y >= UpperBoundFloat - margin)
            {
                y -= Height;
            }

            return new Vector2(x, y);
        }
    }
}