using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CopyRecipeBookMVC.Application.Mapping;
using CopyRecipeBookMVC.Infrastructure;

namespace CopyRecipeBookMVC.Application.Test.Common
{
	public class QueryTestFixtures: IDisposable
	{
		public Context Context { get; private set; }
		public IMapper Mapper { get; private set; }
        public QueryTestFixtures()
        {
            Context = DbContextFactory.Create().Object;
            var configurationProvider =
               new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
			Mapper = configurationProvider.CreateMapper();
		}
        public void Dispose()
        {
            DbContextFactory.Destroy(Context);
        }
        [CollectionDefinition("QueryCollection")]
        public class QueryCollection : ICollectionFixture<QueryTestFixtures>
        { 
        }

    }
}
