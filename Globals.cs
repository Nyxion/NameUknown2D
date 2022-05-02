using Godot;

namespace TopDownGame
{
    public class Globals : Node
    {
        // Maybe remove, dunno if i wanna do a global player or not
        // for respawning and shit.
        private Entity _player;
        public Entity Player
        {
            get => _player;
            set
            {
                _player = value;
            }
        }
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
            Node nodePlayer = GetTree().Root.GetNode<Entity>("World/YSort/Player");
            _player = (Entity)nodePlayer;
            _player.PlayerSpawned += NewPlayer;
        }

        private void NewPlayer(Entity _player)
        {
            Player = _player;
            Player.PlayerSpawned += NewPlayer;
        }

        public void LoadNewScene(Path scenePath)
        {
            throw new System.NotImplementedException();
        }
    }

}