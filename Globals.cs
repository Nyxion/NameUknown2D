using Godot;
using System;

namespace TopDownGame
{
    public class Globals : Node
    {
        private Vector2 _playerStartPos;
        public Vector2 PlayerStartPos
        {
            get => _playerStartPos;
            set
            {
                _playerStartPos = value;
            }
        }
        public override void _Ready()
        {

        }

        public void LoadNewScene(Path scenePath)
        {

        }
    }

}