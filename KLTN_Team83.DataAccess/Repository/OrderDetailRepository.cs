﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KLTN_Team83.DataAccess.Data;
using KLTN_Team83.DataAccess.Repository.IRepository;
using KLTN_Team83.Models;

namespace KLTN_Team83.DataAccess.Repository
{
    public class OrderDetailRepository : Repository<OrderDetail>, IOrderDetailRepository
    {
        private ApplicationDbContext _db;
        public OrderDetailRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(OrderDetail obj)
        {
            _db.OrderDetails.Update(obj);
        }
    }
    
}
