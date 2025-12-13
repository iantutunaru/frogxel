using UnityEditor.Animations;
using UnityEngine;

namespace Frogxel.Lanes
{
    [CreateAssetMenu(fileName = "MovingObstaclesLane", menuName = "Lanes/New Moving Obstacles Lane Config")]
    public class MovingObstaclesLaneConfig : LaneConfig
    {
        [field: Header("Moving Obstacles Lane")]
        [field: SerializeField] public Vector2 MoveDirection { get; private set; }
        [field: SerializeField] public float MoveSpeed { get; private set; }
        [field: SerializeField] public int GapWidth { get; private set; }
        
        [Header("Obstacle")]
        [field: SerializeField] public Sprite Sprite { get; private set; }
        [field: SerializeField] public AnimatorController AnimatorController { get; private set; }
        [field: SerializeField] public int Count { get; private set; }
    }
}