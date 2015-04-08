using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScanerHookKey
{
    public class ScanerKey
    {
        KeyboardHook keyboardHook;
        private DateTime previewTime = DateTime.Now;//初始化类时间
        private StringBuilder inputKey=new StringBuilder();

        /// <summary>
        /// 判断是不是扫描枪输入
        /// </summary>
        private bool isScaner;

        public bool IsScaner
        {
            get { return isScaner; }
            set { isScaner = value; }
        }
        
        /// <summary>
        /// 扫描枪扫描数据
        /// </summary>
        private string returnKey;

        public string ReturnKey
        {
            get { return returnKey; }
            set { returnKey = value; }
        }
        
        /// <summary>
        /// 通过输入数据判断是不是扫描进去的
        /// </summary>
        private int keyNum=0;
        
        public ScanerKey()
        {
            Init();
        }
        public ScanerKey(int keyNum)
        {
            this.keyNum = keyNum;
            Init();
        }

        public void Init()
        {
            isScaner = false;
             keyboardHook.KeyDownEvent += new System.Windows.Forms.KeyEventHandler(hook_KeyDown);//钩住键按下
            keyboardHook.Start();
            
        }
        private void hook_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            string temp = string.Empty;
            DateTime nowTime = DateTime.Now;
            if ((e.KeyData >= Keys.D0 && e.KeyData <= Keys.D9) || (e.KeyData >= Keys.NumPad0 && e.KeyData <= Keys.NumPad9))//判断是不是数字的键盘值
                temp = (e.KeyData.ToString()).Last().ToString();
            else
                temp = e.KeyData.ToString();
            if ((nowTime - previewTime).Milliseconds < 20)//通过判断键盘输入的间隔来确定是扫描枪还是通过键盘输入的
            {
                if (e.KeyValue == (int)Keys.Return && inputKey.Length > keyNum)
                {
                    IsScaner = true;
                    returnKey = inputKey.ToString();
                    inputKey = new StringBuilder();
                    previewTime = DateTime.Now;
                    return;
                }
                inputKey.Append(temp);

            }
            else
            {
                inputKey = new StringBuilder(temp);
                IsScaner = false;
            }
            previewTime = DateTime.Now;
        }

        public void Close()
        {
            keyboardHook.Stop();
        }
        
    }
}
