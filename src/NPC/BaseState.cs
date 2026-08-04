using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projecttamasuccessor.NPC
{
    public abstract class BaseState
    {
        public BaseState(NPCManager actor) { }  
        public abstract void OnStart();

        public abstract void OnStop();

        public abstract BaseState Tick();
    }
}
