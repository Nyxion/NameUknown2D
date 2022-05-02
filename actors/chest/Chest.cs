using Godot;
using System;

namespace topdownGame
{
    public class Chest : Area2D
    {
        private Node Player;
        private AnimatedSprite Sprite;

        public override void _Ready()
        {
            Player = GetTree().Root.FindNode("Player", true, false);
            Player.Connect("interacted", this, "OpenChest");
            Sprite = GetNode<AnimatedSprite>("Sprite");
        }

        private void _on_Sprite_animation_finished()
        {
            if (Sprite.Animation == "Opening")
            {
                QueueFree();
            }
        }

        private void OpenChest(Area2D chest)
        {
            if (chest == null) return;
            var a = chest.GetNode<AnimatedSprite>("Sprite");
            a.Play("Opening");
        }
    }

}