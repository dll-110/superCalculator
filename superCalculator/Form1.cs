using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace superCalculator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }



        decimal lastResult = 0;


        // 设置此窗体为活动窗体：
        // 将创建指定窗口的线程带到前台并激活该窗口。键盘输入直接指向窗口，并为用户更改各种视觉提示。
        // 系统为创建前台窗口的线程分配的优先级略高于其他线程。
        [DllImport("user32.dll", EntryPoint = "SetForegroundWindow")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        // 设置此窗体为活动窗体：
        // 激活窗口。窗口必须附加到调用线程的消息队列。
        [DllImport("user32.dll", EntryPoint = "SetActiveWindow")]
        public static extern IntPtr SetActiveWindow(IntPtr hWnd);

        // 设置窗体位置
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int y, int Width, int Height, int flags);

        private void Form1_Load(object sender, EventArgs e)
        {
            // 设置窗体显示在最上层
            SetWindowPos(this.Handle, -1, 0, 0, 0, 0, 0x0001 | 0x0002 | 0x0010 | 0x0080);

            // 设置本窗体为活动窗体
            SetActiveWindow(this.Handle);
            SetForegroundWindow(this.Handle);
            this.KeyPreview = true;
            // 设置窗体置顶
            this.TopMost = true;
        }

        public void OnKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                int cursorLine = richTextBox1.GetLineFromCharIndex(richTextBox1.SelectionStart);
                int start = richTextBox1.GetFirstCharIndexFromLine(cursorLine);
                int end;
                if (cursorLine + 1 < richTextBox1.Lines.Count())
                {
                    end = richTextBox1.GetFirstCharIndexFromLine(cursorLine + 1);
                }
                else
                {
                    end = richTextBox1.Text.Length;
                }
                string lineT = richTextBox1.Text.Substring(start, end - start);
                                               
                if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Oemplus || (e.KeyCode == Keys.Add && lineT.EndsWith("+")))
                {
                    // 提取等号前面的内容
                    string input = richTextBox1.Lines.LastOrDefault();
                    int index = input.LastIndexOf('=');
                    if (index != -1)
                    {
                        input = input.Substring(0, index);
                    }

                    if (input.EndsWith("+"))
                    {
                        input = input.Substring(0, input.Length - 1);
                    }


                    // 计算表达式的结果
                    lastResult = Calculate(input);
                    if (input.EndsWith("+"))
                    {
                        richTextBox1.AppendText("=" + lastResult);
                    }
                    else
                    {
                        richTextBox1.AppendText("=" + lastResult);
                    }

                    // 在下一行添加结果
                    richTextBox1.AppendText(Environment.NewLine);
                    e.Handled = true;
                }


                int cursorPos = richTextBox1.SelectionStart;



                int lineStartPos = richTextBox1.GetFirstCharIndexFromLine(richTextBox1.GetLineFromCharIndex(cursorPos));

                // 如果当前光标位置在行首，则表示当前行是新的一行
                bool isNewLine = cursorPos == lineStartPos;

                if (e.KeyCode == Keys.Add && lineT.EndsWith("+"))
                {
                    return;
                }

                if ((e.KeyCode == Keys.Add || e.KeyCode == Keys.Subtract || e.KeyCode == Keys.Multiply || e.KeyCode == Keys.Divide) && isNewLine)
                {
                    // 复制上一行结果到当前行符号的前面
                    string lastLine = richTextBox1.Lines.LastOrDefault();
                    richTextBox1.AppendText(lastResult + "");
                    //if (lastLine != null && lastLine.EndsWith("="))
                    //{
                    //    string result = lastLine.Substring(lastLine.LastIndexOf('=') + 1).Trim();
                    //}
                }
            }
            catch (Exception err)
            {
                this.toolStripStatusLabel1.Text = err.Message;
            }
        }



        private decimal Calculate(string input)
        {
            // 利用DataTable的Compute方法计算表达式的结果
            if (!input.Trim().Equals(""))
            {
                DataTable dt = new DataTable();
                object result = dt.Compute(input, "");
                return Convert.ToDecimal(result);
            }
            return 0;
        }

        private void richTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        private void richTextBox1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {

            string lastChars = "";


            Ranks();



           
            // 如果当前字符是加号并且前一个字符也是加号，则将其替换为等号
           // if (e.KeyCode == Keys.Add )
          //  {
                // 将两个加号替换为等号
          //      SendKeys.Send("{=}");

                // 将事件标记为已处理，以防止生成多个键盘事件
         //       e.IsInputKey = true;
         //   }
        }
        private void Ranks()
        {
            /*  得到光标行第一个字符的索引，
             *  即从第1个字符开始到光标行的第1个字符索引*/
            int index = richTextBox1.GetFirstCharIndexOfCurrentLine();
            /*得到光标行的行号,第1行从0开始计算，习惯上我们是从1开始计算，所以+1。 */
            int line = richTextBox1.GetLineFromCharIndex(index) + 1;
            /*  SelectionStart得到光标所在位置的索引
             *  再减去
             *  当前行第一个字符的索引
             *  = 光标所在的列数(从0开始)  */
            int column = richTextBox1.SelectionStart - index + 1;


            this.toolStripStatusLabel1.Text = string.Format("第：{0}行 {1}列,{2}", line.ToString(), column.ToString(), index.ToString());
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void richTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            
        }
    }
}
