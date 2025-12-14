using UnityEngine;

namespace Frogxel.Lanes
{
    [CreateAssetMenu(fileName = "MovingObstaclesLane", menuName = "Lanes/New Moving Obstacles Lane Config")]
    public class MovingObstaclesLaneConfig : MovingLaneConfig
    {
        [field: Header("Moving Obstacles Lane")]
        [field: SerializeField] public Sprite Sprite { get; private set; }
        [field: SerializeField] public RuntimeAnimatorController AnimatorController { get; private set; }
        [field: SerializeField] public int Count { get; private set; }
    }
}