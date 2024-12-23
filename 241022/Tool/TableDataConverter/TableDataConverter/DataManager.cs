using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableDataConverter
{
    public class DataManager
    {
        //델리게이트
        public delegate void SendDataEventHandler(string data);

        //이벤트 생성
        public event SendDataEventHandler putConsole;
        public void Init()
        {
        }
    }
}
