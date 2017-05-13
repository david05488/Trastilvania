using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Trastilvania_Project
{
    public partial class Form1 : Form
    {
        int Lambda_Muros_Horizontales = 1; // Lambda de la función Poisson de distribución para los muros.
        int Lambda_Muros_Verticales = 1; // Lambda de la función Poisson de distribución para los muros.
        double Lambda_Huecos = 0.1;
        double Lambda_Exp = 0.1;
        double Lambda_Trastes = 0.1;
        int Trastes_show = 1;
        int Total_Time = 30;
        int Elapsed_Time = 0;
        int cobertura = 3;
        int Cant_Trastes = 10; //Trastes (1).
        int Cant_Huecos = 10; //Huecos (2).
        int Cant_Wobots = 10; //Wobots (3).
        int Cant_Muros = 10; //Muros (4).
        int [][] World_Data = new int[22][]; //Matrix bidimensional donde se encuentran las coordenadas del terreno.
//////////////Variables de uso del Timer////////////////////////////////////////////////////////////////////////////////////
        static System.Windows.Forms.Timer Time_of_Simulation = new System.Windows.Forms.Timer();
        Point coordenadas = new Point();
        Size medida = new Size(15, 15);
        Rectangle rect;
        Distribution Distrib = new Distribution();
        Algorithm Alg = new Algorithm();
        Type Agent = typeof(Agente);
        Type Hole = typeof(Hueco);
        Type Trast = typeof(Show_up_traste);
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        Image Muro_PNG, Hueco_PNG, Traste_PNG, A_Abajo_PNG, A_Arriba_PNG, A_Derecha_PNG, A_Izquierda_PNG,
                A_Abajo_T_PNG, A_Arriba_T_PNG, A_Izquierda_T_PNG, A_Derecha_T_PNG;
        //Point coordenadas = new Point();
        Builder Machine = new Builder();
        Agente[] Agent_List; //Array de Agentes.
        Hueco[] Hueco_List; //Array de huecos.
        Evento[] Event_List; //Array de objetos, en este caso se usa para el procesamiento conjunto del tiempo de reacción
                              //de los agentes y el tiempo de desaparición/aparición de los huecos.
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //Leyenda de la matrix:
        //0- Casilla vacía.
        //1- Traste.
        //2- Hueco.
        //3- Muro.
        //4- Wobot Arriba.
        //5- Wobot Abajo.
        //6- Wobot Izquierda.
        //7- Wobot Derecha.
        //8- Wobot Arriba con Traste.
        //9- Wobot Abajo con Traste.
        //10- Wobot Izquierda con traste.
        //11- Wobot Derecha con traste.
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Muro_PNG = Trastilvania_Project.Properties.Resources.Roca;
            Hueco_PNG = Trastilvania_Project.Properties.Resources.Hueco;
            Traste_PNG = Trastilvania_Project.Properties.Resources.Traste;
            A_Abajo_PNG = Trastilvania_Project.Properties.Resources.A_Abajo;
            A_Arriba_PNG = Trastilvania_Project.Properties.Resources.A_Arriba;
            A_Derecha_PNG = Trastilvania_Project.Properties.Resources.A_Derecha;
            A_Izquierda_PNG = Trastilvania_Project.Properties.Resources.A_Izquierda;
            A_Abajo_T_PNG = Trastilvania_Project.Properties.Resources.A_Abajo_T;
            A_Arriba_T_PNG = Trastilvania_Project.Properties.Resources.A_Arriba_T;
            A_Derecha_T_PNG = Trastilvania_Project.Properties.Resources.A_Derecha_T;
            A_Izquierda_T_PNG = Trastilvania_Project.Properties.Resources.A_Izquierda_T;           
        }

        private void Reset_button_Click(object sender, EventArgs e)
        {
            Run_button.Enabled = true;
            Drawing_Area.Refresh();
            for (int i = 0; i < 22; i++) //Creando la matrix de datos del mundo.
            {
                World_Data[i] = new int[41];
            }
            Graphics g = Drawing_Area.CreateGraphics();
            Agent_List = new Agente[Cant_Wobots];
            
            World_Data = Machine.Build_Muros(World_Data, Cant_Muros, Lambda_Muros_Horizontales, Lambda_Muros_Verticales);
            World_Data = Machine.Build_Huecos(World_Data, Cant_Huecos, Lambda_Huecos);
            World_Data = Machine.Build_Trastes(World_Data, Cant_Trastes);
            World_Data = Machine.Build_Wobots(World_Data, Cant_Wobots, cobertura);
            Agent_List = Machine.Agent_list; //Array ordenado de agentes.
            Hueco_List = Machine.Hueco_list; //Array ordenado de huecos.
            Event_List = new Evento[Agent_List.Length + Hueco_List.Length + Trastes_show];
            //Código de control//////////////////////////////////////////////////////
            //Agent_List[0].Ypos = 15;
            //Agent_List[0].Xpos = 5;
            //Agent_List[0].orientation = 1;
            //World_Data[Hueco_List[0].Ypos][Hueco_List[0].Xpos] = 0;
            //Hueco_List[0].Xpos = 5;
            //Hueco_List[0].Ypos = 21;
            //World_Data[15][5] = 5;
            //World_Data[16][5] = 1;
            //World_Data[21][5] = 2;
            //World_Data[10][3] = 3;
            //World_Data[10][5] = 3;
            //World_Data[11][5] = 3;
            //World_Data[11][3] = 3;

            //Agent_List[1].Ypos = 13;
            //Agent_List[1].Xpos = 4;
            //Agent_List[1].orientation = 1;
            //World_Data[10][40] = 5;
            //World_Data[0][20] = 2;
            //Código de control/////////////////////////////////////////////////////////
            for (int i = 0; i < Agent_List.Length; i++)
            {
                Agent_List[i].get_neighborhood(World_Data, Agent_List[i].Xpos, Agent_List[i].Ypos); //Obtener vecindad.
                Agent_List[i].get_signal_intensity(cobertura, World_Data, Agent_List[i].Xpos, Agent_List[i].Ypos);
                Agent_List[i].Time_of_reaction = Distrib.nextExponential(Lambda_Exp); //Asignación del tiempo de reacción.
                Event_List[i] = Agent_List[i]; //Insertar todos los agentes en la lista de objetos.
            }
            for (int i = 0; i < Hueco_List.Length; i++) //Asignación del tiempo inicial de desaparición de los huecos.
            {
                Hueco_List[i].Time_of_reaction = Distrib.nextExponential(Lambda_Huecos);
                Event_List[Agent_List.Length + i] = Hueco_List[i];
            }
            for (int i = 0; i < Trastes_show; i++) //Agregar a la lista los eventos de aparición de trastes.
            {
                Event_List[Agent_List.Length + Hueco_List.Length + i] = new Show_up_traste();
                Event_List[Agent_List.Length + Hueco_List.Length + i].Time_of_reaction = Distrib.nextExponential(Lambda_Trastes);
            }
                     
///////////////////////////Dibujar el mundo//////////////////////////////////////////////////////////////////////////////
            for (int i = 0; i < World_Data.Length; i++)
            {
                for (int k = 0; k < World_Data[0].Length; k++)
                {
                    switch (World_Data[i][k])
                    {
                        case (1):
                            coordenadas.X = k * 15;
                            coordenadas.Y = i * 15;
                            g.DrawImage(Traste_PNG, coordenadas);
                            break;
                        case (2):
                            coordenadas.X = k * 15;
                            coordenadas.Y = i * 15;
                            g.DrawImage(Hueco_PNG, coordenadas);
                            break;
                        case (3):
                            //coordenadas.X = 0; //615. (41) 
                            //coordenadas.Y = 330; //330. La medida de cada elemento es de 15 unidades.(22 U)
                            coordenadas.X = k * 15;
                            coordenadas.Y = i * 15;
                            g.DrawImage(Muro_PNG, coordenadas);                           
                            break;
                        case(4):
                            coordenadas.X = k * 15;
                            coordenadas.Y = i * 15;
                            g.DrawImage(A_Arriba_PNG, coordenadas);
                            break;
                        case(5):
                            coordenadas.X = k * 15;
                            coordenadas.Y = i * 15;
                            g.DrawImage(A_Abajo_PNG, coordenadas);
                            break;
                        case(6):
                            coordenadas.X = k * 15;
                            coordenadas.Y = i * 15;
                            g.DrawImage(A_Izquierda_PNG, coordenadas);
                            break;
                        case(7):
                            coordenadas.X = k * 15;
                            coordenadas.Y = i * 15;
                            g.DrawImage(A_Derecha_PNG, coordenadas);
                            break;
                    }
                }
            }
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        }

        private void Run_button_Click(object sender, EventArgs e)
        {
            if (Run_button.Text == "Run")
            {
                Reset_button.Enabled = false;
                Run_button.Text = "Pausa";
                Time_of_Simulation.Tick += new EventHandler(Time_of_Simulation_Tick);
                Time_of_Simulation.Interval = 1000;
                Time_of_Simulation.Start();
            }
            else
            {
                Reset_button.Enabled = true;
                Run_button.Text = "Run";
                //Time_of_Simulation.Tick+=new EventHandler(Time_of_Simulation_Tick);
                Time_of_Simulation.Stop();
                //Time_of_Simulation.Enabled = false;
                //int a = 0;
            }
        }

        void Time_of_Simulation_Tick(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            Random turn = new Random();
            Agente Agent_Tool; //Objeto temporal para el trabajo con los agentes.
            Hueco Hueco_Tool; //Objeto temporal para el trabajo con huecos.
            Show_up_traste Trast_Tool;
            Graphics g = Drawing_Area.CreateGraphics();
            Elapsed_Time++;
            if (Elapsed_Time > Total_Time)
            {
                Time_of_Simulation.Stop();
                Run_button.Text = "Run";
                Reset_button.Enabled = true;
                Run_button.Enabled = false;
            }
            Alg.Quicksort(Event_List, 0, Event_List.Length - 1);
/////////////////Recorriendo la lista de eventos////////////////////////////////////////////////////////////////////////////
            for (int i = 0; i < Event_List.Length; i++)
            {
                if (Event_List[i].GetType() == Agent) //Tratamiento de agentes.
                {
                    Agent_Tool = (Agente)Event_List[i]; //Casteo a Agente.                    
                    switch (Agent_Tool.orientation)
                    {
//////////////////////////////////////////HACIA ARRIBA/////////////////////////////////////////////////////////////////////
                        case (0): //Agente orientado hacia arriba
                            Agent_Tool.get_neighborhood(World_Data, Agent_Tool.Xpos, Agent_Tool.Ypos);//Actualizar vecindad
                            //Si el agente tiene un traste delante de él y este traste no está siendo empujado.
                            if ((Agent_Tool.neighborhood[1] == 1) &&
                                (Agent_Tool.detect_pushing_trast(World_Data, Agent_Tool.Ypos - 1, Agent_Tool.Xpos, 2) == false))
                            {
                                //Caso que haya un traste en un extremo del mundo
                                if (Agent_Tool.Ypos == 1)//Si está frente del traste y luego tiene el límite del mundo
                                {
                                    if (Agent_Tool.Clock_sense == true)
                                    {
                                        World_Data = Agent_Tool.turn_rigth(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                        Agent_Tool = (Agente)Event_List[i];
                                        coordenadas.X = Agent_Tool.Xpos * 15;
                                        coordenadas.Y = Agent_Tool.Ypos * 15;
                                        g.DrawImage(A_Derecha_PNG, coordenadas);
                                        break;
                                    }
                                    else
                                    {
                                        World_Data = Agent_Tool.turn_left(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                        Agent_Tool = (Agente)Event_List[i];
                                        coordenadas.X = Agent_Tool.Xpos * 15;
                                        coordenadas.Y = Agent_Tool.Ypos * 15;
                                        g.DrawImage(A_Izquierda_PNG, coordenadas);
                                        break;
                                    }
                                }
                                //Huecos en las esquinas
                                if ((Agent_Tool.neighborhood[2] == 2) && (Agent_Tool.neighborhood[7] != 3) &&
                                    (Agent_Tool.neighborhood[0] != 3))
                                {
                                    if (Agent_Tool.Clock_sense == false)
                                    {
                                        World_Data = Agent_Tool.turn_left(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                        Agent_Tool = (Agente)Event_List[i];
                                        coordenadas.X = Agent_Tool.Xpos * 15;
                                        coordenadas.Y = Agent_Tool.Ypos * 15;
                                        g.DrawImage(A_Izquierda_PNG, coordenadas);
                                        break;
                                    }
                                }
                                if ((Agent_Tool.neighborhood[0] == 2) && (Agent_Tool.neighborhood[3] != 3) &&
                                    (Agent_Tool.neighborhood[2] != 3))
                                {
                                    if (Agent_Tool.Clock_inverse == false)
                                    {
                                        World_Data = Agent_Tool.turn_rigth(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                        Agent_Tool = (Agente)Event_List[i];
                                        coordenadas.X = Agent_Tool.Xpos * 15;
                                        coordenadas.Y = Agent_Tool.Ypos * 15;
                                        g.DrawImage(A_Derecha_PNG, coordenadas);
                                        break;
                                    }
                                }
                                if (Agent_Tool.Ypos >= 3)
                                {
                                    //El agente sólo moverá el traste si la casilla siguiente es hueco o vacía y la casilla
                                    if ((World_Data[Agent_Tool.Ypos - 2][Agent_Tool.Xpos] == 0) &&//siguiente no es ni
                                        ((World_Data[Agent_Tool.Ypos - 3][Agent_Tool.Xpos] != 3) &&//traste ni muro.
                                        (World_Data[Agent_Tool.Ypos - 3][Agent_Tool.Xpos] != 1)))
                                    {
                                        ////////////////////////////////////////Sección de código para borrar el agente///////////////////////////////////////////
                                        coordenadas.X = Agent_Tool.Xpos * 15;
                                        coordenadas.Y = Agent_Tool.Ypos * 15;
                                        rect = new Rectangle(coordenadas, medida);
                                        Drawing_Area.Invalidate(rect);
                                        Drawing_Area.Update();
                                        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                        ////////////////////////////////////////Sección de código para borrar el traste/////////////////////////////////////////
                                        coordenadas.Y = (Agent_Tool.Ypos - 1) * 15;
                                        rect = new Rectangle(coordenadas, medida);
                                        Drawing_Area.Invalidate(rect);
                                        Drawing_Area.Update();
                                        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                        ////////////////////////////////////////Actualizar la matix de datos y reasignar el agente a Agent_Tool/////////////////
                                        World_Data = Agent_Tool.move_up(World_Data, Event_List, (Agente)Event_List[i], true, Lambda_Exp, Lambda_Huecos);
                                        Agent_Tool = (Agente)Event_List[i];
                                        ////////////////////////////////////////Sección de código que pinta nuevamente el traste y el agente////////////////////
                                        coordenadas.X = Agent_Tool.Xpos * 15;
                                        coordenadas.Y = Agent_Tool.Ypos * 15;
                                        g.DrawImage(A_Arriba_T_PNG, coordenadas);
                                        coordenadas.Y = (Agent_Tool.Ypos - 1) * 15;
                                        g.DrawImage(Traste_PNG, coordenadas);
                                        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                    }
                                    else
                                    {//Si lo próximo que viene es un hueco y el agente está empujando el traste.
                                        if (World_Data[Agent_Tool.Ypos - 2][Agent_Tool.Xpos] == 2)
                                        {
                                            coordenadas.X = Agent_Tool.Xpos * 15;
                                            coordenadas.Y = Agent_Tool.Ypos * 15;
                                            rect = new Rectangle(coordenadas, medida);
                                            Drawing_Area.Invalidate(rect);
                                            Drawing_Area.Update();
                                            coordenadas.X = Agent_Tool.Xpos * 15;
                                            coordenadas.Y = (Agent_Tool.Ypos - 1) * 15;
                                            rect = new Rectangle(coordenadas, medida);
                                            Drawing_Area.Invalidate(rect);
                                            Drawing_Area.Update();
                                            World_Data = Agent_Tool.move_up(World_Data, Event_List, (Agente)Event_List[i], true, Lambda_Exp, Lambda_Huecos);                                                                               
                                            Agent_Tool = (Agente)Event_List[i];
                                            coordenadas.X = Agent_Tool.Xpos * 15;
                                            coordenadas.Y = Agent_Tool.Ypos * 15;
                                            g.DrawImage(A_Arriba_T_PNG, coordenadas);
                                            coordenadas.Y = (Agent_Tool.Ypos - 1) * 15;
                                            g.DrawImage(Traste_PNG, coordenadas);
                                            rect = new Rectangle(coordenadas, medida);
                                            Drawing_Area.Invalidate(rect);
                                            Drawing_Area.Update();
                                        //Tratamiento de huecos//
                                            for (int y = 0; y < World_Data.Length; y++)
                                            {
                                                for (int x = 0; x < World_Data[0].Length; x++)
                                                {
                                                    if (World_Data[y][x] == 20)
                                                    {
                                                        World_Data[y][x] = 2;
                                                        coordenadas.X = x * 15;
                                                        coordenadas.Y = y * 15;
                                                        g.DrawImage(Hueco_PNG, coordenadas);
                                                    }
                                                }
                                            }
                                            break;
                                        }
                                        //Si hay traste doble o el agente se encuentra con un muro en uno de los extremos
                                        if ((World_Data[Agent_Tool.Ypos - 2][Agent_Tool.Xpos] == 1) ||
                                            ((World_Data[Agent_Tool.Ypos - 1][Agent_Tool.Xpos] == 1) &&
                                            (World_Data[Agent_Tool.Ypos - 2][Agent_Tool.Xpos] == 3))||
                                            (World_Data[Agent_Tool.Ypos - 3][Agent_Tool.Xpos] == 3) ||
                                            (World_Data[Agent_Tool.Ypos - 3][Agent_Tool.Xpos] == 1))
                                        {//Si el agente está en el extremo izq del mundo
                                            if ((Agent_Tool.neighborhood[0] == 3) && (Agent_Tool.neighborhood[3] != 3))
                                            {//Si ya tiene un giro contrario a las agujas del reloj entonces turn left
                                                if (Agent_Tool.Clock_inverse == true)
                                                {
                                                    World_Data = Agent_Tool.turn_left(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                    Agent_Tool = (Agente)Event_List[i];
                                                    coordenadas.X = Agent_Tool.Xpos * 15;
                                                    coordenadas.Y = Agent_Tool.Ypos * 15;
                                                    g.DrawImage(A_Izquierda_PNG, coordenadas);
                                                }
                                                else
                                                {//Inicial
                                                    World_Data = Agent_Tool.turn_rigth(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                    Agent_Tool = (Agente)Event_List[i];
                                                    coordenadas.X = Agent_Tool.Xpos * 15;
                                                    coordenadas.Y = Agent_Tool.Ypos * 15;
                                                    g.DrawImage(A_Derecha_PNG, coordenadas);
                                                }
                                            }
                                            else
                                            {//Si el agente está en el extremo der del mundo
                                                if ((Agent_Tool.neighborhood[2] == 3) && (Agent_Tool.neighborhood[7] != 3))
                                                {
                                                    if (Agent_Tool.Clock_sense == true)
                                                    {//Si ya tiene un giro siguiendo las agujas del reloj entonces turn izq
                                                        World_Data = Agent_Tool.turn_rigth(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                        Agent_Tool = (Agente)Event_List[i];
                                                        coordenadas.X = Agent_Tool.Xpos * 15;
                                                        coordenadas.Y = Agent_Tool.Ypos * 15;
                                                        g.DrawImage(A_Derecha_PNG, coordenadas);
                                                    }
                                                    else//Initial
                                                    {
                                                        World_Data = Agent_Tool.turn_left(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                        Agent_Tool = (Agente)Event_List[i];
                                                        coordenadas.X = Agent_Tool.Xpos * 15;
                                                        coordenadas.Y = Agent_Tool.Ypos * 15;
                                                        g.DrawImage(A_Izquierda_PNG, coordenadas);
                                                    }
                                                }
                                                else
                                                {
                                                    int a = turn.Next(2);//Si no está en ningún extremo del mundo
                                                    switch (a)
                                                    {
                                                        case (0):
                                                            if (Agent_Tool.Clock_inverse == true)
                                                            {//Si ya tiene un giro en contra de las agujas del reloj
                                                                World_Data = Agent_Tool.turn_left(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                                Agent_Tool = (Agente)Event_List[i];
                                                                coordenadas.X = Agent_Tool.Xpos * 15;
                                                                coordenadas.Y = Agent_Tool.Ypos * 15;
                                                                g.DrawImage(A_Izquierda_PNG, coordenadas);
                                                            }
                                                            else
                                                            {//Initial
                                                                World_Data = Agent_Tool.turn_rigth(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                                Agent_Tool = (Agente)Event_List[i];
                                                                coordenadas.X = Agent_Tool.Xpos * 15;
                                                                coordenadas.Y = Agent_Tool.Ypos * 15;
                                                                g.DrawImage(A_Derecha_PNG, coordenadas);
                                                            }
                                                            break;
                                                        case (1):
                                                            if (Agent_Tool.Clock_sense == true)
                                                            {//Si ya tiene un giro a favor de las agujas del reloj
                                                                World_Data = Agent_Tool.turn_rigth(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                                Agent_Tool = (Agente)Event_List[i];
                                                                coordenadas.X = Agent_Tool.Xpos * 15;
                                                                coordenadas.Y = Agent_Tool.Ypos * 15;
                                                                g.DrawImage(A_Derecha_PNG, coordenadas);
                                                            }
                                                            else
                                                            {//Initial
                                                                World_Data = Agent_Tool.turn_left(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                                Agent_Tool = (Agente)Event_List[i];
                                                                coordenadas.X = Agent_Tool.Xpos * 15;
                                                                coordenadas.Y = Agent_Tool.Ypos * 15;
                                                                g.DrawImage(A_Izquierda_PNG, coordenadas);
                                                            }
                                                            break;
                                                    }
                                                }
                                            }
                                            break;
                                        }
                                        
                                    }
                                }
                                else
                                {//Caso en que hay un hueco en el extremo superior y el agente se aproxima con un traste
                                    if ((Agent_Tool.Old_Ypos >= 2) && (World_Data[Agent_Tool.Ypos - 2][Agent_Tool.Xpos] == 2))
                                    {
                                        coordenadas.X = Agent_Tool.Xpos * 15;
                                        coordenadas.Y = Agent_Tool.Ypos * 15;
                                        rect = new Rectangle(coordenadas, medida);
                                        Drawing_Area.Invalidate(rect);
                                        Drawing_Area.Update();
                                        coordenadas.X = Agent_Tool.Xpos * 15;
                                        coordenadas.Y = (Agent_Tool.Ypos - 1) * 15;
                                        rect = new Rectangle(coordenadas, medida);
                                        Drawing_Area.Invalidate(rect);
                                        Drawing_Area.Update();
                                        World_Data = Agent_Tool.move_up(World_Data, Event_List, (Agente)Event_List[i], true, Lambda_Exp, Lambda_Huecos);
                                        Agent_Tool = (Agente)Event_List[i];
                                        coordenadas.X = Agent_Tool.Xpos * 15;
                                        coordenadas.Y = Agent_Tool.Ypos * 15;
                                        g.DrawImage(A_Arriba_T_PNG, coordenadas);
                                        coordenadas.Y = (Agent_Tool.Ypos - 1) * 15;
                                        g.DrawImage(Traste_PNG, coordenadas);
                                        rect = new Rectangle(coordenadas, medida);
                                        Drawing_Area.Invalidate(rect);
                                        Drawing_Area.Update();
                                        //Tratamiento de huecos//
                                        for (int y = 0; y < World_Data.Length; y++)
                                        {
                                            for (int x = 0; x < World_Data[0].Length; x++)
                                            {
                                                if (World_Data[y][x] == 20)
                                                {
                                                    World_Data[y][x] = 2;
                                                    coordenadas.X = x * 15;
                                                    coordenadas.Y = y * 15;
                                                    g.DrawImage(Hueco_PNG, coordenadas);
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (Agent_Tool.neighborhood[0] == 3)//Si el agente está en el extremo izq del mundo
                                        {
                                            if (Agent_Tool.Clock_inverse == true)
                                            {//Si el agente ya tiene un giro en contra de las agujas del reloj
                                                World_Data = Agent_Tool.turn_left(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                Agent_Tool = (Agente)Event_List[i];
                                                coordenadas.X = Agent_Tool.Xpos * 15;
                                                coordenadas.Y = Agent_Tool.Ypos * 15;
                                                g.DrawImage(A_Izquierda_PNG, coordenadas);
                                            }
                                            else
                                            {//Initial
                                                World_Data = Agent_Tool.turn_rigth(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                Agent_Tool = (Agente)Event_List[i];
                                                coordenadas.X = Agent_Tool.Xpos * 15;
                                                coordenadas.Y = Agent_Tool.Ypos * 15;
                                                g.DrawImage(A_Derecha_PNG, coordenadas);
                                            }                                           
                                        }
                                        else
                                        {
                                            if (Agent_Tool.neighborhood[2] == 3)//Si el agente está en el extremo der del mundo
                                            {
                                                if (Agent_Tool.Clock_sense == true)
                                                {//Si el agente ya tiene un giro a favor de las agujas del reloj
                                                    World_Data = Agent_Tool.turn_rigth(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                    Agent_Tool = (Agente)Event_List[i];
                                                    coordenadas.X = Agent_Tool.Xpos * 15;
                                                    coordenadas.Y = Agent_Tool.Ypos * 15;
                                                    g.DrawImage(A_Derecha_PNG, coordenadas);
                                                }
                                                else
                                                {//Initial
                                                    World_Data = Agent_Tool.turn_left(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                    Agent_Tool = (Agente)Event_List[i];
                                                    coordenadas.X = Agent_Tool.Xpos * 15;
                                                    coordenadas.Y = Agent_Tool.Ypos * 15;
                                                    g.DrawImage(A_Izquierda_PNG, coordenadas);
                                                }
                                            }
                                            else 
                                            {
                                                int a = turn.Next(2);//Si no está en ningún extremo del mundo
                                                switch (a)
                                                {
                                                    case (0):
                                                        if (Agent_Tool.Clock_inverse == true)
                                                        {//Si el agente ya tiene un giro en contra de las agujas del reloj
                                                            World_Data = Agent_Tool.turn_left(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                            Agent_Tool = (Agente)Event_List[i];
                                                            coordenadas.X = Agent_Tool.Xpos * 15;
                                                            coordenadas.Y = Agent_Tool.Ypos * 15;
                                                            g.DrawImage(A_Izquierda_PNG, coordenadas);
                                                        }
                                                        else
                                                        {//Initial
                                                            World_Data = Agent_Tool.turn_rigth(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                            Agent_Tool = (Agente)Event_List[i];
                                                            coordenadas.X = Agent_Tool.Xpos * 15;
                                                            coordenadas.Y = Agent_Tool.Ypos * 15;
                                                            g.DrawImage(A_Derecha_PNG, coordenadas);
                                                        }
                                                        break;
                                                    case (1):
                                                        if (Agent_Tool.Clock_sense == true)
                                                        {//Si el agente ya tiene un giro en sentido de las agujas del reloj
                                                            World_Data = Agent_Tool.turn_rigth(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                            Agent_Tool = (Agente)Event_List[i];
                                                            coordenadas.X = Agent_Tool.Xpos * 15;
                                                            coordenadas.Y = Agent_Tool.Ypos * 15;
                                                            g.DrawImage(A_Derecha_PNG, coordenadas);
                                                        }
                                                        else
                                                        {//Initial
                                                            World_Data = Agent_Tool.turn_left(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                            Agent_Tool = (Agente)Event_List[i];
                                                            coordenadas.X = Agent_Tool.Xpos * 15;
                                                            coordenadas.Y = Agent_Tool.Ypos * 15;
                                                            g.DrawImage(A_Izquierda_PNG, coordenadas);
                                                        }
                                                        break;
                                                }
                                            }
                                        }
                                    }
                                }
//$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$
                                if (Agent_Tool.Ypos >= 2)
                                {
                                    //Si hay un traste intentando empujar el traste por el lado contrario
                                    if (World_Data[Agent_Tool.Ypos - 2][Agent_Tool.Xpos] == 5)
                                    {
                                        coordenadas.X = Agent_Tool.Xpos * 15;
                                        coordenadas.Y = Agent_Tool.Ypos * 15;
                                        rect = new Rectangle(coordenadas, medida);
                                        Drawing_Area.Invalidate(rect);
                                        Drawing_Area.Update();
                                        if ((Agent_Tool.neighborhood[7] == 1) && //Gira a la izquierda si hay un traste a la izq
                                (Agent_Tool.detect_pushing_trast(World_Data, Agent_Tool.Ypos, Agent_Tool.Xpos - 1, 1) == false))
                                        {
                                            if (Agent_Tool.Clock_sense == true)
                                            {//El agente ya tiene un giro a favor de las agujas del reloj
                                                World_Data = Agent_Tool.turn_rigth(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                Agent_Tool = (Agente)Event_List[i];
                                                coordenadas.X = Agent_Tool.Xpos * 15;
                                                coordenadas.Y = Agent_Tool.Ypos * 15;
                                                g.DrawImage(A_Derecha_PNG, coordenadas);
                                            }
                                            else
                                            {//Initial
                                                World_Data = Agent_Tool.turn_left(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                Agent_Tool = (Agente)Event_List[i];
                                                coordenadas.X = Agent_Tool.Xpos * 15;
                                                coordenadas.Y = Agent_Tool.Ypos * 15;
                                                g.DrawImage(A_Izquierda_PNG, coordenadas);
                                            }
                                        }
                                        else
                                        {
                                            if ((Agent_Tool.neighborhood[3] == 1) &&//Gira a la derecha si hay un traste a la derech
                                   (Agent_Tool.detect_pushing_trast(World_Data, Agent_Tool.Ypos, Agent_Tool.Xpos + 1, 3) == false))
                                            {
                                                if (Agent_Tool.Clock_inverse == true)
                                                {//El agente ya tiene un giro en contra de las agujas del reloj
                                                    World_Data = Agent_Tool.turn_left(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                    Agent_Tool = (Agente)Event_List[i];
                                                    coordenadas.X = Agent_Tool.Xpos * 15;
                                                    coordenadas.Y = Agent_Tool.Ypos * 15;
                                                    g.DrawImage(A_Izquierda_PNG, coordenadas);
                                                }
                                                else
                                                {//Initial
                                                    World_Data = Agent_Tool.turn_rigth(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                    Agent_Tool = (Agente)Event_List[i];
                                                    coordenadas.X = Agent_Tool.Xpos * 15;
                                                    coordenadas.Y = Agent_Tool.Ypos * 15;
                                                    g.DrawImage(A_Derecha_PNG, coordenadas);
                                                }
                                            }
                                            else
                                            {
                                                int a = turn.Next(2);
                                                switch (a)
                                                {
                                                    case (0):
                                                        if (Agent_Tool.Clock_inverse == true)
                                                        {//El agente ya tiene un giro en contra de las agujas del reloj
                                                            World_Data = Agent_Tool.turn_left(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                            Agent_Tool = (Agente)Event_List[i];
                                                            coordenadas.X = Agent_Tool.Xpos * 15;
                                                            coordenadas.Y = Agent_Tool.Ypos * 15;
                                                            g.DrawImage(A_Izquierda_PNG, coordenadas);
                                                        }
                                                        else
                                                        {//Initial
                                                            World_Data = Agent_Tool.turn_rigth(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                            Agent_Tool = (Agente)Event_List[i];
                                                            coordenadas.X = Agent_Tool.Xpos * 15;
                                                            coordenadas.Y = Agent_Tool.Ypos * 15;
                                                            g.DrawImage(A_Derecha_PNG, coordenadas);
                                                        }
                                                        break;
                                                    case (1):
                                                        if (Agent_Tool.Clock_sense == true)
                                                        {//El agente ya tiene un giro a favor de las agujas del reloj
                                                            World_Data = Agent_Tool.turn_rigth(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                            Agent_Tool = (Agente)Event_List[i];
                                                            coordenadas.X = Agent_Tool.Xpos * 15;
                                                            coordenadas.Y = Agent_Tool.Ypos * 15;
                                                            g.DrawImage(A_Derecha_PNG, coordenadas);
                                                        }
                                                        else
                                                        {//Initial
                                                            World_Data = Agent_Tool.turn_left(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                            Agent_Tool = (Agente)Event_List[i];
                                                            coordenadas.X = Agent_Tool.Xpos * 15;
                                                            coordenadas.Y = Agent_Tool.Ypos * 15;
                                                            g.DrawImage(A_Izquierda_PNG, coordenadas);
                                                        }
                                                        break;
                                                }
                                            }
                                        }

                                    }
                                }
//$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$
                            }
                            else
                            {   //Si el traste que tiene delante ya lo está empujando otro agente.
                                if ((Agent_Tool.neighborhood[1] == 1) &&
                                (Agent_Tool.detect_pushing_trast(World_Data, Agent_Tool.Ypos - 1, Agent_Tool.Xpos, 2) == true))
                                {
                                    coordenadas.X = Agent_Tool.Xpos * 15;
                                    coordenadas.Y = Agent_Tool.Ypos * 15;
                                    rect = new Rectangle(coordenadas, medida);
                                    Drawing_Area.Invalidate(rect);
                                    Drawing_Area.Update();
                                    if ((Agent_Tool.neighborhood[7] == 1) && //Gira a la izquierda si hay un traste a la izq
                            (Agent_Tool.detect_pushing_trast(World_Data, Agent_Tool.Ypos, Agent_Tool.Xpos - 1, 1) == false))
                                    {
                                        if (Agent_Tool.Clock_sense == true)
                                        {//El agente ya tiene un giro a favor de las agujas del reloj
                                            World_Data = Agent_Tool.turn_rigth(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                            Agent_Tool = (Agente)Event_List[i];
                                            coordenadas.X = Agent_Tool.Xpos * 15;
                                            coordenadas.Y = Agent_Tool.Ypos * 15;
                                            g.DrawImage(A_Derecha_PNG, coordenadas);
                                        }
                                        else
                                        {//Initial
                                            World_Data = Agent_Tool.turn_left(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                            Agent_Tool = (Agente)Event_List[i];
                                            coordenadas.X = Agent_Tool.Xpos * 15;
                                            coordenadas.Y = Agent_Tool.Ypos * 15;
                                            g.DrawImage(A_Izquierda_PNG, coordenadas);
                                        }
                                    }
                                    else
                                    {
                                        if ((Agent_Tool.neighborhood[3] == 1) &&//Gira a la derecha si hay un traste a la derech
                               (Agent_Tool.detect_pushing_trast(World_Data, Agent_Tool.Ypos, Agent_Tool.Xpos + 1, 3) == false))
                                        {
                                            if (Agent_Tool.Clock_inverse == true)
                                            {//El agente ya tiene un giro en contra de las agujas del reloj
                                                World_Data = Agent_Tool.turn_left(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                Agent_Tool = (Agente)Event_List[i];
                                                coordenadas.X = Agent_Tool.Xpos * 15;
                                                coordenadas.Y = Agent_Tool.Ypos * 15;
                                                g.DrawImage(A_Izquierda_PNG, coordenadas);
                                            }
                                            else
                                            {//Initial
                                                World_Data = Agent_Tool.turn_rigth(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                Agent_Tool = (Agente)Event_List[i];
                                                coordenadas.X = Agent_Tool.Xpos * 15;
                                                coordenadas.Y = Agent_Tool.Ypos * 15;
                                                g.DrawImage(A_Derecha_PNG, coordenadas);
                                            }
                                        }
                                        else
                                        {
                                            int a = turn.Next(2);
                                            switch(a)
                                            {
                                                case (0):
                                                    if (Agent_Tool.Clock_inverse == true)
                                                    {//El agente ya tiene un giro en contra de las agujas del reloj
                                                        World_Data = Agent_Tool.turn_left(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                        Agent_Tool = (Agente)Event_List[i];
                                                        coordenadas.X = Agent_Tool.Xpos * 15;
                                                        coordenadas.Y = Agent_Tool.Ypos * 15;
                                                        g.DrawImage(A_Izquierda_PNG, coordenadas);
                                                    }
                                                    else
                                                    {//Initial
                                                        World_Data = Agent_Tool.turn_rigth(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                        Agent_Tool = (Agente)Event_List[i];
                                                        coordenadas.X = Agent_Tool.Xpos * 15;
                                                        coordenadas.Y = Agent_Tool.Ypos * 15;
                                                        g.DrawImage(A_Derecha_PNG, coordenadas);
                                                    }
                                                    break;
                                                case (1):
                                                    if (Agent_Tool.Clock_sense == true)
                                                    {//El agente ya tiene un giro a favor de las agujas del reloj
                                                        World_Data = Agent_Tool.turn_rigth(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                        Agent_Tool = (Agente)Event_List[i];
                                                        coordenadas.X = Agent_Tool.Xpos * 15;
                                                        coordenadas.Y = Agent_Tool.Ypos * 15;
                                                        g.DrawImage(A_Derecha_PNG, coordenadas);
                                                    }
                                                    else
                                                    {//Initial
                                                        World_Data = Agent_Tool.turn_left(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                        Agent_Tool = (Agente)Event_List[i];
                                                        coordenadas.X = Agent_Tool.Xpos * 15;
                                                        coordenadas.Y = Agent_Tool.Ypos * 15;
                                                        g.DrawImage(A_Izquierda_PNG, coordenadas);
                                                    }
                                                    break;
                                            }
                                        }
                                    }
                                    
                                }
                                //Si el agente no tiene traste frente ni a los lados y tiene algún traste
                                //en las vecindades diagonales inferiores.
//CORRECCIÓN AGREGANDO(Agent_Tool.neighborhood[1] == 2) PARA EL CASO QUE HAYA UN HUECO DELANTE
                                if (((Agent_Tool.neighborhood[1] == 2) || (Agent_Tool.neighborhood[1] == 0)) &&
                                    ((Agent_Tool.neighborhood[4] == 1) || (Agent_Tool.neighborhood[6] == 1)))
                                {
                                    if ((Agent_Tool.neighborhood[4] == 1) && //Si tiene traste a la derecha diagonal inferior y no está push 
                                  (Agent_Tool.detect_pushing_trast(World_Data, Agent_Tool.Ypos + 1, Agent_Tool.Xpos + 1, 7) == false))
                                    {
                                        coordenadas.X = Agent_Tool.Xpos * 15;
                                        coordenadas.Y = Agent_Tool.Ypos * 15;
                                        rect = new Rectangle(coordenadas, medida);
                                        Drawing_Area.Invalidate(rect);
                                        Drawing_Area.Update();
                                        if (Agent_Tool.Clock_inverse == true)
                                        {
                                            if (Agent_Tool.neighborhood[1] == 2)//Caso que tenga que moverse a un hueco
                                            {
                                                World_Data = Agent_Tool.Agent_Restore(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                Agent_Tool = (Agente)Event_List[i];
                                                coordenadas.X = Agent_Tool.Xpos * 15;
                                                coordenadas.Y = Agent_Tool.Ypos * 15;
                                                switch (World_Data[Agent_Tool.Ypos][Agent_Tool.Xpos])
                                                {
                                                    case (4):
                                                        g.DrawImage(A_Arriba_PNG, coordenadas);
                                                        break;
                                                    case (5):
                                                        g.DrawImage(A_Abajo_PNG, coordenadas);
                                                        break;
                                                    case (6):
                                                        g.DrawImage(A_Izquierda_PNG, coordenadas);
                                                        break;
                                                    case (7):
                                                        g.DrawImage(A_Derecha_PNG, coordenadas);
                                                        break;
                                                }
                                                break;
                                            }
                                            //El agente ya tiene un giro en contra de las agujas del reloj (Aquí se sustituyó turn_left por move_up)
                                            World_Data = Agent_Tool.move_up(World_Data, Event_List, (Agente)Event_List[i], false, Lambda_Exp, Lambda_Huecos);
                                            Agent_Tool = (Agente)Event_List[i];
                                            coordenadas.X = Agent_Tool.Xpos * 15;
                                            coordenadas.Y = Agent_Tool.Ypos * 15;
                                            g.DrawImage(A_Arriba_PNG, coordenadas);
                                        }
                                        else
                                        {//Initial
                                            World_Data = Agent_Tool.turn_rigth(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                            Agent_Tool = (Agente)Event_List[i];
                                            coordenadas.X = Agent_Tool.Xpos * 15;
                                            coordenadas.Y = Agent_Tool.Ypos * 15;
                                            g.DrawImage(A_Derecha_PNG, coordenadas);
                                        }
                                        break;
                                    }
                                    if ((Agent_Tool.neighborhood[6] == 1) && //Si tiene traste a la izquierda diagonal inferior y no está push 
                                  (Agent_Tool.detect_pushing_trast(World_Data, Agent_Tool.Ypos + 1, Agent_Tool.Xpos - 1, 7) == false))
                                    {
                                        coordenadas.X = Agent_Tool.Xpos * 15;
                                        coordenadas.Y = Agent_Tool.Ypos * 15;
                                        rect = new Rectangle(coordenadas, medida);
                                        Drawing_Area.Invalidate(rect);
                                        Drawing_Area.Update();
                                        if (Agent_Tool.Clock_sense == true)
                                        {
                                            if (Agent_Tool.neighborhood[1] == 2)//Caso que tenga que moverse a un hueco
                                            {
                                                World_Data = Agent_Tool.Agent_Restore(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                Agent_Tool = (Agente)Event_List[i];
                                                coordenadas.X = Agent_Tool.Xpos * 15;
                                                coordenadas.Y = Agent_Tool.Ypos * 15;
                                                switch (World_Data[Agent_Tool.Ypos][Agent_Tool.Xpos])
                                                {
                                                    case (4):
                                                        g.DrawImage(A_Arriba_PNG, coordenadas);
                                                        break;
                                                    case (5):
                                                        g.DrawImage(A_Abajo_PNG, coordenadas);
                                                        break;
                                                    case (6):
                                                        g.DrawImage(A_Izquierda_PNG, coordenadas);
                                                        break;
                                                    case (7):
                                                        g.DrawImage(A_Derecha_PNG, coordenadas);
                                                        break;
                                                }
                                                break;
                                            }
                                            //El agente ya tiene un giro a favor de las agujas del reloj(Aquí se sustituyó turn_left por move_up)
                                            World_Data = Agent_Tool.move_up(World_Data, Event_List, (Agente)Event_List[i], false, Lambda_Exp, Lambda_Huecos);
                                            Agent_Tool = (Agente)Event_List[i];
                                            coordenadas.X = Agent_Tool.Xpos * 15;
                                            coordenadas.Y = Agent_Tool.Ypos * 15;
                                            g.DrawImage(A_Arriba_PNG, coordenadas);
                                        }
                                        else
                                        {//Initial
                                            World_Data = Agent_Tool.turn_left(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                            Agent_Tool = (Agente)Event_List[i];
                                            coordenadas.X = Agent_Tool.Xpos * 15;
                                            coordenadas.Y = Agent_Tool.Ypos * 15;
                                            g.DrawImage(A_Izquierda_PNG, coordenadas);
                                        }
                                        break;
                                    }
                                }
                                //Si el agente no tiene traste frente ni a los lados.
                                Agent_Tool.get_neighborhood(World_Data, Agent_Tool.Xpos, Agent_Tool.Ypos);//Actualizar vecindad
                                if ((Agent_Tool.neighborhood[1] == 0) && (Agent_Tool.neighborhood[3] != 1) &&
                                    (Agent_Tool.neighborhood[7] != 1))
                                {
                                    coordenadas.X = Agent_Tool.Xpos * 15;
                                    coordenadas.Y = Agent_Tool.Ypos * 15;
                                    rect = new Rectangle(coordenadas, medida);
                                    Drawing_Area.Invalidate(rect);
                                    Drawing_Area.Update();
                                    World_Data = Agent_Tool.move_up(World_Data, Event_List, (Agente)Event_List[i], false, Lambda_Exp, Lambda_Huecos);
                                    Agent_Tool = (Agente)Event_List[i];
                                    coordenadas.X = Agent_Tool.Xpos * 15;
                                    coordenadas.Y = Agent_Tool.Ypos * 15;
                                    g.DrawImage(A_Arriba_PNG, coordenadas);
                                }
                                else
                                {
                                    //Si el agente no tiene traste frente pero tiene alguno a los lados.
                                    if ((Agent_Tool.neighborhood[1] != 1) && ((Agent_Tool.neighborhood[3] == 1) ||
                                        (Agent_Tool.neighborhood[7] == 1)))
                                    {
                                        if ((Agent_Tool.neighborhood[3] == 1) && //Si tiene traste a la derecha y no está push 
                                  (Agent_Tool.detect_pushing_trast(World_Data, Agent_Tool.Ypos, Agent_Tool.Xpos + 1, 3) == false))
                                        {
                                            coordenadas.X = Agent_Tool.Xpos * 15;
                                            coordenadas.Y = Agent_Tool.Ypos * 15;
                                            rect = new Rectangle(coordenadas, medida);
                                            Drawing_Area.Invalidate(rect);
                                            Drawing_Area.Update();
                                            if ((Agent_Tool.Clock_inverse == true) &&
                                               ((Agent_Tool.neighborhood[1] == 0) || (Agent_Tool.neighborhood[1] == 2)))
                                            {
                                                if (Agent_Tool.neighborhood[1] == 2)//Caso que tenga que moverse a un hueco
                                                {
                                                    World_Data = Agent_Tool.Agent_Restore(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                    Agent_Tool = (Agente)Event_List[i];
                                                    coordenadas.X = Agent_Tool.Xpos * 15;
                                                    coordenadas.Y = Agent_Tool.Ypos * 15;
                                                    switch (World_Data[Agent_Tool.Ypos][Agent_Tool.Xpos])
                                                    {
                                                        case (4):
                                                            g.DrawImage(A_Arriba_PNG, coordenadas);
                                                            break;
                                                        case (5):
                                                            g.DrawImage(A_Abajo_PNG, coordenadas);
                                                            break;
                                                        case (6):
                                                            g.DrawImage(A_Izquierda_PNG, coordenadas);
                                                            break;
                                                        case (7):
                                                            g.DrawImage(A_Derecha_PNG, coordenadas);
                                                            break;
                                                    }
                                                    break;
                                                }
                                                //El agente ya tiene un giro en contra de las agujas del reloj (Aquí se sustituyó turn_left por move_up)
                                                World_Data = Agent_Tool.move_up(World_Data, Event_List,(Agente)Event_List[i],false,Lambda_Exp, Lambda_Huecos);
                                                Agent_Tool = (Agente)Event_List[i];
                                                coordenadas.X = Agent_Tool.Xpos * 15;
                                                coordenadas.Y = Agent_Tool.Ypos * 15;
                                                g.DrawImage(A_Arriba_PNG, coordenadas);
                                            }
                                            else
                                            {//Initial
                                                World_Data = Agent_Tool.turn_rigth(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                Agent_Tool = (Agente)Event_List[i];
                                                coordenadas.X = Agent_Tool.Xpos * 15;
                                                coordenadas.Y = Agent_Tool.Ypos * 15;
                                                g.DrawImage(A_Derecha_PNG, coordenadas);
                                            }
                                            break;
                                        }
                                        if ((Agent_Tool.neighborhood[7] == 1) && //Si tiene traste a la izquierda y no está push 
                                  (Agent_Tool.detect_pushing_trast(World_Data, Agent_Tool.Ypos, Agent_Tool.Xpos - 1, 1) == false))
                                        {
                                            coordenadas.X = Agent_Tool.Xpos * 15;
                                            coordenadas.Y = Agent_Tool.Ypos * 15;
                                            rect = new Rectangle(coordenadas, medida);
                                            Drawing_Area.Invalidate(rect);
                                            Drawing_Area.Update();
                                            if ((Agent_Tool.Clock_sense == true) && 
                                                ((Agent_Tool.neighborhood[1] == 0) || (Agent_Tool.neighborhood[1] == 2)))
                                            {
                                                if (Agent_Tool.neighborhood[1] == 2)//Caso que tenga que moverse a un hueco
                                                {
                                                    World_Data = Agent_Tool.Agent_Restore(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                    Agent_Tool = (Agente)Event_List[i];
                                                    coordenadas.X = Agent_Tool.Xpos * 15;
                                                    coordenadas.Y = Agent_Tool.Ypos * 15;
                                                    switch (World_Data[Agent_Tool.Ypos][Agent_Tool.Xpos])
                                                    {
                                                        case (4):
                                                            g.DrawImage(A_Arriba_PNG, coordenadas);
                                                            break;
                                                        case (5):
                                                            g.DrawImage(A_Abajo_PNG, coordenadas);
                                                            break;
                                                        case (6):
                                                            g.DrawImage(A_Izquierda_PNG, coordenadas);
                                                            break;
                                                        case (7):
                                                            g.DrawImage(A_Derecha_PNG, coordenadas);
                                                            break;
                                                    }
                                                    break;
                                                }
                                                //El agente ya tiene un giro a favor de las agujas del reloj(Aquí se sustituyó turn_left por move_up)
                                                World_Data = Agent_Tool.move_up(World_Data, Event_List, (Agente)Event_List[i], false, Lambda_Exp, Lambda_Huecos);
                                                Agent_Tool = (Agente)Event_List[i];
                                                coordenadas.X = Agent_Tool.Xpos * 15;
                                                coordenadas.Y = Agent_Tool.Ypos * 15;
                                                g.DrawImage(A_Arriba_PNG, coordenadas);
                                            }
                                            else
                                            {//Initial
                                                World_Data = Agent_Tool.turn_left(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                Agent_Tool = (Agente)Event_List[i];
                                                coordenadas.X = Agent_Tool.Xpos * 15;
                                                coordenadas.Y = Agent_Tool.Ypos * 15;
                                                g.DrawImage(A_Izquierda_PNG, coordenadas);
                                            }
                                            break;
                                        }
//@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
                                        if ((Agent_Tool.neighborhood[3] == 1) && //Si tiene traste a la derecha y está push 
                                  (Agent_Tool.detect_pushing_trast(World_Data, Agent_Tool.Ypos, Agent_Tool.Xpos + 1, 3) == true))
                                        {
                                            if ((Agent_Tool.neighborhood[1] == 0) || (Agent_Tool.neighborhood[1] == 2))
                                            {
                                                coordenadas.X = Agent_Tool.Xpos * 15;
                                                coordenadas.Y = Agent_Tool.Ypos * 15;
                                                rect = new Rectangle(coordenadas, medida);
                                                Drawing_Area.Invalidate(rect);
                                                Drawing_Area.Update();
                                                if (Agent_Tool.neighborhood[1] == 2)//Caso que tenga que moverse a un hueco
                                                {
                                                    World_Data = Agent_Tool.Agent_Restore(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                    Agent_Tool = (Agente)Event_List[i];
                                                    coordenadas.X = Agent_Tool.Xpos * 15;
                                                    coordenadas.Y = Agent_Tool.Ypos * 15;
                                                    switch (World_Data[Agent_Tool.Ypos][Agent_Tool.Xpos])
                                                    {
                                                        case (4):
                                                            g.DrawImage(A_Arriba_PNG, coordenadas);
                                                            break;
                                                        case (5):
                                                            g.DrawImage(A_Abajo_PNG, coordenadas);
                                                            break;
                                                        case (6):
                                                            g.DrawImage(A_Izquierda_PNG, coordenadas);
                                                            break;
                                                        case (7):
                                                            g.DrawImage(A_Derecha_PNG, coordenadas);
                                                            break;
                                                    }
                                                    break;
                                                }
                                                World_Data = Agent_Tool.move_up(World_Data, Event_List, (Agente)Event_List[i], false, Lambda_Exp, Lambda_Huecos);
                                                Agent_Tool = (Agente)Event_List[i];
                                                coordenadas.X = Agent_Tool.Xpos * 15;
                                                coordenadas.Y = Agent_Tool.Ypos * 15;
                                                g.DrawImage(A_Arriba_PNG, coordenadas);
                                                break;
                                            }
                                            else
                                            {
                                                World_Data = Agent_Tool.turn_left(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                Agent_Tool = (Agente)Event_List[i];
                                                coordenadas.X = Agent_Tool.Xpos * 15;
                                                coordenadas.Y = Agent_Tool.Ypos * 15;
                                                g.DrawImage(A_Izquierda_PNG, coordenadas);
                                                break;
                                            }
                                        }
                                        if ((Agent_Tool.neighborhood[7] == 1) && //Si tiene traste a la izquierda y está push 
                                  (Agent_Tool.detect_pushing_trast(World_Data, Agent_Tool.Ypos, Agent_Tool.Xpos - 1, 1) == true))
                                        {
                                            if ((Agent_Tool.neighborhood[1] == 0) || (Agent_Tool.neighborhood[1] == 2))
                                            {
                                                coordenadas.X = Agent_Tool.Xpos * 15;
                                                coordenadas.Y = Agent_Tool.Ypos * 15;
                                                rect = new Rectangle(coordenadas, medida);
                                                Drawing_Area.Invalidate(rect);
                                                Drawing_Area.Update();
                                                if (Agent_Tool.neighborhood[1] == 2)//Caso que tenga que moverse a un hueco
                                                {
                                                    World_Data = Agent_Tool.Agent_Restore(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                    Agent_Tool = (Agente)Event_List[i];
                                                    coordenadas.X = Agent_Tool.Xpos * 15;
                                                    coordenadas.Y = Agent_Tool.Ypos * 15;
                                                    switch (World_Data[Agent_Tool.Ypos][Agent_Tool.Xpos])
                                                    {
                                                        case (4):
                                                            g.DrawImage(A_Arriba_PNG, coordenadas);
                                                            break;
                                                        case (5):
                                                            g.DrawImage(A_Abajo_PNG, coordenadas);
                                                            break;
                                                        case (6):
                                                            g.DrawImage(A_Izquierda_PNG, coordenadas);
                                                            break;
                                                        case (7):
                                                            g.DrawImage(A_Derecha_PNG, coordenadas);
                                                            break;
                                                    }
                                                    break;
                                                }
                                                World_Data = Agent_Tool.move_up(World_Data, Event_List, (Agente)Event_List[i], false, Lambda_Exp, Lambda_Huecos);
                                                Agent_Tool = (Agente)Event_List[i];
                                                coordenadas.X = Agent_Tool.Xpos * 15;
                                                coordenadas.Y = Agent_Tool.Ypos * 15;
                                                g.DrawImage(A_Arriba_PNG, coordenadas);
                                                break;
                                            }
                                            else
                                            {
                                                World_Data = Agent_Tool.turn_rigth(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                Agent_Tool = (Agente)Event_List[i];
                                                coordenadas.X = Agent_Tool.Xpos * 15;
                                                coordenadas.Y = Agent_Tool.Ypos * 15;
                                                g.DrawImage(A_Derecha_PNG, coordenadas);
                                                break;
                                            }
                                        }
//@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@                                       
                                    }
                                    else
                                    {//Caso en que el agente cae en un hueco
                                        if (Agent_Tool.neighborhood[1] == 2)
                                        {
                                            coordenadas.X = Agent_Tool.Xpos * 15;
                                            coordenadas.Y = Agent_Tool.Ypos * 15;
                                            rect = new Rectangle(coordenadas, medida);
                                            Drawing_Area.Invalidate(rect);
                                            Drawing_Area.Update();
                                            World_Data = Agent_Tool.Agent_Restore(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                            Agent_Tool = (Agente)Event_List[i];
                                            coordenadas.X = Agent_Tool.Xpos * 15;
                                            coordenadas.Y = Agent_Tool.Ypos * 15;
                                            switch (World_Data[Agent_Tool.Ypos][Agent_Tool.Xpos])
                                            {
                                                case (4):
                                                    g.DrawImage(A_Arriba_PNG, coordenadas);
                                                    break;
                                                case (5):
                                                    g.DrawImage(A_Abajo_PNG, coordenadas);
                                                    break;
                                                case (6):
                                                    g.DrawImage(A_Izquierda_PNG, coordenadas);
                                                    break;
                                                case (7):
                                                    g.DrawImage(A_Derecha_PNG, coordenadas);
                                                    break;
                                            }
                                        }
                                        else
                                        {//Caso en que el agente no tiene ni traste ni hueco delante@@@@@@@@@@@@@@@@@@@@1
                                            if (Agent_Tool.neighborhood[7] == 3)
                                            {
                                                if (Agent_Tool.Clock_inverse == true)
                                                    {//El agente ya tiene un giro a favor de las agujas del reloj
                                                        World_Data = Agent_Tool.turn_left(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                        Agent_Tool = (Agente)Event_List[i];
                                                        coordenadas.X = Agent_Tool.Xpos * 15;
                                                        coordenadas.Y = Agent_Tool.Ypos * 15;
                                                        g.DrawImage(A_Izquierda_PNG, coordenadas);
                                                    }
                                                    else
                                                    {//Initial
                                                        World_Data = Agent_Tool.turn_rigth(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                        Agent_Tool = (Agente)Event_List[i];
                                                        coordenadas.X = Agent_Tool.Xpos * 15;
                                                        coordenadas.Y = Agent_Tool.Ypos * 15;
                                                        g.DrawImage(A_Derecha_PNG, coordenadas);
                                                    }
                                                break;
                                            }
                                            if (Agent_Tool.neighborhood[3] == 3)
                                            {
                                                if (Agent_Tool.Clock_sense == true)
                                                {//El agente ya tiene un giro en contra de las agujas del reloj
                                                    World_Data = Agent_Tool.turn_rigth(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                    Agent_Tool = (Agente)Event_List[i];
                                                    coordenadas.X = Agent_Tool.Xpos * 15;
                                                    coordenadas.Y = Agent_Tool.Ypos * 15;
                                                    g.DrawImage(A_Derecha_PNG, coordenadas);
                                                }
                                                else
                                                {//Initial
                                                    World_Data = Agent_Tool.turn_left(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                    Agent_Tool = (Agente)Event_List[i];
                                                    coordenadas.X = Agent_Tool.Xpos * 15;
                                                    coordenadas.Y = Agent_Tool.Ypos * 15;
                                                    g.DrawImage(A_Izquierda_PNG, coordenadas);
                                                }
                                                break;
                                            }
                                         //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
                                            int b = turn.Next(2);
                                            switch (b)
                                            {
                                                case (0):
                                                    if (Agent_Tool.Clock_inverse == true)
                                                    {//El agente ya tiene un giro en contra de las agujas del reloj
                                                        World_Data = Agent_Tool.turn_left(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                        Agent_Tool = (Agente)Event_List[i];
                                                        coordenadas.X = Agent_Tool.Xpos * 15;
                                                        coordenadas.Y = Agent_Tool.Ypos * 15;
                                                        g.DrawImage(A_Izquierda_PNG, coordenadas);
                                                    }
                                                    else
                                                    {//Initial
                                                        World_Data = Agent_Tool.turn_rigth(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                        Agent_Tool = (Agente)Event_List[i];
                                                        coordenadas.X = Agent_Tool.Xpos * 15;
                                                        coordenadas.Y = Agent_Tool.Ypos * 15;
                                                        g.DrawImage(A_Derecha_PNG, coordenadas);
                                                    }
                                                    break;
                                                case (1):
                                                    if (Agent_Tool.Clock_sense == true)
                                                    {//El agente ya tiene un giro a favor de las agujas del reloj
                                                        World_Data = Agent_Tool.turn_rigth(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                        Agent_Tool = (Agente)Event_List[i];
                                                        coordenadas.X = Agent_Tool.Xpos * 15;
                                                        coordenadas.Y = Agent_Tool.Ypos * 15;
                                                        g.DrawImage(A_Derecha_PNG, coordenadas);
                                                    }
                                                    else
                                                    {//Initial
                                                        World_Data = Agent_Tool.turn_left(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                        Agent_Tool = (Agente)Event_List[i];
                                                        coordenadas.X = Agent_Tool.Xpos * 15;
                                                        coordenadas.Y = Agent_Tool.Ypos * 15;
                                                        g.DrawImage(A_Izquierda_PNG, coordenadas);
                                                    }
                                                    break;
                                            }
                                        }
                                    }
                                }
                            }
                           break;
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////HACIA ABAJO///////////////////////////////////////////////////
                        case(1)://Agente orientado hacia abajo
                           Agent_Tool.get_neighborhood(World_Data, Agent_Tool.Xpos, Agent_Tool.Ypos);
                           //Si el agente tiene un traste delante de él y este traste no está siendo empujado.
                           if ((Agent_Tool.neighborhood[5] == 1) &&
                               (Agent_Tool.detect_pushing_trast(World_Data, Agent_Tool.Ypos + 1, Agent_Tool.Xpos, 0) == false))
                           {
                               if (Agent_Tool.Ypos == 20)//Si está frente del traste y luego tiene el límite del mundo
                               {
                                   if (Agent_Tool.Clock_sense == true)
                                   {
                                       World_Data = Agent_Tool.turn_left(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                       Agent_Tool = (Agente)Event_List[i];
                                       coordenadas.X = Agent_Tool.Xpos * 15;
                                       coordenadas.Y = Agent_Tool.Ypos * 15;
                                       g.DrawImage(A_Izquierda_PNG, coordenadas);
                                       break;
                                   }
                                   else
                                   {
                                       World_Data = Agent_Tool.turn_rigth(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                       Agent_Tool = (Agente)Event_List[i];
                                       coordenadas.X = Agent_Tool.Xpos * 15;
                                       coordenadas.Y = Agent_Tool.Ypos * 15;
                                       g.DrawImage(A_Derecha_PNG, coordenadas);
                                       break;
                                   }
                               }
                               if ((Agent_Tool.neighborhood[4] == 2) && (Agent_Tool.neighborhood[7] != 3) &&
                                    (Agent_Tool.neighborhood[6] != 3))
                               {
                                   if (Agent_Tool.Clock_inverse == false)
                                   {
                                       World_Data = Agent_Tool.turn_left(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                       Agent_Tool = (Agente)Event_List[i];
                                       coordenadas.X = Agent_Tool.Xpos * 15;
                                       coordenadas.Y = Agent_Tool.Ypos * 15;
                                       g.DrawImage(A_Izquierda_PNG, coordenadas);
                                       break;
                                   }
                               }
                               if ((Agent_Tool.neighborhood[6] == 2) && (Agent_Tool.neighborhood[3] != 3) &&
                                   (Agent_Tool.neighborhood[4] != 3))
                               {
                                   if (Agent_Tool.Clock_sense == false)
                                   {
                                       World_Data = Agent_Tool.turn_rigth(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                       Agent_Tool = (Agente)Event_List[i];
                                       coordenadas.X = Agent_Tool.Xpos * 15;
                                       coordenadas.Y = Agent_Tool.Ypos * 15;
                                       g.DrawImage(A_Derecha_PNG, coordenadas);
                                       break;
                                   }
                               }
                               if (Agent_Tool.Ypos <= 18)//(21-3)
                               {
                                   //El agente sólo moverá el traste si la casilla siguiente es hueco o vacía y la casilla
                                   if ((World_Data[Agent_Tool.Ypos + 2][Agent_Tool.Xpos] == 0) &&//siguiente no es ni
                                       ((World_Data[Agent_Tool.Ypos + 3][Agent_Tool.Xpos] != 3) &&//traste ni muro.
                                       (World_Data[Agent_Tool.Ypos + 3][Agent_Tool.Xpos] != 1)))
                                   {
                                       ////////////////////////////////////////Sección de código para borrar el agente///////////////////////////////////////////
                                       coordenadas.X = Agent_Tool.Xpos * 15;
                                       coordenadas.Y = Agent_Tool.Ypos * 15;
                                       rect = new Rectangle(coordenadas, medida);
                                       Drawing_Area.Invalidate(rect);
                                       Drawing_Area.Update();
                                       /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                       ////////////////////////////////////////Sección de código para borrar el traste/////////////////////////////////////////
                                       coordenadas.Y = (Agent_Tool.Ypos + 1) * 15;
                                       rect = new Rectangle(coordenadas, medida);
                                       Drawing_Area.Invalidate(rect);
                                       Drawing_Area.Update();
                                       ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                       ////////////////////////////////////////Actualizar la matix de datos y reasignar el agente a Agent_Tool/////////////////
                                       World_Data = Agent_Tool.move_down(World_Data, Event_List, (Agente)Event_List[i], true, Lambda_Exp, Lambda_Huecos);
                                       Agent_Tool = (Agente)Event_List[i];
                                       ////////////////////////////////////////Sección de código que pinta nuevamente el traste y el agente////////////////////
                                       coordenadas.X = Agent_Tool.Xpos * 15;
                                       coordenadas.Y = Agent_Tool.Ypos * 15;
                                       g.DrawImage(A_Abajo_T_PNG, coordenadas);
                                       coordenadas.Y = (Agent_Tool.Ypos + 1) * 15;
                                       g.DrawImage(Traste_PNG, coordenadas);
                                       ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                   }
                                   else
                                   { //Si lo próximo que viene es un hueco y el agente está empujando el traste.
                                       if (World_Data[Agent_Tool.Ypos + 2][Agent_Tool.Xpos] == 2)
                                       {
                                           coordenadas.X = Agent_Tool.Xpos * 15;
                                           coordenadas.Y = Agent_Tool.Ypos * 15;
                                           rect = new Rectangle(coordenadas, medida);
                                           Drawing_Area.Invalidate(rect);
                                           Drawing_Area.Update();
                                           coordenadas.X = Agent_Tool.Xpos * 15;
                                           coordenadas.Y = (Agent_Tool.Ypos + 1) * 15;
                                           rect = new Rectangle(coordenadas, medida);
                                           Drawing_Area.Invalidate(rect);
                                           Drawing_Area.Update();
                                           World_Data = Agent_Tool.move_down(World_Data, Event_List, (Agente)Event_List[i], true, Lambda_Exp, Lambda_Huecos);
                                           Agent_Tool = (Agente)Event_List[i];
                                           coordenadas.X = Agent_Tool.Xpos * 15;
                                           coordenadas.Y = Agent_Tool.Ypos * 15;
                                           g.DrawImage(A_Abajo_T_PNG, coordenadas);
                                           coordenadas.Y = (Agent_Tool.Ypos + 1) * 15;
                                           g.DrawImage(Traste_PNG, coordenadas);
                                           rect = new Rectangle(coordenadas, medida);
                                           Drawing_Area.Invalidate(rect);
                                           Drawing_Area.Update();
                                           //Tratamiento de huecos//
                                           for (int y = 0; y < World_Data.Length; y++)
                                           {
                                               for (int x = 0; x < World_Data[0].Length; x++)
                                               {
                                                   if (World_Data[y][x] == 20)
                                                   {
                                                       World_Data[y][x] = 2;
                                                       coordenadas.X = x * 15;
                                                       coordenadas.Y = y * 15;
                                                       g.DrawImage(Hueco_PNG, coordenadas);
                                                   }
                                               }
                                           }
                                           break;
                                       }
                                       //Si hay traste doble o el agente se encuentra con un muro en uno de los extremos
                                       if ((World_Data[Agent_Tool.Ypos + 2][Agent_Tool.Xpos] == 1) ||
                                           ((World_Data[Agent_Tool.Ypos + 1][Agent_Tool.Xpos] == 1) &&
                                           (World_Data[Agent_Tool.Ypos + 2][Agent_Tool.Xpos] == 3)) ||
                                           (World_Data[Agent_Tool.Ypos + 3][Agent_Tool.Xpos] == 3) ||
                                           (World_Data[Agent_Tool.Ypos + 3][Agent_Tool.Xpos] == 1))
                                       {//Si el agente está en el extremo derecho del mundo
                                           if ((Agent_Tool.neighborhood[4] == 3) && (Agent_Tool.neighborhood[7] != 3))
                                           {//Si ya tiene un giro contrario a las agujas del reloj entonces turn rigth
                                               if (Agent_Tool.Clock_inverse == true)
                                               {
                                                   World_Data = Agent_Tool.turn_rigth(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                   Agent_Tool = (Agente)Event_List[i];
                                                   coordenadas.X = Agent_Tool.Xpos * 15;
                                                   coordenadas.Y = Agent_Tool.Ypos * 15;
                                                   g.DrawImage(A_Derecha_PNG, coordenadas);
                                               }
                                               else
                                               {//Inicial
                                                   World_Data = Agent_Tool.turn_left(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                   Agent_Tool = (Agente)Event_List[i];
                                                   coordenadas.X = Agent_Tool.Xpos * 15;
                                                   coordenadas.Y = Agent_Tool.Ypos * 15;
                                                   g.DrawImage(A_Izquierda_PNG, coordenadas);
                                               }
                                           }
                                           else
                                           {//Si el agente está en el extremo Izquierdo del mundo
                                               if ((Agent_Tool.neighborhood[6] == 3) && (Agent_Tool.neighborhood[3] != 3))
                                               {
                                                   if (Agent_Tool.Clock_sense == true)
                                                   {//Si ya tiene un giro siguiendo las agujas del reloj entonces turn izq
                                                       World_Data = Agent_Tool.turn_left(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                       Agent_Tool = (Agente)Event_List[i];
                                                       coordenadas.X = Agent_Tool.Xpos * 15;
                                                       coordenadas.Y = Agent_Tool.Ypos * 15;
                                                       g.DrawImage(A_Izquierda_PNG, coordenadas);
                                                   }
                                                   else//Initial
                                                   {
                                                       World_Data = Agent_Tool.turn_rigth(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                       Agent_Tool = (Agente)Event_List[i];
                                                       coordenadas.X = Agent_Tool.Xpos * 15;
                                                       coordenadas.Y = Agent_Tool.Ypos * 15;
                                                       g.DrawImage(A_Derecha_PNG, coordenadas);
                                                   }
                                               }
                                               else
                                               {
                                                   int a = turn.Next(2);//Si no está en ningún extremo del mundo
                                                   switch (a)
                                                   {
                                                       case (0):
                                                           if (Agent_Tool.Clock_inverse == true)
                                                           {//Si ya tiene un giro en contra de las agujas del reloj
                                                               World_Data = Agent_Tool.turn_rigth(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                               Agent_Tool = (Agente)Event_List[i];
                                                               coordenadas.X = Agent_Tool.Xpos * 15;
                                                               coordenadas.Y = Agent_Tool.Ypos * 15;
                                                               g.DrawImage(A_Derecha_PNG, coordenadas);
                                                           }
                                                           else
                                                           {//Initial
                                                               World_Data = Agent_Tool.turn_left(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                               Agent_Tool = (Agente)Event_List[i];
                                                               coordenadas.X = Agent_Tool.Xpos * 15;
                                                               coordenadas.Y = Agent_Tool.Ypos * 15;
                                                               g.DrawImage(A_Izquierda_PNG, coordenadas);
                                                           }
                                                           break;
                                                       case (1):
                                                           if (Agent_Tool.Clock_sense == true)
                                                           {//Si ya tiene un giro a favor de las agujas del reloj
                                                               World_Data = Agent_Tool.turn_left(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                               Agent_Tool = (Agente)Event_List[i];
                                                               coordenadas.X = Agent_Tool.Xpos * 15;
                                                               coordenadas.Y = Agent_Tool.Ypos * 15;
                                                               g.DrawImage(A_Izquierda_PNG, coordenadas);
                                                           }
                                                           else
                                                           {//Initial
                                                               World_Data = Agent_Tool.turn_rigth(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                               Agent_Tool = (Agente)Event_List[i];
                                                               coordenadas.X = Agent_Tool.Xpos * 15;
                                                               coordenadas.Y = Agent_Tool.Ypos * 15;
                                                               g.DrawImage(A_Derecha_PNG, coordenadas);
                                                           }
                                                           break;
                                                   }
                                               }
                                           }
                                           break;
                                       }

                                   }
                               }
                               else
                               {//Caso en que hay un hueco en el extremo inferior y el agente se aproxima con un traste
                                   if ((Agent_Tool.Old_Ypos <= 19) && (World_Data[Agent_Tool.Ypos + 2][Agent_Tool.Xpos] == 2))//(21-2)=19 inferiormente. Porque 0+2 superiormente.
                                   {
                                       coordenadas.X = Agent_Tool.Xpos * 15;
                                       coordenadas.Y = Agent_Tool.Ypos * 15;
                                       rect = new Rectangle(coordenadas, medida);
                                       Drawing_Area.Invalidate(rect);
                                       Drawing_Area.Update();
                                       coordenadas.X = Agent_Tool.Xpos * 15;
                                       coordenadas.Y = (Agent_Tool.Ypos + 1) * 15;
                                       rect = new Rectangle(coordenadas, medida);
                                       Drawing_Area.Invalidate(rect);
                                       Drawing_Area.Update();
                                       World_Data = Agent_Tool.move_down(World_Data, Event_List, (Agente)Event_List[i], true, Lambda_Exp, Lambda_Huecos);
                                       Agent_Tool = (Agente)Event_List[i];
                                       coordenadas.X = Agent_Tool.Xpos * 15;
                                       coordenadas.Y = Agent_Tool.Ypos * 15;
                                       g.DrawImage(A_Abajo_T_PNG, coordenadas);
                                       coordenadas.Y = (Agent_Tool.Ypos + 1) * 15;
                                       g.DrawImage(Traste_PNG, coordenadas);
                                       rect = new Rectangle(coordenadas, medida);
                                       Drawing_Area.Invalidate(rect);
                                       Drawing_Area.Update();
                                       //Tratamiento de huecos//
                                       for (int y = 0; y < World_Data.Length; y++)
                                       {
                                           for (int x = 0; x < World_Data[0].Length; x++)
                                           {
                                               if (World_Data[y][x] == 20)
                                               {
                                                   World_Data[y][x] = 2;
                                                   coordenadas.X = x * 15;
                                                   coordenadas.Y = y * 15;
                                                   g.DrawImage(Hueco_PNG, coordenadas);
                                               }
                                           }
                                       }
                                   }
                                   else
                                   {
                                       if (Agent_Tool.neighborhood[4] == 3)//Si el agente está en el extremo derecho del mundo
                                       {
                                           if (Agent_Tool.Clock_inverse == true)
                                           {//Si el agente ya tiene un giro en contra de las agujas del reloj
                                               World_Data = Agent_Tool.turn_rigth(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                               Agent_Tool = (Agente)Event_List[i];
                                               coordenadas.X = Agent_Tool.Xpos * 15;
                                               coordenadas.Y = Agent_Tool.Ypos * 15;
                                               g.DrawImage(A_Derecha_PNG, coordenadas);
                                           }
                                           else
                                           {//Initial
                                               World_Data = Agent_Tool.turn_left(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                               Agent_Tool = (Agente)Event_List[i];
                                               coordenadas.X = Agent_Tool.Xpos * 15;
                                               coordenadas.Y = Agent_Tool.Ypos * 15;
                                               g.DrawImage(A_Izquierda_PNG, coordenadas);
                                           }
                                       }
                                       else
                                       {
                                           if (Agent_Tool.neighborhood[6] == 3)//Si el agente está en el extremo izquierdo del mundo
                                           {
                                               if (Agent_Tool.Clock_sense == true)
                                               {//Si el agente ya tiene un giro a favor de las agujas del reloj
                                                   World_Data = Agent_Tool.turn_left(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                   Agent_Tool = (Agente)Event_List[i];
                                                   coordenadas.X = Agent_Tool.Xpos * 15;
                                                   coordenadas.Y = Agent_Tool.Ypos * 15;
                                                   g.DrawImage(A_Izquierda_PNG, coordenadas);
                                               }
                                               else
                                               {//Initial
                                                   World_Data = Agent_Tool.turn_rigth(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                   Agent_Tool = (Agente)Event_List[i];
                                                   coordenadas.X = Agent_Tool.Xpos * 15;
                                                   coordenadas.Y = Agent_Tool.Ypos * 15;
                                                   g.DrawImage(A_Derecha_PNG, coordenadas);
                                               }
                                           }
                                           else
                                           {
                                               int a = turn.Next(2);//Si no está en ningún extremo del mundo
                                               switch (a)
                                               {
                                                   case (0):
                                                       if (Agent_Tool.Clock_inverse == true)
                                                       {//Si el agente ya tiene un giro en contra de las agujas del reloj
                                                           World_Data = Agent_Tool.turn_rigth(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                           Agent_Tool = (Agente)Event_List[i];
                                                           coordenadas.X = Agent_Tool.Xpos * 15;
                                                           coordenadas.Y = Agent_Tool.Ypos * 15;
                                                           g.DrawImage(A_Derecha_PNG, coordenadas);
                                                       }
                                                       else
                                                       {//Initial
                                                           World_Data = Agent_Tool.turn_left(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                           Agent_Tool = (Agente)Event_List[i];
                                                           coordenadas.X = Agent_Tool.Xpos * 15;
                                                           coordenadas.Y = Agent_Tool.Ypos * 15;
                                                           g.DrawImage(A_Izquierda_PNG, coordenadas);
                                                       }
                                                       break;
                                                   case (1):
                                                       if (Agent_Tool.Clock_sense == true)
                                                       {//Si el agente ya tiene un giro en sentido de las agujas del reloj
                                                           World_Data = Agent_Tool.turn_left(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                           Agent_Tool = (Agente)Event_List[i];
                                                           coordenadas.X = Agent_Tool.Xpos * 15;
                                                           coordenadas.Y = Agent_Tool.Ypos * 15;
                                                           g.DrawImage(A_Izquierda_PNG, coordenadas);
                                                       }
                                                       else
                                                       {//Initial
                                                           World_Data = Agent_Tool.turn_rigth(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                           Agent_Tool = (Agente)Event_List[i];
                                                           coordenadas.X = Agent_Tool.Xpos * 15;
                                                           coordenadas.Y = Agent_Tool.Ypos * 15;
                                                           g.DrawImage(A_Derecha_PNG, coordenadas);
                                                       }
                                                       break;
                                               }
                                           }
                                       }
                                   }
                               }
//$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$
                               if (Agent_Tool.Ypos <= 19)//(21-2)
                               {
                                   //Si hay un traste intentando empujar el traste por el lado contrario
                                   if (World_Data[Agent_Tool.Ypos + 2][Agent_Tool.Xpos] == 4)
                                   {
                                       coordenadas.X = Agent_Tool.Xpos * 15;
                                       coordenadas.Y = Agent_Tool.Ypos * 15;
                                       rect = new Rectangle(coordenadas, medida);
                                       Drawing_Area.Invalidate(rect);
                                       Drawing_Area.Update();
                                       if ((Agent_Tool.neighborhood[7] == 1) && //Gira a la izquierda si hay un traste a la izq
                               (Agent_Tool.detect_pushing_trast(World_Data, Agent_Tool.Ypos, Agent_Tool.Xpos - 1, 1) == false))
                                       {
                                           if (Agent_Tool.Clock_inverse == true)
                                           {//El agente ya tiene un giro a favor de las agujas del reloj
                                               World_Data = Agent_Tool.turn_rigth(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                               Agent_Tool = (Agente)Event_List[i];
                                               coordenadas.X = Agent_Tool.Xpos * 15;
                                               coordenadas.Y = Agent_Tool.Ypos * 15;
                                               g.DrawImage(A_Derecha_PNG, coordenadas);
                                           }
                                           else
                                           {//Initial
                                               World_Data = Agent_Tool.turn_left(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                               Agent_Tool = (Agente)Event_List[i];
                                               coordenadas.X = Agent_Tool.Xpos * 15;
                                               coordenadas.Y = Agent_Tool.Ypos * 15;
                                               g.DrawImage(A_Izquierda_PNG, coordenadas);
                                           }
                                       }
                                       else
                                       {
                                           if ((Agent_Tool.neighborhood[3] == 1) &&//Gira a la derecha si hay un traste a la derech
                                  (Agent_Tool.detect_pushing_trast(World_Data, Agent_Tool.Ypos, Agent_Tool.Xpos + 1, 3) == false))
                                           {
                                               if (Agent_Tool.Clock_sense == true)
                                               {//El agente ya tiene un giro en contra de las agujas del reloj
                                                   World_Data = Agent_Tool.turn_left(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                   Agent_Tool = (Agente)Event_List[i];
                                                   coordenadas.X = Agent_Tool.Xpos * 15;
                                                   coordenadas.Y = Agent_Tool.Ypos * 15;
                                                   g.DrawImage(A_Izquierda_PNG, coordenadas);
                                               }
                                               else
                                               {//Initial
                                                   World_Data = Agent_Tool.turn_rigth(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                   Agent_Tool = (Agente)Event_List[i];
                                                   coordenadas.X = Agent_Tool.Xpos * 15;
                                                   coordenadas.Y = Agent_Tool.Ypos * 15;
                                                   g.DrawImage(A_Derecha_PNG, coordenadas);
                                               }
                                           }
                                           else
                                           {
                                               int a = turn.Next(2);
                                               switch (a)
                                               {
                                                   case (0):
                                                       if (Agent_Tool.Clock_sense == true)
                                                       {//El agente ya tiene un giro en contra de las agujas del reloj
                                                           World_Data = Agent_Tool.turn_left(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                           Agent_Tool = (Agente)Event_List[i];
                                                           coordenadas.X = Agent_Tool.Xpos * 15;
                                                           coordenadas.Y = Agent_Tool.Ypos * 15;
                                                           g.DrawImage(A_Izquierda_PNG, coordenadas);
                                                       }
                                                       else
                                                       {//Initial
                                                           World_Data = Agent_Tool.turn_rigth(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                           Agent_Tool = (Agente)Event_List[i];
                                                           coordenadas.X = Agent_Tool.Xpos * 15;
                                                           coordenadas.Y = Agent_Tool.Ypos * 15;
                                                           g.DrawImage(A_Derecha_PNG, coordenadas);
                                                       }
                                                       break;
                                                   case (1):
                                                       if (Agent_Tool.Clock_inverse == true)
                                                       {//El agente ya tiene un giro a favor de las agujas del reloj
                                                           World_Data = Agent_Tool.turn_rigth(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                           Agent_Tool = (Agente)Event_List[i];
                                                           coordenadas.X = Agent_Tool.Xpos * 15;
                                                           coordenadas.Y = Agent_Tool.Ypos * 15;
                                                           g.DrawImage(A_Derecha_PNG, coordenadas);
                                                       }
                                                       else
                                                       {//Initial
                                                           World_Data = Agent_Tool.turn_left(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                           Agent_Tool = (Agente)Event_List[i];
                                                           coordenadas.X = Agent_Tool.Xpos * 15;
                                                           coordenadas.Y = Agent_Tool.Ypos * 15;
                                                           g.DrawImage(A_Izquierda_PNG, coordenadas);
                                                       }
                                                       break;
                                               }
                                           }
                                       }

                                   }
                               }
//$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$
                           }
                           else
                           {   //Si el traste que tiene delante ya lo está empujando otro agente.
                               if ((Agent_Tool.neighborhood[5] == 1) &&
                               (Agent_Tool.detect_pushing_trast(World_Data, Agent_Tool.Ypos + 1, Agent_Tool.Xpos, 0) == true))
                               {
                                   coordenadas.X = Agent_Tool.Xpos * 15;
                                   coordenadas.Y = Agent_Tool.Ypos * 15;
                                   rect = new Rectangle(coordenadas, medida);
                                   Drawing_Area.Invalidate(rect);
                                   Drawing_Area.Update();
                                   if ((Agent_Tool.neighborhood[3] == 1) && //Gira a la derecha si hay un traste a la derecha
                           (Agent_Tool.detect_pushing_trast(World_Data, Agent_Tool.Ypos, Agent_Tool.Xpos + 1, 3) == false))
                                   {
                                       if (Agent_Tool.Clock_sense == true)
                                       {//El agente ya tiene un giro a favor de las agujas del reloj
                                           World_Data = Agent_Tool.turn_left(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                           Agent_Tool = (Agente)Event_List[i];
                                           coordenadas.X = Agent_Tool.Xpos * 15;
                                           coordenadas.Y = Agent_Tool.Ypos * 15;
                                           g.DrawImage(A_Izquierda_PNG, coordenadas);
                                       }
                                       else
                                       {//Initial
                                           World_Data = Agent_Tool.turn_rigth(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                           Agent_Tool = (Agente)Event_List[i];
                                           coordenadas.X = Agent_Tool.Xpos * 15;
                                           coordenadas.Y = Agent_Tool.Ypos * 15;
                                           g.DrawImage(A_Derecha_PNG, coordenadas);
                                       }
                                   }
                                   else
                                   {
                                       if ((Agent_Tool.neighborhood[7] == 1) &&//Gira a la izquierda si hay un traste a la izquierda
                              (Agent_Tool.detect_pushing_trast(World_Data, Agent_Tool.Ypos, Agent_Tool.Xpos - 1, 1) == false))
                                       {
                                           if (Agent_Tool.Clock_inverse == true)
                                           {//El agente ya tiene un giro en contra de las agujas del reloj
                                               World_Data = Agent_Tool.turn_rigth(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                               Agent_Tool = (Agente)Event_List[i];
                                               coordenadas.X = Agent_Tool.Xpos * 15;
                                               coordenadas.Y = Agent_Tool.Ypos * 15;
                                               g.DrawImage(A_Derecha_PNG, coordenadas);
                                           }
                                           else
                                           {//Initial
                                               World_Data = Agent_Tool.turn_left(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                               Agent_Tool = (Agente)Event_List[i];
                                               coordenadas.X = Agent_Tool.Xpos * 15;
                                               coordenadas.Y = Agent_Tool.Ypos * 15;
                                               g.DrawImage(A_Izquierda_PNG, coordenadas);
                                           }
                                       }
                                       else
                                       {
                                           int a = turn.Next(2);
                                           switch (a)
                                           {
                                               case (0):
                                                   if (Agent_Tool.Clock_inverse == true)
                                                   {//El agente ya tiene un giro en contra de las agujas del reloj
                                                       World_Data = Agent_Tool.turn_rigth(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                       Agent_Tool = (Agente)Event_List[i];
                                                       coordenadas.X = Agent_Tool.Xpos * 15;
                                                       coordenadas.Y = Agent_Tool.Ypos * 15;
                                                       g.DrawImage(A_Derecha_PNG, coordenadas);
                                                   }
                                                   else
                                                   {//Initial
                                                       World_Data = Agent_Tool.turn_left(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                       Agent_Tool = (Agente)Event_List[i];
                                                       coordenadas.X = Agent_Tool.Xpos * 15;
                                                       coordenadas.Y = Agent_Tool.Ypos * 15;
                                                       g.DrawImage(A_Izquierda_PNG, coordenadas);
                                                   }
                                                   break;
                                               case (1):
                                                   if (Agent_Tool.Clock_sense == true)
                                                   {//El agente ya tiene un giro a favor de las agujas del reloj
                                                       World_Data = Agent_Tool.turn_left(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                       Agent_Tool = (Agente)Event_List[i];
                                                       coordenadas.X = Agent_Tool.Xpos * 15;
                                                       coordenadas.Y = Agent_Tool.Ypos * 15;
                                                       g.DrawImage(A_Izquierda_PNG, coordenadas);
                                                   }
                                                   else
                                                   {//Initial
                                                       World_Data = Agent_Tool.turn_rigth(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                       Agent_Tool = (Agente)Event_List[i];
                                                       coordenadas.X = Agent_Tool.Xpos * 15;
                                                       coordenadas.Y = Agent_Tool.Ypos * 15;
                                                       g.DrawImage(A_Derecha_PNG, coordenadas);
                                                   }
                                                   break;
                                           }
                                       }
                                   }

                               }
                               //Si el agente no tiene traste frente ni a los lados y tiene algún traste
                               //en las vecindades diagonales inferiores.
//CORRECCIÓN AGREGANDO(Agent_Tool.neighborhood[5] == 2) PARA EL CASO QUE HAYA UN HUECO DELANTE
                               if (((Agent_Tool.neighborhood[5] == 2) || (Agent_Tool.neighborhood[5] == 0)) && 
                                   ((Agent_Tool.neighborhood[0] == 1) || (Agent_Tool.neighborhood[2] == 1)))
                               {
                                   if ((Agent_Tool.neighborhood[0] == 1) && //Si tiene traste a la izquierda diagonal inferior y no está push 
                                 (Agent_Tool.detect_pushing_trast(World_Data, Agent_Tool.Ypos - 1, Agent_Tool.Xpos - 1, 7) == false))
                                   {
                                       coordenadas.X = Agent_Tool.Xpos * 15;
                                       coordenadas.Y = Agent_Tool.Ypos * 15;
                                       rect = new Rectangle(coordenadas, medida);
                                       Drawing_Area.Invalidate(rect);
                                       Drawing_Area.Update();
                                       if (Agent_Tool.Clock_inverse == true)
                                       {
                                           if (Agent_Tool.neighborhood[5] == 2)//Caso que tenga que moverse a un hueco
                                           {
                                               World_Data = Agent_Tool.Agent_Restore(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                               Agent_Tool = (Agente)Event_List[i];
                                               coordenadas.X = Agent_Tool.Xpos * 15;
                                               coordenadas.Y = Agent_Tool.Ypos * 15;
                                               switch (World_Data[Agent_Tool.Ypos][Agent_Tool.Xpos])
                                               {
                                                   case (4):
                                                       g.DrawImage(A_Arriba_PNG, coordenadas);
                                                       break;
                                                   case (5):
                                                       g.DrawImage(A_Abajo_PNG, coordenadas);
                                                       break;
                                                   case (6):
                                                       g.DrawImage(A_Izquierda_PNG, coordenadas);
                                                       break;
                                                   case (7):
                                                       g.DrawImage(A_Derecha_PNG, coordenadas);
                                                       break;
                                               }
                                               break;
                                           }
                                           //El agente ya tiene un giro en contra de las agujas del reloj (Aquí se sustituyó turn_rigth por move_down)
                                           World_Data = Agent_Tool.move_down(World_Data, Event_List, (Agente)Event_List[i], false, Lambda_Exp, Lambda_Huecos);
                                           Agent_Tool = (Agente)Event_List[i];
                                           coordenadas.X = Agent_Tool.Xpos * 15;
                                           coordenadas.Y = Agent_Tool.Ypos * 15;
                                           g.DrawImage(A_Abajo_PNG, coordenadas);
                                       }
                                       else
                                       {//Initial
                                           World_Data = Agent_Tool.turn_left(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                           Agent_Tool = (Agente)Event_List[i];
                                           coordenadas.X = Agent_Tool.Xpos * 15;
                                           coordenadas.Y = Agent_Tool.Ypos * 15;
                                           g.DrawImage(A_Izquierda_PNG, coordenadas);
                                       }
                                       break;
                                   }
                                   if ((Agent_Tool.neighborhood[2] == 1) && //Si tiene traste a la derecha diagonal inferior y no está push 
                                 (Agent_Tool.detect_pushing_trast(World_Data, Agent_Tool.Ypos - 1, Agent_Tool.Xpos + 1, 7) == false))
                                   {
                                       coordenadas.X = Agent_Tool.Xpos * 15;
                                       coordenadas.Y = Agent_Tool.Ypos * 15;
                                       rect = new Rectangle(coordenadas, medida);
                                       Drawing_Area.Invalidate(rect);
                                       Drawing_Area.Update();
                                       if (Agent_Tool.Clock_sense == true)
                                       {
                                           if (Agent_Tool.neighborhood[5] == 2)//Caso que tenga que moverse a un hueco
                                           {
                                               World_Data = Agent_Tool.Agent_Restore(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                               Agent_Tool = (Agente)Event_List[i];
                                               coordenadas.X = Agent_Tool.Xpos * 15;
                                               coordenadas.Y = Agent_Tool.Ypos * 15;
                                               switch (World_Data[Agent_Tool.Ypos][Agent_Tool.Xpos])
                                               {
                                                   case (4):
                                                       g.DrawImage(A_Arriba_PNG, coordenadas);
                                                       break;
                                                   case (5):
                                                       g.DrawImage(A_Abajo_PNG, coordenadas);
                                                       break;
                                                   case (6):
                                                       g.DrawImage(A_Izquierda_PNG, coordenadas);
                                                       break;
                                                   case (7):
                                                       g.DrawImage(A_Derecha_PNG, coordenadas);
                                                       break;
                                               }
                                               break;
                                           }
                                           //El agente ya tiene un giro a favor de las agujas del reloj(Aquí se sustituyó turn_rigth por move_down)
                                           World_Data = Agent_Tool.move_down(World_Data, Event_List, (Agente)Event_List[i], false, Lambda_Exp, Lambda_Huecos);
                                           Agent_Tool = (Agente)Event_List[i];
                                           coordenadas.X = Agent_Tool.Xpos * 15;
                                           coordenadas.Y = Agent_Tool.Ypos * 15;
                                           g.DrawImage(A_Abajo_PNG, coordenadas);
                                       }
                                       else
                                       {//Initial
                                           World_Data = Agent_Tool.turn_rigth(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                           Agent_Tool = (Agente)Event_List[i];
                                           coordenadas.X = Agent_Tool.Xpos * 15;
                                           coordenadas.Y = Agent_Tool.Ypos * 15;
                                           g.DrawImage(A_Derecha_PNG, coordenadas);
                                       }
                                       break;
                                   }
                               }
                               //Si el agente no tiene traste frente ni a los lados.
                               Agent_Tool.get_neighborhood(World_Data, Agent_Tool.Xpos, Agent_Tool.Ypos);
                               if ((Agent_Tool.neighborhood[5] == 0) && (Agent_Tool.neighborhood[3] != 1) &&
                                   (Agent_Tool.neighborhood[7] != 1))
                               {
                                   coordenadas.X = Agent_Tool.Xpos * 15;
                                   coordenadas.Y = Agent_Tool.Ypos * 15;
                                   rect = new Rectangle(coordenadas, medida);
                                   Drawing_Area.Invalidate(rect);
                                   Drawing_Area.Update();
                                   World_Data = Agent_Tool.move_down(World_Data, Event_List, (Agente)Event_List[i], false, Lambda_Exp, Lambda_Huecos);
                                   Agent_Tool = (Agente)Event_List[i];
                                   coordenadas.X = Agent_Tool.Xpos * 15;
                                   coordenadas.Y = Agent_Tool.Ypos * 15;
                                   g.DrawImage(A_Abajo_PNG, coordenadas);
                               }
                               else
                               {
                                   //Si el agente no tiene traste frente pero tiene alguno a los lados.
                                   if ((Agent_Tool.neighborhood[5] != 1) && ((Agent_Tool.neighborhood[3] == 1) ||
                                       (Agent_Tool.neighborhood[7] == 1)))
                                   {
                                       if ((Agent_Tool.neighborhood[7] == 1) && //Si tiene traste a la izquierda y no está push 
                                 (Agent_Tool.detect_pushing_trast(World_Data, Agent_Tool.Ypos, Agent_Tool.Xpos - 1, 1) == false))
                                       {
                                           coordenadas.X = Agent_Tool.Xpos * 15;
                                           coordenadas.Y = Agent_Tool.Ypos * 15;
                                           rect = new Rectangle(coordenadas, medida);
                                           Drawing_Area.Invalidate(rect);
                                           Drawing_Area.Update();
                                           if ((Agent_Tool.Clock_sense == true) &&
                                               ((Agent_Tool.neighborhood[5] == 0) || (Agent_Tool.neighborhood[5] == 2)))
                                           {
                                               if (Agent_Tool.neighborhood[5] == 2)//Caso que tenga que moverse a un hueco
                                               {
                                                   World_Data = Agent_Tool.Agent_Restore(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                   Agent_Tool = (Agente)Event_List[i];
                                                   coordenadas.X = Agent_Tool.Xpos * 15;
                                                   coordenadas.Y = Agent_Tool.Ypos * 15;
                                                   switch (World_Data[Agent_Tool.Ypos][Agent_Tool.Xpos])
                                                   {
                                                       case (4):
                                                           g.DrawImage(A_Arriba_PNG, coordenadas);
                                                           break;
                                                       case (5):
                                                           g.DrawImage(A_Abajo_PNG, coordenadas);
                                                           break;
                                                       case (6):
                                                           g.DrawImage(A_Izquierda_PNG, coordenadas);
                                                           break;
                                                       case (7):
                                                           g.DrawImage(A_Derecha_PNG, coordenadas);
                                                           break;
                                                   }
                                                   break;
                                               }
                                               //El agente ya tiene un giro en contra de las agujas del reloj (Aquí se sustituyó turn_rigth por move_down)
                                               World_Data = Agent_Tool.move_down(World_Data, Event_List, (Agente)Event_List[i], false, Lambda_Exp, Lambda_Huecos);
                                               Agent_Tool = (Agente)Event_List[i];
                                               coordenadas.X = Agent_Tool.Xpos * 15;
                                               coordenadas.Y = Agent_Tool.Ypos * 15;
                                               g.DrawImage(A_Abajo_PNG, coordenadas);
                                           }
                                           else
                                           {//Initial
                                               World_Data = Agent_Tool.turn_left(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                               Agent_Tool = (Agente)Event_List[i];
                                               coordenadas.X = Agent_Tool.Xpos * 15;
                                               coordenadas.Y = Agent_Tool.Ypos * 15;
                                               g.DrawImage(A_Izquierda_PNG, coordenadas);
                                           }
                                           break;
                                       }
                                       if ((Agent_Tool.neighborhood[3] == 1) && //Si tiene traste a la izquierda y no está push 
                                 (Agent_Tool.detect_pushing_trast(World_Data, Agent_Tool.Ypos, Agent_Tool.Xpos + 1, 3) == false))
                                       {
                                           coordenadas.X = Agent_Tool.Xpos * 15;
                                           coordenadas.Y = Agent_Tool.Ypos * 15;
                                           rect = new Rectangle(coordenadas, medida);
                                           Drawing_Area.Invalidate(rect);
                                           Drawing_Area.Update();
                                           if ((Agent_Tool.Clock_sense == true) && 
                                               ((Agent_Tool.neighborhood[5] == 0) || (Agent_Tool.neighborhood[5] == 2)))
                                           {
                                               if (Agent_Tool.neighborhood[5] == 2)//Caso que tenga que moverse a un hueco
                                            {
                                                World_Data = Agent_Tool.Agent_Restore(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                Agent_Tool = (Agente)Event_List[i];
                                                coordenadas.X = Agent_Tool.Xpos * 15;
                                                coordenadas.Y = Agent_Tool.Ypos * 15;
                                                switch (World_Data[Agent_Tool.Ypos][Agent_Tool.Xpos])
                                                {
                                                    case (4):
                                                        g.DrawImage(A_Arriba_PNG, coordenadas);
                                                        break;
                                                    case (5):
                                                        g.DrawImage(A_Abajo_PNG, coordenadas);
                                                        break;
                                                    case (6):
                                                        g.DrawImage(A_Izquierda_PNG, coordenadas);
                                                        break;
                                                    case (7):
                                                        g.DrawImage(A_Derecha_PNG, coordenadas);
                                                        break;
                                                }
                                                break;
                                            }
                                               //El agente ya tiene un giro a favor de las agujas del reloj(Aquí se sustituyó turn_rigth por move_down)
                                               World_Data = Agent_Tool.move_down(World_Data, Event_List, (Agente)Event_List[i], false, Lambda_Exp, Lambda_Huecos);
                                               Agent_Tool = (Agente)Event_List[i];
                                               coordenadas.X = Agent_Tool.Xpos * 15;
                                               coordenadas.Y = Agent_Tool.Ypos * 15;
                                               g.DrawImage(A_Abajo_PNG, coordenadas);
                                           }
                                           else
                                           {//Initial
                                               World_Data = Agent_Tool.turn_rigth(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                               Agent_Tool = (Agente)Event_List[i];
                                               coordenadas.X = Agent_Tool.Xpos * 15;
                                               coordenadas.Y = Agent_Tool.Ypos * 15;
                                               g.DrawImage(A_Derecha_PNG, coordenadas);
                                           }
                                           break;
                                       }
//@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
                                       if ((Agent_Tool.neighborhood[7] == 1) && //Si tiene traste a la izquierda y está push 
                                 (Agent_Tool.detect_pushing_trast(World_Data, Agent_Tool.Ypos, Agent_Tool.Xpos - 1, 1) == true))
                                       {
                                           if ((Agent_Tool.neighborhood[5] == 0) || ((Agent_Tool.neighborhood[5] == 2)))
                                           {
                                               coordenadas.X = Agent_Tool.Xpos * 15;
                                               coordenadas.Y = Agent_Tool.Ypos * 15;
                                               rect = new Rectangle(coordenadas, medida);
                                               Drawing_Area.Invalidate(rect);
                                               Drawing_Area.Update();
                                               if (Agent_Tool.neighborhood[5] == 2)//Caso que tenga que moverse a un hueco
                                               {
                                                   World_Data = Agent_Tool.Agent_Restore(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                   Agent_Tool = (Agente)Event_List[i];
                                                   coordenadas.X = Agent_Tool.Xpos * 15;
                                                   coordenadas.Y = Agent_Tool.Ypos * 15;
                                                   switch (World_Data[Agent_Tool.Ypos][Agent_Tool.Xpos])
                                                   {
                                                       case (4):
                                                           g.DrawImage(A_Arriba_PNG, coordenadas);
                                                           break;
                                                       case (5):
                                                           g.DrawImage(A_Abajo_PNG, coordenadas);
                                                           break;
                                                       case (6):
                                                           g.DrawImage(A_Izquierda_PNG, coordenadas);
                                                           break;
                                                       case (7):
                                                           g.DrawImage(A_Derecha_PNG, coordenadas);
                                                           break;
                                                   }
                                                   break;
                                               }
                                               World_Data = Agent_Tool.move_down(World_Data, Event_List, (Agente)Event_List[i], false, Lambda_Exp, Lambda_Huecos);
                                               Agent_Tool = (Agente)Event_List[i];
                                               coordenadas.X = Agent_Tool.Xpos * 15;
                                               coordenadas.Y = Agent_Tool.Ypos * 15;
                                               g.DrawImage(A_Abajo_PNG, coordenadas);
                                               break;
                                           }
                                           else 
                                           {
                                               World_Data = Agent_Tool.turn_rigth(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                               Agent_Tool = (Agente)Event_List[i];
                                               coordenadas.X = Agent_Tool.Xpos * 15;
                                               coordenadas.Y = Agent_Tool.Ypos * 15;
                                               g.DrawImage(A_Derecha_PNG, coordenadas);
                                               break;
                                           }
                                       }
                                       if ((Agent_Tool.neighborhood[3] == 1) && //Si tiene traste a la derecha y está push 
                                 (Agent_Tool.detect_pushing_trast(World_Data, Agent_Tool.Ypos, Agent_Tool.Xpos + 1, 3) == true))
                                       {
                                           if ((Agent_Tool.neighborhood[5] == 0) || (Agent_Tool.neighborhood[5] == 2))
                                           {
                                               coordenadas.X = Agent_Tool.Xpos * 15;
                                               coordenadas.Y = Agent_Tool.Ypos * 15;
                                               rect = new Rectangle(coordenadas, medida);
                                               Drawing_Area.Invalidate(rect);
                                               Drawing_Area.Update();
                                               if (Agent_Tool.neighborhood[5] == 2)//Caso que tenga que moverse a un hueco
                                               {
                                                   World_Data = Agent_Tool.Agent_Restore(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                   Agent_Tool = (Agente)Event_List[i];
                                                   coordenadas.X = Agent_Tool.Xpos * 15;
                                                   coordenadas.Y = Agent_Tool.Ypos * 15;
                                                   switch (World_Data[Agent_Tool.Ypos][Agent_Tool.Xpos])
                                                   {
                                                       case (4):
                                                           g.DrawImage(A_Arriba_PNG, coordenadas);
                                                           break;
                                                       case (5):
                                                           g.DrawImage(A_Abajo_PNG, coordenadas);
                                                           break;
                                                       case (6):
                                                           g.DrawImage(A_Izquierda_PNG, coordenadas);
                                                           break;
                                                       case (7):
                                                           g.DrawImage(A_Derecha_PNG, coordenadas);
                                                           break;
                                                   }
                                                   break;
                                               }
                                               World_Data = Agent_Tool.move_down(World_Data, Event_List, (Agente)Event_List[i], false, Lambda_Exp, Lambda_Huecos);
                                               Agent_Tool = (Agente)Event_List[i];
                                               coordenadas.X = Agent_Tool.Xpos * 15;
                                               coordenadas.Y = Agent_Tool.Ypos * 15;
                                               g.DrawImage(A_Abajo_PNG, coordenadas);
                                               break;
                                           }
                                           else
                                           {
                                               World_Data = Agent_Tool.turn_left(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                               Agent_Tool = (Agente)Event_List[i];
                                               coordenadas.X = Agent_Tool.Xpos * 15;
                                               coordenadas.Y = Agent_Tool.Ypos * 15;
                                               g.DrawImage(A_Izquierda_PNG, coordenadas);
                                               break;
                                           }
                                       }
//@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
                                   }
                                   else
                                   {//Caso en que el agente cae en un hueco
                                       if (Agent_Tool.neighborhood[5] == 2)
                                       {
                                           coordenadas.X = Agent_Tool.Xpos * 15;
                                           coordenadas.Y = Agent_Tool.Ypos * 15;
                                           rect = new Rectangle(coordenadas, medida);
                                           Drawing_Area.Invalidate(rect);
                                           Drawing_Area.Update();
                                           World_Data = Agent_Tool.Agent_Restore(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                           Agent_Tool = (Agente)Event_List[i];
                                           coordenadas.X = Agent_Tool.Xpos * 15;
                                           coordenadas.Y = Agent_Tool.Ypos * 15;
                                           switch (World_Data[Agent_Tool.Ypos][Agent_Tool.Xpos])
                                           {
                                               case (4):
                                                   g.DrawImage(A_Arriba_PNG, coordenadas);
                                                   break;
                                               case (5):
                                                   g.DrawImage(A_Abajo_PNG, coordenadas);
                                                   break;
                                               case (6):
                                                   g.DrawImage(A_Izquierda_PNG, coordenadas);
                                                   break;
                                               case (7):
                                                   g.DrawImage(A_Derecha_PNG, coordenadas);
                                                   break;
                                           }
                                       }
                                       else
                                       {//Caso en que el agente no tiene ni traste ni hueco delante@@@@@@@@@@@@@@@@@@@@
                                           if (Agent_Tool.neighborhood[7] == 3)
                                            {
                                                if (Agent_Tool.Clock_sense == true)
                                                    {//El agente ya tiene un giro a favor de las agujas del reloj
                                                        World_Data = Agent_Tool.turn_left(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                        Agent_Tool = (Agente)Event_List[i];
                                                        coordenadas.X = Agent_Tool.Xpos * 15;
                                                        coordenadas.Y = Agent_Tool.Ypos * 15;
                                                        g.DrawImage(A_Izquierda_PNG, coordenadas);
                                                    }
                                                    else
                                                    {//Initial
                                                        World_Data = Agent_Tool.turn_rigth(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                        Agent_Tool = (Agente)Event_List[i];
                                                        coordenadas.X = Agent_Tool.Xpos * 15;
                                                        coordenadas.Y = Agent_Tool.Ypos * 15;
                                                        g.DrawImage(A_Derecha_PNG, coordenadas);
                                                    }
                                                break;
                                            }
                                           if (Agent_Tool.neighborhood[3] == 3)
                                           {
                                               if (Agent_Tool.Clock_sense == true)
                                               {//El agente ya tiene un giro en contra de las agujas del reloj
                                                   World_Data = Agent_Tool.turn_rigth(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                   Agent_Tool = (Agente)Event_List[i];
                                                   coordenadas.X = Agent_Tool.Xpos * 15;
                                                   coordenadas.Y = Agent_Tool.Ypos * 15;
                                                   g.DrawImage(A_Derecha_PNG, coordenadas);
                                               }
                                               else
                                               {//Initial
                                                   World_Data = Agent_Tool.turn_left(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                   Agent_Tool = (Agente)Event_List[i];
                                                   coordenadas.X = Agent_Tool.Xpos * 15;
                                                   coordenadas.Y = Agent_Tool.Ypos * 15;
                                                   g.DrawImage(A_Izquierda_PNG, coordenadas);
                                               }
                                               break;
                                           }
                                           //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
                                           int b = turn.Next(2);
                                           switch (b)
                                           {
                                               case (0):
                                                   if (Agent_Tool.Clock_inverse == true)
                                                   {//El agente ya tiene un giro en contra de las agujas del reloj
                                                       World_Data = Agent_Tool.turn_rigth(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                       Agent_Tool = (Agente)Event_List[i];
                                                       coordenadas.X = Agent_Tool.Xpos * 15;
                                                       coordenadas.Y = Agent_Tool.Ypos * 15;
                                                       g.DrawImage(A_Derecha_PNG, coordenadas);
                                                   }
                                                   else
                                                   {//Initial
                                                       World_Data = Agent_Tool.turn_left(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                       Agent_Tool = (Agente)Event_List[i];
                                                       coordenadas.X = Agent_Tool.Xpos * 15;
                                                       coordenadas.Y = Agent_Tool.Ypos * 15;
                                                       g.DrawImage(A_Izquierda_PNG, coordenadas);
                                                   }
                                                   break;
                                               case (1):
                                                   if (Agent_Tool.Clock_sense == true)
                                                   {//El agente ya tiene un giro a favor de las agujas del reloj
                                                       World_Data = Agent_Tool.turn_left(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                       Agent_Tool = (Agente)Event_List[i];
                                                       coordenadas.X = Agent_Tool.Xpos * 15;
                                                       coordenadas.Y = Agent_Tool.Ypos * 15;
                                                       g.DrawImage(A_Izquierda_PNG, coordenadas);
                                                   }
                                                   else
                                                   {//Initial
                                                       World_Data = Agent_Tool.turn_rigth(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                       Agent_Tool = (Agente)Event_List[i];
                                                       coordenadas.X = Agent_Tool.Xpos * 15;
                                                       coordenadas.Y = Agent_Tool.Ypos * 15;
                                                       g.DrawImage(A_Derecha_PNG, coordenadas);
                                                   }
                                                   break;
                                           }
                                       }
                                   }
                               }
                           }
                           break;
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////HACIA LA IZQUIERDA//////////////////////////////////////////////////////////////////
                        case(2)://Agente orientado hacia la izquierda.
                           Agent_Tool.get_neighborhood(World_Data, Agent_Tool.Xpos, Agent_Tool.Ypos);//Actualizar vecindad
                           //Si el agente tiene un traste delante de él y este traste no está siendo empujado.
                           if ((Agent_Tool.neighborhood[7] == 1) &&
                               (Agent_Tool.detect_pushing_trast(World_Data, Agent_Tool.Ypos, Agent_Tool.Xpos - 1, 1) == false))
                           {
                               if (Agent_Tool.Xpos == 1)//Si está frente del traste y luego tiene el límite del mundo
                               {
                                   if (Agent_Tool.Clock_sense == true)
                                   {
                                       World_Data = Agent_Tool.turn_up(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                       Agent_Tool = (Agente)Event_List[i];
                                       coordenadas.X = Agent_Tool.Xpos * 15;
                                       coordenadas.Y = Agent_Tool.Ypos * 15;
                                       g.DrawImage(A_Arriba_PNG, coordenadas);
                                       break;
                                   }
                                   else
                                   {
                                       World_Data = Agent_Tool.turn_down(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                       Agent_Tool = (Agente)Event_List[i];
                                       coordenadas.X = Agent_Tool.Xpos * 15;
                                       coordenadas.Y = Agent_Tool.Ypos * 15;
                                       g.DrawImage(A_Abajo_PNG, coordenadas);
                                       break;
                                   }
                               }
                               if ((Agent_Tool.neighborhood[0] == 2) && (Agent_Tool.neighborhood[5] != 3) &&
                                    (Agent_Tool.neighborhood[6] != 3))
                               {
                                   if (Agent_Tool.Clock_sense == false)
                                   {
                                       World_Data = Agent_Tool.turn_down(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                       Agent_Tool = (Agente)Event_List[i];
                                       coordenadas.X = Agent_Tool.Xpos * 15;
                                       coordenadas.Y = Agent_Tool.Ypos * 15;
                                       g.DrawImage(A_Abajo_PNG, coordenadas);
                                       break;
                                   }
                               }
                               if ((Agent_Tool.neighborhood[6] == 2) && (Agent_Tool.neighborhood[0] != 3) &&
                                   (Agent_Tool.neighborhood[1] != 3))
                               {
                                   if (Agent_Tool.Clock_inverse == false)
                                   {
                                       World_Data = Agent_Tool.turn_up(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                       Agent_Tool = (Agente)Event_List[i];
                                       coordenadas.X = Agent_Tool.Xpos * 15;
                                       coordenadas.Y = Agent_Tool.Ypos * 15;
                                       g.DrawImage(A_Arriba_PNG, coordenadas);
                                       break;
                                   }
                               }
                               if (Agent_Tool.Xpos >= 3)
                               {
                                   //El agente sólo moverá el traste si la casilla siguiente es hueco o vacía y la casilla
                                   if ((World_Data[Agent_Tool.Ypos][Agent_Tool.Xpos - 2] == 0) &&//siguiente no es ni
                                       ((World_Data[Agent_Tool.Ypos][Agent_Tool.Xpos - 3] != 3) &&//traste ni muro.
                                       (World_Data[Agent_Tool.Ypos][Agent_Tool.Xpos - 3] != 1)))
                                   {
                                       ////////////////////////////////////////Sección de código para borrar el agente///////////////////////////////////////////
                                       coordenadas.X = Agent_Tool.Xpos * 15;
                                       coordenadas.Y = Agent_Tool.Ypos * 15;
                                       rect = new Rectangle(coordenadas, medida);
                                       Drawing_Area.Invalidate(rect);
                                       Drawing_Area.Update();
                                       /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                       ////////////////////////////////////////Sección de código para borrar el traste/////////////////////////////////////////
                                       coordenadas.X = (Agent_Tool.Xpos - 1) * 15;
                                       rect = new Rectangle(coordenadas, medida);
                                       Drawing_Area.Invalidate(rect);
                                       Drawing_Area.Update();
                                       ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                       ////////////////////////////////////////Actualizar la matix de datos y reasignar el agente a Agent_Tool/////////////////
                                       World_Data = Agent_Tool.move_left(World_Data, Event_List, (Agente)Event_List[i], true, Lambda_Exp, Lambda_Huecos);
                                       Agent_Tool = (Agente)Event_List[i];
                                       ////////////////////////////////////////Sección de código que pinta nuevamente el traste y el agente////////////////////
                                       coordenadas.X = Agent_Tool.Xpos * 15;
                                       coordenadas.Y = Agent_Tool.Ypos * 15;
                                       g.DrawImage(A_Izquierda_T_PNG, coordenadas);
                                       coordenadas.X = (Agent_Tool.Xpos - 1) * 15;
                                       g.DrawImage(Traste_PNG, coordenadas);
                                       ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                   }
                                   else
                                   { //Si lo próximo que viene es un hueco y el agente está empujando el traste.
                                       if (World_Data[Agent_Tool.Ypos][Agent_Tool.Xpos - 2] == 2)
                                       {
                                           coordenadas.X = Agent_Tool.Xpos * 15;
                                           coordenadas.Y = Agent_Tool.Ypos * 15;
                                           rect = new Rectangle(coordenadas, medida);
                                           Drawing_Area.Invalidate(rect);
                                           Drawing_Area.Update();
                                           coordenadas.Y = Agent_Tool.Ypos * 15;
                                           coordenadas.X = (Agent_Tool.Xpos - 1) * 15;
                                           rect = new Rectangle(coordenadas, medida);
                                           Drawing_Area.Invalidate(rect);
                                           Drawing_Area.Update();
                                           World_Data = Agent_Tool.move_left(World_Data, Event_List, (Agente)Event_List[i], true, Lambda_Exp, Lambda_Huecos);
                                           Agent_Tool = (Agente)Event_List[i];
                                           coordenadas.X = Agent_Tool.Xpos * 15;
                                           coordenadas.Y = Agent_Tool.Ypos * 15;
                                           g.DrawImage(A_Izquierda_T_PNG, coordenadas);
                                           coordenadas.X = (Agent_Tool.Xpos - 1) * 15;
                                           g.DrawImage(Traste_PNG, coordenadas);
                                           rect = new Rectangle(coordenadas, medida);
                                           Drawing_Area.Invalidate(rect);
                                           Drawing_Area.Update();
                                           //Tratamiento de huecos//
                                           for (int y = 0; y < World_Data.Length; y++)
                                           {
                                               for (int x = 0; x < World_Data[0].Length; x++)
                                               {
                                                   if (World_Data[y][x] == 20)
                                                   {
                                                       World_Data[y][x] = 2;
                                                       coordenadas.X = x * 15;
                                                       coordenadas.Y = y * 15;
                                                       g.DrawImage(Hueco_PNG, coordenadas);
                                                   }
                                               }
                                           }
                                           break;
                                       }
                                       //Si hay traste doble o el agente se encuentra con un muro en uno de los extremos
                                       if ((World_Data[Agent_Tool.Ypos][Agent_Tool.Xpos - 2] == 1) ||
                                           ((World_Data[Agent_Tool.Ypos][Agent_Tool.Xpos - 1] == 1) &&
                                           (World_Data[Agent_Tool.Ypos][Agent_Tool.Xpos - 2] == 3)) ||
                                           (World_Data[Agent_Tool.Ypos][Agent_Tool.Xpos - 3] == 3) ||
                                           (World_Data[Agent_Tool.Ypos][Agent_Tool.Xpos - 3] == 1))
                                       {//Si el agente está en el extremo inferior del mundo
                                           if ((Agent_Tool.neighborhood[6] == 3) && (Agent_Tool.neighborhood[1] != 3))
                                           {//Si ya tiene un giro contrario a las agujas del reloj entonces turn left
                                               if (Agent_Tool.Clock_inverse == true)
                                               {
                                                   World_Data = Agent_Tool.turn_down(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                   Agent_Tool = (Agente)Event_List[i];
                                                   coordenadas.X = Agent_Tool.Xpos * 15;
                                                   coordenadas.Y = Agent_Tool.Ypos * 15;
                                                   g.DrawImage(A_Abajo_PNG, coordenadas);
                                               }
                                               else
                                               {//Inicial
                                                   World_Data = Agent_Tool.turn_up(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                   Agent_Tool = (Agente)Event_List[i];
                                                   coordenadas.X = Agent_Tool.Xpos * 15;
                                                   coordenadas.Y = Agent_Tool.Ypos * 15;
                                                   g.DrawImage(A_Arriba_PNG, coordenadas);
                                               }
                                           }
                                           else
                                           {//Si el agente está en el extremo superior del mundo
                                               if ((Agent_Tool.neighborhood[0] == 3) && (Agent_Tool.neighborhood[5] != 3))
                                               {
                                                   if (Agent_Tool.Clock_sense == true)
                                                   {//Si ya tiene un giro siguiendo las agujas del reloj entonces turn izq
                                                       World_Data = Agent_Tool.turn_up(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                       Agent_Tool = (Agente)Event_List[i];
                                                       coordenadas.X = Agent_Tool.Xpos * 15;
                                                       coordenadas.Y = Agent_Tool.Ypos * 15;
                                                       g.DrawImage(A_Arriba_PNG, coordenadas);
                                                   }
                                                   else//Initial
                                                   {
                                                       World_Data = Agent_Tool.turn_down(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                       Agent_Tool = (Agente)Event_List[i];
                                                       coordenadas.X = Agent_Tool.Xpos * 15;
                                                       coordenadas.Y = Agent_Tool.Ypos * 15;
                                                       g.DrawImage(A_Abajo_PNG, coordenadas);
                                                   }
                                               }
                                               else
                                               {
                                                   int a = turn.Next(2);//Si no está en ningún extremo del mundo
                                                   switch (a)
                                                   {
                                                       case (0):
                                                           if (Agent_Tool.Clock_inverse == true)
                                                           {//Si ya tiene un giro en contra de las agujas del reloj
                                                               World_Data = Agent_Tool.turn_up(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                               Agent_Tool = (Agente)Event_List[i];
                                                               coordenadas.X = Agent_Tool.Xpos * 15;
                                                               coordenadas.Y = Agent_Tool.Ypos * 15;
                                                               g.DrawImage(A_Arriba_PNG, coordenadas);
                                                           }
                                                           else
                                                           {//Initial
                                                               World_Data = Agent_Tool.turn_down(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                               Agent_Tool = (Agente)Event_List[i];
                                                               coordenadas.X = Agent_Tool.Xpos * 15;
                                                               coordenadas.Y = Agent_Tool.Ypos * 15;
                                                               g.DrawImage(A_Abajo_PNG, coordenadas);
                                                           }
                                                           break;
                                                       case (1):
                                                           if (Agent_Tool.Clock_sense == true)
                                                           {//Si ya tiene un giro a favor de las agujas del reloj
                                                               World_Data = Agent_Tool.turn_down(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                               Agent_Tool = (Agente)Event_List[i];
                                                               coordenadas.X = Agent_Tool.Xpos * 15;
                                                               coordenadas.Y = Agent_Tool.Ypos * 15;
                                                               g.DrawImage(A_Abajo_PNG, coordenadas);
                                                           }
                                                           else
                                                           {//Initial
                                                               World_Data = Agent_Tool.turn_up(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                               Agent_Tool = (Agente)Event_List[i];
                                                               coordenadas.X = Agent_Tool.Xpos * 15;
                                                               coordenadas.Y = Agent_Tool.Ypos * 15;
                                                               g.DrawImage(A_Arriba_PNG, coordenadas);
                                                           }
                                                           break;
                                                   }
                                               }
                                           }
                                           break;
                                       }

                                   }
                               }
                               else
                               {//Caso en que hay un hueco en el extremo superior y el agente se aproxima con un traste
                                   if ((Agent_Tool.Old_Xpos >= 2) && (World_Data[Agent_Tool.Ypos][Agent_Tool.Xpos - 2] == 2))
                                   {
                                       coordenadas.X = Agent_Tool.Xpos * 15;
                                       coordenadas.Y = Agent_Tool.Ypos * 15;
                                       rect = new Rectangle(coordenadas, medida);
                                       Drawing_Area.Invalidate(rect);
                                       Drawing_Area.Update();
                                       coordenadas.Y = Agent_Tool.Ypos * 15;
                                       coordenadas.X = (Agent_Tool.Xpos - 1) * 15;
                                       rect = new Rectangle(coordenadas, medida);
                                       Drawing_Area.Invalidate(rect);
                                       Drawing_Area.Update();
                                       World_Data = Agent_Tool.move_left(World_Data, Event_List, (Agente)Event_List[i], true, Lambda_Exp, Lambda_Huecos);
                                       Agent_Tool = (Agente)Event_List[i];
                                       coordenadas.X = Agent_Tool.Xpos * 15;
                                       coordenadas.Y = Agent_Tool.Ypos * 15;
                                       g.DrawImage(A_Izquierda_T_PNG, coordenadas);
                                       coordenadas.X = (Agent_Tool.Xpos - 1) * 15;
                                       g.DrawImage(Traste_PNG, coordenadas);
                                       rect = new Rectangle(coordenadas, medida);
                                       Drawing_Area.Invalidate(rect);
                                       Drawing_Area.Update();
                                       //Tratamiento de huecos//
                                       for (int y = 0; y < World_Data.Length; y++)
                                       {
                                           for (int x = 0; x < World_Data[0].Length; x++)
                                           {
                                               if (World_Data[y][x] == 20)
                                               {
                                                   World_Data[y][x] = 2;
                                                   coordenadas.X = x * 15;
                                                   coordenadas.Y = y * 15;
                                                   g.DrawImage(Hueco_PNG, coordenadas);
                                               }
                                           }
                                       }
                                   }
                                   else
                                   {
                                       if (Agent_Tool.neighborhood[6] == 3)//Si el agente está en el extremo inferior del mundo
                                       {
                                           if (Agent_Tool.Clock_inverse == true)
                                           {//Si el agente ya tiene un giro en contra de las agujas del reloj
                                               World_Data = Agent_Tool.turn_down(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                               Agent_Tool = (Agente)Event_List[i];
                                               coordenadas.X = Agent_Tool.Xpos * 15;
                                               coordenadas.Y = Agent_Tool.Ypos * 15;
                                               g.DrawImage(A_Abajo_PNG, coordenadas);
                                           }
                                           else
                                           {//Initial
                                               World_Data = Agent_Tool.turn_up(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                               Agent_Tool = (Agente)Event_List[i];
                                               coordenadas.X = Agent_Tool.Xpos * 15;
                                               coordenadas.Y = Agent_Tool.Ypos * 15;
                                               g.DrawImage(A_Arriba_PNG, coordenadas);
                                           }
                                       }
                                       else
                                       {
                                           if (Agent_Tool.neighborhood[0] == 3)//Si el agente está en el extremo superior del mundo
                                           {
                                               if (Agent_Tool.Clock_sense == true)
                                               {//Si el agente ya tiene un giro a favor de las agujas del reloj
                                                   World_Data = Agent_Tool.turn_up(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                   Agent_Tool = (Agente)Event_List[i];
                                                   coordenadas.X = Agent_Tool.Xpos * 15;
                                                   coordenadas.Y = Agent_Tool.Ypos * 15;
                                                   g.DrawImage(A_Arriba_PNG, coordenadas);
                                               }
                                               else
                                               {//Initial
                                                   World_Data = Agent_Tool.turn_down(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                   Agent_Tool = (Agente)Event_List[i];
                                                   coordenadas.X = Agent_Tool.Xpos * 15;
                                                   coordenadas.Y = Agent_Tool.Ypos * 15;
                                                   g.DrawImage(A_Abajo_PNG, coordenadas);
                                               }
                                           }
                                           else
                                           {
                                               int a = turn.Next(2);//Si no está en ningún extremo del mundo
                                               switch (a)
                                               {
                                                   case (0):
                                                       if (Agent_Tool.Clock_inverse == true)
                                                       {//Si el agente ya tiene un giro en contra de las agujas del reloj
                                                           World_Data = Agent_Tool.turn_down(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                           Agent_Tool = (Agente)Event_List[i];
                                                           coordenadas.X = Agent_Tool.Xpos * 15;
                                                           coordenadas.Y = Agent_Tool.Ypos * 15;
                                                           g.DrawImage(A_Abajo_PNG, coordenadas);
                                                       }
                                                       else
                                                       {//Initial
                                                           World_Data = Agent_Tool.turn_up(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                           Agent_Tool = (Agente)Event_List[i];
                                                           coordenadas.X = Agent_Tool.Xpos * 15;
                                                           coordenadas.Y = Agent_Tool.Ypos * 15;
                                                           g.DrawImage(A_Arriba_PNG, coordenadas);
                                                       }
                                                       break;
                                                   case (1):
                                                       if (Agent_Tool.Clock_sense == true)
                                                       {//Si el agente ya tiene un giro en sentido de las agujas del reloj
                                                           World_Data = Agent_Tool.turn_up(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                           Agent_Tool = (Agente)Event_List[i];
                                                           coordenadas.X = Agent_Tool.Xpos * 15;
                                                           coordenadas.Y = Agent_Tool.Ypos * 15;
                                                           g.DrawImage(A_Arriba_PNG, coordenadas);
                                                       }
                                                       else
                                                       {//Initial
                                                           World_Data = Agent_Tool.turn_down(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                           Agent_Tool = (Agente)Event_List[i];
                                                           coordenadas.X = Agent_Tool.Xpos * 15;
                                                           coordenadas.Y = Agent_Tool.Ypos * 15;
                                                           g.DrawImage(A_Abajo_PNG, coordenadas);
                                                       }
                                                       break;
                                               }
                                           }
                                       }
                                   }
                               }
//$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$
                               if (Agent_Tool.Xpos >= 2)
                               {
                                   //Si hay un traste intentando empujar el traste por el lado contrario
                                   if (World_Data[Agent_Tool.Ypos][Agent_Tool.Xpos - 2] == 7)
                                   {
                                       coordenadas.X = Agent_Tool.Xpos * 15;
                                       coordenadas.Y = Agent_Tool.Ypos * 15;
                                       rect = new Rectangle(coordenadas, medida);
                                       Drawing_Area.Invalidate(rect);
                                       Drawing_Area.Update();
                                       if ((Agent_Tool.neighborhood[5] == 1) && //Gira abajo si hay un traste a la abajo
                               (Agent_Tool.detect_pushing_trast(World_Data, Agent_Tool.Ypos + 1, Agent_Tool.Xpos, 0) == false))
                                       {
                                           if (Agent_Tool.Clock_sense == true)
                                           {//El agente ya tiene un giro a favor de las agujas del reloj
                                               World_Data = Agent_Tool.turn_up(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                               Agent_Tool = (Agente)Event_List[i];
                                               coordenadas.X = Agent_Tool.Xpos * 15;
                                               coordenadas.Y = Agent_Tool.Ypos * 15;
                                               g.DrawImage(A_Arriba_PNG, coordenadas);
                                           }
                                           else
                                           {//Initial
                                               World_Data = Agent_Tool.turn_down(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                               Agent_Tool = (Agente)Event_List[i];
                                               coordenadas.X = Agent_Tool.Xpos * 15;
                                               coordenadas.Y = Agent_Tool.Ypos * 15;
                                               g.DrawImage(A_Abajo_PNG, coordenadas);
                                           }
                                       }
                                       else
                                       {
                                           if ((Agent_Tool.neighborhood[1] == 1) &&//Gira a la derecha si hay un traste a la derech
                                  (Agent_Tool.detect_pushing_trast(World_Data, Agent_Tool.Ypos - 1, Agent_Tool.Xpos, 2) == false))
                                           {
                                               if (Agent_Tool.Clock_inverse == true)
                                               {//El agente ya tiene un giro en contra de las agujas del reloj
                                                   World_Data = Agent_Tool.turn_down(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                   Agent_Tool = (Agente)Event_List[i];
                                                   coordenadas.X = Agent_Tool.Xpos * 15;
                                                   coordenadas.Y = Agent_Tool.Ypos * 15;
                                                   g.DrawImage(A_Abajo_PNG, coordenadas);
                                               }
                                               else
                                               {//Initial
                                                   World_Data = Agent_Tool.turn_up(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                   Agent_Tool = (Agente)Event_List[i];
                                                   coordenadas.X = Agent_Tool.Xpos * 15;
                                                   coordenadas.Y = Agent_Tool.Ypos * 15;
                                                   g.DrawImage(A_Arriba_PNG, coordenadas);
                                               }
                                           }
                                           else
                                           {
                                               int a = turn.Next(2);
                                               switch (a)
                                               {
                                                   case (0):
                                                       if (Agent_Tool.Clock_inverse == true)
                                                       {//El agente ya tiene un giro en contra de las agujas del reloj
                                                           World_Data = Agent_Tool.turn_down(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                           Agent_Tool = (Agente)Event_List[i];
                                                           coordenadas.X = Agent_Tool.Xpos * 15;
                                                           coordenadas.Y = Agent_Tool.Ypos * 15;
                                                           g.DrawImage(A_Abajo_PNG, coordenadas);
                                                       }
                                                       else
                                                       {//Initial
                                                           World_Data = Agent_Tool.turn_up(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                           Agent_Tool = (Agente)Event_List[i];
                                                           coordenadas.X = Agent_Tool.Xpos * 15;
                                                           coordenadas.Y = Agent_Tool.Ypos * 15;
                                                           g.DrawImage(A_Arriba_PNG, coordenadas);
                                                       }
                                                       break;
                                                   case (1):
                                                       if (Agent_Tool.Clock_sense == true)
                                                       {//El agente ya tiene un giro a favor de las agujas del reloj
                                                           World_Data = Agent_Tool.turn_up(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                           Agent_Tool = (Agente)Event_List[i];
                                                           coordenadas.X = Agent_Tool.Xpos * 15;
                                                           coordenadas.Y = Agent_Tool.Ypos * 15;
                                                           g.DrawImage(A_Arriba_PNG, coordenadas);
                                                       }
                                                       else
                                                       {//Initial
                                                           World_Data = Agent_Tool.turn_down(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                           Agent_Tool = (Agente)Event_List[i];
                                                           coordenadas.X = Agent_Tool.Xpos * 15;
                                                           coordenadas.Y = Agent_Tool.Ypos * 15;
                                                           g.DrawImage(A_Abajo_PNG, coordenadas);
                                                       }
                                                       break;
                                               }
                                           }
                                       }

                                   }
                               }
//$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$
                           }
                           else
                           {   //Si el traste que tiene delante ya lo está empujando otro agente.
                               if ((Agent_Tool.neighborhood[7] == 1) &&
                               (Agent_Tool.detect_pushing_trast(World_Data, Agent_Tool.Ypos, Agent_Tool.Xpos - 1, 1) == true))
                               {
                                   coordenadas.X = Agent_Tool.Xpos * 15;
                                   coordenadas.Y = Agent_Tool.Ypos * 15;
                                   rect = new Rectangle(coordenadas, medida);
                                   Drawing_Area.Invalidate(rect);
                                   Drawing_Area.Update();
                                   if ((Agent_Tool.neighborhood[5] == 1) && //Gira abajo si hay un traste a la abajo
                           (Agent_Tool.detect_pushing_trast(World_Data, Agent_Tool.Ypos + 1, Agent_Tool.Xpos, 0) == false))
                                   {
                                       if (Agent_Tool.Clock_sense == true)
                                       {//El agente ya tiene un giro a favor de las agujas del reloj
                                           World_Data = Agent_Tool.turn_up(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                           Agent_Tool = (Agente)Event_List[i];
                                           coordenadas.X = Agent_Tool.Xpos * 15;
                                           coordenadas.Y = Agent_Tool.Ypos * 15;
                                           g.DrawImage(A_Arriba_PNG, coordenadas);
                                       }
                                       else
                                       {//Initial
                                           World_Data = Agent_Tool.turn_down(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                           Agent_Tool = (Agente)Event_List[i];
                                           coordenadas.X = Agent_Tool.Xpos * 15;
                                           coordenadas.Y = Agent_Tool.Ypos * 15;
                                           g.DrawImage(A_Abajo_PNG, coordenadas);
                                       }
                                   }
                                   else
                                   {
                                       if ((Agent_Tool.neighborhood[1] == 1) &&//Gira arriba si hay un traste arriba
                              (Agent_Tool.detect_pushing_trast(World_Data, Agent_Tool.Ypos - 1, Agent_Tool.Xpos, 2) == false))
                                       {
                                           if (Agent_Tool.Clock_inverse == true)
                                           {//El agente ya tiene un giro en contra de las agujas del reloj
                                               World_Data = Agent_Tool.turn_down(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                               Agent_Tool = (Agente)Event_List[i];
                                               coordenadas.X = Agent_Tool.Xpos * 15;
                                               coordenadas.Y = Agent_Tool.Ypos * 15;
                                               g.DrawImage(A_Abajo_PNG, coordenadas);
                                           }
                                           else
                                           {//Initial
                                               World_Data = Agent_Tool.turn_up(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                               Agent_Tool = (Agente)Event_List[i];
                                               coordenadas.X = Agent_Tool.Xpos * 15;
                                               coordenadas.Y = Agent_Tool.Ypos * 15;
                                               g.DrawImage(A_Arriba_PNG, coordenadas);
                                           }
                                       }
                                       else
                                       {
                                           int a = turn.Next(2);
                                           switch (a)
                                           {
                                               case (0):
                                                   if (Agent_Tool.Clock_inverse == true)
                                                   {//El agente ya tiene un giro en contra de las agujas del reloj
                                                       World_Data = Agent_Tool.turn_down(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                       Agent_Tool = (Agente)Event_List[i];
                                                       coordenadas.X = Agent_Tool.Xpos * 15;
                                                       coordenadas.Y = Agent_Tool.Ypos * 15;
                                                       g.DrawImage(A_Abajo_PNG, coordenadas);
                                                   }
                                                   else
                                                   {//Initial
                                                       World_Data = Agent_Tool.turn_up(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                       Agent_Tool = (Agente)Event_List[i];
                                                       coordenadas.X = Agent_Tool.Xpos * 15;
                                                       coordenadas.Y = Agent_Tool.Ypos * 15;
                                                       g.DrawImage(A_Arriba_PNG, coordenadas);
                                                   }
                                                   break;
                                               case (1):
                                                   if (Agent_Tool.Clock_sense == true)
                                                   {//El agente ya tiene un giro a favor de las agujas del reloj
                                                       World_Data = Agent_Tool.turn_up(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                       Agent_Tool = (Agente)Event_List[i];
                                                       coordenadas.X = Agent_Tool.Xpos * 15;
                                                       coordenadas.Y = Agent_Tool.Ypos * 15;
                                                       g.DrawImage(A_Arriba_PNG, coordenadas);
                                                   }
                                                   else
                                                   {//Initial
                                                       World_Data = Agent_Tool.turn_down(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                       Agent_Tool = (Agente)Event_List[i];
                                                       coordenadas.X = Agent_Tool.Xpos * 15;
                                                       coordenadas.Y = Agent_Tool.Ypos * 15;
                                                       g.DrawImage(A_Abajo_PNG, coordenadas);
                                                   }
                                                   break;
                                           }
                                       }
                                   }

                               }
                               //Si el agente no tiene traste frente ni a los lados y tiene algún traste
                               //en las vecindades diagonales inferiores.
//CORRECCIÓN AGREGANDO(Agent_Tool.neighborhood[7] == 2) PARA EL CASO QUE HAYA UN HUECO DELANTE
                               if (((Agent_Tool.neighborhood[7] == 2) || (Agent_Tool.neighborhood[7] == 0)) &&
                                   ((Agent_Tool.neighborhood[2] == 1) || (Agent_Tool.neighborhood[4] == 1)))
                               {
                                   if ((Agent_Tool.neighborhood[2] == 1) && //Si tiene traste a la derecha diagonal inferior y no está push 
                                 (Agent_Tool.detect_pushing_trast(World_Data, Agent_Tool.Ypos - 1, Agent_Tool.Xpos + 1, 7) == false))
                                   {
                                       coordenadas.X = Agent_Tool.Xpos * 15;
                                       coordenadas.Y = Agent_Tool.Ypos * 15;
                                       rect = new Rectangle(coordenadas, medida);
                                       Drawing_Area.Invalidate(rect);
                                       Drawing_Area.Update();
                                       if (Agent_Tool.Clock_inverse == true)
                                       {
                                           if (Agent_Tool.neighborhood[7] == 2)//Caso que tenga que moverse a un hueco
                                           {
                                               World_Data = Agent_Tool.Agent_Restore(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                               Agent_Tool = (Agente)Event_List[i];
                                               coordenadas.X = Agent_Tool.Xpos * 15;
                                               coordenadas.Y = Agent_Tool.Ypos * 15;
                                               switch (World_Data[Agent_Tool.Ypos][Agent_Tool.Xpos])
                                               {
                                                   case (4):
                                                       g.DrawImage(A_Arriba_PNG, coordenadas);
                                                       break;
                                                   case (5):
                                                       g.DrawImage(A_Abajo_PNG, coordenadas);
                                                       break;
                                                   case (6):
                                                       g.DrawImage(A_Izquierda_PNG, coordenadas);
                                                       break;
                                                   case (7):
                                                       g.DrawImage(A_Derecha_PNG, coordenadas);
                                                       break;
                                               }
                                               break;
                                           }
                                           //El agente ya tiene un giro en contra de las agujas del reloj (Aquí se sustituyó turn_left por move_up)
                                           World_Data = Agent_Tool.move_left(World_Data, Event_List, (Agente)Event_List[i], false, Lambda_Exp, Lambda_Huecos);
                                           Agent_Tool = (Agente)Event_List[i];
                                           coordenadas.X = Agent_Tool.Xpos * 15;
                                           coordenadas.Y = Agent_Tool.Ypos * 15;
                                           g.DrawImage(A_Izquierda_PNG, coordenadas);
                                       }
                                       else
                                       {//Initial
                                           World_Data = Agent_Tool.turn_up(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                           Agent_Tool = (Agente)Event_List[i];
                                           coordenadas.X = Agent_Tool.Xpos * 15;
                                           coordenadas.Y = Agent_Tool.Ypos * 15;
                                           g.DrawImage(A_Arriba_PNG, coordenadas);
                                       }
                                       break;
                                   }
                                   if ((Agent_Tool.neighborhood[4] == 1) && //Si tiene traste a la izquierda diagonal inferior y no está push 
                                 (Agent_Tool.detect_pushing_trast(World_Data, Agent_Tool.Ypos + 1, Agent_Tool.Xpos + 1, 7) == false))
                                   {
                                       coordenadas.X = Agent_Tool.Xpos * 15;
                                       coordenadas.Y = Agent_Tool.Ypos * 15;
                                       rect = new Rectangle(coordenadas, medida);
                                       Drawing_Area.Invalidate(rect);
                                       Drawing_Area.Update();
                                       if (Agent_Tool.Clock_sense == true)
                                       {
                                           if (Agent_Tool.neighborhood[7] == 2)//Caso que tenga que moverse a un hueco
                                           {
                                               World_Data = Agent_Tool.Agent_Restore(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                               Agent_Tool = (Agente)Event_List[i];
                                               coordenadas.X = Agent_Tool.Xpos * 15;
                                               coordenadas.Y = Agent_Tool.Ypos * 15;
                                               switch (World_Data[Agent_Tool.Ypos][Agent_Tool.Xpos])
                                               {
                                                   case (4):
                                                       g.DrawImage(A_Arriba_PNG, coordenadas);
                                                       break;
                                                   case (5):
                                                       g.DrawImage(A_Abajo_PNG, coordenadas);
                                                       break;
                                                   case (6):
                                                       g.DrawImage(A_Izquierda_PNG, coordenadas);
                                                       break;
                                                   case (7):
                                                       g.DrawImage(A_Derecha_PNG, coordenadas);
                                                       break;
                                               }
                                               break;
                                           }
                                           //El agente ya tiene un giro a favor de las agujas del reloj(Aquí se sustituyó turn_left por move_up)
                                           World_Data = Agent_Tool.move_left(World_Data, Event_List, (Agente)Event_List[i], false, Lambda_Exp, Lambda_Huecos);
                                           Agent_Tool = (Agente)Event_List[i];
                                           coordenadas.X = Agent_Tool.Xpos * 15;
                                           coordenadas.Y = Agent_Tool.Ypos * 15;
                                           g.DrawImage(A_Izquierda_PNG, coordenadas);
                                       }
                                       else
                                       {//Initial
                                           World_Data = Agent_Tool.turn_down(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                           Agent_Tool = (Agente)Event_List[i];
                                           coordenadas.X = Agent_Tool.Xpos * 15;
                                           coordenadas.Y = Agent_Tool.Ypos * 15;
                                           g.DrawImage(A_Abajo_PNG, coordenadas);
                                       }
                                       break;
                                   }
                               }
                               //Si el agente no tiene traste frente ni a los lados.
                               Agent_Tool.get_neighborhood(World_Data, Agent_Tool.Xpos, Agent_Tool.Ypos);//Actualizar vecindad
                               if ((Agent_Tool.neighborhood[7] == 0) && (Agent_Tool.neighborhood[1] != 1) &&
                                   (Agent_Tool.neighborhood[5] != 1))
                               {
                                   coordenadas.X = Agent_Tool.Xpos * 15;
                                   coordenadas.Y = Agent_Tool.Ypos * 15;
                                   rect = new Rectangle(coordenadas, medida);
                                   Drawing_Area.Invalidate(rect);
                                   Drawing_Area.Update();
                                   World_Data = Agent_Tool.move_left(World_Data, Event_List, (Agente)Event_List[i], false, Lambda_Exp, Lambda_Huecos);
                                   Agent_Tool = (Agente)Event_List[i];
                                   coordenadas.X = Agent_Tool.Xpos * 15;
                                   coordenadas.Y = Agent_Tool.Ypos * 15;
                                   g.DrawImage(A_Izquierda_PNG, coordenadas);
                               }
                               else
                               {
                                   //Si el agente no tiene traste frente pero tiene alguno a los lados.
                                   if ((Agent_Tool.neighborhood[7] != 1) && ((Agent_Tool.neighborhood[1] == 1) ||
                                       (Agent_Tool.neighborhood[5] == 1)))
                                   {
                                       if ((Agent_Tool.neighborhood[1] == 1) && //Si tiene traste a la derecha y no está push 
                                 (Agent_Tool.detect_pushing_trast(World_Data, Agent_Tool.Ypos - 1, Agent_Tool.Xpos, 2) == false))
                                       {
                                           coordenadas.X = Agent_Tool.Xpos * 15;
                                           coordenadas.Y = Agent_Tool.Ypos * 15;
                                           rect = new Rectangle(coordenadas, medida);
                                           Drawing_Area.Invalidate(rect);
                                           Drawing_Area.Update();
                                           if ((Agent_Tool.Clock_inverse == true) &&
                                               ((Agent_Tool.neighborhood[7] == 0) || (Agent_Tool.neighborhood[7] == 2)))
                                           {
                                               if (Agent_Tool.neighborhood[7] == 2)//Caso que tenga que moverse a un hueco
                                               {
                                                   World_Data = Agent_Tool.Agent_Restore(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                   Agent_Tool = (Agente)Event_List[i];
                                                   coordenadas.X = Agent_Tool.Xpos * 15;
                                                   coordenadas.Y = Agent_Tool.Ypos * 15;
                                                   switch (World_Data[Agent_Tool.Ypos][Agent_Tool.Xpos])
                                                   {
                                                       case (4):
                                                           g.DrawImage(A_Arriba_PNG, coordenadas);
                                                           break;
                                                       case (5):
                                                           g.DrawImage(A_Abajo_PNG, coordenadas);
                                                           break;
                                                       case (6):
                                                           g.DrawImage(A_Izquierda_PNG, coordenadas);
                                                           break;
                                                       case (7):
                                                           g.DrawImage(A_Derecha_PNG, coordenadas);
                                                           break;
                                                   }
                                                   break;
                                               }
                                               //El agente ya tiene un giro en contra de las agujas del reloj (Aquí se sustituyó turn_left por move_up)
                                               World_Data = Agent_Tool.move_left(World_Data, Event_List, (Agente)Event_List[i], false, Lambda_Exp, Lambda_Huecos);
                                               Agent_Tool = (Agente)Event_List[i];
                                               coordenadas.X = Agent_Tool.Xpos * 15;
                                               coordenadas.Y = Agent_Tool.Ypos * 15;
                                               g.DrawImage(A_Izquierda_PNG, coordenadas);
                                           }
                                           else
                                           {//Initial
                                               World_Data = Agent_Tool.turn_up(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                               Agent_Tool = (Agente)Event_List[i];
                                               coordenadas.X = Agent_Tool.Xpos * 15;
                                               coordenadas.Y = Agent_Tool.Ypos * 15;
                                               g.DrawImage(A_Arriba_PNG, coordenadas);
                                           }
                                           break;
                                       }
                                       if ((Agent_Tool.neighborhood[5] == 1) && //Si tiene traste abajo y no está push 
                                 (Agent_Tool.detect_pushing_trast(World_Data, Agent_Tool.Ypos + 1, Agent_Tool.Xpos, 0) == false))
                                       {
                                           coordenadas.X = Agent_Tool.Xpos * 15;
                                           coordenadas.Y = Agent_Tool.Ypos * 15;
                                           rect = new Rectangle(coordenadas, medida);
                                           Drawing_Area.Invalidate(rect);
                                           Drawing_Area.Update();
                                           if ((Agent_Tool.Clock_sense == true) &&
                                               ((Agent_Tool.neighborhood[7] == 0) || (Agent_Tool.neighborhood[7] == 2)))
                                           {
                                               if (Agent_Tool.neighborhood[7] == 2)//Caso que tenga que moverse a un hueco
                                               {
                                                   World_Data = Agent_Tool.Agent_Restore(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                   Agent_Tool = (Agente)Event_List[i];
                                                   coordenadas.X = Agent_Tool.Xpos * 15;
                                                   coordenadas.Y = Agent_Tool.Ypos * 15;
                                                   switch (World_Data[Agent_Tool.Ypos][Agent_Tool.Xpos])
                                                   {
                                                       case (4):
                                                           g.DrawImage(A_Arriba_PNG, coordenadas);
                                                           break;
                                                       case (5):
                                                           g.DrawImage(A_Abajo_PNG, coordenadas);
                                                           break;
                                                       case (6):
                                                           g.DrawImage(A_Izquierda_PNG, coordenadas);
                                                           break;
                                                       case (7):
                                                           g.DrawImage(A_Derecha_PNG, coordenadas);
                                                           break;
                                                   }
                                                   break;
                                               }
                                               //El agente ya tiene un giro a favor de las agujas del reloj(Aquí se sustituyó turn_left por move_up)
                                               World_Data = Agent_Tool.move_left(World_Data, Event_List, (Agente)Event_List[i], false, Lambda_Exp, Lambda_Huecos);
                                               Agent_Tool = (Agente)Event_List[i];
                                               coordenadas.X = Agent_Tool.Xpos * 15;
                                               coordenadas.Y = Agent_Tool.Ypos * 15;
                                               g.DrawImage(A_Izquierda_PNG, coordenadas);
                                           }
                                           else
                                           {//Initial
                                               World_Data = Agent_Tool.turn_down(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                               Agent_Tool = (Agente)Event_List[i];
                                               coordenadas.X = Agent_Tool.Xpos * 15;
                                               coordenadas.Y = Agent_Tool.Ypos * 15;
                                               g.DrawImage(A_Abajo_PNG, coordenadas);
                                           }
                                           break;
                                       }
//@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
                                       if ((Agent_Tool.neighborhood[1] == 1) && //Si tiene traste a la derecha y está push 
                                 (Agent_Tool.detect_pushing_trast(World_Data, Agent_Tool.Ypos - 1, Agent_Tool.Xpos, 2) == true))
                                       {
                                           if (((Agent_Tool.neighborhood[7] == 0) || (Agent_Tool.neighborhood[7] == 2)))
                                           {
                                               coordenadas.X = Agent_Tool.Xpos * 15;
                                               coordenadas.Y = Agent_Tool.Ypos * 15;
                                               rect = new Rectangle(coordenadas, medida);
                                               Drawing_Area.Invalidate(rect);
                                               Drawing_Area.Update();
                                               if (Agent_Tool.neighborhood[7] == 2)//Caso que tenga que moverse a un hueco
                                               {
                                                   World_Data = Agent_Tool.Agent_Restore(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                   Agent_Tool = (Agente)Event_List[i];
                                                   coordenadas.X = Agent_Tool.Xpos * 15;
                                                   coordenadas.Y = Agent_Tool.Ypos * 15;
                                                   switch (World_Data[Agent_Tool.Ypos][Agent_Tool.Xpos])
                                                   {
                                                       case (4):
                                                           g.DrawImage(A_Arriba_PNG, coordenadas);
                                                           break;
                                                       case (5):
                                                           g.DrawImage(A_Abajo_PNG, coordenadas);
                                                           break;
                                                       case (6):
                                                           g.DrawImage(A_Izquierda_PNG, coordenadas);
                                                           break;
                                                       case (7):
                                                           g.DrawImage(A_Derecha_PNG, coordenadas);
                                                           break;
                                                   }
                                                   break;
                                               }
                                               World_Data = Agent_Tool.move_left(World_Data, Event_List, (Agente)Event_List[i], false, Lambda_Exp, Lambda_Huecos);
                                               Agent_Tool = (Agente)Event_List[i];
                                               coordenadas.X = Agent_Tool.Xpos * 15;
                                               coordenadas.Y = Agent_Tool.Ypos * 15;
                                               g.DrawImage(A_Izquierda_PNG, coordenadas);
                                               break;
                                           }
                                           else
                                           {
                                               World_Data = Agent_Tool.turn_down(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                               Agent_Tool = (Agente)Event_List[i];
                                               coordenadas.X = Agent_Tool.Xpos * 15;
                                               coordenadas.Y = Agent_Tool.Ypos * 15;
                                               g.DrawImage(A_Abajo_PNG, coordenadas);
                                               break;
                                           }
                                       }
                                       if ((Agent_Tool.neighborhood[5] == 1) && //Si tiene traste abajo y está push 
                                 (Agent_Tool.detect_pushing_trast(World_Data, Agent_Tool.Ypos + 1, Agent_Tool.Xpos, 0) == true))
                                       {
                                           if ((Agent_Tool.neighborhood[7] == 0) || (Agent_Tool.neighborhood[7] == 2))
                                           {
                                               coordenadas.X = Agent_Tool.Xpos * 15;
                                               coordenadas.Y = Agent_Tool.Ypos * 15;
                                               rect = new Rectangle(coordenadas, medida);
                                               Drawing_Area.Invalidate(rect);
                                               Drawing_Area.Update();
                                               if (Agent_Tool.neighborhood[7] == 2)//Caso que tenga que moverse a un hueco
                                               {
                                                   World_Data = Agent_Tool.Agent_Restore(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                   Agent_Tool = (Agente)Event_List[i];
                                                   coordenadas.X = Agent_Tool.Xpos * 15;
                                                   coordenadas.Y = Agent_Tool.Ypos * 15;
                                                   switch (World_Data[Agent_Tool.Ypos][Agent_Tool.Xpos])
                                                   {
                                                       case (4):
                                                           g.DrawImage(A_Arriba_PNG, coordenadas);
                                                           break;
                                                       case (5):
                                                           g.DrawImage(A_Abajo_PNG, coordenadas);
                                                           break;
                                                       case (6):
                                                           g.DrawImage(A_Izquierda_PNG, coordenadas);
                                                           break;
                                                       case (7):
                                                           g.DrawImage(A_Derecha_PNG, coordenadas);
                                                           break;
                                                   }
                                                   break;
                                               }
                                               World_Data = Agent_Tool.move_left(World_Data, Event_List, (Agente)Event_List[i], false, Lambda_Exp, Lambda_Huecos);
                                               Agent_Tool = (Agente)Event_List[i];
                                               coordenadas.X = Agent_Tool.Xpos * 15;
                                               coordenadas.Y = Agent_Tool.Ypos * 15;
                                               g.DrawImage(A_Izquierda_PNG, coordenadas);
                                               break;
                                           }
                                           else
                                           {
                                               World_Data = Agent_Tool.turn_up(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                               Agent_Tool = (Agente)Event_List[i];
                                               coordenadas.X = Agent_Tool.Xpos * 15;
                                               coordenadas.Y = Agent_Tool.Ypos * 15;
                                               g.DrawImage(A_Arriba_PNG, coordenadas);
                                               break;
                                           }
                                       }
//@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ 
                                   }
                                   else
                                   {//Caso en que el agente cae en un hueco
                                       if (Agent_Tool.neighborhood[7] == 2)
                                       {
                                           coordenadas.X = Agent_Tool.Xpos * 15;
                                           coordenadas.Y = Agent_Tool.Ypos * 15;
                                           rect = new Rectangle(coordenadas, medida);
                                           Drawing_Area.Invalidate(rect);
                                           Drawing_Area.Update();
                                           World_Data = Agent_Tool.Agent_Restore(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                           Agent_Tool = (Agente)Event_List[i];
                                           coordenadas.X = Agent_Tool.Xpos * 15;
                                           coordenadas.Y = Agent_Tool.Ypos * 15;
                                           switch (World_Data[Agent_Tool.Ypos][Agent_Tool.Xpos])
                                           {
                                               case (4):
                                                   g.DrawImage(A_Arriba_PNG, coordenadas);
                                                   break;
                                               case (5):
                                                   g.DrawImage(A_Abajo_PNG, coordenadas);
                                                   break;
                                               case (6):
                                                   g.DrawImage(A_Izquierda_PNG, coordenadas);
                                                   break;
                                               case (7):
                                                   g.DrawImage(A_Derecha_PNG, coordenadas);
                                                   break;
                                           }
                                       }
                                       else
                                       {//Caso en que el agente no tiene ni traste ni hueco delante@@@@@@@@@@@@@@@@@@@@1
                                           if (Agent_Tool.neighborhood[5] == 3)
                                            {
                                                if (Agent_Tool.Clock_inverse == true)
                                                    {//El agente ya tiene un giro a favor de las agujas del reloj
                                                        World_Data = Agent_Tool.turn_down(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                        Agent_Tool = (Agente)Event_List[i];
                                                        coordenadas.X = Agent_Tool.Xpos * 15;
                                                        coordenadas.Y = Agent_Tool.Ypos * 15;
                                                        g.DrawImage(A_Abajo_PNG, coordenadas);
                                                    }
                                                    else
                                                    {//Initial
                                                        World_Data = Agent_Tool.turn_up(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                        Agent_Tool = (Agente)Event_List[i];
                                                        coordenadas.X = Agent_Tool.Xpos * 15;
                                                        coordenadas.Y = Agent_Tool.Ypos * 15;
                                                        g.DrawImage(A_Arriba_PNG, coordenadas);
                                                    }
                                                break;
                                            }
                                           if (Agent_Tool.neighborhood[1] == 3)
                                           {
                                               if (Agent_Tool.Clock_sense == true)
                                               {//El agente ya tiene un giro en contra de las agujas del reloj
                                                   World_Data = Agent_Tool.turn_up(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                   Agent_Tool = (Agente)Event_List[i];
                                                   coordenadas.X = Agent_Tool.Xpos * 15;
                                                   coordenadas.Y = Agent_Tool.Ypos * 15;
                                                   g.DrawImage(A_Arriba_PNG, coordenadas);
                                               }
                                               else
                                               {//Initial
                                                   World_Data = Agent_Tool.turn_down(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                   Agent_Tool = (Agente)Event_List[i];
                                                   coordenadas.X = Agent_Tool.Xpos * 15;
                                                   coordenadas.Y = Agent_Tool.Ypos * 15;
                                                   g.DrawImage(A_Abajo_PNG, coordenadas);
                                               }
                                               break;
                                           }
                                           //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
                                           int b = turn.Next(2);
                                           switch (b)
                                           {
                                               case (0):
                                                   if (Agent_Tool.Clock_inverse == true)
                                                   {//El agente ya tiene un giro en contra de las agujas del reloj
                                                       World_Data = Agent_Tool.turn_down(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                       Agent_Tool = (Agente)Event_List[i];
                                                       coordenadas.X = Agent_Tool.Xpos * 15;
                                                       coordenadas.Y = Agent_Tool.Ypos * 15;
                                                       g.DrawImage(A_Abajo_PNG, coordenadas);
                                                   }
                                                   else
                                                   {//Initial
                                                       World_Data = Agent_Tool.turn_up(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                       Agent_Tool = (Agente)Event_List[i];
                                                       coordenadas.X = Agent_Tool.Xpos * 15;
                                                       coordenadas.Y = Agent_Tool.Ypos * 15;
                                                       g.DrawImage(A_Arriba_PNG, coordenadas);
                                                   }
                                                   break;
                                               case (1):
                                                   if (Agent_Tool.Clock_sense == true)
                                                   {//El agente ya tiene un giro a favor de las agujas del reloj
                                                       World_Data = Agent_Tool.turn_up(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                       Agent_Tool = (Agente)Event_List[i];
                                                       coordenadas.X = Agent_Tool.Xpos * 15;
                                                       coordenadas.Y = Agent_Tool.Ypos * 15;
                                                       g.DrawImage(A_Arriba_PNG, coordenadas);
                                                   }
                                                   else
                                                   {//Initial
                                                       World_Data = Agent_Tool.turn_down(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                       Agent_Tool = (Agente)Event_List[i];
                                                       coordenadas.X = Agent_Tool.Xpos * 15;
                                                       coordenadas.Y = Agent_Tool.Ypos * 15;
                                                       g.DrawImage(A_Abajo_PNG, coordenadas);
                                                   }
                                                   break;
                                           }
                                       }
                                   }
                               }
                           }
                           break;
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////HACIA LA DERECHA///////////////////////////////////////////////////////////////////////
                        case(3)://Agente orientado hacia la derecha
                           Agent_Tool.get_neighborhood(World_Data, Agent_Tool.Xpos, Agent_Tool.Ypos);//Actualizar vecindad
                           //Si el agente tiene un traste delante de él y este traste no está siendo empujado.
                           if ((Agent_Tool.neighborhood[3] == 1) &&
                               (Agent_Tool.detect_pushing_trast(World_Data, Agent_Tool.Ypos, Agent_Tool.Xpos + 1, 3) == false))
                           {
                               if (Agent_Tool.Xpos == 39)//Si está frente del traste y luego tiene el límite del mundo
                               {
                                   if (Agent_Tool.Clock_sense == true)
                                   {
                                       World_Data = Agent_Tool.turn_down(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                       Agent_Tool = (Agente)Event_List[i];
                                       coordenadas.X = Agent_Tool.Xpos * 15;
                                       coordenadas.Y = Agent_Tool.Ypos * 15;
                                       g.DrawImage(A_Abajo_PNG, coordenadas);
                                       break;
                                   }
                                   else
                                   {
                                       World_Data = Agent_Tool.turn_up(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                       Agent_Tool = (Agente)Event_List[i];
                                       coordenadas.X = Agent_Tool.Xpos * 15;
                                       coordenadas.Y = Agent_Tool.Ypos * 15;
                                       g.DrawImage(A_Arriba_PNG, coordenadas);
                                       break;
                                   }
                               }
                               if ((Agent_Tool.neighborhood[2] == 2) && (Agent_Tool.neighborhood[4] != 3) &&
                                    (Agent_Tool.neighborhood[5] != 3))
                               {
                                   if (Agent_Tool.Clock_inverse == false)
                                   {
                                       World_Data = Agent_Tool.turn_down(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                       Agent_Tool = (Agente)Event_List[i];
                                       coordenadas.X = Agent_Tool.Xpos * 15;
                                       coordenadas.Y = Agent_Tool.Ypos * 15;
                                       g.DrawImage(A_Abajo_PNG, coordenadas);
                                       break;
                                   }
                               }
                               if ((Agent_Tool.neighborhood[4] == 2) && (Agent_Tool.neighborhood[1] != 3) &&
                                   (Agent_Tool.neighborhood[2] != 3))
                               {
                                   if (Agent_Tool.Clock_sense == false)
                                   {
                                       World_Data = Agent_Tool.turn_up(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                       Agent_Tool = (Agente)Event_List[i];
                                       coordenadas.X = Agent_Tool.Xpos * 15;
                                       coordenadas.Y = Agent_Tool.Ypos * 15;
                                       g.DrawImage(A_Arriba_PNG, coordenadas);
                                       break;
                                   }
                               }
                               if (Agent_Tool.Xpos <= 37)
                               {
                                   //El agente sólo moverá el traste si la casilla siguiente es hueco o vacía y la casilla
                                   if ((World_Data[Agent_Tool.Ypos][Agent_Tool.Xpos + 2] == 0) &&//siguiente no es ni
                                       ((World_Data[Agent_Tool.Ypos][Agent_Tool.Xpos + 3] != 3) &&//traste ni muro.
                                       (World_Data[Agent_Tool.Ypos][Agent_Tool.Xpos + 3] != 1)))
                                   {
                                       ////////////////////////////////////////Sección de código para borrar el agente///////////////////////////////////////////
                                       coordenadas.X = Agent_Tool.Xpos * 15;
                                       coordenadas.Y = Agent_Tool.Ypos * 15;
                                       rect = new Rectangle(coordenadas, medida);
                                       Drawing_Area.Invalidate(rect);
                                       Drawing_Area.Update();
                                       /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                       ////////////////////////////////////////Sección de código para borrar el traste/////////////////////////////////////////
                                       coordenadas.X = (Agent_Tool.Xpos + 1) * 15;
                                       rect = new Rectangle(coordenadas, medida);
                                       Drawing_Area.Invalidate(rect);
                                       Drawing_Area.Update();
                                       ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                       ////////////////////////////////////////Actualizar la matix de datos y reasignar el agente a Agent_Tool/////////////////
                                       World_Data = Agent_Tool.move_right(World_Data, Event_List, (Agente)Event_List[i], true, Lambda_Exp, Lambda_Huecos);
                                       Agent_Tool = (Agente)Event_List[i];
                                       ////////////////////////////////////////Sección de código que pinta nuevamente el traste y el agente////////////////////
                                       coordenadas.X = Agent_Tool.Xpos * 15;
                                       coordenadas.Y = Agent_Tool.Ypos * 15;
                                       g.DrawImage(A_Derecha_T_PNG, coordenadas);
                                       coordenadas.X = (Agent_Tool.Xpos + 1) * 15;
                                       g.DrawImage(Traste_PNG, coordenadas);
                                       ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                   }
                                   else
                                   { //Si lo próximo que viene es un hueco y el agente está empujando el traste.
                                       if (World_Data[Agent_Tool.Ypos][Agent_Tool.Xpos + 2] == 2)
                                       {
                                           coordenadas.X = Agent_Tool.Xpos * 15;
                                           coordenadas.Y = Agent_Tool.Ypos * 15;
                                           rect = new Rectangle(coordenadas, medida);
                                           Drawing_Area.Invalidate(rect);
                                           Drawing_Area.Update();
                                           coordenadas.Y = Agent_Tool.Ypos * 15;
                                           coordenadas.X = (Agent_Tool.Xpos + 1) * 15;
                                           rect = new Rectangle(coordenadas, medida);
                                           Drawing_Area.Invalidate(rect);
                                           Drawing_Area.Update();
                                           World_Data = Agent_Tool.move_right(World_Data, Event_List, (Agente)Event_List[i], true, Lambda_Exp, Lambda_Huecos);
                                           Agent_Tool = (Agente)Event_List[i];
                                           coordenadas.X = Agent_Tool.Xpos * 15;
                                           coordenadas.Y = Agent_Tool.Ypos * 15;
                                           g.DrawImage(A_Derecha_T_PNG, coordenadas);
                                           coordenadas.X = (Agent_Tool.Xpos + 1) * 15;
                                           g.DrawImage(Traste_PNG, coordenadas);
                                           rect = new Rectangle(coordenadas, medida);
                                           Drawing_Area.Invalidate(rect);
                                           Drawing_Area.Update();
                                           //Tratamiento de huecos//
                                           for (int y = 0; y < World_Data.Length; y++)
                                           {
                                               for (int x = 0; x < World_Data[0].Length; x++)
                                               {
                                                   if (World_Data[y][x] == 20)
                                                   {
                                                       World_Data[y][x] = 2;
                                                       coordenadas.X = x * 15;
                                                       coordenadas.Y = y * 15;
                                                       g.DrawImage(Hueco_PNG, coordenadas);
                                                   }
                                               }
                                           }
                                           break;
                                       }
                                       //Si hay traste doble o el agente se encuentra con un muro en uno de los extremos
                                       if ((World_Data[Agent_Tool.Ypos][Agent_Tool.Xpos + 2] == 1) ||
                                           ((World_Data[Agent_Tool.Ypos][Agent_Tool.Xpos + 1] == 1) &&
                                           (World_Data[Agent_Tool.Ypos][Agent_Tool.Xpos + 2] == 3)) ||
                                           (World_Data[Agent_Tool.Ypos][Agent_Tool.Xpos + 3] == 3) ||
                                           (World_Data[Agent_Tool.Ypos][Agent_Tool.Xpos + 3] == 1))
                                       {//Si el agente está en el extremo inferior del mundo
                                           if ((Agent_Tool.neighborhood[2] == 3) && (Agent_Tool.neighborhood[5] != 3))
                                           {//Si ya tiene un giro contrario a las agujas del reloj entonces turn left
                                               if (Agent_Tool.Clock_inverse == true)
                                               {
                                                   World_Data = Agent_Tool.turn_up(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                   Agent_Tool = (Agente)Event_List[i];
                                                   coordenadas.X = Agent_Tool.Xpos * 15;
                                                   coordenadas.Y = Agent_Tool.Ypos * 15;
                                                   g.DrawImage(A_Arriba_PNG, coordenadas);
                                               }
                                               else
                                               {//Inicial
                                                   World_Data = Agent_Tool.turn_down(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                   Agent_Tool = (Agente)Event_List[i];
                                                   coordenadas.X = Agent_Tool.Xpos * 15;
                                                   coordenadas.Y = Agent_Tool.Ypos * 15;
                                                   g.DrawImage(A_Abajo_PNG, coordenadas);
                                               }
                                           }
                                           else
                                           {//Si el agente está en el extremo superior del mundo
                                               if ((Agent_Tool.neighborhood[4] == 3) && (Agent_Tool.neighborhood[1] != 3))
                                               {
                                                   if (Agent_Tool.Clock_sense == true)
                                                   {//Si ya tiene un giro siguiendo las agujas del reloj entonces turn izq
                                                       World_Data = Agent_Tool.turn_down(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                       Agent_Tool = (Agente)Event_List[i];
                                                       coordenadas.X = Agent_Tool.Xpos * 15;
                                                       coordenadas.Y = Agent_Tool.Ypos * 15;
                                                       g.DrawImage(A_Abajo_PNG, coordenadas);
                                                   }
                                                   else//Initial
                                                   {
                                                       World_Data = Agent_Tool.turn_up(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                       Agent_Tool = (Agente)Event_List[i];
                                                       coordenadas.X = Agent_Tool.Xpos * 15;
                                                       coordenadas.Y = Agent_Tool.Ypos * 15;
                                                       g.DrawImage(A_Arriba_PNG, coordenadas);
                                                   }
                                               }
                                               else
                                               {
                                                   int a = turn.Next(2);//Si no está en ningún extremo del mundo
                                                   switch (a)
                                                   {
                                                       case (0):
                                                           if (Agent_Tool.Clock_inverse == true)
                                                           {//Si ya tiene un giro en contra de las agujas del reloj
                                                               World_Data = Agent_Tool.turn_up(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                               Agent_Tool = (Agente)Event_List[i];
                                                               coordenadas.X = Agent_Tool.Xpos * 15;
                                                               coordenadas.Y = Agent_Tool.Ypos * 15;
                                                               g.DrawImage(A_Arriba_PNG, coordenadas);
                                                           }
                                                           else
                                                           {//Initial
                                                               World_Data = Agent_Tool.turn_down(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                               Agent_Tool = (Agente)Event_List[i];
                                                               coordenadas.X = Agent_Tool.Xpos * 15;
                                                               coordenadas.Y = Agent_Tool.Ypos * 15;
                                                               g.DrawImage(A_Abajo_PNG, coordenadas);
                                                           }
                                                           break;
                                                       case (1):
                                                           if (Agent_Tool.Clock_sense == true)
                                                           {//Si ya tiene un giro a favor de las agujas del reloj
                                                               World_Data = Agent_Tool.turn_down(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                               Agent_Tool = (Agente)Event_List[i];
                                                               coordenadas.X = Agent_Tool.Xpos * 15;
                                                               coordenadas.Y = Agent_Tool.Ypos * 15;
                                                               g.DrawImage(A_Abajo_PNG, coordenadas);
                                                           }
                                                           else
                                                           {//Initial
                                                               World_Data = Agent_Tool.turn_up(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                               Agent_Tool = (Agente)Event_List[i];
                                                               coordenadas.X = Agent_Tool.Xpos * 15;
                                                               coordenadas.Y = Agent_Tool.Ypos * 15;
                                                               g.DrawImage(A_Arriba_PNG, coordenadas);
                                                           }
                                                           break;
                                                   }
                                               }
                                           }
                                           break;
                                       }

                                   }
                               }
                               else
                               {//Caso en que hay un hueco en el extremo superior y el agente se aproxima con un traste
                                   if ((Agent_Tool.Old_Xpos <= 38) && (World_Data[Agent_Tool.Ypos][Agent_Tool.Xpos + 2] == 2))
                                   {
                                       coordenadas.X = Agent_Tool.Xpos * 15;
                                       coordenadas.Y = Agent_Tool.Ypos * 15;
                                       rect = new Rectangle(coordenadas, medida);
                                       Drawing_Area.Invalidate(rect);
                                       Drawing_Area.Update();
                                       coordenadas.Y = Agent_Tool.Ypos * 15;
                                       coordenadas.X = (Agent_Tool.Xpos + 1) * 15;
                                       rect = new Rectangle(coordenadas, medida);
                                       Drawing_Area.Invalidate(rect);
                                       Drawing_Area.Update();
                                       World_Data = Agent_Tool.move_right(World_Data, Event_List, (Agente)Event_List[i], true, Lambda_Exp, Lambda_Huecos);
                                       Agent_Tool = (Agente)Event_List[i];
                                       coordenadas.X = Agent_Tool.Xpos * 15;
                                       coordenadas.Y = Agent_Tool.Ypos * 15;
                                       g.DrawImage(A_Derecha_T_PNG, coordenadas);
                                       coordenadas.X = (Agent_Tool.Xpos + 1) * 15;
                                       g.DrawImage(Traste_PNG, coordenadas);
                                       rect = new Rectangle(coordenadas, medida);
                                       Drawing_Area.Invalidate(rect);
                                       Drawing_Area.Update();
                                       //Tratamiento de huecos//
                                       for (int y = 0; y < World_Data.Length; y++)
                                       {
                                           for (int x = 0; x < World_Data[0].Length; x++)
                                           {
                                               if (World_Data[y][x] == 20)
                                               {
                                                   World_Data[y][x] = 2;
                                                   coordenadas.X = x * 15;
                                                   coordenadas.Y = y * 15;
                                                   g.DrawImage(Hueco_PNG, coordenadas);
                                               }
                                           }
                                       }
                                   }
                                   else
                                   {
                                       if (Agent_Tool.neighborhood[2] == 3)//Si el agente está en el extremo superior del mundo
                                       {
                                           if (Agent_Tool.Clock_inverse == true)
                                           {//Si el agente ya tiene un giro en contra de las agujas del reloj
                                               World_Data = Agent_Tool.turn_up(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                               Agent_Tool = (Agente)Event_List[i];
                                               coordenadas.X = Agent_Tool.Xpos * 15;
                                               coordenadas.Y = Agent_Tool.Ypos * 15;
                                               g.DrawImage(A_Arriba_PNG, coordenadas);
                                           }
                                           else
                                           {//Initial
                                               World_Data = Agent_Tool.turn_down(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                               Agent_Tool = (Agente)Event_List[i];
                                               coordenadas.X = Agent_Tool.Xpos * 15;
                                               coordenadas.Y = Agent_Tool.Ypos * 15;
                                               g.DrawImage(A_Abajo_PNG, coordenadas);
                                           }
                                       }
                                       else
                                       {
                                           if (Agent_Tool.neighborhood[4] == 3)//Si el agente está en el extremo superior del mundo
                                           {
                                               if (Agent_Tool.Clock_sense == true)
                                               {//Si el agente ya tiene un giro a favor de las agujas del reloj
                                                   World_Data = Agent_Tool.turn_down(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                   Agent_Tool = (Agente)Event_List[i];
                                                   coordenadas.X = Agent_Tool.Xpos * 15;
                                                   coordenadas.Y = Agent_Tool.Ypos * 15;
                                                   g.DrawImage(A_Abajo_PNG, coordenadas);
                                               }
                                               else
                                               {//Initial
                                                   World_Data = Agent_Tool.turn_up(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                   Agent_Tool = (Agente)Event_List[i];
                                                   coordenadas.X = Agent_Tool.Xpos * 15;
                                                   coordenadas.Y = Agent_Tool.Ypos * 15;
                                                   g.DrawImage(A_Arriba_PNG, coordenadas);
                                               }
                                           }
                                           else
                                           {
                                               int a = turn.Next(2);//Si no está en ningún extremo del mundo
                                               switch (a)
                                               {
                                                   case (0):
                                                       if (Agent_Tool.Clock_inverse == true)
                                                       {//Si el agente ya tiene un giro en contra de las agujas del reloj
                                                           World_Data = Agent_Tool.turn_up(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                           Agent_Tool = (Agente)Event_List[i];
                                                           coordenadas.X = Agent_Tool.Xpos * 15;
                                                           coordenadas.Y = Agent_Tool.Ypos * 15;
                                                           g.DrawImage(A_Arriba_PNG, coordenadas);
                                                       }
                                                       else
                                                       {//Initial
                                                           World_Data = Agent_Tool.turn_down(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                           Agent_Tool = (Agente)Event_List[i];
                                                           coordenadas.X = Agent_Tool.Xpos * 15;
                                                           coordenadas.Y = Agent_Tool.Ypos * 15;
                                                           g.DrawImage(A_Abajo_PNG, coordenadas);
                                                       }
                                                       break;
                                                   case (1):
                                                       if (Agent_Tool.Clock_sense == true)
                                                       {//Si el agente ya tiene un giro en sentido de las agujas del reloj
                                                           World_Data = Agent_Tool.turn_down(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                           Agent_Tool = (Agente)Event_List[i];
                                                           coordenadas.X = Agent_Tool.Xpos * 15;
                                                           coordenadas.Y = Agent_Tool.Ypos * 15;
                                                           g.DrawImage(A_Abajo_PNG, coordenadas);
                                                       }
                                                       else
                                                       {//Initial
                                                           World_Data = Agent_Tool.turn_up(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                           Agent_Tool = (Agente)Event_List[i];
                                                           coordenadas.X = Agent_Tool.Xpos * 15;
                                                           coordenadas.Y = Agent_Tool.Ypos * 15;
                                                           g.DrawImage(A_Arriba_PNG, coordenadas);
                                                       }
                                                       break;
                                               }
                                           }
                                       }
                                   }
                               }
//$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$
                               if (Agent_Tool.Xpos <= 38)
                               {
                                   //Si hay un traste intentando empujar el traste por el lado contrario
                                   if (World_Data[Agent_Tool.Ypos][Agent_Tool.Xpos + 2] == 6)
                                   {
                                       coordenadas.X = Agent_Tool.Xpos * 15;
                                       coordenadas.Y = Agent_Tool.Ypos * 15;
                                       rect = new Rectangle(coordenadas, medida);
                                       Drawing_Area.Invalidate(rect);
                                       Drawing_Area.Update();
                                       if ((Agent_Tool.neighborhood[1] == 1) && //Gira arriba si hay un traste a la arriba
                               (Agent_Tool.detect_pushing_trast(World_Data, Agent_Tool.Ypos - 1, Agent_Tool.Xpos, 2) == false))
                                       {
                                           if (Agent_Tool.Clock_sense == true)
                                           {//El agente ya tiene un giro a favor de las agujas del reloj
                                               World_Data = Agent_Tool.turn_down(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                               Agent_Tool = (Agente)Event_List[i];
                                               coordenadas.X = Agent_Tool.Xpos * 15;
                                               coordenadas.Y = Agent_Tool.Ypos * 15;
                                               g.DrawImage(A_Abajo_PNG, coordenadas);
                                           }
                                           else
                                           {//Initial
                                               World_Data = Agent_Tool.turn_up(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                               Agent_Tool = (Agente)Event_List[i];
                                               coordenadas.X = Agent_Tool.Xpos * 15;
                                               coordenadas.Y = Agent_Tool.Ypos * 15;
                                               g.DrawImage(A_Arriba_PNG, coordenadas);
                                           }
                                       }
                                       else
                                       {
                                           if ((Agent_Tool.neighborhood[5] == 1) &&//Gira abajo si hay un traste abajo
                                  (Agent_Tool.detect_pushing_trast(World_Data, Agent_Tool.Ypos + 1, Agent_Tool.Xpos, 0) == false))
                                           {
                                               if (Agent_Tool.Clock_inverse == true)
                                               {//El agente ya tiene un giro en contra de las agujas del reloj
                                                   World_Data = Agent_Tool.turn_up(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                   Agent_Tool = (Agente)Event_List[i];
                                                   coordenadas.X = Agent_Tool.Xpos * 15;
                                                   coordenadas.Y = Agent_Tool.Ypos * 15;
                                                   g.DrawImage(A_Arriba_PNG, coordenadas);
                                               }
                                               else
                                               {//Initial
                                                   World_Data = Agent_Tool.turn_down(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                   Agent_Tool = (Agente)Event_List[i];
                                                   coordenadas.X = Agent_Tool.Xpos * 15;
                                                   coordenadas.Y = Agent_Tool.Ypos * 15;
                                                   g.DrawImage(A_Abajo_PNG, coordenadas);
                                               }
                                           }
                                           else
                                           {
                                               int a = turn.Next(2);
                                               switch (a)
                                               {
                                                   case (0):
                                                       if (Agent_Tool.Clock_inverse == true)
                                                       {//El agente ya tiene un giro en contra de las agujas del reloj
                                                           World_Data = Agent_Tool.turn_up(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                           Agent_Tool = (Agente)Event_List[i];
                                                           coordenadas.X = Agent_Tool.Xpos * 15;
                                                           coordenadas.Y = Agent_Tool.Ypos * 15;
                                                           g.DrawImage(A_Arriba_PNG, coordenadas);
                                                       }
                                                       else
                                                       {//Initial
                                                           World_Data = Agent_Tool.turn_down(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                           Agent_Tool = (Agente)Event_List[i];
                                                           coordenadas.X = Agent_Tool.Xpos * 15;
                                                           coordenadas.Y = Agent_Tool.Ypos * 15;
                                                           g.DrawImage(A_Abajo_PNG, coordenadas);
                                                       }
                                                       break;
                                                   case (1):
                                                       if (Agent_Tool.Clock_sense == true)
                                                       {//El agente ya tiene un giro a favor de las agujas del reloj
                                                           World_Data = Agent_Tool.turn_down(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                           Agent_Tool = (Agente)Event_List[i];
                                                           coordenadas.X = Agent_Tool.Xpos * 15;
                                                           coordenadas.Y = Agent_Tool.Ypos * 15;
                                                           g.DrawImage(A_Abajo_PNG, coordenadas);
                                                       }
                                                       else
                                                       {//Initial
                                                           World_Data = Agent_Tool.turn_up(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                           Agent_Tool = (Agente)Event_List[i];
                                                           coordenadas.X = Agent_Tool.Xpos * 15;
                                                           coordenadas.Y = Agent_Tool.Ypos * 15;
                                                           g.DrawImage(A_Arriba_PNG, coordenadas);
                                                       }
                                                       break;
                                               }
                                           }
                                       }

                                   }
                               }
//$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$
                           }
                           else
                           {   //Si el traste que tiene delante ya lo está empujando otro agente.
                               if ((Agent_Tool.neighborhood[3] == 1) &&
                               (Agent_Tool.detect_pushing_trast(World_Data, Agent_Tool.Ypos, Agent_Tool.Xpos + 1, 3) == true))
                               {
                                   coordenadas.X = Agent_Tool.Xpos * 15;
                                   coordenadas.Y = Agent_Tool.Ypos * 15;
                                   rect = new Rectangle(coordenadas, medida);
                                   Drawing_Area.Invalidate(rect);
                                   Drawing_Area.Update();
                                   if ((Agent_Tool.neighborhood[1] == 1) && //Gira abajo si hay un traste a la arriba
                           (Agent_Tool.detect_pushing_trast(World_Data, Agent_Tool.Ypos - 1, Agent_Tool.Xpos, 2) == false))
                                   {
                                       if (Agent_Tool.Clock_sense == true)
                                       {//El agente ya tiene un giro a favor de las agujas del reloj
                                           World_Data = Agent_Tool.turn_down(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                           Agent_Tool = (Agente)Event_List[i];
                                           coordenadas.X = Agent_Tool.Xpos * 15;
                                           coordenadas.Y = Agent_Tool.Ypos * 15;
                                           g.DrawImage(A_Abajo_PNG, coordenadas);
                                       }
                                       else
                                       {//Initial
                                           World_Data = Agent_Tool.turn_up(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                           Agent_Tool = (Agente)Event_List[i];
                                           coordenadas.X = Agent_Tool.Xpos * 15;
                                           coordenadas.Y = Agent_Tool.Ypos * 15;
                                           g.DrawImage(A_Arriba_PNG, coordenadas);
                                       }
                                   }
                                   else
                                   {
                                       if ((Agent_Tool.neighborhood[5] == 1) &&//Gira abajo si hay un traste abajo
                              (Agent_Tool.detect_pushing_trast(World_Data, Agent_Tool.Ypos - 1, Agent_Tool.Xpos, 2) == false))
                                       {
                                           if (Agent_Tool.Clock_inverse == true)
                                           {//El agente ya tiene un giro en contra de las agujas del reloj
                                               World_Data = Agent_Tool.turn_up(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                               Agent_Tool = (Agente)Event_List[i];
                                               coordenadas.X = Agent_Tool.Xpos * 15;
                                               coordenadas.Y = Agent_Tool.Ypos * 15;
                                               g.DrawImage(A_Arriba_PNG, coordenadas);
                                           }
                                           else
                                           {//Initial
                                               World_Data = Agent_Tool.turn_down(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                               Agent_Tool = (Agente)Event_List[i];
                                               coordenadas.X = Agent_Tool.Xpos * 15;
                                               coordenadas.Y = Agent_Tool.Ypos * 15;
                                               g.DrawImage(A_Abajo_PNG, coordenadas);
                                           }
                                       }
                                       else
                                       {
                                           int a = turn.Next(2);
                                           switch (a)
                                           {
                                               case (0):
                                                   if (Agent_Tool.Clock_inverse == true)
                                                   {//El agente ya tiene un giro en contra de las agujas del reloj
                                                       World_Data = Agent_Tool.turn_up(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                       Agent_Tool = (Agente)Event_List[i];
                                                       coordenadas.X = Agent_Tool.Xpos * 15;
                                                       coordenadas.Y = Agent_Tool.Ypos * 15;
                                                       g.DrawImage(A_Arriba_PNG, coordenadas);
                                                   }
                                                   else
                                                   {//Initial
                                                       World_Data = Agent_Tool.turn_down(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                       Agent_Tool = (Agente)Event_List[i];
                                                       coordenadas.X = Agent_Tool.Xpos * 15;
                                                       coordenadas.Y = Agent_Tool.Ypos * 15;
                                                       g.DrawImage(A_Abajo_PNG, coordenadas);
                                                   }
                                                   break;
                                               case (1):
                                                   if (Agent_Tool.Clock_sense == true)
                                                   {//El agente ya tiene un giro a favor de las agujas del reloj
                                                       World_Data = Agent_Tool.turn_down(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                       Agent_Tool = (Agente)Event_List[i];
                                                       coordenadas.X = Agent_Tool.Xpos * 15;
                                                       coordenadas.Y = Agent_Tool.Ypos * 15;
                                                       g.DrawImage(A_Abajo_PNG, coordenadas);
                                                   }
                                                   else
                                                   {//Initial
                                                       World_Data = Agent_Tool.turn_up(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                       Agent_Tool = (Agente)Event_List[i];
                                                       coordenadas.X = Agent_Tool.Xpos * 15;
                                                       coordenadas.Y = Agent_Tool.Ypos * 15;
                                                       g.DrawImage(A_Arriba_PNG, coordenadas);
                                                   }
                                                   break;
                                           }
                                       }
                                   }

                               }
                               //Si el agente no tiene traste frente ni a los lados y tiene algún traste
                               //en las vecindades diagonales inferiores.
//CORRECCIÓN AGREGANDO(Agent_Tool.neighborhood[3] == 2) PARA EL CASO QUE HAYA UN HUECO DELANTE
                               if (((Agent_Tool.neighborhood[3] == 2) || (Agent_Tool.neighborhood[3] == 0)) &&
                                   ((Agent_Tool.neighborhood[6] == 1) || (Agent_Tool.neighborhood[0] == 1)))
                               {
                                   if ((Agent_Tool.neighborhood[6] == 1) && //Si tiene traste a la derecha diagonal inferior y no está push 
                                 (Agent_Tool.detect_pushing_trast(World_Data, Agent_Tool.Ypos + 1, Agent_Tool.Xpos - 1, 7) == false))
                                   {
                                       coordenadas.X = Agent_Tool.Xpos * 15;
                                       coordenadas.Y = Agent_Tool.Ypos * 15;
                                       rect = new Rectangle(coordenadas, medida);
                                       Drawing_Area.Invalidate(rect);
                                       Drawing_Area.Update();
                                       if (Agent_Tool.Clock_inverse == true)
                                       {
                                           if (Agent_Tool.neighborhood[3] == 2)//Caso que tenga que moverse a un hueco
                                           {
                                               World_Data = Agent_Tool.Agent_Restore(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                               Agent_Tool = (Agente)Event_List[i];
                                               coordenadas.X = Agent_Tool.Xpos * 15;
                                               coordenadas.Y = Agent_Tool.Ypos * 15;
                                               switch (World_Data[Agent_Tool.Ypos][Agent_Tool.Xpos])
                                               {
                                                   case (4):
                                                       g.DrawImage(A_Arriba_PNG, coordenadas);
                                                       break;
                                                   case (5):
                                                       g.DrawImage(A_Abajo_PNG, coordenadas);
                                                       break;
                                                   case (6):
                                                       g.DrawImage(A_Izquierda_PNG, coordenadas);
                                                       break;
                                                   case (7):
                                                       g.DrawImage(A_Derecha_PNG, coordenadas);
                                                       break;
                                               }
                                               break;
                                           }
                                           //El agente ya tiene un giro en contra de las agujas del reloj (Aquí se sustituyó turn_left por move_up)
                                           World_Data = Agent_Tool.move_right(World_Data, Event_List, (Agente)Event_List[i], false, Lambda_Exp, Lambda_Huecos);
                                           Agent_Tool = (Agente)Event_List[i];
                                           coordenadas.X = Agent_Tool.Xpos * 15;
                                           coordenadas.Y = Agent_Tool.Ypos * 15;
                                           g.DrawImage(A_Derecha_PNG, coordenadas);
                                       }
                                       else
                                       {//Initial
                                           World_Data = Agent_Tool.turn_down(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                           Agent_Tool = (Agente)Event_List[i];
                                           coordenadas.X = Agent_Tool.Xpos * 15;
                                           coordenadas.Y = Agent_Tool.Ypos * 15;
                                           g.DrawImage(A_Abajo_PNG, coordenadas);
                                       }
                                       break;
                                   }
                                   if ((Agent_Tool.neighborhood[0] == 1) && //Si tiene traste a la izquierda diagonal inferior y no está push 
                                 (Agent_Tool.detect_pushing_trast(World_Data, Agent_Tool.Ypos - 1, Agent_Tool.Xpos - 1, 7) == false))
                                   {
                                       coordenadas.X = Agent_Tool.Xpos * 15;
                                       coordenadas.Y = Agent_Tool.Ypos * 15;
                                       rect = new Rectangle(coordenadas, medida);
                                       Drawing_Area.Invalidate(rect);
                                       Drawing_Area.Update();
                                       if (Agent_Tool.Clock_sense == true)
                                       {
                                           if (Agent_Tool.neighborhood[3] == 2)//Caso que tenga que moverse a un hueco
                                           {
                                               World_Data = Agent_Tool.Agent_Restore(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                               Agent_Tool = (Agente)Event_List[i];
                                               coordenadas.X = Agent_Tool.Xpos * 15;
                                               coordenadas.Y = Agent_Tool.Ypos * 15;
                                               switch (World_Data[Agent_Tool.Ypos][Agent_Tool.Xpos])
                                               {
                                                   case (4):
                                                       g.DrawImage(A_Arriba_PNG, coordenadas);
                                                       break;
                                                   case (5):
                                                       g.DrawImage(A_Abajo_PNG, coordenadas);
                                                       break;
                                                   case (6):
                                                       g.DrawImage(A_Izquierda_PNG, coordenadas);
                                                       break;
                                                   case (7):
                                                       g.DrawImage(A_Derecha_PNG, coordenadas);
                                                       break;
                                               }
                                               break;
                                           }
                                           //El agente ya tiene un giro a favor de las agujas del reloj(Aquí se sustituyó turn_left por move_up)
                                           World_Data = Agent_Tool.move_right(World_Data, Event_List, (Agente)Event_List[i], false, Lambda_Exp, Lambda_Huecos);
                                           Agent_Tool = (Agente)Event_List[i];
                                           coordenadas.X = Agent_Tool.Xpos * 15;
                                           coordenadas.Y = Agent_Tool.Ypos * 15;
                                           g.DrawImage(A_Derecha_PNG, coordenadas);
                                       }
                                       else
                                       {//Initial
                                           World_Data = Agent_Tool.turn_up(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                           Agent_Tool = (Agente)Event_List[i];
                                           coordenadas.X = Agent_Tool.Xpos * 15;
                                           coordenadas.Y = Agent_Tool.Ypos * 15;
                                           g.DrawImage(A_Arriba_PNG, coordenadas);
                                       }
                                       break;
                                   }
                               }
                               //Si el agente no tiene traste frente ni a los lados.
                               Agent_Tool.get_neighborhood(World_Data, Agent_Tool.Xpos, Agent_Tool.Ypos);//Actualizar vecindad
                               if ((Agent_Tool.neighborhood[3] == 0) && (Agent_Tool.neighborhood[5] != 1) &&
                                   (Agent_Tool.neighborhood[1] != 1))
                               {
                                   coordenadas.X = Agent_Tool.Xpos * 15;
                                   coordenadas.Y = Agent_Tool.Ypos * 15;
                                   rect = new Rectangle(coordenadas, medida);
                                   Drawing_Area.Invalidate(rect);
                                   Drawing_Area.Update();
                                   World_Data = Agent_Tool.move_right(World_Data, Event_List, (Agente)Event_List[i], false, Lambda_Exp, Lambda_Huecos);
                                   Agent_Tool = (Agente)Event_List[i];
                                   coordenadas.X = Agent_Tool.Xpos * 15;
                                   coordenadas.Y = Agent_Tool.Ypos * 15;
                                   g.DrawImage(A_Derecha_PNG, coordenadas);
                               }
                               else
                               {
                                   //Si el agente no tiene traste frente pero tiene alguno a los lados.
                                   if ((Agent_Tool.neighborhood[3] != 1) && ((Agent_Tool.neighborhood[5] == 1) ||
                                       (Agent_Tool.neighborhood[1] == 1)))
                                   {
                                       if ((Agent_Tool.neighborhood[5] == 1) && //Si tiene traste a la derecha y no está push 
                                 (Agent_Tool.detect_pushing_trast(World_Data, Agent_Tool.Ypos + 1, Agent_Tool.Xpos, 0) == false))
                                       {
                                           coordenadas.X = Agent_Tool.Xpos * 15;
                                           coordenadas.Y = Agent_Tool.Ypos * 15;
                                           rect = new Rectangle(coordenadas, medida);
                                           Drawing_Area.Invalidate(rect);
                                           Drawing_Area.Update();
                                           if ((Agent_Tool.Clock_inverse == true) &&
                                               ((Agent_Tool.neighborhood[3] == 0) || (Agent_Tool.neighborhood[3] == 2)))
                                           {
                                               if (Agent_Tool.neighborhood[3] == 2)//Caso que tenga que moverse a un hueco
                                               {
                                                   World_Data = Agent_Tool.Agent_Restore(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                   Agent_Tool = (Agente)Event_List[i];
                                                   coordenadas.X = Agent_Tool.Xpos * 15;
                                                   coordenadas.Y = Agent_Tool.Ypos * 15;
                                                   switch (World_Data[Agent_Tool.Ypos][Agent_Tool.Xpos])
                                                   {
                                                       case (4):
                                                           g.DrawImage(A_Arriba_PNG, coordenadas);
                                                           break;
                                                       case (5):
                                                           g.DrawImage(A_Abajo_PNG, coordenadas);
                                                           break;
                                                       case (6):
                                                           g.DrawImage(A_Izquierda_PNG, coordenadas);
                                                           break;
                                                       case (7):
                                                           g.DrawImage(A_Derecha_PNG, coordenadas);
                                                           break;
                                                   }
                                                   break;
                                               }
                                               //El agente ya tiene un giro en contra de las agujas del reloj (Aquí se sustituyó turn_left por move_up)
                                               World_Data = Agent_Tool.move_right(World_Data, Event_List, (Agente)Event_List[i], false, Lambda_Exp, Lambda_Huecos);
                                               Agent_Tool = (Agente)Event_List[i];
                                               coordenadas.X = Agent_Tool.Xpos * 15;
                                               coordenadas.Y = Agent_Tool.Ypos * 15;
                                               g.DrawImage(A_Derecha_PNG, coordenadas);
                                           }
                                           else
                                           {//Initial
                                               World_Data = Agent_Tool.turn_down(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                               Agent_Tool = (Agente)Event_List[i];
                                               coordenadas.X = Agent_Tool.Xpos * 15;
                                               coordenadas.Y = Agent_Tool.Ypos * 15;
                                               g.DrawImage(A_Abajo_PNG, coordenadas);
                                           }
                                           break;
                                       }
                                       if ((Agent_Tool.neighborhood[1] == 1) && //Si tiene traste abajo y no está push 
                                 (Agent_Tool.detect_pushing_trast(World_Data, Agent_Tool.Ypos - 1, Agent_Tool.Xpos, 0) == false))
                                       {
                                           coordenadas.X = Agent_Tool.Xpos * 15;
                                           coordenadas.Y = Agent_Tool.Ypos * 15;
                                           rect = new Rectangle(coordenadas, medida);
                                           Drawing_Area.Invalidate(rect);
                                           Drawing_Area.Update();
                                           if ((Agent_Tool.Clock_sense == true) &&
                                               ((Agent_Tool.neighborhood[3] == 0) || (Agent_Tool.neighborhood[3] == 2)))
                                           {
                                               if (Agent_Tool.neighborhood[3] == 2)//Caso que tenga que moverse a un hueco
                                               {
                                                   World_Data = Agent_Tool.Agent_Restore(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                   Agent_Tool = (Agente)Event_List[i];
                                                   coordenadas.X = Agent_Tool.Xpos * 15;
                                                   coordenadas.Y = Agent_Tool.Ypos * 15;
                                                   switch (World_Data[Agent_Tool.Ypos][Agent_Tool.Xpos])
                                                   {
                                                       case (4):
                                                           g.DrawImage(A_Arriba_PNG, coordenadas);
                                                           break;
                                                       case (5):
                                                           g.DrawImage(A_Abajo_PNG, coordenadas);
                                                           break;
                                                       case (6):
                                                           g.DrawImage(A_Izquierda_PNG, coordenadas);
                                                           break;
                                                       case (7):
                                                           g.DrawImage(A_Derecha_PNG, coordenadas);
                                                           break;
                                                   }
                                                   break;
                                               }
                                               //El agente ya tiene un giro a favor de las agujas del reloj(Aquí se sustituyó turn_left por move_up)
                                               World_Data = Agent_Tool.move_right(World_Data, Event_List, (Agente)Event_List[i], false, Lambda_Exp, Lambda_Huecos);
                                               Agent_Tool = (Agente)Event_List[i];
                                               coordenadas.X = Agent_Tool.Xpos * 15;
                                               coordenadas.Y = Agent_Tool.Ypos * 15;
                                               g.DrawImage(A_Derecha_PNG, coordenadas);
                                           }
                                           else
                                           {//Initial
                                               World_Data = Agent_Tool.turn_up(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                               Agent_Tool = (Agente)Event_List[i];
                                               coordenadas.X = Agent_Tool.Xpos * 15;
                                               coordenadas.Y = Agent_Tool.Ypos * 15;
                                               g.DrawImage(A_Arriba_PNG, coordenadas);
                                           }
                                           break;
                                       }
//@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
                                       if ((Agent_Tool.neighborhood[5] == 1) && //Si tiene traste a la derecha y está push 
                                 (Agent_Tool.detect_pushing_trast(World_Data, Agent_Tool.Ypos + 1, Agent_Tool.Xpos, 0) == true))
                                       {
                                           if ((Agent_Tool.neighborhood[3] == 0) || (Agent_Tool.neighborhood[3] == 2))
                                           {
                                               coordenadas.X = Agent_Tool.Xpos * 15;
                                               coordenadas.Y = Agent_Tool.Ypos * 15;
                                               rect = new Rectangle(coordenadas, medida);
                                               Drawing_Area.Invalidate(rect);
                                               Drawing_Area.Update();
                                               if (Agent_Tool.neighborhood[3] == 2)//Caso que tenga que moverse a un hueco
                                               {
                                                   World_Data = Agent_Tool.Agent_Restore(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                   Agent_Tool = (Agente)Event_List[i];
                                                   coordenadas.X = Agent_Tool.Xpos * 15;
                                                   coordenadas.Y = Agent_Tool.Ypos * 15;
                                                   switch (World_Data[Agent_Tool.Ypos][Agent_Tool.Xpos])
                                                   {
                                                       case (4):
                                                           g.DrawImage(A_Arriba_PNG, coordenadas);
                                                           break;
                                                       case (5):
                                                           g.DrawImage(A_Abajo_PNG, coordenadas);
                                                           break;
                                                       case (6):
                                                           g.DrawImage(A_Izquierda_PNG, coordenadas);
                                                           break;
                                                       case (7):
                                                           g.DrawImage(A_Derecha_PNG, coordenadas);
                                                           break;
                                                   }
                                                   break;
                                               }
                                               World_Data = Agent_Tool.move_right(World_Data, Event_List, (Agente)Event_List[i], false, Lambda_Exp, Lambda_Huecos);
                                               Agent_Tool = (Agente)Event_List[i];
                                               coordenadas.X = Agent_Tool.Xpos * 15;
                                               coordenadas.Y = Agent_Tool.Ypos * 15;
                                               g.DrawImage(A_Derecha_PNG, coordenadas);
                                               break;
                                           }
                                           else
                                           {
                                               World_Data = Agent_Tool.turn_up(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                               Agent_Tool = (Agente)Event_List[i];
                                               coordenadas.X = Agent_Tool.Xpos * 15;
                                               coordenadas.Y = Agent_Tool.Ypos * 15;
                                               g.DrawImage(A_Arriba_PNG, coordenadas);
                                               break;
                                           }
                                       }
                                       if ((Agent_Tool.neighborhood[1] == 1) && //Si tiene traste abajo y está push 
                                 (Agent_Tool.detect_pushing_trast(World_Data, Agent_Tool.Ypos - 1, Agent_Tool.Xpos, 0) == true))
                                       {
                                           if ((Agent_Tool.neighborhood[3] == 0) || (Agent_Tool.neighborhood[3] == 2))
                                           {
                                               coordenadas.X = Agent_Tool.Xpos * 15;
                                               coordenadas.Y = Agent_Tool.Ypos * 15;
                                               rect = new Rectangle(coordenadas, medida);
                                               Drawing_Area.Invalidate(rect);
                                               Drawing_Area.Update();
                                               if (Agent_Tool.neighborhood[3] == 2)//Caso que tenga que moverse a un hueco
                                               {
                                                   World_Data = Agent_Tool.Agent_Restore(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                   Agent_Tool = (Agente)Event_List[i];
                                                   coordenadas.X = Agent_Tool.Xpos * 15;
                                                   coordenadas.Y = Agent_Tool.Ypos * 15;
                                                   switch (World_Data[Agent_Tool.Ypos][Agent_Tool.Xpos])
                                                   {
                                                       case (4):
                                                           g.DrawImage(A_Arriba_PNG, coordenadas);
                                                           break;
                                                       case (5):
                                                           g.DrawImage(A_Abajo_PNG, coordenadas);
                                                           break;
                                                       case (6):
                                                           g.DrawImage(A_Izquierda_PNG, coordenadas);
                                                           break;
                                                       case (7):
                                                           g.DrawImage(A_Derecha_PNG, coordenadas);
                                                           break;
                                                   }
                                                   break;
                                               }
                                               World_Data = Agent_Tool.move_right(World_Data, Event_List, (Agente)Event_List[i], false, Lambda_Exp, Lambda_Huecos);
                                               Agent_Tool = (Agente)Event_List[i];
                                               coordenadas.X = Agent_Tool.Xpos * 15;
                                               coordenadas.Y = Agent_Tool.Ypos * 15;
                                               g.DrawImage(A_Derecha_PNG, coordenadas);
                                               break;
                                           }
                                           else
                                           {
                                               World_Data = Agent_Tool.turn_down(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                               Agent_Tool = (Agente)Event_List[i];
                                               coordenadas.X = Agent_Tool.Xpos * 15;
                                               coordenadas.Y = Agent_Tool.Ypos * 15;
                                               g.DrawImage(A_Abajo_PNG, coordenadas);
                                               break;
                                           }
                                       }
//@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ 
                                   }
                                   else
                                   {//Caso en que el agente cae en un hueco
                                       if (Agent_Tool.neighborhood[3] == 2)
                                       {
                                           coordenadas.X = Agent_Tool.Xpos * 15;
                                           coordenadas.Y = Agent_Tool.Ypos * 15;
                                           rect = new Rectangle(coordenadas, medida);
                                           Drawing_Area.Invalidate(rect);
                                           Drawing_Area.Update();
                                           World_Data = Agent_Tool.Agent_Restore(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                           Agent_Tool = (Agente)Event_List[i];
                                           coordenadas.X = Agent_Tool.Xpos * 15;
                                           coordenadas.Y = Agent_Tool.Ypos * 15;
                                           switch (World_Data[Agent_Tool.Ypos][Agent_Tool.Xpos])
                                           {
                                               case (4):
                                                   g.DrawImage(A_Arriba_PNG, coordenadas);
                                                   break;
                                               case (5):
                                                   g.DrawImage(A_Abajo_PNG, coordenadas);
                                                   break;
                                               case (6):
                                                   g.DrawImage(A_Izquierda_PNG, coordenadas);
                                                   break;
                                               case (7):
                                                   g.DrawImage(A_Derecha_PNG, coordenadas);
                                                   break;
                                           }
                                       }
                                       else
                                       {//Caso en que el agente no tiene ni traste ni hueco delante@@@@@@@@@@@@@@@@@@@@1
                                           if (Agent_Tool.neighborhood[1] == 3)
                                           {
                                               if (Agent_Tool.Clock_inverse == true)
                                               {//El agente ya tiene un giro a favor de las agujas del reloj
                                                   World_Data = Agent_Tool.turn_up(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                   Agent_Tool = (Agente)Event_List[i];
                                                   coordenadas.X = Agent_Tool.Xpos * 15;
                                                   coordenadas.Y = Agent_Tool.Ypos * 15;
                                                   g.DrawImage(A_Arriba_PNG, coordenadas);
                                               }
                                               else
                                               {//Initial
                                                   World_Data = Agent_Tool.turn_down(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                   Agent_Tool = (Agente)Event_List[i];
                                                   coordenadas.X = Agent_Tool.Xpos * 15;
                                                   coordenadas.Y = Agent_Tool.Ypos * 15;
                                                   g.DrawImage(A_Abajo_PNG, coordenadas);
                                               }
                                               break;
                                           }
                                           if (Agent_Tool.neighborhood[5] == 3)
                                           {
                                               if (Agent_Tool.Clock_sense == true)
                                               {//El agente ya tiene un giro en contra de las agujas del reloj
                                                   World_Data = Agent_Tool.turn_down(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                   Agent_Tool = (Agente)Event_List[i];
                                                   coordenadas.X = Agent_Tool.Xpos * 15;
                                                   coordenadas.Y = Agent_Tool.Ypos * 15;
                                                   g.DrawImage(A_Abajo_PNG, coordenadas);
                                               }
                                               else
                                               {//Initial
                                                   World_Data = Agent_Tool.turn_up(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                   Agent_Tool = (Agente)Event_List[i];
                                                   coordenadas.X = Agent_Tool.Xpos * 15;
                                                   coordenadas.Y = Agent_Tool.Ypos * 15;
                                                   g.DrawImage(A_Arriba_PNG, coordenadas);
                                               }
                                               break;
                                           }
                                           //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
                                           int b = turn.Next(2);
                                           switch (b)
                                           {
                                               case (0):
                                                   if (Agent_Tool.Clock_inverse == true)
                                                   {//El agente ya tiene un giro en contra de las agujas del reloj
                                                       World_Data = Agent_Tool.turn_up(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                       Agent_Tool = (Agente)Event_List[i];
                                                       coordenadas.X = Agent_Tool.Xpos * 15;
                                                       coordenadas.Y = Agent_Tool.Ypos * 15;
                                                       g.DrawImage(A_Arriba_PNG, coordenadas);
                                                   }
                                                   else
                                                   {//Initial
                                                       World_Data = Agent_Tool.turn_down(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                       Agent_Tool = (Agente)Event_List[i];
                                                       coordenadas.X = Agent_Tool.Xpos * 15;
                                                       coordenadas.Y = Agent_Tool.Ypos * 15;
                                                       g.DrawImage(A_Abajo_PNG, coordenadas);
                                                   }
                                                   break;
                                               case (1):
                                                   if (Agent_Tool.Clock_sense == true)
                                                   {//El agente ya tiene un giro a favor de las agujas del reloj
                                                       World_Data = Agent_Tool.turn_down(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                       Agent_Tool = (Agente)Event_List[i];
                                                       coordenadas.X = Agent_Tool.Xpos * 15;
                                                       coordenadas.Y = Agent_Tool.Ypos * 15;
                                                       g.DrawImage(A_Abajo_PNG, coordenadas);
                                                   }
                                                   else
                                                   {//Initial
                                                       World_Data = Agent_Tool.turn_up(World_Data, (Agente)Event_List[i], Lambda_Exp);
                                                       Agent_Tool = (Agente)Event_List[i];
                                                       coordenadas.X = Agent_Tool.Xpos * 15;
                                                       coordenadas.Y = Agent_Tool.Ypos * 15;
                                                       g.DrawImage(A_Arriba_PNG, coordenadas);
                                                   }
                                                   break;
                                           }
                                       }
                                   }
                               }
                           }
                           break;
                    }
                                
                }
                if (Event_List[i].GetType() == Hole)//Tratamiento de los huecos
                {
                    Hueco_Tool = (Hueco)Event_List[i];
                    if (((Hueco_Tool.Time_of_reaction * 100) + Elapsed_Time) <= Elapsed_Time + 1)
                    {
                        Hueco_Tool.Time_of_reaction = Distrib.nextExponential(Lambda_Exp);
                        coordenadas.X = Hueco_Tool.Xpos * 15;
                        coordenadas.Y = Hueco_Tool.Ypos * 15;
                        rect = new Rectangle(coordenadas, medida);
                        Drawing_Area.Invalidate(rect);
                        Drawing_Area.Update();
                        World_Data[Hueco_Tool.Ypos][Hueco_Tool.Xpos] = 0;
                        World_Data = Hueco_Tool.Hole_Moving(World_Data, (Hueco)Event_List[i]);
                        Hueco_Tool = (Hueco)Event_List[i];
                        coordenadas.X = Hueco_Tool.Xpos * 15;
                        coordenadas.Y = Hueco_Tool.Ypos * 15;
                        g.DrawImage(Hueco_PNG, coordenadas);
                    }
                    else
                    {
                        Event_List[i].Time_of_reaction -= 0.001;
                        //Hueco_Tool.Time_of_reaction -= 0.001;
                        //Hueco_Tool = (Hueco)Event_List[i];
                    }
                }
                if (Event_List[i].GetType() == Trast)//Tratamiento de los trastes
                {
                    int[] Cord = new int[2];
                    Trast_Tool = (Show_up_traste)Event_List[i];
                    if (((Trast_Tool.Time_of_reaction * 100) + Elapsed_Time) <= Elapsed_Time + 1)
                    {
                        Trast_Tool.Time_of_reaction = Distrib.nextExponential(Lambda_Trastes);
                        Cord = Trast_Tool.Trast_show(World_Data);
                        World_Data[Cord[0]][Cord[1]] = 1;
                        coordenadas.X = Cord[1] * 15;
                        coordenadas.Y = Cord[0] * 15;
                        g.DrawImage(Traste_PNG, coordenadas);
                    }
                    else
                    {
                        Event_List[i].Time_of_reaction -= 0.01;
                        //Trast_Tool.Time_of_reaction -= 0.001;
                        //Trast_Tool = (Show_up_traste)Event_List[i];
                    }
                    
                }
            }
        }

        private void TB_Trastes_TextChanged(object sender, EventArgs e)
        {
            String m = TB_Trastes.Text;
            if(m.Equals("0")==true)
            {
                    TB_Trastes.Text = "";
                    Err_Dialog error = new Err_Dialog("-0- Número no permitido!!!");
                    error.ShowDialog();
            }
            if (m.Equals("") == false)
            {
                try
                {
                    Cant_Trastes = int.Parse(m);
                }
                catch (FormatException)
                {
                    TB_Trastes.Text = "";
                    Err_Dialog error = new Err_Dialog("Entrada no válida!!!");
                    error.ShowDialog();
                }
            }
        }

        private void TB_Huecos_TextChanged(object sender, EventArgs e)
        {
            String m = TB_Huecos.Text;
            if (m.Equals("0") == true)
            {
                TB_Huecos.Text = "";
                Err_Dialog error = new Err_Dialog("-0- Número no permitido!!!");
                error.ShowDialog();
            }
            if (m.Equals("") == false)
            {
                try
                {
                    Cant_Huecos = int.Parse(m);
                }
                catch (FormatException)
                {
                    TB_Huecos.Text = "";
                    Err_Dialog error = new Err_Dialog("Entrada no válida!!!");
                    error.ShowDialog();
                }
            }
        }

        private void TB_Wobots_TextChanged(object sender, EventArgs e)
        {
            String m = TB_Wobots.Text;
            if (m.Equals("0") == true)
            {
                TB_Wobots.Text = "";
                Err_Dialog error = new Err_Dialog("-0- Número no permitido!!!");
                error.ShowDialog();
            }
            if (m.Equals("") == false)
            {
                try
                {
                    Cant_Wobots = int.Parse(m);
                }
                catch (FormatException)
                {
                    TB_Wobots.Text = "";
                    Err_Dialog error = new Err_Dialog("Entrada no válida!!!");
                    error.ShowDialog();
                }
            }
        }

        private void TB_Muros_TextChanged(object sender, EventArgs e)
        {
            String m = TB_Muros.Text;
            if (m.Equals("") == false)
            {
                try
                {
                    Cant_Muros = int.Parse(m);
                }
                catch (FormatException)
                {
                    TB_Muros.Text = "";
                    Cant_Muros = 0;
                    Err_Dialog error = new Err_Dialog("Entrada no válida!!!");
                    error.ShowDialog();
                }
            }
        }

        private void TB_Muros_H_TextChanged(object sender, EventArgs e)
        {
            String m = TB_Muros_H.Text;
            if (m.Equals("0") == true)
            {
                TB_Muros_H.Text = "";
                Err_Dialog error = new Err_Dialog("-0- Número no permitido!!!");
                error.ShowDialog();
            }
            if (m.Equals("") == false)
            {
                try
                {
                    Lambda_Muros_Horizontales = int.Parse(m);
                }
                catch (FormatException)
                {
                    TB_Muros_H.Text = "";
                    Err_Dialog error = new Err_Dialog("Entrada no válida!!!");
                    error.ShowDialog();
                }
            }
        }

        private void TB_Muros_V_TextChanged(object sender, EventArgs e)
        {
            String m = TB_Muros_V.Text;
            if (m.Equals("0") == true)
            {
                TB_Muros_V.Text = "";
                Err_Dialog error = new Err_Dialog("-0- Número no permitido!!!");
                error.ShowDialog();
            }
            if (m.Equals("") == false)
            {
                try
                {
                    Lambda_Muros_Verticales = int.Parse(m);
                }
                catch (FormatException)
                {
                    TB_Muros_V.Text = "";
                    Err_Dialog error = new Err_Dialog("Entrada no válida!!!");
                    error.ShowDialog();
                }
            }
        }

        private void TB_Exponential_TextChanged(object sender, EventArgs e)
        {
            String m = TB_Exponential.Text;
            if (m.Equals("0") == true)
            {
                TB_Exponential.Text = "";
                Err_Dialog error = new Err_Dialog("-0- Número no permitido!!!");
                error.ShowDialog();
            }
            if (m.Equals("") == false)
            {
                try
                {
                    Lambda_Exp = double.Parse(m);
                }
                catch (FormatException)
                {
                    TB_Exponential.Text = "";
                    Err_Dialog error = new Err_Dialog("Entrada no válida!!!");
                    error.ShowDialog();
                }
            }
        }

        private void TB_Lambda_Huecos_TextChanged(object sender, EventArgs e)
        {
            String m = TB_Lambda_Huecos.Text;
            if (m.Equals("0") == true)
            {
                TB_Lambda_Huecos.Text = "";
                Err_Dialog error = new Err_Dialog("-0- Número no permitido!!!");
                error.ShowDialog();
            }
            if (m.Equals("") == false)
            {
                try
                {
                    Lambda_Huecos = double.Parse(m);
                }
                catch (FormatException)
                {
                    TB_Lambda_Huecos.Text = "";
                    Err_Dialog error = new Err_Dialog("Entrada no válida!!!");
                    error.ShowDialog();
                }
            }
        }

        private void TB_Lambda_Trastes_TextChanged(object sender, EventArgs e)
        {
            String m = TB_Lambda_Trastes.Text;
            if (m.Equals("0") == true)
            {
                TB_Lambda_Trastes.Text = "";
                Err_Dialog error = new Err_Dialog("-0- Número no permitido!!!");
                error.ShowDialog();
            }
            if (m.Equals("") == false)
            {
                try
                {
                    Lambda_Trastes = double.Parse(m);
                }
                catch (FormatException)
                {
                    TB_Lambda_Trastes.Text = "";
                    Err_Dialog error = new Err_Dialog("Entrada no válida!!!");
                    error.ShowDialog();
                }
            }
        }

        private void TB_Cobertura_TextChanged(object sender, EventArgs e)
        {
            String m = TB_Cobertura.Text;
            if (m.Equals("0") == true)
            {
                TB_Cobertura.Text = "";
                Err_Dialog error = new Err_Dialog("-0- Número no permitido!!!");
                error.ShowDialog();
            }
            if (m.Equals("") == false)
            {
                try
                {
                    cobertura = int.Parse(m);
                }
                catch (FormatException)
                {
                    TB_Cobertura.Text = "";
                    Err_Dialog error = new Err_Dialog("Entrada no válida!!!");
                    error.ShowDialog();
                }
            }
        }

        private void TB_Time_TextChanged(object sender, EventArgs e)
        {
            String m = TB_Time.Text;
            if (m.Equals("0") == true)
            {
                TB_Time.Text = "";
                Err_Dialog error = new Err_Dialog("-0- Número no permitido!!!");
                error.ShowDialog();
            }
            if (m.Equals("") == false)
            {
                try
                {
                    Total_Time = int.Parse(m);
                }
                catch (FormatException)
                {
                    TB_Time.Text = "";
                    Err_Dialog error = new Err_Dialog("Entrada no válida!!!");
                    error.ShowDialog();
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void aboutTrastilvaniaToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
