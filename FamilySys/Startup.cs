﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FamilySys.Models;
using FamilySys.Models.DbModels;
using FamilySys.Modules;
using FamilySys.Services;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sakura.AspNetCore.Mvc;

namespace FamilySys {
	public class Startup {
		public Startup(IConfiguration configuration) {
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services) {
			services.Configure<CookiePolicyOptions>(options => {
				// This lambda determines whether user consent for non-essential cookies is needed for a given request.
				options.CheckConsentNeeded = context => false;
				options.MinimumSameSitePolicy = SameSiteMode.None;
			});

			//数据库上下文注入
			services.AddDbContext<FamilySysDbContext>(options =>
				options.UseSqlServer(Configuration.GetConnectionString("FamilySysCon")));

			//个人模块注入
			services.AddScoped<Encryption>();
			services.AddScoped<Barker>();

			//Session注入
			services.AddDistributedMemoryCache();

			services.AddSession(options => {
					options.IdleTimeout = TimeSpan.FromMinutes(10);
					options.Cookie.HttpOnly = true;
					options.Cookie.IsEssential = true;
				}
			);

			//分页包注入
			services.AddBootstrapPagerGenerator(options => {
				options.ConfigureDefault();
			});

			//后台定时任务注入
			services.AddHostedService<TimedTask>();
			services.AddScoped<GenerateMonthlyRank.IMonthlyRank, GenerateMonthlyRank.MonthlyRank>();

			//services.AddMvc(option => option.EnableEndpointRouting = false).SetCompatibilityVersion(version: CompatibilityVersion.Version_3_0);
			services.AddMvc(option => option.EnableEndpointRouting = false);
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostEnvironment env) {
			if (env.IsDevelopment()) {
				app.UseDeveloperExceptionPage();
			} else {
				app.UseExceptionHandler("/Home/Error");
				app.UseHsts();
			}

			// app.UseHttpsRedirection();
			app.UseStaticFiles();
			app.UseCookiePolicy();
			app.UseSession(); //启用Session

			app.UseMvc(routes => {
				routes.MapRoute(
					name: "default",
					template: "{controller=Home}/{action=Index}/{id?}");
			});
		}
	}
}
