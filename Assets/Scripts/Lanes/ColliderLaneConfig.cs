using UnityEngine;

namespace Frogxel.Lanes
{
    [CreateAssetMenu(fileName = "ColliderLaneConfig", menuName = "Lanes/New Collider Lane Config")]
    public class ColliderLaneConfig : LaneConfig
    {
        [field: Header("Obstacle Lane")]
        [field: SerializeField] public string Layer { get; private set; }
    }
}