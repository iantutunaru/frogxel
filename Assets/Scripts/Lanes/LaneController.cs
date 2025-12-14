using UnityEngine;

namespace Frogxel.Lanes
{
    public class LaneController : MonoBehaviour
    {
        [Header("Lane")]
        [SerializeField] private SpriteRenderer background;

        public virtual void Init(LaneConfig laneConfig, int width)
        {
            background.sprite = laneConfig.Background;
            background.drawMode = SpriteDrawMode.Tiled;
            background.size = new Vector2(width, laneConfig.Height);
        }
    }
}