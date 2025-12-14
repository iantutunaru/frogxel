using UnityEngine;

namespace Frogxel.Lanes
{
    [CreateAssetMenu(fileName = "LaneConfig", menuName = "Lanes/New Lane Config")]
    public class LaneConfig : ScriptableObject
    {
        [field: Header("Lane")]
        [field: SerializeField] public LaneController Prefab { get; private set; }
        [field: SerializeField] public Sprite Background { get; private set; }
        [field: SerializeField] public int Height { get; private set; }
    }
}