using Godot;
using System;

namespace TopDownGame
{
    public class Chest : Area2D
    {
        private Node nodePlayer;
        private Entity player;
        private AnimatedSprite sprite;

        public override void _Ready()
        {
            sprite = GetNode<AnimatedSprite>("Sprite");
            nodePlayer = GetTree().Root.GetNode<Entity>("World/YSort/Player");
            player = (Entity)nodePlayer;

        }
        public void SpawnCompleted(Entity _player)
        {
            player = _player;
            player.Connect("interacted", this, "OpenChest");
            player.PlayerSpawned += SpawnCompleted;
        }
        private void _on_Sprite_animation_finished()
        {
            if (sprite.Animation == "Opening")
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