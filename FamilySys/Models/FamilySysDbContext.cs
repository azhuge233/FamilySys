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
		public DbSet<Housework> Houseworks { get; set; }
		public DbSet<Announcement> Announcements { get; set; }
		public DbSet<Dream> Dreams { get; set; }
		public DbSet<UserDreamVote> UserDreamVotes { get; set; }
		public DbSet<Rate> Rates { get; set; }
		public DbSet<ScoreRecord> ScoreRecords { get; set; }
		public DbSet<MonthlyRank> MonthlyRanks { get; set; }
		public DbSet<Bark> Barks { get; set; }
	}
}
