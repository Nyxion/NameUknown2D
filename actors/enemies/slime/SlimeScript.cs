using Godot;
using System;
using static Godot.GD;

namespace TopDownGame.Characters
{
    public class SlimeScript : Entity
    {
        private AnimatedSprite Sprite;
        private PlayerDetectionZone ObjectDetectionZone;
        private SoftCollision Bumping;
        private WanderingController WanderingController;
        public override void _Ready()
        {
            Sprite = GetNode<AnimatedSprite>("Sprite");
            ObjectDetectionZone = GetNode<PlayerDetectionZone>("ObjectDetectionZone");
            Bumping = GetNode<SoftCollision>("SoftCollision");
            WanderingController = GetNode<WanderingController>("WanderingController");

            HurtBox = GetNode<Hurtbox>("Hurtbox");
            OnHitAnimation = GetNode<AnimationPlayer>("OnHitBlinkAnimationPlayer");

            Sprite.Playing = true;
            CurrentState = EntityState.WANDER;
            ChosenType = EntityType.ENEMY;
        }
        public override void _PhysicsProcess(float delta)
        {
            if (Bumping.IsColliding())
            {
                Velocity += Bumping.GetPushVector() * delta * 400;
            }
            base._PhysicsProcess(delta);
            Velocity = MoveAndSlide(Velocity);
        }
        public override void _IdleState(float _delta)
        {
            Velocity = Velocity.MoveToward(Vector2.Zero, Friction * _delta);
            DoSomething();
        }
        public override void _ChaseState(float _delta)
        {
            var player = (KinematicBody2D)ObjectDetectionZone.player;
            if (player != null)
            {
                AccelTowardPoint(player.GlobalPosition, _delta);
            }
            else
            {
                CurrentState = EntityState.IDLE;
            }
        }
        public override void _WanderState(float _delta)
        {
            DoSomething();
            AccelTowardPoint(WanderingController.TargetPos, _delta);
            if (GlobalPosition.DistanceTo(WanderingController.TargetPos) <= 4)
            {
                UpdateStateTimer();
            }
        }
        private void AccelTowardPoint(Vector2 point, float delta)
        {
            var dir = GlobalPosition.DirectionTo(point);
            Velocity = Velocity.MoveToward(dir * MaxSpeed, Acceleration * delta);
            Sprite.FlipH = Velocity.x < 0;
        }
        private void DoSomething()
        {
            FindPlayer();
            if (WanderingController.TimeLeft() == 0)
            {
                UpdateStateTimer();
            }
        }
        private void UpdateStateTimer()
        {
            CurrentState = PickRandomState();
            WanderingController.StartTimer(new RandomNumberGenerator().RandfRange(1, 3));
        }
        private EntityState PickRandomState()
        {
            // We only pick between IDLE and WANDER for Slime
            Array values = Enum.GetValues(typeof(EntityState));
            Random random = new Random();
            EntityState randomState = (EntityState)values.GetValue(random.Next(0, 2));
            return randomState;
        }
        private void FindPlayer()
        {
            if (ObjectDetectionZone.TimeToDie())
            {
                CurrentState = EntityState.CHASE;
            }
        }
    }
}
