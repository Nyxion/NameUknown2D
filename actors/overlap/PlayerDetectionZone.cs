using Godot;
using System;

namespace TopDownGame
{
    public class PlayerDetectionZone : Area2D
    {
        public Node player;

        public bool TimeToDie()
        {
            return player != null;
        }

        public void _on_PlayerDetectionZone_body_entered(Node body)
        {
            player = body;
        }

        public void _on_PlayerDetectionZone_body_exited(Node body)
        {
            player = null;
        }
    }

}