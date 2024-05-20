using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Task_3_WPF
{
    public class MethodViewModel
    {
        public MethodInfo MethodInfo { get; }

        public string Name => MethodInfo.Name;

        public MethodViewModel(MethodInfo methodInfo)
        {
            MethodInfo = methodInfo;
        }
    }

}
