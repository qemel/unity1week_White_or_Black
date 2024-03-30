using System.Collections.Generic;
using u1w_2024_3.Src.Model;
using UnityEngine;
using VContainer;

namespace u1w_2024_3.Src.View
{
    public sealed class WallRepository : MonoBehaviour
    {
        [Inject] private readonly StageField _stageField;
        [Inject] private readonly CameraMaxMovement _movement;

        [SerializeField] private PlayerInfoByColor _white;
        [SerializeField] private PlayerInfoByColor _black;

        public Transform WhiteParent => _whiteParent;
        public Transform BlackParent => _blackParent;

        [SerializeField] private Transform _whiteParent;
        [SerializeField] private Transform _blackParent;

        [SerializeField] private Wall _wallPrefab;

        [SerializeField] private bool _generateWall = true;

        public IReadOnlyDictionary<Vector2Int, Wall> Walls => _walls;
        private readonly Dictionary<Vector2Int, Wall> _walls = new();

        private void Awake()
        {
            if (!_generateWall) return;

            var children = GetComponentsInChildren<Wall>();

            foreach (var wall in children)
            {
                var position = new Vector2Int((int)wall.transform.position.x, (int)wall.transform.position.y);
                wall.Init(_white);
                _walls.Add(position, wall);
            }
        }

        private void Start()
        {
            if (!_generateWall) return;
            
            for (var y = _stageField.LowerBoundGrid - _movement.MaxY;
                 y <= _stageField.UpperBoundGrid + _movement.MaxY;
                 y++)
            {
                for (var x = _stageField.LeftBoundGrid - _movement.MaxX;
                     x <= _stageField.RightBoundGrid + _movement.MaxX;
                     x++)
                {
                    var position = new Vector2Int(x, y);
                    if (!_walls.ContainsKey(position))
                    {
                        var wall = Instantiate(_wallPrefab, new Vector3(x, y, 0),
                            Quaternion.identity, _blackParent);
                        wall.Init(_black);
                        _walls.Add(position, wall);
                    }
                }
            }
        }
    }
}