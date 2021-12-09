﻿using Core.Entities;
using Core.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Concrete
{
    public class UserToUser:IEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int FollowerId { get; set; }
        public DateTime Date { get; set; }

    }
}
