using Godot;
using System;

namespace topdownGame
{
    public class Main : Node
    {
        [Export]
        private bool development = false;
        public override void _Ready()
        {
            if (!development)
            {
                RandomNumberGenerator rng = new RandomNumberGenerator();
                rng.Randomize();
            }

        }
    }
}
