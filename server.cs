using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace WindowsFormsApp6  {

    public partial class Form1 : Form    {

        bool conectado=false;
        IPAddress localAdd;
        int puerto;
        TcpListener listener;

        public Form1()
        {
            InitializeComponent();
            string IPLocal;
            // Poner ip local en textBox1.Text al iniciar:
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    IPLocal = ip.ToString();
                    textBox1.Text = IPLocal;
                }
            }


        }

        void Conectar() {
            
            try  {
                Invoke(new Action(() => {
                    toolStripStatusLabel1.Text = "Servidor iniciado en...    " +
                        IPAddress.Parse(((IPEndPoint)listener.LocalEndpoint).Address.ToString()) +
                        ":" + ((IPEndPoint)listener.LocalEndpoint).Port.ToString();
                }));

                listener.Start();

                while (conectado) {
                    if (listener.Pending()) {
                        TcpClient cliente = listener.AcceptTcpClient();
                        NetworkStream nwStream = cliente.GetStream();
                        byte[] buffer = new byte[cliente.ReceiveBufferSize];
                        int bytes = nwStream.Read(buffer, 0, cliente.ReceiveBufferSize);

                        string mensaje = Encoding.ASCII.GetString(buffer, 0, bytes);

                        Invoke(new Action(() => {
                            label1.Text = mensaje;
                            toolStripStatusLabel1.Text = "Mensaje entrante de " +
                                ((IPEndPoint)cliente.Client.RemoteEndPoint).Address;
                        }));

                        cliente.Close();
                    }
                }

                listener.Stop();
                Invoke(new Action(() => {
                    label1.Text = "";
                    toolStripStatusLabel1.Text = "Servidor detenido.";
                }));
            }
            catch (SocketException ex)     {
                Console.WriteLine("SocketException: {0}", ex);              
            }
            finally       {
                listener.Stop();
            }

        }

        private void button1_Click(object sender, EventArgs e)  {
            if (!conectado)   {
                String s = textBox1.Text;
                String s2 = textBox2.Text;
                if (IPAddress.TryParse(s, out localAdd) && Int32.TryParse(s2, out puerto)) {
                    conectado = true;
                    listener = new TcpListener(localAdd, puerto);
                    Thread hilo1 = new Thread(Conectar);
                    hilo1.Start();
                }
                else toolStripStatusLabel1.Text = "Revisar entrada de datos.";
            }
        }

        private void button2_Click(object sender, EventArgs e)  {          
            conectado = false;
        }

        private void Form1_Load(object sender, EventArgs e) {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)  {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)  {

        }

        private void groupBox1_Enter(object sender, EventArgs e)    {

        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)  {

        }
    }
}
