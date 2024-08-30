using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaiTapNhom
{
    public class SinhVien
    {
        public int MSSV { get; set; }
        public string HoTen { get; set; }
        public double DiemTB { get; set; }
        public int HoTenSize { get; set; }
        public int Size { get; set; }

        public SinhVien() { }

        public SinhVien(int mssv, string hoTen, double diemTB)
        {
            MSSV = mssv;
            HoTen = hoTen;
            DiemTB = diemTB;
            HoTenSize = Encoding.UTF8.GetByteCount(hoTen);
            Size = 4 + 4 + HoTenSize + 8; // 4 bytes for int, 4 bytes for int (HoTenSize), HoTenSize bytes for string, 8 bytes for double
        }

        public static byte[] GetBytes(SinhVien sv)
        {
            byte[] mssvBytes = BitConverter.GetBytes(sv.MSSV);
            byte[] hoTenBytes = Encoding.UTF8.GetBytes(sv.HoTen);
            byte[] diemTBBytes = BitConverter.GetBytes(sv.DiemTB);
            byte[] hoTenSizeBytes = BitConverter.GetBytes(sv.HoTenSize);

            // Combine all bytes into one array
            byte[] result = new byte[sv.Size];
            Buffer.BlockCopy(mssvBytes, 0, result, 0, 4);
            Buffer.BlockCopy(hoTenSizeBytes, 0, result, 4, 4);
            Buffer.BlockCopy(hoTenBytes, 0, result, 8, hoTenBytes.Length);
            Buffer.BlockCopy(diemTBBytes, 0, result, 8 + hoTenBytes.Length, 8);

            return result;
        }

        public static SinhVien GetSV(byte[] data)
        {
            int mssv = BitConverter.ToInt32(data, 0);
            int hoTenSize = BitConverter.ToInt32(data, 4);
            string hoTen = Encoding.UTF8.GetString(data, 8, hoTenSize);
            double diemTB = BitConverter.ToDouble(data, 8 + hoTenSize);

            return new SinhVien(mssv, hoTen, diemTB)
            {
                HoTenSize = hoTenSize,
                Size = data.Length
            };
        }
    }
}
