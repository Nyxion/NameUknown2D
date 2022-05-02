using Godot;
using System;

namespace TopDownGame
{
    public class BloodSplatter : Particles2D
    {
        public override void _Ready()
        {
            OneShot = true;
            Emitting = true;
            var time = (Lifetime * 2) / SpeedScale;
            var _t = GetTree().CreateTimer(time).Connect("timeout", this, "queue_free");
        }
    }
}
