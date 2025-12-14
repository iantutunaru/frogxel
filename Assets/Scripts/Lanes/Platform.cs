using UnityEngine;

namespace Frogxel.Lanes
{
    public class Platform : Moveable
    {
        [field: SerializeField] public int Size { get; private set; }

        protected override int GetWidth()
        {
            return Size;
        }
    }
}