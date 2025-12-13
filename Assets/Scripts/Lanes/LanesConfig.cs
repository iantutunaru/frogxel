using System.Collections.Generic;
using UnityEngine;

namespace Frogxel.Lanes
{
    [CreateAssetMenu(fileName = "LanesConfig", menuName = "Lanes/New Lanes Config")]
    public class LanesConfig : ScriptableObject
    {
        [field: SerializeField] public int LaneWidth { get; private set; }
        [field: SerializeField] public List<LaneConfig> Lanes { get; private set; }
    }
}