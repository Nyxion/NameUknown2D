using System.Collections.Generic;
using Godot;
using System;

namespace TopDownGame.UI
{
    public class HealthUI : Control
    {
        private PackedScene emptyHeart = ResourceLoader.Load<PackedScene>("res://UI/EmptyHeart.tscn");
        private PackedScene halfHeart = ResourceLoader.Load<PackedScene>("res://UI/HalfHeart.tscn");
        private PackedScene fullHeart = ResourceLoader.Load<PackedScene>("res://UI/FullHeart.tscn");
        private Node nodePlayer;
        private Entity player;
        private int maxHearts = 0;
        private int hearts = 0;
        private List<TextureRect> heartArray = new List<TextureRect>();
        public override void _Ready()
        {
            nodePlayer = GetTree().Root.GetNode<Entity>("World/YSort/Player");
            player = (Entity)nodePlayer;
            player.Connect("hp_changed", this, "SetHearts");
            player.Connect("max_hp_changed", this, "SetMaxHearts");
            player.PlayerSpawned += SpawnCompleted;
            InitHearts();
        }
        public void SpawnCompleted(Entity _player)
        {
            _player.Connect("hp_changed", this, "SetHearts");
            _player.PlayerSpawned += SpawnCompleted;
            player = _player;

            InitHearts();
        }
        private void InitHearts()
        {
            if (heartArray.Count >= 1) heartArray.Clear();

            hearts = player.Health;

            for (int i = 0; i < maxHearts; i++)
            {
                if (i % 2 == 0)
                {
                    var tempHeart = (TextureRect)fullHeart.Instance();
                    if (heartArray.Count == 0)
                    {
                        tempHeart.SetGlobalPosition(RectGlobalPosition);
                        AddChild(tempHeart);
                        heartArray.Add(tempHeart);
                    }
                    else if (heartArray.Count >= 1)
                    {
                        var lastAdded = heartArray[heartArray.Count - 1].RectGlobalPosition;
                        tempHeart.SetGlobalPosition(new Vector2(lastAdded.x + 16, lastAdded.y));
                        AddChild(tempHeart);
                        heartArray.Add(tempHeart);
                    }
                }
            }

        }
        private void SetHearts(int value)
        {
            if (value == 0 && heartArray.Count <= 0) return;
            if (heartArray.Count == maxHearts) return;
            TextureRect lastElement = heartArray[heartArray.Count - 1];
            String nameOfLast = lastElement.Name.ToLower();

            if (hearts > value)
            {
                if (nameOfLast.Contains("fullheart"))
                {
                    var tempHalfHeart = (TextureRect)halfHeart.Instance();
                    tempHalfHeart.SetGlobalPosition(lastElement.RectGlobalPosition);
                    RemoveChild(lastElement);
                    AddChild(tempHalfHeart);
                    heartArray.Remove(lastElement);
                    heartArray.Add(tempHalfHeart);
                }
                else if (nameOfLast.Contains("halfheart"))
                {
                    RemoveChild(lastElement);
                    heartArray.Remove(lastElement);
                }
            }
            else
            {
                GD.Print("Add half a heart.");
            }
            hearts = value;
        }
        private void SetMaxHearts(int value)
        {
            maxHearts = value;
            if (maxHearts % 2 == 0) InitHearts();
        }
    }
}
