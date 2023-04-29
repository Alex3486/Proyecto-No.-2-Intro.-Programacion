using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_No._2_Intro.Programacion
{
    internal class Program
    {
        static string nombre_jugador1, nombre_jugador2, nombre_jugador3 = "COMPUTADORA";
        static string jugador1 = "o", jugador2 = "x", borrador = "";
        static int FILAS = 6, COLUMNAS = 7;
        static string[,] tablero_de_juego = new string[FILAS, COLUMNAS];
        static string minutos, segundos, dato;

        static int conecta = 4, no_conecta = 20;
        static int conecta_vertical = 21, conecta_horizontal = 22, conecta_inclinado1 = 23, conecta_inclinado2 = 24;
        static int fila_invalida = 25, columna_llena = 26, error_ninguno = 27, fila_no_encontrada = 28;

        static void Main(string[] args)
        {
            string nombre_jugadorActual, jugadorActual, min, seg, turnos_total;
            int opcion, opcion_2, columna = 0, estado, ganador, turno = 0, turno_total, m, s, partidas_guardadas = 0;
            Stopwatch tiempo = new Stopwatch();

            string dato_acumulado = "";

        Opciones:
            Console.WriteLine("-------------------------------------------------------------------");
            Console.WriteLine("                           Connect Four");
            Console.WriteLine("\n" + "1. Jugador contra Jugador");
            Console.WriteLine("2. Jugador contra CPU");
            Console.WriteLine("3. Partidas guardadas");
            Console.Write("\n" + "Seleccione una opción: ");
            opcion = Convert.ToInt32(Console.ReadLine());

            switch (opcion)
            {
                case 1:
                    Nombres:
                    Console.WriteLine("\n" + "-------------------------------------------------------------------");
                    Console.WriteLine("                  Jugador contra Jugador");
                    Console.Write("\n" + "Ingrese el nombre del Jugador 1: ");
                    nombre_jugador1 = Console.ReadLine();
                    Console.Write("Ingrese el nombre del Jugador 2: ");
                    nombre_jugador2 = Console.ReadLine();

                    if (nombre_jugador1 == nombre_jugador3 || nombre_jugador2 == nombre_jugador3)
                    {
                        Console.WriteLine("\n" + "Error: Uno de los nombres o los dos nombres de los jugadores son igual a 'COMPUTADORA'");
                        Console.WriteLine("El nombre 'COMPUTADORA' solo se utiliza cuando se juega con la CPU, vuelva a ingresar los nombres");
                        Console.ReadKey();
                        Console.Clear();
                        goto Nombres;
                    }

                    limpiar_tablero(tablero_de_juego);
                    nombre_jugadorActual = nombre_jugador1;
                    jugadorActual = jugador1;
                    tiempo.Start();
                    Console.WriteLine("\n" + "Comienza el jugador: " + nombre_jugadorActual);

                    while (true)
                    {
                        Console.WriteLine("\n" + "-------------------------------------------------------------------");
                        Console.WriteLine("Turno del jugador: " + nombre_jugadorActual);
                        turno++;
                        dibujar_tablero(tablero_de_juego);
                        columna = Columna_Jugador();
                        estado = colocar_pieza(jugadorActual, columna, tablero_de_juego);

                        if (estado == columna_llena)
                        {
                            Console.Write("Error: columna llena" + "\n");
                            nombre_jugadorActual = siguiente_nombre_jugador(nombre_jugadorActual);
                            jugadorActual = siguiente_jugador(jugadorActual);
                            turno--;
                        }
                        else if (estado == fila_invalida)
                        {
                            Console.Write("Error: columna inválida" + "\n");
                            nombre_jugadorActual = siguiente_nombre_jugador(nombre_jugadorActual);
                            jugadorActual = siguiente_jugador(jugadorActual);
                            turno--;
                        }
                        else if (estado == error_ninguno)
                        {
                            ganador = Ganador(jugadorActual, tablero_de_juego);

                            if (ganador != no_conecta)
                            {
                                dibujar_tablero(tablero_de_juego);
                                Console.WriteLine("Gana el jugador: " + nombre_jugadorActual);
                                tiempo.Stop();
                                turno_total = (turno / 2) + 1;
                                turnos_total = Convert.ToString(turno_total);
                                Console.WriteLine("El jugador gano en " + turno_total + " turnos");

                                if (tiempo.Elapsed.TotalMinutes <= 60 && tiempo.Elapsed.TotalSeconds <= 60)
                                {
                                    m = Convert.ToInt32(tiempo.Elapsed.TotalMinutes);
                                    s = Convert.ToInt32(tiempo.Elapsed.TotalSeconds);
                                }
                                else
                                {
                                    m = (Convert.ToInt32(tiempo.Elapsed.TotalMinutes)) / 60;
                                    s = (Convert.ToInt32(tiempo.Elapsed.TotalSeconds)) / 60;
                                }

                                min = Convert.ToString(m);
                                seg = Convert.ToString(s);
                                Tiempo_minutos(min);
                                Tiempo_segundos(seg);
                                Console.WriteLine("Tiempo que tardo en ganar el jugador (en minutos y segundos): " + minutos + ":" + segundos);

                                partidas_guardadas++;
                                dato = ("| Nombre: " + nombre_jugadorActual + " | turnos: " + turnos_total + " | tiempo (en minutos y segundos): " + minutos + ":" + segundos + " |");
                                dato_acumulado = dato_acumulado + "\n" + dato;

                            Opcion1_1:
                                Console.WriteLine("\n" + "-------------------------------------------------------------------");
                                Console.WriteLine("1. Iniciar nueva partida");
                                Console.WriteLine("2. Cerrar el programa");
                                Console.Write("\n" + "Seleccione una opción: ");
                                opcion_2 = Convert.ToInt32(Console.ReadLine());

                                switch (opcion_2)
                                {
                                    case 1:
                                        nombre_jugador1 = String.Empty;
                                        nombre_jugador2 = String.Empty;
                                        turno = 0;
                                        columna = 0;
                                        estado = 0;
                                        ganador = 0;
                                        turno_total = 0;
                                        tiempo.Restart();
                                        tiempo.Stop();
                                        m = 0;
                                        s = 0;
                                        min = String.Empty;
                                        seg = String.Empty;
                                        minutos = String.Empty;
                                        segundos = String.Empty;
                                        Console.Clear();
                                        goto Opciones;

                                    case 2:
                                        Environment.Exit(1);
                                        break;

                                    default:
                                        Console.WriteLine("\n" + "Error: Seleccione una opción válida");
                                        Console.ReadLine();
                                        goto Opcion1_1;
                                }
                                break;
                            }
                            else if (Empate(tablero_de_juego))
                            {
                                dibujar_tablero(tablero_de_juego);
                                Console.Write("Empate");
                                tiempo.Stop();
                                turno_total = (turno / 2) + 1;
                                turnos_total = Convert.ToString(turno_total);
                                Console.WriteLine("Los jugadores empataron en " + turno_total + " turnos");

                                if (tiempo.Elapsed.TotalMinutes <= 60 && tiempo.Elapsed.TotalSeconds <= 60)
                                {
                                    m = Convert.ToInt32(tiempo.Elapsed.TotalMinutes);
                                    s = Convert.ToInt32(tiempo.Elapsed.TotalSeconds);
                                }
                                else
                                {
                                    m = (Convert.ToInt32(tiempo.Elapsed.TotalMinutes)) / 60;
                                    s = (Convert.ToInt32(tiempo.Elapsed.TotalSeconds)) / 60;
                                }

                                min = Convert.ToString(m);
                                seg = Convert.ToString(s);
                                Tiempo_minutos(min);
                                Tiempo_segundos(seg);
                                Console.WriteLine("Tiempo que tardaron en empatar los jugadores (en minutos y segundos): " + minutos + ":" + segundos);

                                Opcion1_2:
                                Console.WriteLine("-------------------------------------------------------------------");
                                Console.WriteLine("1. Iniciar nueva partida");
                                Console.WriteLine("2. Cerrar el programa");
                                Console.Write("\n" + "Seleccione una opción: ");
                                opcion_2 = Convert.ToInt32(Console.ReadLine());

                                switch (opcion_2)
                                {
                                    case 1:
                                        nombre_jugador1 = String.Empty;
                                        nombre_jugador2 = String.Empty;
                                        turno = 0;
                                        columna = 0;
                                        estado = 0;
                                        ganador = 0;
                                        turno_total = 0;
                                        tiempo.Restart();
                                        tiempo.Stop();
                                        m = 0;
                                        s = 0;
                                        min = String.Empty;
                                        seg = String.Empty;
                                        minutos = String.Empty;
                                        segundos = String.Empty;
                                        Console.Clear();
                                        goto Opciones;

                                    case 2:
                                        Environment.Exit(1);
                                        break;

                                    default:
                                        Console.WriteLine("\n" + "Error: Seleccione una opción válida");
                                        Console.ReadLine();
                                        goto Opcion1_2;
                                }
                                break;
                            }
                        }

                        nombre_jugadorActual = siguiente_nombre_jugador(nombre_jugadorActual);
                        jugadorActual = siguiente_jugador(jugadorActual);
                    }

                    break;

                case 2:
                    Nombre:
                    Console.WriteLine("\n" + "-------------------------------------------------------------------");
                    Console.WriteLine("                    Jugador contra CPU");
                    Console.Write("\n" + "Ingrese su nombre para jugador 1: ");
                    nombre_jugador1 = Console.ReadLine();
                    nombre_jugador2 = nombre_jugador3;
                    Console.WriteLine("Jugador 2: " + nombre_jugador2);

                    if (nombre_jugador1 == nombre_jugador3)
                    {
                        Console.WriteLine("\n" + "Error: Su nombre no tiene que ser igual al de 'COMPUTADORA', vuelva a ingresar su nombre");
                        Console.ReadKey();
                        Console.Clear();
                        goto Nombre;
                    }

                    limpiar_tablero(tablero_de_juego);
                    nombre_jugadorActual = nombre_jugador1;
                    jugadorActual = jugador1;
                    tiempo.Start();
                    Console.WriteLine("\n" + "Comienza el jugador: " + nombre_jugadorActual);

                    while (true)
                    {
                        Console.WriteLine("\n" + "-------------------------------------------------------------------");
                        Console.WriteLine("Turno del jugador: " + nombre_jugadorActual);
                        turno++;
                        dibujar_tablero(tablero_de_juego);
                        
                        if (nombre_jugadorActual == nombre_jugador2)
                        {
                            columna = Columna_CPU();
                        }
                        else
                        {
                            columna = Columna_Jugador();
                        }

                        estado = colocar_pieza(jugadorActual, columna, tablero_de_juego);

                        if (estado == columna_llena)
                        {
                            Console.Write("Error: columna llena" + "\n");
                            nombre_jugadorActual = siguiente_nombre_jugador(nombre_jugadorActual);
                            jugadorActual = siguiente_jugador(jugadorActual);
                            turno--;
                        }
                        else if (estado == fila_invalida)
                        {
                            Console.Write("Error: columna inválida" + "\n");
                            nombre_jugadorActual = siguiente_nombre_jugador(nombre_jugadorActual);
                            jugadorActual = siguiente_jugador(jugadorActual);
                            turno--;
                        }
                        else if (estado == error_ninguno)
                        {
                            ganador = Ganador(jugadorActual, tablero_de_juego);

                            if (ganador != no_conecta)
                            {
                                dibujar_tablero(tablero_de_juego);
                                Console.WriteLine("Gana el jugador: " + nombre_jugadorActual);
                                tiempo.Stop();
                                turno_total = (turno / 2) + 1;
                                turnos_total = Convert.ToString(turno_total);
                                Console.WriteLine("El jugador gano en " + turno_total + " turnos");

                                if (tiempo.Elapsed.TotalMinutes <= 60 && tiempo.Elapsed.TotalSeconds <= 60)
                                {
                                    m = Convert.ToInt32(tiempo.Elapsed.TotalMinutes);
                                    s = Convert.ToInt32(tiempo.Elapsed.TotalSeconds);
                                }
                                else
                                {
                                    m = (Convert.ToInt32(tiempo.Elapsed.TotalMinutes)) / 60;
                                    s = (Convert.ToInt32(tiempo.Elapsed.TotalSeconds)) / 60;
                                }

                                min = Convert.ToString(m);
                                seg = Convert.ToString(s);
                                Tiempo_minutos(min);
                                Tiempo_segundos(seg);
                                Console.WriteLine("Tiempo que tardo en ganar el jugador (en minutos y segundos): " + minutos + ":" + segundos);

                                partidas_guardadas++;
                                dato = ("| Nombre: " + nombre_jugadorActual + " | turnos: " + turnos_total + " | tiempo (en minutos y segundos): " + minutos + ":" + segundos + " |");

                                dato_acumulado = dato_acumulado + "\n" + dato;

                            Opcion2_1:
                                Console.WriteLine("\n" + "-------------------------------------------------------------------");
                                Console.WriteLine("1. Iniciar nueva partida");
                                Console.WriteLine("2. Cerrar el programa");
                                Console.Write("\n" + "Seleccione una opción: ");
                                opcion_2 = Convert.ToInt32(Console.ReadLine());

                                switch (opcion_2)
                                {
                                    case 1:
                                        nombre_jugador1 = String.Empty;
                                        nombre_jugador2 = String.Empty;
                                        turno = 0;
                                        columna = 0;
                                        estado = 0;
                                        ganador = 0;
                                        turno_total = 0;
                                        tiempo.Restart();
                                        tiempo.Stop();
                                        m = 0;
                                        s = 0;
                                        min = String.Empty;
                                        seg = String.Empty;
                                        minutos = String.Empty;
                                        segundos = String.Empty;
                                        Console.Clear();
                                        goto Opciones;

                                    case 2:
                                        Environment.Exit(1);
                                        break;

                                    default:
                                        Console.WriteLine("\n" + "Error: Seleccione una opción válida");
                                        Console.ReadLine();
                                        goto Opcion2_1;
                                }
                                break;
                            }
                            else if (Empate(tablero_de_juego))
                            {
                                dibujar_tablero(tablero_de_juego);
                                Console.Write("Empate");
                                tiempo.Stop();
                                turno_total = (turno / 2) + 1;
                                turnos_total = Convert.ToString(turno_total);
                                Console.WriteLine("Los jugadores empataron en " + turno_total + " turnos");

                                if (tiempo.Elapsed.TotalMinutes <= 60 && tiempo.Elapsed.TotalSeconds <= 60)
                                {
                                    m = Convert.ToInt32(tiempo.Elapsed.TotalMinutes);
                                    s = Convert.ToInt32(tiempo.Elapsed.TotalSeconds);
                                }
                                else
                                {
                                    m = (Convert.ToInt32(tiempo.Elapsed.TotalMinutes)) / 60;
                                    s = (Convert.ToInt32(tiempo.Elapsed.TotalSeconds)) / 60;
                                }

                                min = Convert.ToString(m);
                                seg = Convert.ToString(s);
                                Tiempo_minutos(min);
                                Tiempo_segundos(seg);
                                Console.WriteLine("Tiempo que tardaron en empatar los jugadores (en minutos y segundos): " + minutos + ":" + segundos);

                            Opcion2_2:
                                Console.WriteLine("\n" + "-------------------------------------------------------------------");
                                Console.WriteLine("1. Iniciar nueva partida");
                                Console.WriteLine("2. Cerrar el programa");
                                Console.Write("\n" + "Seleccione una opción: ");
                                opcion_2 = Convert.ToInt32(Console.ReadLine());

                                switch (opcion_2)
                                {
                                    case 1:
                                        nombre_jugador1 = String.Empty;
                                        nombre_jugador2 = String.Empty;
                                        turno = 0;
                                        columna = 0;
                                        estado = 0;
                                        ganador = 0;
                                        turno_total = 0;
                                        tiempo.Restart();
                                        tiempo.Stop();
                                        m = 0;
                                        s = 0;
                                        min = String.Empty;
                                        seg = String.Empty;
                                        minutos = String.Empty;
                                        segundos = String.Empty;
                                        Console.Clear();
                                        goto Opciones;

                                    case 2:
                                        Environment.Exit(1);
                                        break;

                                    default:
                                        Console.WriteLine("\n" + "Error: Seleccione una opción válida");
                                        Console.ReadLine();
                                        goto Opcion2_2;
                                }
                                break;
                            }
                        }

                        nombre_jugadorActual = siguiente_nombre_jugador(nombre_jugadorActual);
                        jugadorActual = siguiente_jugador(jugadorActual);
                    }

                    break;

                case 3:
                    Console.WriteLine("-------------------------------------------------------------------");
                    Console.WriteLine(dato_acumulado);

                    break;

                default:
                    Console.WriteLine("Error: Seleccione una opción válida");
                    Console.ReadKey();
                    Console.Clear();
                    goto Opciones;
            }

            Console.ReadKey();
        }

        static void dibujar_tablero(string[,] tablero)
        {
            Console.Write("\n");

            for (int encabezado = 0; encabezado < COLUMNAS; encabezado++)
            {
                Console.Write("| " + (encabezado + 1) + " ");

                if ((encabezado + 1) >= COLUMNAS)
                {
                    Console.Write("|");
                }
            }

            Console.Write("\n");
            for (int i = 0; i < FILAS; i++)
            {
                for (int j = 0; j < COLUMNAS; j++)
                {
                    Console.Write("| " + tablero[i, j] + " ");

                    if ((j + 1) >= COLUMNAS)
                    {
                        Console.Write("|");
                    }
                }

                Console.Write("\n");
            }
        }

        static void limpiar_tablero(string[,] tablero)
        {
            for (int m = 0; m < FILAS; m++)
            {
                for (int n = 0; n < COLUMNAS; n++)
                {
                    tablero[m, n] = borrador;
                }
            }
        }

        static int Columna_Jugador()
        {
            int columna;

            Console.Write("Escribe la columna en donde colocar la pieza: ");
            columna = Convert.ToInt32(Console.ReadLine());
            columna--;
            return columna;
        }

        static int colocar_pieza(string jugador, int columna, string[,] tablero)
        {
            int fila;

            if (columna < 0 || columna >= COLUMNAS)
            {
                return fila_invalida;
            }

            fila = fila_desocupada(columna, tablero);

            if (fila == fila_no_encontrada)
            {
                return columna_llena;
            }

            tablero[fila, columna] = jugador;
            return error_ninguno;
        }

        static int fila_desocupada(int columna, string[,] tablero)
        {
            for (int i = FILAS - 1; i >= 0; i--)
            {
                if (tablero[i, columna] == borrador)
                {
                    return i;
                }
            }

            return fila_no_encontrada;
        }

        static int Vertical(int x, int y, string jugador, string[,] tablero)
        {
            int y_inicio, contador = 0;

            y_inicio = (y - conecta >= 0) ? y - conecta + 1 : 0;

            for (int m = y_inicio; m <= y; m++)
            {
                if (tablero[m, x] == jugador)
                {
                    contador++;
                }
                else
                {
                    contador = 0;
                }
            }

            return contador;
        }

        static int Horizontal(int x, int y, string jugador, string[,] tablero)
        {
            int x_final, contador = 0;

            x_final = (x + conecta < COLUMNAS) ? x + conecta - 1 : COLUMNAS - 1;

            for (int m = x_final; x <= m; x++)
            {
                if (tablero[y, x] == jugador)
                {
                    contador++;
                }
                else
                {
                    contador = 0;
                }
            }

            return contador;
        }

        static int Inclinado_1(int x, int y, string jugador, string[,] tablero)
        {
            int x_final, y_inicio, contador = 0;

            x_final = (x + conecta < COLUMNAS) ? x + conecta - 1 : COLUMNAS - 1;
            y_inicio = (y - conecta >= 0) ? y - conecta + 1 : 0;

            while (x <= x_final && y_inicio <= y)
            {
                if (tablero[y, x] == jugador)
                {
                    contador++;
                }
                else
                {
                    contador = 0;
                }

                x++;
                y--;
            }

            return contador;
        }

        static int Inclinado_2(int x, int y, string jugador, string[,] tablero)
        {
            int x_final, y_final, contador = 0;

            x_final = (x + conecta < COLUMNAS) ? x + conecta - 1 : COLUMNAS - 1;
            y_final = (y + conecta < FILAS) ? y + conecta - 1 : FILAS - 1;

            while (x <= x_final && y <= y_final)
            {
                if (tablero[y, x] == jugador)
                {
                    contador++;
                }
                else
                {
                    contador = 0;
                }

                x++;
                y++;
            }

            return contador;
        }

        static int Ganador(string jugador, string[,] tablero)
        {
            for (int y = 0; y < FILAS; y++)
            {
                for (int x = 0; x < COLUMNAS; x++)
                {
                    if (Vertical(x, y, jugador, tablero) >= conecta)
                    {
                        return conecta_vertical;
                    }
                    if (Horizontal(x, y, jugador, tablero) >= conecta)
                    {
                        return conecta_horizontal;
                    }
                    if (Inclinado_1(x, y, jugador,tablero) >= conecta)
                    {
                        return conecta_inclinado1;
                    }
                    if (Inclinado_2(x, y, jugador,tablero) >= conecta)
                    {
                        return conecta_inclinado2;
                    }
                }
            }

            return no_conecta;
        }

        static bool Empate(string[,] tablero)
        {
            int resultado;

            for(int i = 0; i < COLUMNAS; i++)
            {
                resultado = fila_desocupada(i, tablero);

                if (resultado != fila_no_encontrada)
                {
                    return false;
                }
            }

            return true;
        }

        static string siguiente_nombre_jugador(string jugador)
        {
            if (jugador == nombre_jugador1)
            {
                return nombre_jugador2;
            }
            else
            {
                return nombre_jugador1;
            }
        }

        static string siguiente_jugador(string jugador)
        {
            if (jugador == jugador1)
            {
                return jugador2;
            }
            else
            {
                return jugador1;
            }
        }

        static int Columna_CPU()
        {
            int columna;

            Random cpu = new Random();
            columna = cpu.Next(1, 8);
            Console.Write("La 'COMPUTADORA' coloco la pieza en la columna: " + columna + "\n");
            return columna;
        }

        static string Tiempo_minutos(string m)
        {
            if (m.Length < 2)
            {
                minutos = m.PadLeft(2, '0');
                return minutos;
            }
            else
            {
                minutos = m;
                return minutos;
            }
        }

        static string Tiempo_segundos(string s)
        {
            if (s.Length < 2)
            {
                segundos = s.PadLeft(2, '0');
                return segundos;
            }
            else
            {
                segundos = s;
                return segundos;
            }
        }
    }
}
