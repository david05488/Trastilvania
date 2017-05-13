using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trastilvania_Project
{
   public class Agente : Evento //Agente hereda de la clase Evento.
    {
        public int Xpos, Ypos, Old_Xpos, Old_Ypos, index, cobertura, orientation;
        public bool Clock_sense, Clock_inverse = false; //Para la prevención de ciclos.
        public int[] neighborhood=new int[8];
        public double signal_intensity, prev_signal_intensity;//, Time_of_reaction;

        public Agente(int Xpos, int Ypos, int index, int cobertura, /*int[][] World_Data,*/ int orientation)
        {
            this.Xpos = Xpos;
            this.Ypos = Ypos;
            this.index = index;
            this.cobertura = cobertura;
            this.orientation = orientation;
            //base.Time_of_reaction = Time_of_reaction; //Para asignar directamente a la clase base o padre.
            //this.get_neighborhood(World_Data, Xpos, Ypos);
        }
        public void get_neighborhood(int[][] World_Data, int Xpos, int Ypos)
        {
            //neighborhood = new int[8];
            int count = 0;
            int Xaux = Xpos;
            int Yaux = Ypos;
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            try  /////////////////////////////////////////////////////////////0 0 0//////////////////////////////////////
            {    /////////////////////////////////////////////////////////////x x x//////////////////////////////////////
                for (int i = 0; i < 3; i++) //////////////////////////////////x x x//////////////////////////////////////
                {                          
                    neighborhood[i] = World_Data[Yaux - 1][Xaux - 1];
                    count++;
                    Xaux++;
                }
                Xaux = Xpos;
                count = 4;
            }
            catch 
            {
                if (count == 0)
                {
                    try
                    {
                        neighborhood[0] = 3;
                        count++;
                        for (int i = 1; i < 3; i++)
                        {
                            neighborhood[i] = World_Data[Yaux - 1][Xaux];
                            count++;
                            Xaux++;
                        }
                        Xaux = Xpos;
                        count = 4;
                    }
                    catch (Exception)
                    {
                        for (int i = count; i < 3; i++)
                        {
                            neighborhood[i] = 3;
                        }
                        Xaux = Xpos;
                        count = 4;
                    }
                }
                else
                {
                    for (int i = count; i < 3; i++)
                    {
                        neighborhood[i] = 3;
                    }
                    Xaux = Xpos;
                    count = 4;
                }
                }
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            try /////////////////////////////////////////////////////////x x x///////////////////////////////////////////
            {   /////////////////////////////////////////////////////////x x 0///////////////////////////////////////////
                neighborhood[3] = World_Data[Ypos][Xpos + 1]; ///////////x x x///////////////////////////////////////////
            }
            catch (Exception)
            {
                neighborhood[3] = 3;
            }
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            try ////////////////////////////////////////////////////////////x x x////////////////////////////////////////
            {   ////////////////////////////////////////////////////////////x x x////////////////////////////////////////
                for (int i = 4; i < 7; i++) ////////////////////////////////0 0 0////////////////////////////////////////
                {
                    neighborhood[i] = World_Data[Yaux + 1][Xaux + 1];
                    Xaux--;
                    count++;
                }
                Xaux = Xpos;
            }
            catch (Exception)
            {
                if (count == 4)
                {
                    try
                    {
                        neighborhood[4] = 3;
                        count++;
                        for (int i = 5; i < 7; i++)
                        {
                            neighborhood[i] = World_Data[Yaux + 1][Xaux];
                            Xaux--;
                            count++;
                        }
                    }
                    catch (Exception)
                    {
                        for (int i = count; i < 7; i++)
                        {
                            neighborhood[i] = 3;
                        }
                        Xaux = Xpos;
                    }
                }
                else
                {
                    for (int k = count; k < 7; k++)
                    {
                        neighborhood[k] = 3;
                    }
                }
            }
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            try //////////////////////////////////////////////////////////x x x//////////////////////////////////////////
            {   //////////////////////////////////////////////////////////0 x x//////////////////////////////////////////
                neighborhood[7] = World_Data[Ypos][Xpos - 1]; ////////////x x x//////////////////////////////////////////
            }
            catch (Exception)
            {
                neighborhood[7] = 3;
            }
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        }
        public void get_signal_intensity(int cobertura, int[][] World_Data, int Xpos, int Ypos)
        {
            //int Xaux = Xpos;
            //int Yaux = Ypos;
            //int increment = 1;
            double signal = 0;
            int[] limits = get_limits(Xpos, Ypos, cobertura);
            for (int i = 0; i < limits.Length; i++)
            {
                switch (i)
                {
                    case(0):
                        for (int k = 1; k <= limits[i]; k++) //Hacia la izquierda
                        {
                            if ((World_Data[Ypos][Xpos - k]) == 2)
                            {
                                signal += 1 / (Math.Pow(2, k));
                            }
                            for (int n = 1; (n <= k)&&(n <= limits[2])&&(k <= limits[0]); n++)
                            {
                                if ((World_Data[Ypos - n][Xpos - k]) == 2)
                                {
                                    signal += 1 / (Math.Pow(2, k));
                                }
                            }
                            for (int n = 1; (n <= k) && (n <= limits[3]) && (k <= limits[0]); n++)
                            {
                                if ((World_Data[Ypos + n][Xpos - k]) == 2)
                                {
                                    signal += 1 / (Math.Pow(2, k));
                                }
                            }
                        }
                        break;
                    case(1):
                        for (int k = 1; k <= limits[i]; k++) //Hacia la derecha
                        {
                            if ((World_Data[Ypos][Xpos + k]) == 2)
                            {
                                signal += 1 / (Math.Pow(2, k));
                            }
                            for (int n = 1; (n <= k) && (n <= limits[2]) && (k <= limits[1]); n++)
                            {
                                if ((World_Data[Ypos - n][Xpos + k]) == 2)
                                {
                                    signal += 1 / (Math.Pow(2, k));
                                }
                            }
                            for (int n = 1; (n <= k) && (n <= limits[3]) && (k <= limits[1]); n++)
                            {
                                if ((World_Data[Ypos + n][Xpos + k]) == 2)
                                {
                                    signal += 1 / (Math.Pow(2, k));
                                }
                            }
                        }
                        break;
                    case(2):
                        for (int k = 1; k <= limits[i]; k++) //Hacia arriba
                        {
                            if ((World_Data[Ypos - k][Xpos]) == 2)
                            {
                                signal += 1 / (Math.Pow(2, k));
                            }
                            for (int n = 1; (n <= k) && (n <= limits[0]) && (k <= limits[2]); n++)
                            {
                                if ((World_Data[Ypos - k][Xpos - n]) == 2)
                                {
                                    signal += 1 / (Math.Pow(2, k));
                                }
                            }
                            for (int n = 1; (n <= k) && (n <= limits[1]) && (k <= limits[3]); n++)
                            {
                                if ((World_Data[Ypos + k][Xpos + n]) == 2)
                                {
                                    signal += 1 / (Math.Pow(2, k));
                                }
                            }
                        }
                        break;
                    case(3):
                        for (int k = 1; k <= limits[i]; k++) //Hacia abajo
                        {
                            if ((World_Data[Ypos + k][Xpos]) == 2)
                            {
                                signal += 1 / (Math.Pow(2, k));
                            }
                            for (int n = 1; (n <= k) && (n <= limits[0]) && (k <= limits[2]); n++)
                            {
                                if ((World_Data[Ypos - k][Xpos - n]) == 2)
                                {
                                    signal += 1 / (Math.Pow(2, k));
                                }
                            }
                            for (int n = 1; (n <= k) && (n <= limits[1]) && (k <= limits[3]); n++)
                            {
                                if ((World_Data[Ypos + k][Xpos + n]) == 2)
                                {
                                    signal += 1 / (Math.Pow(2, k));
                                }
                            }
                        }
                        break;
                }
            
            }

            prev_signal_intensity = signal_intensity;
            signal_intensity = signal;
        }
        private int[] get_limits(int Xpos, int Ypos, int cobertura)
        {
            //int aux = 0;                                               //Leyenda:
            int[] limits = new int[4];                                 //limits[0]--> Límite izquierdo
            if (Xpos <= cobertura) limits[0] = Xpos;                   //limits[1]--> Límite derecho
            else limits[0] = cobertura;                                //limits[2]--> Límite superior
            if ((Xpos + cobertura) > 40) limits[1] = 40 - Xpos;        //limits[3]--> Límite inferior
            else limits[1] = cobertura;
            if (Ypos <= cobertura) limits[2] = Ypos;
            else limits[2] = cobertura;
            if ((Ypos + cobertura) > 21) limits[3] = 21 - Ypos;
            else limits[3] = cobertura;
            return limits;
        }
    }
}
