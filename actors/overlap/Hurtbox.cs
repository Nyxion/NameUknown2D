using Godot;
using System;
using static Godot.GD;

namespace TopDownGame
{
    public class Hurtbox : Area2D
    {
        private PackedScene hitEffect = ResourceLoader.Load<PackedScene>("res://actors/effects/HitEffect.tscn");

        [Signal]
        delegate void InvincibilityStarted();
        [Signal]
        delegate void InvincibilityEnded();

        private bool _invincible = false;
        public bool Invincible
        {
            get => _invincible;
            set
            {
                _invincible = value;
            }
        }

        private Timer timer;
        private CollisionShape2D collisionShape;

        public override void _Ready()
        {
            timer = GetNode<Timer>("Timer");
            collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
        }
        public void CreateEffect()
        {
            var effect = hitEffect.Instance();
            var main = GetTree().CurrentScene;
            main.AddChild(effect);
            GD.Print(effect._Get("global_position"));
        }
        public void StartInvincibility(float duration)
        {
            Invincible = true;
            EmitSignal("InvincibilityStarted");
            timer.Start(duration);
        }
        public void _on_Hurtbox_InvincibilityStarted()
        {
            SetDeferred("monitoring", false);
            collisionShape.SetDeferred("disabled", true);
        }
        public void _on_Hurtbox_InvincibilityEnded()
        {
            SetDeferred("monitoring", true);
            collisionShape.SetDeferred("disabled", false);
        }
        public void _on_Timer_timeout()
        {
            Invincible = false;
            EmitSignal("InvincibilityEnded");
        }
    }
}
