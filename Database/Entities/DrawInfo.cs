using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikalaBot.Database.Entities
{
    public class DrawInfo
    {
        [Key]
        public long QQ { get; set; }
        public int Crystal { get; set; }
        public int Draw { get; set; }
        public int DrawTen { get; set; }
    }
}
