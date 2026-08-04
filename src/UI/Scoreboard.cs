using Godot;
using projecttamasuccessor.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projecttamasuccessor.UI
{
        public partial class Scoreboard : Node
        {
            [Export] private Label _scoreLabel;

            private EventBus _events;
            private int _score = 0;

            // Called when the node enters the scene tree for the first time.
            public override void _Ready()
            {
                _events = GetNode<EventBus>("/root/EventBus");
                _events.CoinCollected += UpdateScore;
            }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _events.CoinCollected -= UpdateScore;
            }
            base.Dispose(disposing);
        }

        private void UpdateScore(int amount)
        {
              _score += amount;
             _scoreLabel.Text = $"{_score}";
        }
    }
}
