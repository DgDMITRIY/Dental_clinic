using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1.Classes
{
    class Patient : AbstractPerson
    {
        public override string Name { get; set; }
        
        public string Male { get; set; }
        public string Birth_date { get; set; }
        public string Receipt { get; set; }
        public string Disease { get; set; }
        
    }
}
