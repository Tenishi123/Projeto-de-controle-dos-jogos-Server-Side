using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.IO;
using projetoConexão.Properties;
using System.Threading;
using System.Diagnostics;

namespace projetoConexão
{
    public partial class Form1 : Form
    {
        string processo = "";
        string IP = "192.168.1.52";
        int port = 80;
        TcpClient cliente = new TcpClient();
        NetworkStream clienteStream;
        BinaryWriter binaryWriter;
        BinaryReader binaryReader;
        int TempoDeExecuxao = 0;
        Process[] processes;
        public Form1()
        {
            InitializeComponent();
        }

        private void conectar()
        {
            try
            {
                cliente.Connect(IP, port);//tenta conectar com o cliente
            }
            catch
            {

                MessageBox.Show("Conexão falhou");

            }

            if (cliente.Connected)//verifica se o cliente conectou
            {

                clienteStream = cliente.GetStream();//Agrega o cliente ao network stream

            }

        }

        private void enviarRequest()
        {

            binaryWriter = new BinaryWriter(clienteStream);//Agrega o cliente ao binary Writer
            binaryWriter.Write(true);//Envia um tipo booleano

        }

        private bool receberRequestAutorizacao()
        {

            binaryReader = new BinaryReader(clienteStream);//Agrega o cliente ao binary Reader
            bool condicao = false;

            try
            {
                condicao = binaryReader.ReadBoolean();//Agrega a variavel condição o que é recebido do servidor
            }
            catch
            {

                return false;

            }

            if (condicao)//verifica condição
            {

                return true;

            }
            else
            {

                return false;

            }

        }

        void startJogo()
        {

            Random rand = new Random();
            int numRandom = rand.Next(1, 1);//randomiza um numero

            if (numRandom == 1)//verifica o numero randomizado
            {

                Process.Start(Application.StartupPath + "\\Um\\projetoJogo1.exe");//inicia o jogo
                processo = "projetoJogo1";//agrega o nome do processo á uma variavel

            }


        }

        private void Form1_Load(object sender, EventArgs e)
        {

            do
            {

                conectar();

            } while (!cliente.Connected);//Executa até se conectar ao cliente



            do
            {



            } while (!receberRequestAutorizacao());//Executa até receber o request de autorização

            int controlLaco = 0;

            do
            {

                startJogo();//inicia um jogo

                do
                {

                     processes = Process.GetProcessesByName(processo);//pega o numero de processos com a String passada

                } while (processes.Length > 0);//Executa enquanto o jogo rodar

                controlLaco++;

            } while (controlLaco <= 0);//Numero de jogos que irá rodar

            enviarRequest();//Envia um request que o usuario terminou

            terminado.Start();//Envia informações para o servidor verificar

        }

        private void BtnReiniciar_Click(object sender, EventArgs e)
        {

            Application.Restart();

        }

        private void Terminado_Tick(object sender, EventArgs e)
        {

            binaryWriter = new BinaryWriter(clienteStream);
            binaryWriter.Write(false);

        }
    }
}
