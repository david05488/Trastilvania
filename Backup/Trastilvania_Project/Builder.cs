using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trastilvania_Project
{
    public class Builder
    {
        public Agente[] Agent_list;
        public Hueco[] Hueco_list;
        public Builder()
        {
        }
        public int[][] Build_Muros(int[][] Worl_Data, int Cant, int Lambda_Muros_Horizontales, int Lambda_Muros_Verticales)
        {
            Random Uniform_Distribution = new Random();
            Distribution Distrib = new Distribution();
            int Xcord, Ycord, Cant_Bloques_H, Cant_Bloques_V = 0;
            for (int i = 1; i <= Cant; i++)
            {
                Xcord = Uniform_Distribution.Next(40); //El 23 y el 43 son los índices de la matrix del mundo.
                Ycord = Uniform_Distribution.Next(21);
                Worl_Data[Ycord][Xcord] = 3; //El "3" significa "muro" en la leyenda.
                Cant_Bloques_H = Distrib.next_Poisson(Lambda_Muros_Horizontales);
                Cant_Bloques_V = Distrib.next_Poisson(Lambda_Muros_Verticales);
                if ((Xcord.Equals(Ycord))&&(Xcord==0))
                {
                    //Cant_Bloques = Builder.next_Poisson(Lambda_Muros);
                    for (int k = 0; k <= Cant_Bloques_V; k++)
                    {
                        if (Ycord + k > 21) break;
                        Worl_Data[Ycord + k][Xcord] = 3;
                    }
                }
                else
                {
                    //if (((Xcord / (Xcord + Ycord)) >= (Ycord / (Xcord + Ycord))))
                    if(Xcord<=Ycord)
                    {
                        //Cant_Bloques = Builder.next_Poisson(Lambda_Muros);
                        for (int k = 0; k <= Cant_Bloques_H; k++)
                        {
                            if (Xcord + k > 40) break;
                            Worl_Data[Ycord][Xcord + k] = 3;
                        }
                    }
                    else
                    {
                        //Cant_Bloques = Builder.next_Poisson(Lambda_Muros);
                        for (int k = 0; k <= Cant_Bloques_V; k++)
                        {
                            if (Ycord + k > 21) break;
                            Worl_Data[Ycord + k][Xcord] = 3;
                        }
                    }
                }
            }
            return Worl_Data;
        }

        public int[][] Build_Huecos(int[][] World_Data, int Cant, double Lambda_Huecos)
        {
            Hueco_list = new Hueco[Cant];
            double Time_of_existence;
            Random Uniform_Distribution = new Random();
            Distribution Builder = new Distribution();
            int Xcord, Ycord = 0;
            for (int i = 1; i <= Cant; i++)
            {
                Xcord = Uniform_Distribution.Next(40); //El 23 y el 43 son los índices de la matrix del mundo.
                Ycord = Uniform_Distribution.Next(21);
                while (World_Data[Ycord][Xcord] != 0)
                {
                    Xcord = Uniform_Distribution.Next(40); 
                    Ycord = Uniform_Distribution.Next(21);
                }
                Time_of_existence = Builder.nextExponential(Lambda_Huecos);
                Hueco_list[i - 1] = new Hueco(Xcord, Ycord, Time_of_existence, i);
                World_Data[Ycord][Xcord] = 2;
            }
            return World_Data;
        }

        public int[][] Build_Trastes(int[][] World_Data, int Cant)
        {
            Random Uniform_Distribution = new Random();
            Distribution Builder = new Distribution();
            int Xcord, Ycord = 0;
            for (int i = 1; i <= Cant; i++)
            {
                Xcord = Uniform_Distribution.Next(40); //El 21 y el 40 son los índices de la matrix del mundo.
                Ycord = Uniform_Distribution.Next(21);
                while (World_Data[Ycord][Xcord] != 0)
                {
                    Xcord = Uniform_Distribution.Next(40);
                    Ycord = Uniform_Distribution.Next(21);
                }
                World_Data[Ycord][Xcord] = 1;
            }
            return World_Data;
        }

        public int[][] Build_Wobots(int[][] World_Data, int Cant, int cobertura)
        {
            Agent_list = new Agente[Cant];                              //Leyenda (orientation):
            int orientation = 0;
            Random Uniform_Distribution = new Random();                 //0- A_Arriba.
            Distribution Builder = new Distribution();                  //1- A_Abajo.
            int Xcord, Ycord = 0;                                       //2- A_Izquierda.
            for (int i = 1; i <= Cant; i++)                             //3- A_Derecha.
            {
                Xcord = Uniform_Distribution.Next(40); //El 23 y el 43 son los índices de la matrix del mundo.
                Ycord = Uniform_Distribution.Next(21);
                while (World_Data[Ycord][Xcord] != 0)
                {
                    Xcord = Uniform_Distribution.Next(40);
                    Ycord = Uniform_Distribution.Next(21);
                    orientation = Uniform_Distribution.Next(4);
                }
                Agent_list[i - 1] = new Agente(Xcord, Ycord, i, cobertura, orientation);
                switch (orientation)
                {
                    case(0):
                        World_Data[Ycord][Xcord] = 4;
                        break;
                    case(1):
                        World_Data[Ycord][Xcord] = 5;
                        break;
                    case(2):
                        World_Data[Ycord][Xcord] = 6;
                        break;
                    case(3):
                        World_Data[Ycord][Xcord] = 7;
                        break;
                }
                
            }
            return World_Data;
        }
    }
}
