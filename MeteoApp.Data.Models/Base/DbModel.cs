using System;
using System.Collections.Generic;
using System.Text;

namespace MeteoApp.Data.Models.Base
{
    public class DbModel
    {
        public int Id { get; set; }
        public DateTime AddedOn { get; set; }
        public DateTime ChangedOn { get; set; }
    }
}
