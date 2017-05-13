using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trastilvania_Project
{
    public class Hueco : Evento
    {
        public int Xpos, Ypos, index;
        //public double Time_of_reaction;
        public Hueco(int Xpos, int Ypos, double Time_of_existence, int index)
        {
            this.Xpos = Xpos;
            this.Ypos = Ypos;
            this.index = index;
            this.Time_of_reaction = Time_of_existence;
            //base.Time_of_reaction = Time_of_reaction;
        }
    }
}
