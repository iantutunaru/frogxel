using UnityEngine;

namespace Frogxel.Lanes
{
    [CreateAssetMenu(fileName = "MovingLaneConfig", menuName = "Lanes/New Moving Lane Config")]
    public class MovingLaneConfig : LaneConfig
    {
        [field: Header("Moving Lane")]
        [field: SerializeField] public GameObject MoveablePrefab { get; private set; }
        [field: SerializeField] public Vector2 MoveDirection { get; private set; }
        [field: SerializeField] public float MoveSpeed { get; private set; }
        [field: SerializeField] public float GapWidth { get; private set; }
    }
}