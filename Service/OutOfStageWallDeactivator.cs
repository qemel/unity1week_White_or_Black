using System.Collections.Generic;
using MessagePipe;
using R3;
using u1w_2024_3.Src.Model;
using u1w_2024_3.Src.Model.Message;
using u1w_2024_3.Src.View;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace u1w_2024_3.Src.Service
{
    public sealed class OutOfStageWallDeactivator : IStartable
    {
        [Inject] private readonly StageField _stageField;
        [Inject] private readonly WallRepository _stageWalls;
        [Inject] private readonly ISubscriber<CamMovementEvent> _subscriber;
        [Inject] private readonly Wall _wallPrefab;

        /// <summary>
        /// 生成した壁のリスト
        /// </summary>
        private readonly Dictionary<Vector2Int, Wall> _inits = new();

        private readonly CompositeDisposable _disposable = new();

        public void Start()
        {
            _subscriber.Subscribe(e =>
            {
                if (e.Type == CamMovementEvent.EventType.StageFieldMoved)
                {
                    foreach (var wall in _stageWalls.Walls)
                    {
                        if (_stageField.IsOutOfBoundsGrid(wall.Key))
                        {
                            wall.Value.SetActive(false);
                        }
                        else
                        {
                            wall.Value.SetActive(true);
                        }
                    }
                }
                else if (e.Type == CamMovementEvent.EventType.StageFieldMoved)
                {
                    var move = e.Move;

                    if (move == Vector2Int.right)
                    {
                        // Left Edge
                        for (var y = _stageField.LowerBoundGrid; y <= _stageField.UpperBoundGrid; y++)
                        {
                            var edgePos = new Vector2Int(_stageField.LeftBoundGrid, y);
                            var initPos = new Vector2Int(_stageField.LeftBoundGrid - 1, y);
                            HandleInit(edgePos, initPos);
                        }
                    }
                    else if (move == Vector2Int.left)
                    {
                        // Right Edge
                        for (var y = _stageField.LowerBoundGrid; y <= _stageField.UpperBoundGrid; y++)
                        {
                            var edgePos = new Vector2Int(_stageField.RightBoundGrid, y);
                            var initPos = new Vector2Int(_stageField.RightBoundGrid + 1, y);
                            HandleInit(edgePos, initPos);
                        }
                    }
                    else if (move == Vector2Int.up)
                    {
                        // Lower Edge
                        for (var x = _stageField.LeftBoundGrid; x <= _stageField.RightBoundGrid; x++)
                        {
                            var edgePos = new Vector2Int(x, _stageField.LowerBoundGrid);
                            var initPos = new Vector2Int(x, _stageField.LowerBoundGrid - 1);
                            HandleInit(edgePos, initPos);
                        }
                    }
                    else if (move == Vector2Int.down)
                    {
                        // Upper Edge
                        for (var x = _stageField.LeftBoundGrid; x <= _stageField.RightBoundGrid; x++)
                        {
                            var edgePos = new Vector2Int(x, _stageField.UpperBoundGrid);
                            var initPos = new Vector2Int(x, _stageField.UpperBoundGrid + 1);
                            HandleInit(edgePos, initPos);
                        }
                    }
                }
            }).AddTo(_disposable);
        }

        private void HandleInit(Vector2Int edgePos, Vector2Int initPos)
        {
            if (_stageWalls.Walls.ContainsKey(edgePos))
            {
                if (_inits.ContainsKey(edgePos)) return;

                var info = _stageWalls.Walls[edgePos].PlayerInfoByColor;
                var wall = Object.Instantiate(_wallPrefab, new Vector3(initPos.x, initPos.y, 0),
                    Quaternion.identity,
                    info.Color == Color.white ? _stageWalls.WhiteParent : _stageWalls.BlackParent);
                wall.Init(info);
                var canAdd = _inits.TryAdd(initPos, wall);
                if (!canAdd)
                {
                    Object.Destroy(wall.gameObject);
                }
            }
            else
            {
                if (!_inits.ContainsKey(initPos)) return;

                _inits.Remove(initPos);
                Object.Destroy(_inits[initPos].gameObject);
            }

            DestroyAllInitsInBounds();
        }

        private void DestroyAllInitsInBounds()
        {
            for (var y = _stageField.LowerBoundGrid; y <= _stageField.UpperBoundGrid; y++)
            {
                for (var x = _stageField.LeftBoundGrid; x <= _stageField.RightBoundGrid; x++)
                {
                    var pos = new Vector2Int(x, y);
                    if (_inits.ContainsKey(pos))
                    {
                        Object.Destroy(_inits[pos].gameObject);
                        _inits.Remove(pos);
                    }
                }
            }
        }
    }
}