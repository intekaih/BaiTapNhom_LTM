using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using BaiTapNhom;

namespace Client
{
    internal class Client
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;


            SinhVien sinhVien = new SinhVien();

            IPEndPoint svEndPoint = new IPEndPoint(IPAddress.Loopback, 5000);
            Socket svSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            Console.WriteLine("Đang kết nối với server...");

            try
            {
                svSocket.Connect(svEndPoint);
                Console.WriteLine("Kết nối thành công.");

                // Nhập thông tin sinh viên từ người dùng
                Console.Write("Nhập MSSV: ");
                int mssv = int.Parse(Console.ReadLine());
                Console.Write("Nhập họ tên: ");
                string hoTen = Console.ReadLine();
                Console.Write("Nhập điểm trung bình: ");
                double diemTB = double.Parse(Console.ReadLine());

                SinhVien sv = new SinhVien(mssv, hoTen, diemTB);

                // Gửi thông tin sinh viên đến server
                byte[] data = SinhVien.GetBytes(sv);
                svSocket.Send(data);

                // Nhận thông tin sinh viên từ server
                byte[] buffer = new byte[1024];
                int byteReceive = svSocket.Receive(buffer);
                SinhVien receivedSv = SinhVien.GetSV(buffer);
                Console.WriteLine("Thông tin nhận từ server:");
                Console.WriteLine($"MSSV: {receivedSv.MSSV}");
                Console.WriteLine($"Họ tên: {receivedSv.HoTen}");
                Console.WriteLine($"Điểm TB: {receivedSv.DiemTB}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi: " + ex.Message);
            }
            finally
            {
                svSocket.Shutdown(SocketShutdown.Both);
                svSocket.Close();
            }

            Console.ReadKey();
        }
    }
}
