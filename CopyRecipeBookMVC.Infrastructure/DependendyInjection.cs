using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CopyRecipeBookMVC.Domain.Interfaces;
using CopyRecipeBookMVC.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace CopyRecipeBookMVC.Infrastructure
{
	public static class DependendyInjection
	{
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddTransient<IRecipeRepository, RecipeRepository>();
            services.AddTransient<IIngredientRepository, IngredientRepository>();
            services.AddTransient<ITimeRepositoy, TimeRepository>();
            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddTransient<IDifficultyRepository, DifficultyRepository>();
            return services;
        }
    }
}
