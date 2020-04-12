using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace WindowsApp
{
    public partial class Form1 : Form
    {
        [DllImport("MyDLL.dll", CallingConvention = CallingConvention.Cdecl)]
        private extern static int add(int a, int b);

        // ポインタはC#側ではIntPtrに
        [DllImport("MyDLL.dll", CallingConvention = CallingConvention.Cdecl)]
        private extern static IntPtr arr_a(int length);

        [DllImport("MyDLL.dll", CallingConvention = CallingConvention.Cdecl)]
        private extern static void arr_b(IntPtr arr, int length);

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        
            callAdd(display);
            callArr_a(display);
            callArr_b(display);
            Console.WriteLine("test");
            //Console.ReadKey();
        }

        // 戻り値がintの場合
        private static void callAdd(TextBox textbox)
        {
            int addResult = add(1, 2);

            textbox.Text = "== add ==" + "\r\n";
            textbox.AppendText(addResult + "\r\n");

            Console.WriteLine("== add ==");
            Console.WriteLine(addResult + "\n");
        }

        // 戻り値がポインタの場合
        private static void callArr_a(TextBox textbox)
        {
            const int NUM = 5;
            int[] arr = new int[NUM];

            // 戻り値のポインタをIntPtrで受け取る
            IntPtr ptr = arr_a(NUM);

            // マネージ配列へコピー
            Marshal.Copy(ptr, arr, 0, NUM);

            // これでメモリが解放される？
            // (追記)さすがに駄目でした
            //Marshal.FreeCoTaskMem(ptr);

            textbox.AppendText("== arr_a ==" + "\r\n");
            Console.WriteLine("== arr_a ==");
            foreach (int n in arr)
            {
                textbox.AppendText(n + " ");
                Console.Write(n + " ");
            }
            textbox.AppendText("\r\n");
            Console.WriteLine("\n");
        }

        // 引数がポインタの場合
        private static void callArr_b(TextBox textbox)
        {
            const int NUM = 5;
            int[] arr = new int[NUM];

            // アンマネージ配列のメモリを確保
            IntPtr ptr = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(int)) * NUM);

            // 引数でポインタを渡す
            arr_b(ptr, NUM);

            // マネージ配列へコピー
            Marshal.Copy(ptr, arr, 0, NUM);

            // アンマネージ配列のメモリを解放
            Marshal.FreeCoTaskMem(ptr);

            textbox.AppendText("== arr_b ==" + "\r\n");
            Console.WriteLine("== arr_b ==");
            foreach (int n in arr)
            {
                textbox.AppendText(n + " ");
                Console.Write(n + " ");
            }
            textbox.AppendText("\r\n");
            Console.WriteLine("\n");
        }
    }
}
