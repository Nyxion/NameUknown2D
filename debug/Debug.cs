using System.Text.RegularExpressions;
using System;
using Godot;

namespace TopDownGame.Debug
{
    public class Debug : Button
    {
        private Node nodePlayer;
        private Entity player;
        public override void _Ready()
        {
            nodePlayer = GetNode<Entity>("../../YSort/Player");
            player = (Entity)nodePlayer;
            player.PlayerSpawned += SpawnCompleted;
        }

        public void SpawnCompleted(Entity _player)
        {
            _player.PlayerSpawned += SpawnCompleted;
            player = _player;
        }
        private void _on_Add_Health_pressed()
        {
            if (player == null) return;
            player.Health++;
        }
        private void _on_Add_MaxHealth_pressed()
        {
            if (player == null) return;
            player.MaxHealth += 2;
        }
    }
}
