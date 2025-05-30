﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLTN_Team83.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IBlogRepository Blog { get; }
        ITypeBlogRepository TypeBlog { get; }
        IApplicationUserRepository ApplicationUser { get; }
        IProductRepository Product { get; }
        IProductImageRepository ProductImage { get; }
        ICategoryRepository Category { get; }
        IShoppingCartRepository ShoppingCart { get; }
        ICompanyRepository Company { get; }
        IGoalRepository Goal { get; }
        IHabitRepository Habit { get; }
        IHabitLogRepository HabitLog { get; }
        IOrderDetailRepository OrderDetail { get; }
        IOrderHeaderRepository OrderHeader { get; }


        void Save();
    }
}
