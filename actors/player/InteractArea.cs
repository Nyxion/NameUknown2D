using Godot;

namespace TopDownGame
{
    public class InteractArea : Area2D
    {
        [Signal] delegate void player_in_area();
        [Signal] delegate void player_not_in_area();

        private void _on_InteractArea_area_entered(Area2D area)
        {
            if (area.Name.Contains("Chest"))
            {
                EmitSignal("player_in_area", area, 1);
            }
        }

        private void _on_InteractArea_area_exited(Area2D area)
        {
            if (area.Name.Contains("Chest"))
            {
                EmitSignal("player_not_in_area", null, 0);
            }
        }
    }
}
