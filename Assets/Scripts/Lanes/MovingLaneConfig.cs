using UnityEngine;

namespace Frogxel.Lanes
{
    public class MovingLaneConfig : LaneConfig
    {
        [field: Header("Moving Lane")]
        [field: SerializeField] public Moveable MoveablePrefab { get; private set; }
        [field: SerializeField] public Vector2 MoveDirection { get; private set; }
        [field: SerializeField] public float MoveSpeed { get; private set; }
        [field: SerializeField] public int GapWidth { get; private set; }
    }
}