using Godot;
using System;

namespace TopDownGame
{
    public class SoftCollision : Area2D
    {
        public bool IsColliding()
        {
            var areas = GetOverlappingAreas();
            return areas.Count > 0;
        }

        public Vector2 GetPushVector()
        {
            var areas = GetOverlappingAreas();
            var push_vector = Vector2.Zero;
            if (IsColliding())
            {
                Area2D area = (Area2D)areas[0];
                push_vector = area.GlobalPosition.DirectionTo(GlobalPosition);
                push_vector.Normalized();
            }
            return push_vector;
        }
    }

}