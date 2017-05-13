using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trastilvania_Project
{
    class Algorithm
    {
        public void Quicksort(Evento[] v, int prim, int ult)
        {
            if (prim < ult)
            {
                /* Selecciona un elemento del vector y coloca los menores
                 que él a su izquierda y los mayores a su derecha */
                int p = Pivote(v, prim, ult, ult);

                /* Repite el proceso para cada una de las 
                 particiones generadas en el paso anterior */
                Quicksort(v, prim, p - 1);
                Quicksort(v, p + 1, ult);
            }
        }

        /* Implementación no clásica de la función Pivote. En lugar de
        recorrer el vector simultáneamente desde ambos extremos hasta el
        cruce de índices, se recorre desde el comienzo hasta el final */
        int Pivote(Evento[] v, int prim, int ult, int piv)
        {
            double p = v[piv].Time_of_reaction;
            int j = prim;

            // Mueve el pivote a la última posición del vector
            Intercambia(v, piv, ult);

            /* Recorre el vector moviendo los elementos menores
             o iguales que el pivote al comienzo del mismo */
            for (int i = prim; i < ult; i++)
            {
                if (v[i].Time_of_reaction <= p)
                {
                    Intercambia(v, i, j);
                    j++;
                }
            }

            // Mueve el pivote a la posición que le corresponde
            Intercambia(v, j, ult);

            return j;
        }

        void Intercambia(Evento[] v, int a, int b)
        {
            if (a != b)
            {
                Evento tmp = v[a];
                v[a] = v[b];
                v[b] = tmp;
            }
        }

    }
}
