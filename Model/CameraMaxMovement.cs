using System;
using UnityEngine;

namespace u1w_2024_3.Src.Model
{
    [Serializable]
    public sealed class CameraMaxMovement
    {
        public int MaxX => _maxX;
        public int MaxY => _maxY;

        [SerializeField] private int _maxX;
        [SerializeField] private int _maxY;
    }
}