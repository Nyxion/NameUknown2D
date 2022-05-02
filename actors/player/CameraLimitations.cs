using Godot;

namespace TopDownGame
{
    public class CameraLimitations : Camera2D
    {
        private Node limits;
        private Position2D topLeft;
        private Position2D bottomRight;
        public override void _Ready()
        {
            limits = GetNode<Node>("Limits");
            topLeft = limits.GetNode<Position2D>("TopLeft");
            bottomRight = limits.GetNode<Position2D>("BottomRight");

            LimitTop = (int)topLeft.Position.y;
            LimitLeft = (int)topLeft.Position.x;

            LimitBottom = (int)bottomRight.Position.y;
            LimitRight = (int)bottomRight.Position.x;
        }
    }
}
