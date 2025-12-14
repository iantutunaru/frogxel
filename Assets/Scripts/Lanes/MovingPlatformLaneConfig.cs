using UnityEngine;

namespace Frogxel.Lanes
{
    [CreateAssetMenu(fileName = "MovingPlatformLaneConfig", menuName = "Lanes/New Moving Platform Lane Config")]
    public class MovingPlatformLaneConfig : MovingLaneConfig
    {
        [field: Header("Moving Platform Lane")]
        [field: SerializeField] public string Layer { get; private set; }
    }
}