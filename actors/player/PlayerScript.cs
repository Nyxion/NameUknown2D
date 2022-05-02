using Godot;
using static Godot.GD;

namespace TopDownGame.Characters
{
    public class PlayerScript : Entity
    {
        [Signal] delegate void interacted();
        private AnimationTree playerAnimationTree;
        private Weapon playerWeapon;
        private Sprite playerSprite;
        private InteractArea interactArea;
        private AnimationNodeStateMachinePlayback animationState;
        private bool _canInteract = false;
        public bool Interactable
        {
            get => _canInteract;
            set
            {
                _canInteract = !_canInteract;
            }
        }
        private Area2D currentChest;
        public override void _Ready()
        {
            playerAnimationTree = GetNode<AnimationTree>("AnimationTree");
            playerAnimationTree.Active = true;
            animationState = (AnimationNodeStateMachinePlayback)playerAnimationTree.Get("parameters/playback");

            interactArea = GetNode<InteractArea>("InteractArea");
            interactArea.Connect("player_in_area", this, "AbleToInteract");
            interactArea.Connect("player_not_in_area", this, "AbleToInteract");

            playerSprite = GetNode<Sprite>("Sprite");
            playerWeapon = playerSprite.GetNode<Weapon>("Weapon");

            HurtBox = GetNode<Hurtbox>("Hurtbox");
            OnHitAnimation = GetNode<AnimationPlayer>("OnHitBlinkAnimationPlayer");

            Globals = GetNode<Globals>("/root/Globals");
            Globals.PlayerStartPos = GlobalPosition;

            // Start off on move state
            CurrentState = EntityState.MOVE;
            ChosenType = EntityType.PLAYER;
        }
        public override void _Input(InputEvent @event)
        {
            if (@event.IsActionPressed("sword_attack"))
            {
                CurrentState = EntityState.ATTACK;
            }

            if (@event.IsActionPressed("interact"))
            {
                if (currentChest != null)
                {
                    EmitSignal("interacted", currentChest);
                }
                else
                {
                    GD.Print("Unable to interact with anything.");
                }

            }
        }
        public override void _MoveState(float delta)
        {
            var inputVector = Vector2.Zero;

            inputVector.x = Input.GetActionStrength("move_right") - Input.GetActionStrength("move_left");
            inputVector.y = Input.GetActionStrength("move_down") - Input.GetActionStrength("move_up");
            inputVector = inputVector.Normalized();

            if (inputVector != Vector2.Zero)
            {
                playerWeapon.KnockbackVector = inputVector;
                playerAnimationTree.Set("parameters/Idle/blend_position", inputVector);
                playerAnimationTree.Set("parameters/Walk/blend_position", inputVector);
                playerAnimationTree.Set("parameters/Attack/blend_position", inputVector);
                animationState.Travel("Walk");
                Velocity = Velocity.MoveToward(inputVector * MaxSpeed, Acceleration * delta);
            }
            else
            {
                animationState.Travel("Idle");
                Velocity = Velocity.MoveToward(Vector2.Zero, Friction * delta);
            }
            Velocity = MoveAndSlide(Velocity);
        }
        public override void _AttackState(float delta)
        {
            Velocity = Velocity.MoveToward(Vector2.Zero, (Friction * delta) / 2);
            Velocity = MoveAndSlide(Velocity);
            animationState.Travel("Attack");
        }
        public void AttackAnimationFinished()
        {
            // Move state
            CurrentState = EntityState.MOVE;
        }
        public void AbleToInteract(Area2D chest, int able)
        {
            if (able == 0 && chest == null) return;

            currentChest = chest;
        }
    }
}
