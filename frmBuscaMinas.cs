using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JuegoBuscaMinas
{
    public partial class frmBuscaMinas : Form
    {
        private static int intentosJugados;
        private static int TOTFILAS = 9;
        private static int TOTCOL = 9;
        private static int TOTBOMBAS = 12;
        private static string[,] matriz = new string[TOTFILAS, TOTCOL];
        public frmBuscaMinas()
        {
            InitializeComponent();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnJugar_Click(object sender, EventArgs e)
        {
            Button[,] botones = new Button[TOTFILAS, TOTCOL];//Crea los botones con sus dimensiones


            this.groupBox1.Controls.Clear(); //Borrar todos los controles

            crearMatriz(botones);
            crearBombas(botones);
            ponerGuiasBombas(botones);
            copiarMatriz(botones, matriz);
            encerarBotones(botones);
        }

        /// <summary>
        /// Encerar la matriz de botones
        /// </summary>
        /// <param name="botones"></param>
        private void encerarBotones(Button[,] botones)
        {
            int totalFilas = botones.GetLength(0);
            int totalColumnas = botones.GetLength(1);
            for (int i = 0; i < totalFilas; i++)
            {
                for (int j = 0; j < totalColumnas; j++)
                {
                    botones[i, j].Text = "";
                }
            }
        }

        /// <summary>
        /// Copia la matriz de botones (propiedad Text) en la matriz de string
        /// </summary>
        /// <param name="botones"></param>
        /// <param name="matriz"></param>
        private void copiarMatriz(Button[,] botones, string[,] matriz)
        {
            int totalFilas = botones.GetLength(0);
            int totalColumnas = botones.GetLength(1);
            for (int i = 0; i < totalFilas; i++)
            {
                for (int j = 0; j < totalColumnas; j++)
                {
                    matriz[i, j] = botones[i, j].Text;
                }
            }
        }

        private void validaJuego_Click(object sender, EventArgs e)
        {

            string texto = (sender as Button).Tag.ToString();
            string[] aux = texto.Split('-');

            int fila = Int32.Parse(aux[0]);
            int col = Int32.Parse(aux[1]);

            //mostrar el contenido de la matriz en el boton que se hizo clic
            (sender as Button).Text = matriz[fila, col];

            //desactivar el boton 
            if (matriz[fila, col] == "")
                (sender as Button).Enabled = false;
            //MessageBox.Show($"Hiciste clic: {texto}");

            //Si presiona boton que contiene una bomba automaticamente el Jugador Pierde y el juego termina
            if ((sender as Button).Text == "💣")
            {
                pictureBox1.Image = JuegoBuscaMinas.Properties.Resources.perdisteemoji;
                MessageBox.Show("Perdiste");
                groupBox1.Enabled = false;
            }

            intentosJugados++;//Cada vez que presiona un boton este aumenta su valor

            //Si el jugador no presiona ninguna bomba este gana la partida y el juego termina 
            if (intentosJugados == 69)
            {
                pictureBox1.Image = JuegoBuscaMinas.Properties.Resources.ganasteemoji;
                MessageBox.Show("Ganaste");
                groupBox1.Enabled = false;
            }
        }

        private void crearMatriz(Button[,] botones)
        {
            int x = 10, y = 10;
            for (int i = 0; i < botones.GetLength(0); i++)
            {
                for (int j = 0; j < botones.GetLength(1); j++)
                {
                    botones[i, j] = new Button();
                    botones[i, j].Text = "";
                    botones[i, j].Font = new Font(botones[i, j].Font, FontStyle.Bold);
                    botones[i, j].Cursor = Cursors.Hand;
                    botones[i, j].Location = new System.Drawing.Point(x, y);
                    botones[i, j].Size = new System.Drawing.Size(32, 27);
                    botones[i, j].Click += new System.EventHandler(this.validaJuego_Click);
                    //Almaceno las posiciones de filas y columnas 
                    botones[i, j].Tag = i.ToString() + "-" + j.ToString();

                    this.groupBox1.Controls.Add(botones[i, j]);
                    x += 30;
                }
                x = 10; y += 27;
            }

        }

        private void ponerGuiasBombas(Button[,] botones)
        {
            int totalFilas = botones.GetLength(0);
            int totalColumnas = botones.GetLength(1);

            for (int i = 0; i < totalFilas; i++)
            {
                for (int j = 0; j < totalColumnas; j++)
                {

                    if (botones[i, j].Text != "💣")
                    {
                        int bombas = 0;
                        botones[i, j].Text = "";

                        if (totalColumnas > j + 1)
                            if (botones[i, j + 1].Text == "💣") //buscando bomba a la derecha de la posición actual
                                bombas++;

                        if (totalColumnas > j - 1 && j - 1 >= 0)
                            if (botones[i, j - 1].Text == "💣") //buscando bomba a la izquierda de la posición actual
                                bombas++;

                        if (totalFilas > i + 1)
                            if (botones[i + 1, j].Text == "💣") //buscando bomba abajo de la posición actual
                                bombas++;

                        if (totalFilas > i - 1 && i - 1 >= 0)
                            if (botones[i - 1, j].Text == "💣") //buscando bomba arriba de la posición actual
                                bombas++;

                        if (totalColumnas > j + 1 && totalFilas > i - 1 && i - 1 >= 0)
                        {
                            if (botones[i - 1, j + 1].Text == "💣") //Buscando bomba en diagonal superior derecha de la posición actual
                                bombas++;
                        }

                        if (totalColumnas > j - 1 && j - 1 >= 0 && totalFilas > i - 1 && i - 1 >= 0)
                        {
                            if (botones[i - 1, j - 1].Text == "💣") //Buscando bomba en diagonal superior izquierda de la posición actual
                                bombas++;
                        }

                        if (totalColumnas > j + 1 && totalFilas > i + 1)
                        {
                            if (botones[i + 1, j + 1].Text == "💣") //Buscando bomba en diagonal inferior derecha de la posición actual
                                bombas++;
                        }

                        if (totalColumnas > j - 1 && j - 1 >= 0 && totalFilas > i + 1)
                        {
                            if (botones[i + 1, j - 1].Text == "💣") //Buscando bomba en diagonal inferior izquierda de la posición actual
                                bombas++;
                        }

                        if (bombas > 0)
                        {
                            botones[i, j].Text = bombas.ToString(); //mostrando la cantidad de bombas
                            botones[i, j].ForeColor = Color.BlueViolet;
                        }

                    }

                }
            }

        }

        private void crearBombas(Button[,] botones)
        {
            var seed = Environment.TickCount;
            var random = new Random(seed);

            for (int i = 1; i < TOTBOMBAS; i++)
            {
                var x = random.Next(0, TOTFILAS);
                var y = random.Next(0, TOTCOL);
                botones[x, y].Text = "💣";
                botones[x, y].ForeColor = Color.Red;

            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

    }
} //Desarrollado por Angel Mora
    

