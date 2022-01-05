using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace WindowsFormsApp5
{
    public partial class Form1 : Form    {

        string IPLocal;

        public Form1()
        {
            InitializeComponent();
            // Poner ip local en textBox1.Text al iniciar:
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    IPLocal = ip.ToString();
                    this.Text = "Cliente pruebas TCP: " + IPLocal + "  - Fernando Alvarez";

                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Conectando....";
            int puerto;
            IPAddress dir;
            string n = textBox2.Text;
            string IPservidor = textBox1.Text; ;
            string msg = textBox3.Text;


            if (Int32.TryParse(n,out puerto) && IPAddress.TryParse(IPservidor,out dir)) {

                try
                {
                    TcpClient cliente = new TcpClient(IPservidor, puerto);
                    NetworkStream stream = cliente.GetStream();
                    byte[] bytes = ASCIIEncoding.ASCII.GetBytes(msg);
                    stream.Write(bytes, 0, bytes.Length);
                    toolStripStatusLabel1.Text = "Enviado... " + msg;
                    cliente.Close();
                    textBox3.Text = "";
                }
                catch (ArgumentNullException ex)
                {
                    Console.WriteLine("ArgumentNullException: {0}", ex);
                }
                catch (SocketException ex)
                {
                    Console.WriteLine("SocketException: {0}", ex);
                    toolStripStatusLabel1.Text = "El servidor no responde.";
                }
            }
            else toolStripStatusLabel1.Text = "Revisar entrada de datos.";

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {

        }
    }
}
