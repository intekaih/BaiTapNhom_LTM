using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BaiTapNhom
{
    internal class Sever
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;


            // Tạo EndPoint cổng kết nối cho các client
            IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Any, 5000);
            // Tạo socket ipv4, TCP
            Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            // Nối socket với endpoint
            serverSocket.Bind(serverEndPoint);
            // Socket lắng nghe
            serverSocket.Listen(10);

            Console.WriteLine("Server đang chờ kết nối...");

            Socket clientSocket = serverSocket.Accept();
            Console.WriteLine("Client đã kết nối.");

            try
            {
                byte[] buffer = new byte[1024];

                while (true)
                {
                    int byteReceive = clientSocket.Receive(buffer);
                    if (byteReceive == 0)
                    {
                        Console.WriteLine("Client đã ngắt kết nối.");
                        break;
                    }

                    SinhVien sv = SinhVien.GetSV(buffer);
                    Console.WriteLine("Nhận từ client:");
                    Console.WriteLine($"MSSV: {sv.MSSV}");
                    Console.WriteLine($"Họ tên: {sv.HoTen}");
                    Console.WriteLine($"Điểm TB: {sv.DiemTB}");

                    // Gửi lại thông tin sinh viên cho client
                    byte[] responseBytes = SinhVien.GetBytes(sv);
                    clientSocket.Send(responseBytes);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi: " + ex.Message);
            }
            finally
            {
                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();
                serverSocket.Close();
            }

            Console.ReadKey();
        }
    }
}
