using UnityEditor.Animations;
using UnityEngine;

namespace Frogxel.Lanes
{
    [CreateAssetMenu(fileName = "MovingObstaclesLane", menuName = "Lanes/New Moving Obstacles Lane Config")]
    public class MovingObstaclesLaneConfig : MovingLaneConfig
    {
        [Header("Moving Obstacles Lane")]
        [field: SerializeField] public Sprite Sprite { get; private set; }
        [field: SerializeField] public AnimatorController AnimatorController { get; private set; }
        [field: SerializeField] public int Count { get; private set; }
    }
}