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
            nodePlayer = GetTree().Root.GetNode<Entity>("World/YSort/Player");
            player = (Entity)nodePlayer;
            player.PlayerSpawned += SpawnCompleted;
            player.Connect("interacted", this, "OpenChest");
            sprite = GetNode<AnimatedSprite>("Sprite");
        }
        public void SpawnCompleted(Entity _player)
        {
            _player.Connect("interacted", this, "OpenChest");
            _player.PlayerSpawned += SpawnCompleted;
            player = _player;
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