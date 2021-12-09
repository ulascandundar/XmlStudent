using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DTOs
{
    public class RefreshDto:IDto
    {
        public string Mail { get; set; }
        public string Fav { get; set; }
    }
}
