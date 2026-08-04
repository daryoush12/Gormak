using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;

namespace projecttamasuccessor.Events
{

    public partial class EventBus : Node
    {
        [Signal] public delegate void CoinCollectedEventHandler(int score);
    }

}
