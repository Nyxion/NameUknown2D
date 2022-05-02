using System;
using Godot;
using static Godot.GD;

namespace TopDownGame
{
    public class Entity : KinematicBody2D
    {
        private PackedScene damageIndicator = Load<PackedScene>("res://actors/particles/BloodSplatter.tscn");
        [Signal]
        delegate void hp_changed(int value);
        [Signal]
        delegate void died();
        public enum EntityState
        {
            IDLE,
            WANDER,
            ATTACK,
            CHASE,
            MOVE,
            COUNT
        }
        public EntityState CurrentState;
        public enum EntityType
        {
            ENEMY,
            PLAYER,
            NPC,
            COUNT
        }
        public EntityType ChosenType;
#pragma warning disable CS0414
        [Export(PropertyHint.Range, "10,500,10")] public float Acceleration = 300;
        [Export(PropertyHint.Range, "10,200,10")] public float MaxSpeed = 50;
#pragma warning restore CS0414
        [Export(PropertyHint.Range, "10,500,10")] public float Friction = 300;
        [Export] private int _health = 10;
        public int Health
        {
            get => _health;
            set
            {
                EmitSignal("hp_changed", value);
                _health = value;
            }
        }
        [Export] private bool canRecieveKnockback = false;
        public Globals Globals;
        public Hurtbox HurtBox;
        public AnimationPlayer OnHitAnimation;
        public Vector2 Knockback = Vector2.Zero;
        public Vector2 Velocity = Vector2.Zero;
        public override void _Ready()
        {
            CurrentState = EntityState.IDLE;
        }
        public override void _PhysicsProcess(float delta)
        {
            if (canRecieveKnockback) RecieveKnockback(delta);

            switch (CurrentState)
            {
                // Idle
                case EntityState.IDLE:
                    {
                        _IdleState(delta);
                        break;
                    }
                // Attack
                case EntityState.ATTACK:
                    {
                        _AttackState(delta);
                        break;
                    }
                // Chase
                case EntityState.CHASE:
                    {
                        _ChaseState(delta);
                        break;
                    }
                // Wander
                case EntityState.WANDER:
                    {
                        _WanderState(delta);
                        break;
                    }
                // Move
                case EntityState.MOVE:
                    {
                        _MoveState(delta);
                        break;
                    }
            }
        }
        public virtual void _IdleState(float delta)
        {
            throw new NotImplementedException();
        }
        public virtual void _AttackState(float delta)
        {
            throw new NotImplementedException();
        }
        public virtual void _ChaseState(float delta)
        {
            throw new NotImplementedException();
        }
        public virtual void _WanderState(float delta)
        {
            throw new NotImplementedException();
        }
        public virtual void _MoveState(float delta)
        {
            throw new NotImplementedException();
        }
        public virtual void TakeDamage(int damage)
        {
            Health -= damage;
            if (Health == 0)
            {
                EmitSignal("died");
            }
        }
        public virtual void Death()
        {
            QueueFree();
            SpawnPlayer();
        }
        public event Action<Entity> PlayerSpawned;
        public void SpawnPlayer()
        {
            if (ChosenType == EntityType.PLAYER)
            {
                Globals = GetNode<Globals>("/root/Globals");
                var ySort = GetNode<YSort>("/root/World/YSort");
                var new_player = (Entity)Load<PackedScene>("res://actors/player/Player.tscn").Instance();
                new_player.GlobalPosition = Globals.PlayerStartPos;
                ySort.CallDeferred("add_child", new_player);
                PlayerSpawned?.Invoke(new_player);
            }
        }
        private void RecieveKnockback(float delta)
        {
            Knockback = Knockback.MoveToward(Vector2.Zero, Friction * delta);
            Knockback = MoveAndSlide(Knockback);
        }
        // Damage Splatter stuff
        // TODO: Remake function/method into single
        private void EnemyDamageTaken()
        {
            var BSInstance = (Particles2D)damageIndicator.Instance();
            BSInstance.ProcessMaterial = Load<Material>("res://actors/particles/slime_bloodsplatter_particle_material.tres");
            var _pm = BSInstance.ProcessMaterial;
            AddChild(BSInstance);
            BSInstance.SetDeferred("emitting", true);
        }
        private void PlayerDamageTaken()
        {
            var BSInstance = (Particles2D)damageIndicator.Instance();
            BSInstance.ProcessMaterial = Load<Material>("res://actors/particles/player_bloodsplatter_particle_material.tres");
            var _pm = BSInstance.ProcessMaterial;
            AddChild(BSInstance);
            BSInstance.SetDeferred("emitting", true);
        }
        // Handle Signals
        private void _on_Entity_Hurtbox_InvincibilityEnded()
        {
            OnHitAnimation.Play("RESET");
        }
        private void _on_Entity_Hurtbox_InvincibilityStarted()
        {
            OnHitAnimation.Play("Start");
        }
        private void _on_Hurtbox_area_entered(Area2D area)
        {
            if (!HurtBox.Invincible)
            {
                switch (ChosenType)
                {
                    // Enemy
                    case EntityType.ENEMY:
                        EnemyDamageTaken();
                        break;
                    // Player
                    case EntityType.PLAYER:
                        PlayerDamageTaken();
                        break;
                }
                TakeDamage(1);
                HurtBox.StartInvincibility(0.5F);
            }
        }
        public void _on_Entity_died()
        {
            Death();
        }
    }
}