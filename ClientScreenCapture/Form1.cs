using System.Drawing.Imaging;
using System.Net;
using System.Net.Sockets;
using System.Text;
using ScreenShotExample;

namespace ClientScreenCapture
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void btnSendScreen_Click(object sender, EventArgs e)
        {
            // ��������� �볺��� - ������� �������� �����
            TcpClient tcpClient = new TcpClient();

            try
            {
                // ��������� ������ ������ ��� ���������� �� ���������� �������
                IPAddress remouteAddress = IPAddress.Parse("192.168.56.1");

                // ϳ��������� �볺��� �� �������
                // - ������ IP-������ ������� �� ���� �������
                //tcpClient.Connect(remouteAddress, 11000); // ����������� �����
                await tcpClient.ConnectAsync(remouteAddress, 11000); // ����� async/await (Task)

                // ----------------------------------------- ²������� ����� �� ������ ------------------------------
                // ��������� ������ NetworkStream ��� �������� ����� - ����� ���������� �� �볺���
                NetworkStream ns = tcpClient.GetStream();

                // ------- ������ �������� ����� �� �������� ����������
                byte[] buffForImage = ClientsScreenForServer();
                await ns.WriteAsync(buffForImage); 
                // ----------------------------------------------------------------------------------------------------

            }
            catch (SystemException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                // �������� ����������
                tcpClient.Close();
            }
        }

        private byte[] ClientsScreenForServer()
        {
            ScreenCapture sc = new ScreenCapture();
            Image image = sc.CaptureScreen();
            byte[] buffer = null;
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, ImageFormat.Jpeg);
                buffer = ms.ToArray();
            }
            return buffer;
        }
    }
}