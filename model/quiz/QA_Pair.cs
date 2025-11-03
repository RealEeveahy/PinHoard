using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinHoard.model.quiz
{
    internal class QA_Pair
    {
        public string q;
        public string a;
        public QA_Pair(string q, string a)
        {
            this.q = q;
            this.a = a;
        }
    }
}
