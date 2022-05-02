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
        private List<TextureRect> heartArray = new List<TextureRect>();
        public override void _Ready()
        {
            nodePlayer = GetTree().Root.GetNode<Entity>("World/YSort/Player");
            player = (Entity)nodePlayer;
            InitHeartsUI();
        }
        public void SpawnCompleted(Entity _player)
        {
            player = _player;
            InitHeartsUI();
        }
        private void InitHeartsUI()
        {
            player.Connect("hp_changed", this, "SetHearts");
            player.PlayerSpawned += SpawnCompleted;

            if (heartArray.Count >= 1) heartArray.Clear();

            for (int i = 0; i < player.Health; i++)
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
            if (heartArray.Count == player.Health) return;
            TextureRect lastElement = heartArray[heartArray.Count - 1];
            String nameOfLast = lastElement.Name.ToLower();

            if (player.Health > value)
            {
                GD.Print("Lost a health point " + value + " - current: " + player.Health);
                ModifyHealthUI(removing: true);
            }
            else
            {
                GD.Print("Gained a health point");
                ModifyHealthUI();
            }
        }
        private void ModifyHealthUI(bool removing = false)
        {
            TextureRect lastElement = heartArray[heartArray.Count - 1];
            Vector2 lastElementPos = lastElement.RectGlobalPosition;
            String nameOfLast = lastElement.Name.ToLower();

            if (nameOfLast.Contains("fullheart") && removing)
            {
                TextureRect tempHalfHeart = (TextureRect)halfHeart.Instance();
                tempHalfHeart.SetGlobalPosition(lastElement.RectGlobalPosition);
                RemoveChild(lastElement);
                AddChild(tempHalfHeart);
                heartArray.Remove(lastElement);
                heartArray.Add(tempHalfHeart);
            }
            else if (nameOfLast.Contains("halfheart") && removing)
            {
                RemoveChild(lastElement);
                heartArray.Remove(lastElement);
            }
            else if (nameOfLast.Contains("halfheart") && !removing)
            {
                RemoveChild(lastElement);
                heartArray.Remove(lastElement);

                TextureRect tempFullHeart = (TextureRect)fullHeart.Instance();
                tempFullHeart.SetGlobalPosition(new Vector2(lastElementPos.x, lastElementPos.y));
                AddChild(tempFullHeart);
                heartArray.Add(tempFullHeart);
            }
            else if (nameOfLast.Contains("fullheart") && !removing)
            {
                TextureRect tempHalfHeart = (TextureRect)halfHeart.Instance();
                tempHalfHeart.SetGlobalPosition(new Vector2(lastElementPos.x + 16, lastElementPos.y));
                AddChild(tempHalfHeart);
                heartArray.Add(tempHalfHeart);
            }
        }
    }
}
