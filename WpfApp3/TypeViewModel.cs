using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_3_WPF
{
    public class TypeViewModel
    {
        public Type Type { get; }
        public string DisplayName { get; }

        public TypeViewModel(Type type, string displayName)
        {
            Type = type;
            DisplayName = displayName;
        }
    }


}
