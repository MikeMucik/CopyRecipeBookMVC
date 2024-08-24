using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CopyRecipeBookMVC.Domain.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CopyRecipeBookMVC.Infrastructure
{
	public class Context : IdentityDbContext
	{
		public DbSet<Category> Categories { get; set; }
		public DbSet<Difficulty> Difficulties { get; set; }
		public DbSet<Ingredient> Ingredients { get; set; }
		public DbSet<Recipe> Recipes { get; set; }
		public DbSet<RecipeIngredient> RecipeIngredient { get; set; }
		public DbSet<Time> Times { get; set; }		
		public DbSet<Unit> Units { get; set; }
        public Context(DbContextOptions options) : base(options)
        {            
        }
		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			builder.Entity<RecipeIngredient>()
				.HasKey(it => new { it.RecipeId, it.IngredientId, it.UnitId });

			builder.Entity<RecipeIngredient>()
				.Property(ri => ri.Quantity)
				.HasColumnType("decimal(4,1)");

			builder.Entity<RecipeIngredient>()
				.HasOne<Recipe>(r => r.Recipe)
				.WithMany(ri => ri.RecipeIngredient)
				.HasForeignKey(r  => r.RecipeId);

			builder.Entity<RecipeIngredient>()
				.HasOne<Ingredient>(i => i.Ingredient)
				.WithMany(ri => ri.RecipeIngredient)
				.HasForeignKey(i => i.IngredientId);

			builder.Entity<RecipeIngredient>()
				.HasOne<Unit>(i => i.Unit)
				.WithMany(ri => ri.RecipeIngredients)
				.HasForeignKey(i => i.UnitId);
		}
	}
}
