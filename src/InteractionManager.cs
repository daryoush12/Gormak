using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projecttamasuccessor
{
    public partial class InteractionManager: Area2D
    {
        public void _on_area_2d_body_entered(Node2D body)
        {
            if (body.IsInGroup("Player"))
            {
                GD.Print("Player entered interaction area");
                // Handle interaction logic here
            }
        }
    }
}
