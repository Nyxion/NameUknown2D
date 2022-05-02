using Godot;
using System;

namespace TopDownGame
{
    public class WanderingController : Node2D
    {
        [Export]
        private int wanderRange = 32;
        private Timer timer = null;
        public Vector2 StartPos = Vector2.Zero;
        public Vector2 TargetPos = Vector2.Zero;
        public override void _Ready()
        {
            timer = GetNode<Timer>("Timer");
            StartPos = GlobalPosition;
            TargetPos = GlobalPosition;
            _UpdateTargetPos();
        }

        private void _UpdateTargetPos()
        {
            var targetVector = new Vector2(new Random().Next(-wanderRange, wanderRange), new Random().Next(-wanderRange, wanderRange));
            TargetPos = StartPos + targetVector;
        }

        public float TimeLeft()
        {
            return timer.TimeLeft;
        }

        public void StartTimer(float duration)
        {
            timer.Start(duration);
        }

        private void _on_Timer_timeout()
        {
            _UpdateTargetPos();
        }
    }
}
