using UnityEngine;

namespace Btkalman.Util {
    public enum Direction {
        NONE,
        LEFT,
        RIGHT,
        UP,
        DOWN,
    }

    public class Directions {
        public static Vector2 ToVector2(Direction d) {
            switch (d) {
                case Direction.NONE:
                    return Vector2.zero;
                case Direction.LEFT:
                    return Vector2.left;
                case Direction.RIGHT:
                    return Vector2.right;
                case Direction.UP:
                    return Vector2.up;
                case Direction.DOWN:
                    return Vector2.down;
                default:
                    throw new System.ArgumentException("no direction " + d);
            }
        }

        public static float X(Direction d) {
            return d == Direction.LEFT ? -1f : d == Direction.RIGHT ? 1f : 0f;
        }

        public static float Y(Direction d) {
            return d == Direction.DOWN ? -1f : d == Direction.UP ? 1f : 0f;
        }
    }
}