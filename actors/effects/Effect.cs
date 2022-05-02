using Godot;
using System;

namespace topdownGame
{
    public class Effect : AnimatedSprite
    {

        public override void _Ready()
        {
            var _a = Connect("AnimationFinished", this, "_on_AnimationFinished");
            Play("Animate");
        }

        private void _on_AnimationFinished()
        {
            QueueFree();
        }
    }
}
