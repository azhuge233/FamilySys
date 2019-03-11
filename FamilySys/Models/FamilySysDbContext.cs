﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FamilySys.Models.DbModels;
using Microsoft.EntityFrameworkCore;

namespace FamilySys.Models {
	public class FamilySysDbContext: DbContext {
		public FamilySysDbContext(DbContextOptions<FamilySysDbContext> options) : base(options)
		{
		}

		public DbSet<User> Users { get; set; }
	}
}