using System;
using System.Collections.Generic;
using System.Text;

namespace TailorTools.Props.Models
{
    public class Property
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public int Length { get; set; }
        public int Precision { get; set; }
        public bool Nullable { get; set; }
    }
}
