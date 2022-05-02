using Godot;

namespace TopDownGame
{
    public class Hitbox : Area2D
    {
        [Export]
#pragma warning disable CS0414
        private int damage = 1;
#pragma warning restore CS0414
    }
}
