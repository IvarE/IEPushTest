using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using WindowsFormsApplication1.ServiceReference1;
using System.ServiceModel;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label1.Text = "Running";
            label1.Refresh();

            // local
            string serviceUrl = "http://localhost:55467/Service1.svc";
            BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.TransportCredentialOnly);

            // 
            binding.MaxReceivedMessageSize = int.MaxValue;
            binding.MaxBufferSize = int.MaxValue;
            binding.SendTimeout = TimeSpan.FromHours(20);
            binding.CloseTimeout = TimeSpan.FromHours(20);
            binding.OpenTimeout = TimeSpan.FromHours(20);
            binding.ReceiveTimeout = TimeSpan.FromHours(20);
            
            ServiceReference1.Service1Client _client = new Service1Client(binding, new EndpointAddress(serviceUrl));
            bool _ok = _client.CreateContacts();

            label1.Text = _ok.ToString();
            label1.Refresh();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            label1.Text = "Running";
            label1.Refresh();

            // local
            string serviceUrl = "http://localhost:55467/Service1.svc";
            BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.TransportCredentialOnly);

            // 
            binding.MaxReceivedMessageSize = int.MaxValue;
            binding.MaxBufferSize = int.MaxValue;
            binding.SendTimeout = TimeSpan.FromHours(20);
            binding.CloseTimeout = TimeSpan.FromHours(20);
            binding.OpenTimeout = TimeSpan.FromHours(20);
            binding.ReceiveTimeout = TimeSpan.FromHours(20);

            ServiceReference1.Service1Client _client = new Service1Client(binding, new EndpointAddress(serviceUrl));
            bool _ok = _client.CreateTravelCards();

            label1.Text = _ok.ToString();
            label1.Refresh();
        }
    }
}
