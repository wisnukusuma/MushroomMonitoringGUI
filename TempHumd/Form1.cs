using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;


namespace TempHumd
{
    public partial class Form1 : Form
    {
        private string data;
        private string data1, data3, data5;
        private int data2, data4, data6;
        //private string data3;
        private int _baudRate = 9600;
        private int _dataBits = 8;
        private Handshake _handshake = Handshake.None;
        private Parity _parity = Parity.None;
        private StopBits _stopBits = StopBits.One;
        int row = 0;
        int[] suhu1, suhu2;
        int[] kelembaban1, kelembaban2;
        int i = 0;
        int k = 0;
        public Form1()
        {
            InitializeComponent();
            //button2.Enabled = true;
            button3.Enabled = false;
            suhu1 = new Int32[50];
            suhu2 = new Int32[50];
            kelembaban1 = new Int32[50];
            kelembaban2 = new Int32[50];

        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            ldPort();
            setSerial();
        }

        private void setSerial()
        {
            myport = new SerialPort();
            myport.BaudRate = _baudRate;
            myport.Parity = _parity;
            myport.Handshake = _handshake;
            myport.DataBits = _dataBits;
            myport.StopBits = _stopBits;
        }

        private void ldPort()
        {
            string[] ArrayComPortsNames = null;
            int index = -1;
            string ComPortName = null;
            ArrayComPortsNames = SerialPort.GetPortNames();
            if (ArrayComPortsNames.Length != 0)
            {
                do
                {
                    index += 1;
                    comboBox1.Items.Add(ArrayComPortsNames[index]);
                }
                while (!((ArrayComPortsNames[index] == ComPortName) || (index == ArrayComPortsNames.GetUpperBound(0))));
                Array.Sort(ArrayComPortsNames);
                if (index == ArrayComPortsNames.GetUpperBound(0))
                {
                    ComPortName = ArrayComPortsNames[0];
                }
                comboBox1.Text = ArrayComPortsNames[0];
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {            
            for (int j = 1; j <= i; j++)
            {
                
                chart1.Series["Suhu1"].Points.AddXY(j, suhu1[j]);
                chart1.Series["Suhu2"].Points.AddXY(j, suhu2[j]);

            }
            for (int j = 1; j <= k; j++)
            {

                chart2.Series["Kelembaban1"].Points.AddXY(j, kelembaban1[j]);
                chart2.Series["Kelembaban2"].Points.AddXY(j, kelembaban2[j]);

            }

        }

        private void comboBox1_DropDown(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            ldPort();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (myport.IsOpen) myport.Close();
                myport.PortName = comboBox1.Text;
                myport.Open();
                
            }
            catch (Exception er)
            {
                MessageBox.Show("Port tidak dapat dibuka " + er.ToString(), "Buka Port Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (myport.IsOpen) { button1.Enabled = false; button3.Enabled = true; comboBox1.Enabled = false; myport.DataReceived += terima; }

        }

        void terima(object sender, SerialDataReceivedEventArgs e)
        {
            data = myport.ReadLine();
            
        
            if (data.Substring(0, 1) == "1" && data.Substring (30,1)=="#")
            {
                i++;
                data1 = data.Substring(2, 18);
                //data2 = Convert.ToInt32(data1);
                data3 = data.Substring(21, 4);
                data4 = Convert.ToInt32(data3)/100;
                data5 = data.Substring(26, 4);
                data6 = Convert.ToInt32(data5)/100;
                this.Invoke((MethodInvoker)delegate {
                    dataGridView1.Rows.Add();
                    row = dataGridView1.Rows.Count - 2;
                    dataGridView1["Column1", row].Value = 1;
                    dataGridView1["Column2", row].Value = data1;
                    dataGridView1["Column3", row].Value = data4; suhu1[i] = data4;
                    dataGridView1["Column4", row].Value = data6; kelembaban1[i] = data6;
                });
             }
            else if (data.Substring(0, 1) == "2" && data.Substring(30, 1) == "#")
            {
                k++; 
                data1 = data.Substring(2, 18);
                //data2 = Convert.ToInt32(data1);
                data3 = data.Substring(21, 4);
                data4 = Convert.ToInt32(data3)/100;
                data5 = data.Substring(26, 4);
                data6 = Convert.ToInt32(data5)/100;
                this.Invoke((MethodInvoker)delegate {
                    dataGridView1.Rows.Add();
                    row = dataGridView1.Rows.Count - 2;
                    dataGridView1["Column1", row].Value = 2;
                    dataGridView1["Column2", row].Value = data1;
                    dataGridView1["Column3", row].Value = data4; suhu2[k] = data4;
                    dataGridView1["Column4", row].Value = data6; kelembaban2[k] = data6;
                });
            }
        }

        

        private void button3_Click(object sender, EventArgs e)
        {
            button1.Enabled = true;
            //button2.Enabled = true;
            button3.Enabled = false;

            myport.Close();
        }        
    }
}
