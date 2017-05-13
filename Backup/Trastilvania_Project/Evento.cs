using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trastilvania_Project
{
    public class Evento
    {
        public double Time_of_reaction;
        Type Hole = typeof(Hueco);

        public int[][] move_up(int[][] World_Data, Evento[] Event_List, Agente wobot, bool push_trast, double Lambda_Exp, double Lambda_Huecos)
        {
            Distribution dist = new Distribution();
            Random Uniform_Distribution = new Random();
            int Hole_X_Pos, Hole_Y_Pos = 0;
            if (push_trast == true)
            {
                if (World_Data[wobot.Ypos - 2][wobot.Xpos] == 0)//Mover el traste a casilla vacía.
                {
                    World_Data[wobot.Ypos][wobot.Xpos] = 0;
                    World_Data[wobot.Ypos - 1][wobot.Xpos] = 8;
                    World_Data[wobot.Ypos - 2][wobot.Xpos] = 1;
                    //wobot.prev_signal_intensity = wobot.signal_intensity;
                    wobot.Old_Xpos = wobot.Xpos;
                    wobot.Old_Ypos = wobot.Ypos;
                    wobot.Ypos -= 1; // wobot.Ypos = wobot.Ypos - 1;
                    wobot.get_neighborhood(World_Data, wobot.Xpos, wobot.Ypos);
                    wobot.get_signal_intensity(wobot.cobertura, World_Data, wobot.Xpos, wobot.Ypos);
                    wobot.Time_of_reaction = dist.nextExponential(Lambda_Exp);
                }
                else
                {
                    if (World_Data[wobot.Ypos - 2][wobot.Xpos] == 2)//Mover el traste al hueco.
                    {
                        World_Data[wobot.Ypos][wobot.Xpos] = 0;
                        World_Data[wobot.Ypos - 1][wobot.Xpos] = 4;
                        //World_Data[wobot.Ypos - 2][wobot.Xpos] = 0;
                        //Tratamiento de huecos
                        Hole_X_Pos = Uniform_Distribution.Next(41);
                        Hole_Y_Pos = Uniform_Distribution.Next(22);
                        while (World_Data[Hole_Y_Pos][Hole_X_Pos] != 0)
                        {
                            Hole_X_Pos = Uniform_Distribution.Next(41);
                            Hole_Y_Pos = Uniform_Distribution.Next(22);
                        }
//Aquí asigno el entero 20 para luego localizarlo en el form y poder pintar el hueco.
                        World_Data[Hole_Y_Pos][Hole_X_Pos] = 20;
//Actualizar las coordenadas del objeto hueco de Event_List.
                        Hole_Restore(World_Data, Event_List, wobot.Xpos, wobot.Ypos - 2, Lambda_Huecos);
                        World_Data[wobot.Ypos - 2][wobot.Xpos] = 0;
                        ///////////////////////////////////////////////
                        wobot.Old_Xpos = wobot.Xpos;
                        wobot.Old_Ypos = wobot.Ypos;
                        wobot.Ypos -= 1; // wobot.Ypos = wobot.Ypos - 1;
                        wobot.get_neighborhood(World_Data, wobot.Xpos, wobot.Ypos);
                        wobot.get_signal_intensity(wobot.cobertura, World_Data, wobot.Xpos, wobot.Ypos);
                        wobot.Time_of_reaction = dist.nextExponential(Lambda_Exp);
                    }
                }
            }
            else //Mover el agente a la próxima casilla vacía de arriba.
            {
                //if (World_Data[wobot.Ypos - 1][wobot.Xpos] != 0) return World_Data;
                World_Data[wobot.Ypos][wobot.Xpos] = 0;
                World_Data[wobot.Ypos - 1][wobot.Xpos] = 4;
                wobot.Old_Xpos = wobot.Xpos;
                wobot.Old_Ypos = wobot.Ypos;
                wobot.Ypos -= 1;
                wobot.get_neighborhood(World_Data, wobot.Xpos, wobot.Ypos);
                wobot.get_signal_intensity(wobot.cobertura, World_Data, wobot.Xpos, wobot.Ypos);
                wobot.Time_of_reaction = dist.nextExponential(Lambda_Exp);
            }
            wobot.Clock_sense = false;
            wobot.Clock_inverse = false;
            return World_Data;
        }

        public int[][] move_down(int[][] World_Data, Evento[] Event_List, Agente wobot, bool push_trast, double Lambda_Exp, double Lambda_Huecos)
        {
            Distribution dist = new Distribution();
            Random Uniform_Distribution = new Random();
            int Hole_X_Pos, Hole_Y_Pos = 0;
            if (push_trast == true)
            {
                if (World_Data[wobot.Ypos + 2][wobot.Xpos] == 0)//Mover el traste a casilla vacía.
                {
                    World_Data[wobot.Ypos][wobot.Xpos] = 0;
                    World_Data[wobot.Ypos + 1][wobot.Xpos] = 9;
                    World_Data[wobot.Ypos + 2][wobot.Xpos] = 1;
                    //wobot.prev_signal_intensity = wobot.signal_intensity;
                    wobot.Old_Xpos = wobot.Xpos;
                    wobot.Old_Ypos = wobot.Ypos;
                    wobot.Ypos += 1; // wobot.Ypos = wobot.Ypos - 1;
                    wobot.get_neighborhood(World_Data, wobot.Xpos, wobot.Ypos);
                    wobot.get_signal_intensity(wobot.cobertura, World_Data, wobot.Xpos, wobot.Ypos);
                    wobot.Time_of_reaction = dist.nextExponential(Lambda_Exp);
                }
                else
                {
                    if (World_Data[wobot.Ypos + 2][wobot.Xpos] == 2)//Mover el traste al hueco.
                    {
                        World_Data[wobot.Ypos][wobot.Xpos] = 0;
                        World_Data[wobot.Ypos + 1][wobot.Xpos] = 4;
                        //World_Data[wobot.Ypos + 2][wobot.Xpos] = 0;
                        //Tratamiento de huecos
                        Hole_X_Pos = Uniform_Distribution.Next(41);
                        Hole_Y_Pos = Uniform_Distribution.Next(22);
                        while (World_Data[Hole_Y_Pos][Hole_X_Pos] != 0)
                        {
                            Hole_X_Pos = Uniform_Distribution.Next(41);
                            Hole_Y_Pos = Uniform_Distribution.Next(22);
                        }
                        //Aquí asigno el entero 20 para luego localizarlo en el form y poder pintar el hueco.
                        World_Data[Hole_Y_Pos][Hole_X_Pos] = 20;
                        //Actualizar las coordenadas del objeto hueco de Event_List.
                        Hole_Restore(World_Data, Event_List, wobot.Xpos, wobot.Ypos + 2, Lambda_Huecos);
                        World_Data[wobot.Ypos + 2][wobot.Xpos] = 0;
                        ///////////////////////////////////////////////
                        wobot.Old_Xpos = wobot.Xpos;
                        wobot.Old_Ypos = wobot.Ypos;
                        wobot.Ypos += 1; // wobot.Ypos = wobot.Ypos - 1;
                        wobot.get_neighborhood(World_Data, wobot.Xpos, wobot.Ypos);
                        wobot.get_signal_intensity(wobot.cobertura, World_Data, wobot.Xpos, wobot.Ypos);
                        wobot.Time_of_reaction = dist.nextExponential(Lambda_Exp);
                    }
                }
            }
            else //Mover el agente a la próxima casilla vacía de abajo.
            {
                //if (World_Data[wobot.Ypos + 1][wobot.Xpos] != 0) return World_Data;
                World_Data[wobot.Ypos][wobot.Xpos] = 0;
                World_Data[wobot.Ypos + 1][wobot.Xpos] = 4;
                wobot.Old_Xpos = wobot.Xpos;
                wobot.Old_Ypos = wobot.Ypos;
                wobot.Ypos += 1;
                wobot.get_neighborhood(World_Data, wobot.Xpos, wobot.Ypos);
                wobot.get_signal_intensity(wobot.cobertura, World_Data, wobot.Xpos, wobot.Ypos);
                wobot.Time_of_reaction = dist.nextExponential(Lambda_Exp);
            }
            wobot.Clock_sense = false;
            wobot.Clock_inverse = false;
            return World_Data;
        }

        public int[][] move_left(int[][] World_Data, Evento[] Event_List, Agente wobot, bool push_trast, double Lambda_Exp, double Lambda_Huecos)
        {
            Distribution dist = new Distribution();
            Random Uniform_Distribution = new Random();
            int Hole_X_Pos, Hole_Y_Pos = 0;
            if (push_trast == true)
            {
                if (World_Data[wobot.Ypos][wobot.Xpos - 2] == 0)//Mover el traste a casilla vacía.
                {
                    World_Data[wobot.Ypos][wobot.Xpos] = 0;
                    World_Data[wobot.Ypos][wobot.Xpos - 1] = 10;
                    World_Data[wobot.Ypos][wobot.Xpos - 2] = 1;
                    //wobot.prev_signal_intensity = wobot.signal_intensity;
                    wobot.Old_Xpos = wobot.Xpos;
                    wobot.Old_Ypos = wobot.Ypos;
                    wobot.Xpos -= 1; // wobot.Ypos = wobot.Ypos - 1;
                    wobot.get_neighborhood(World_Data, wobot.Xpos, wobot.Ypos);
                    wobot.get_signal_intensity(wobot.cobertura, World_Data, wobot.Xpos, wobot.Ypos);
                    wobot.Time_of_reaction = dist.nextExponential(Lambda_Exp);
                }
                else
                {
                    if (World_Data[wobot.Ypos][wobot.Xpos - 2] == 2)//Mover el traste al hueco.
                    {
                        World_Data[wobot.Ypos][wobot.Xpos] = 0;
                        World_Data[wobot.Ypos][wobot.Xpos - 1] = 6;
                        //World_Data[wobot.Ypos][wobot.Xpos - 2] = 0;
                        //Tratamiento de huecos
                        Hole_X_Pos = Uniform_Distribution.Next(41);
                        Hole_Y_Pos = Uniform_Distribution.Next(22);
                        while (World_Data[Hole_Y_Pos][Hole_X_Pos] != 0)
                        {
                            Hole_X_Pos = Uniform_Distribution.Next(41);
                            Hole_Y_Pos = Uniform_Distribution.Next(22);
                        }
                        //Aquí asigno el entero 20 para luego localizarlo en el form y poder pintar el hueco.
                        World_Data[Hole_Y_Pos][Hole_X_Pos] = 20;
                        //Actualizar las coordenadas del objeto hueco de Event_List.
                        Hole_Restore(World_Data, Event_List, wobot.Xpos - 2, wobot.Ypos, Lambda_Huecos);
                        World_Data[wobot.Ypos][wobot.Xpos - 2] = 0;
                        ///////////////////////////////////////////////
                        wobot.Old_Xpos = wobot.Xpos;
                        wobot.Old_Ypos = wobot.Ypos;
                        wobot.Xpos -= 1; // wobot.Ypos = wobot.Ypos - 1;
                        wobot.get_neighborhood(World_Data, wobot.Xpos, wobot.Ypos);
                        wobot.get_signal_intensity(wobot.cobertura, World_Data, wobot.Xpos, wobot.Ypos);
                        wobot.Time_of_reaction = dist.nextExponential(Lambda_Exp);
                    }
                }
            }
            else //Mover el agente a la próxima casilla vacía de arriba.
            {
                //if (World_Data[wobot.Ypos - 1][wobot.Xpos] != 0) return World_Data;
                World_Data[wobot.Ypos][wobot.Xpos] = 0;
                World_Data[wobot.Ypos][wobot.Xpos - 1] = 6;
                wobot.Old_Xpos = wobot.Xpos;
                wobot.Old_Ypos = wobot.Ypos;
                wobot.Xpos -= 1;
                wobot.get_neighborhood(World_Data, wobot.Xpos, wobot.Ypos);
                wobot.get_signal_intensity(wobot.cobertura, World_Data, wobot.Xpos, wobot.Ypos);
                wobot.Time_of_reaction = dist.nextExponential(Lambda_Exp);
            }
            wobot.Clock_sense = false;
            wobot.Clock_inverse = false;
            return World_Data;
        }

        public int[][] move_right(int[][] World_Data, Evento[] Event_List, Agente wobot, bool push_trast, double Lambda_Exp, double Lambda_Huecos)
        {
            Distribution dist = new Distribution();
            Random Uniform_Distribution = new Random();
            int Hole_X_Pos, Hole_Y_Pos = 0;
            if (push_trast == true)
            {
                if (World_Data[wobot.Ypos][wobot.Xpos + 2] == 0)//Mover el traste a casilla vacía.
                {
                    World_Data[wobot.Ypos][wobot.Xpos] = 0;
                    World_Data[wobot.Ypos][wobot.Xpos + 1] = 11;
                    World_Data[wobot.Ypos][wobot.Xpos + 2] = 1;
                    //wobot.prev_signal_intensity = wobot.signal_intensity;
                    wobot.Old_Xpos = wobot.Xpos;
                    wobot.Old_Ypos = wobot.Ypos;
                    wobot.Xpos += 1; // wobot.Ypos = wobot.Ypos - 1;
                    wobot.get_neighborhood(World_Data, wobot.Xpos, wobot.Ypos);
                    wobot.get_signal_intensity(wobot.cobertura, World_Data, wobot.Xpos, wobot.Ypos);
                    wobot.Time_of_reaction = dist.nextExponential(Lambda_Exp);
                }
                else
                {
                    if (World_Data[wobot.Ypos][wobot.Xpos + 2] == 2)//Mover el traste al hueco.
                    {
                        World_Data[wobot.Ypos][wobot.Xpos] = 0;
                        World_Data[wobot.Ypos][wobot.Xpos + 1] = 7;
                        //World_Data[wobot.Ypos][wobot.Xpos + 2] = 0;
                        //Tratamiento de huecos
                        Hole_X_Pos = Uniform_Distribution.Next(41);
                        Hole_Y_Pos = Uniform_Distribution.Next(22);
                        while (World_Data[Hole_Y_Pos][Hole_X_Pos] != 0)
                        {
                            Hole_X_Pos = Uniform_Distribution.Next(41);
                            Hole_Y_Pos = Uniform_Distribution.Next(22);
                        }
                        //Aquí asigno el entero 20 para luego localizarlo en el form y poder pintar el hueco.
                        World_Data[Hole_Y_Pos][Hole_X_Pos] = 20;
                        //Actualizar las coordenadas del objeto hueco de Event_List.
                        Hole_Restore(World_Data, Event_List, wobot.Xpos + 2, wobot.Ypos, Lambda_Huecos);
                        World_Data[wobot.Ypos][wobot.Xpos + 2] = 0;
                        ///////////////////////////////////////////////
                        wobot.Old_Xpos = wobot.Xpos;
                        wobot.Old_Ypos = wobot.Ypos;
                        wobot.Xpos += 1; // wobot.Ypos = wobot.Ypos - 1;
                        wobot.get_neighborhood(World_Data, wobot.Xpos, wobot.Ypos);
                        wobot.get_signal_intensity(wobot.cobertura, World_Data, wobot.Xpos, wobot.Ypos);
                        wobot.Time_of_reaction = dist.nextExponential(Lambda_Exp);
                    }
                }
            }
            else //Mover el agente a la próxima casilla vacía de arriba.
            {
                //if (World_Data[wobot.Ypos - 1][wobot.Xpos] != 0) return World_Data;
                World_Data[wobot.Ypos][wobot.Xpos] = 0;
                World_Data[wobot.Ypos][wobot.Xpos + 1] = 7;
                wobot.Old_Xpos = wobot.Xpos;
                wobot.Old_Ypos = wobot.Ypos;
                wobot.Xpos += 1;
                wobot.get_neighborhood(World_Data, wobot.Xpos, wobot.Ypos);
                wobot.get_signal_intensity(wobot.cobertura, World_Data, wobot.Xpos, wobot.Ypos);
                wobot.Time_of_reaction = dist.nextExponential(Lambda_Exp);
            }
            wobot.Clock_sense = false;
            wobot.Clock_inverse = false;
            return World_Data;
        }

        public int[][] turn_left(int[][] World_Data, Agente wobot, double Lambda_Exp)
        {
            Distribution dist = new Distribution();
            //wobot.orientation = 2;
            World_Data[wobot.Ypos][wobot.Xpos] = 6;
            wobot.Time_of_reaction = dist.nextExponential(Lambda_Exp);
            switch (wobot.orientation)
            {
                case(0)://Agente orientado hacia arriba
                    wobot.Clock_sense = false;
                    wobot.Clock_inverse = true;
                    wobot.orientation = 2;
                    break;
                case(1)://Agente orientado hacia abajo
                    wobot.Clock_sense = true;
                    wobot.Clock_inverse = false;
                    wobot.orientation = 2;
                    break;
            }
            return World_Data;
        }

        public int[][] turn_rigth(int[][] World_Data, Agente wobot, double Lambda_Exp)
        {
            Distribution dist = new Distribution();
            //wobot.orientation = 3;
            World_Data[wobot.Ypos][wobot.Xpos] = 7;
            wobot.Time_of_reaction = dist.nextExponential(Lambda_Exp);
            switch (wobot.orientation)
            {
                case (0)://Agente orientado hacia arriba
                    wobot.Clock_sense = true;
                    wobot.Clock_inverse = false;
                    wobot.orientation = 3;
                    break;
                case (1)://Agente orientado hacia abajo
                    wobot.Clock_sense = false;
                    wobot.Clock_inverse = true;
                    wobot.orientation = 3;
                    break;
            }
            return World_Data;
        }

        public int[][] turn_up(int[][] World_Data, Agente wobot, double Lambda_Exp)
        {
            Distribution dist = new Distribution();
            //wobot.orientation = 2;
            World_Data[wobot.Ypos][wobot.Xpos] = 4;
            wobot.Time_of_reaction = dist.nextExponential(Lambda_Exp);
            switch (wobot.orientation)
            {
                case (2)://Agente orientado hacia la izquierda
                    wobot.Clock_sense = true;
                    wobot.Clock_inverse = false;
                    wobot.orientation = 0;
                    break;
                case (3)://Agente orientado hacia la derecha
                    wobot.Clock_sense = false;
                    wobot.Clock_inverse = true;
                    wobot.orientation = 0;
                    break;
            }
            return World_Data;
        }

        public int[][] turn_down(int[][] World_Data, Agente wobot, double Lambda_Exp)
        {
            Distribution dist = new Distribution();
            //wobot.orientation = 2;
            World_Data[wobot.Ypos][wobot.Xpos] = 5;
            wobot.Time_of_reaction = dist.nextExponential(Lambda_Exp);
            switch (wobot.orientation)
            {
                case (2)://Agente orientado hacia la izquierda
                    wobot.Clock_sense = true;
                    wobot.Clock_inverse = false;
                    wobot.orientation = 1;
                    break;
                case (3)://Agente orientado hacia la derecha
                    wobot.Clock_sense = false;
                    wobot.Clock_inverse = true;
                    wobot.orientation = 1;
                    break;
            }
            return World_Data;
        }

        public bool detect_pushing_trast(int[][] World_Data, int Ycord_Trast, int Xcord_Trast, double Agent_pos)
        {//Agent_pos se usa para ignorar la casilla del agente actual.
            if ((Ycord_Trast > 0) && (Agent_pos != 0))
            {
                if (World_Data[Ycord_Trast - 1][Xcord_Trast] == 9) return true;//Lo están empujando hacia abajo.
            }
            if ((Ycord_Trast < World_Data.Length - 1) && (Agent_pos != 2))
            {
                if (World_Data[Ycord_Trast + 1][Xcord_Trast] == 8) return true;//Lo están empujando hacia arriba.
            }
            if ((Xcord_Trast > 0) && (Agent_pos != 3))
            {
                if (World_Data[Ycord_Trast][Xcord_Trast - 1] == 11) return true;//Lo están empujando hacia la derecha.
            }
            if ((Xcord_Trast < World_Data[0].Length - 1) && (Agent_pos != 1))
            {
                if (World_Data[Ycord_Trast][Xcord_Trast + 1] == 10) return true;//Lo están empujando hacia la izquierda.
            }
            return false;
        }

        public int[][] Agent_Restore(int[][] World_Data, Agente wobot, double Lambda_Exp)
        {
            Random Uniform_Distribution = new Random();                 
            Distribution Builder = new Distribution();
            World_Data[wobot.Ypos][wobot.Xpos] = 0;// LÍNEA IMPORTANTÍSIMA
            int Xcord, Ycord, orientation = 0;
            Xcord = Uniform_Distribution.Next(40); //El 21 y el 40 son los índices de la matrix del mundo.
            Ycord = Uniform_Distribution.Next(21);
            orientation = Uniform_Distribution.Next(4);
            while (World_Data[Ycord][Xcord] != 0)
            {
                Xcord = Uniform_Distribution.Next(40);
                Ycord = Uniform_Distribution.Next(21);
                orientation = Uniform_Distribution.Next(4);
            }
            wobot.Old_Xpos = 0;
            wobot.Old_Ypos = 0;
            wobot.prev_signal_intensity = 0;
            wobot.Xpos = Xcord;
            wobot.Ypos = Ycord;
            wobot.orientation = orientation;
            wobot.get_neighborhood(World_Data, wobot.Xpos, wobot.Ypos);
            wobot.get_signal_intensity(wobot.cobertura, World_Data, wobot.Xpos, wobot.Ypos);
            wobot.Time_of_reaction = Builder.nextExponential(Lambda_Exp);
            switch (orientation)
            {
                case (0):
                    World_Data[Ycord][Xcord] = 4;
                    break;
                case (1):
                    World_Data[Ycord][Xcord] = 5;
                    break;
                case (2):
                    World_Data[Ycord][Xcord] = 6;
                    break;
                case (3):
                    World_Data[Ycord][Xcord] = 7;
                    break;
            }
            wobot.Clock_sense = false;
            wobot.Clock_inverse = false;
            return World_Data;
        }

        public void Hole_Restore(int[][] World_Data, Evento[] Event_List, int Xpos, int Ypos, double Lambda_Huecos)
        {
            Distribution dist = new Distribution();
            for (int c = 0; c < Event_List.Length; c++)
            {
                if (Event_List[c].GetType() == Hole)
                {
                    Hueco hole = (Hueco)Event_List[c];
                    if ((hole.Xpos == Xpos) && (hole.Ypos == Ypos))
                    {
                        for (int i = 0; i < World_Data.Length; i++)
                        {
                            for (int k = 0; k < World_Data[0].Length; k++)
                            {
                                if (World_Data[i][k] == 20)
                                {
                                    hole.Xpos = k;
                                    hole.Ypos = i;
                                    hole.Time_of_reaction = dist.nextExponential(Lambda_Huecos);
                                    break;
                                }
                            }
                        }
                        break;
                    }
                }
            }
        }

        public int[][] Hole_Moving(int[][] World_Data, Hueco hole)
        {
            Random Uniform_Distribution = new Random();
            int Hole_X_Pos, Hole_Y_Pos = 0;

            Hole_X_Pos = Uniform_Distribution.Next(41);
            Hole_Y_Pos = Uniform_Distribution.Next(22);
            while (World_Data[Hole_Y_Pos][Hole_X_Pos] != 0)
            {
                Hole_X_Pos = Uniform_Distribution.Next(41);
                Hole_Y_Pos = Uniform_Distribution.Next(22);
            }
            World_Data[Hole_Y_Pos][Hole_X_Pos] = 2;
            hole.Xpos = Hole_X_Pos;
            hole.Ypos = Hole_Y_Pos;
            return World_Data;
        }

        public int[] Trast_show(int[][] World_Data)
        {
            Random Uniform_Distribution = new Random();
            int Trast_X_Pos, Trast_Y_Pos = 0;
            int[] Cord = new int[2];

            Trast_X_Pos = Uniform_Distribution.Next(41);
            Trast_Y_Pos = Uniform_Distribution.Next(22);
            while (World_Data[Trast_Y_Pos][Trast_X_Pos] != 0)
            {
                Trast_X_Pos = Uniform_Distribution.Next(41);
                Trast_Y_Pos = Uniform_Distribution.Next(22);
            }
            World_Data[Trast_Y_Pos][Trast_X_Pos] = 1;
            Cord[1] = Trast_X_Pos;
            Cord[0] = Trast_Y_Pos;
            return Cord;
        }
    }
}
