using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Contracts.IRepositories;
using Repositories;

namespace Repositories.Repositories
	{
		public class UserLoginRepository : RepositoryBase<UserLogin>, IUserLoginRepository
		{
			public UserLoginRepository(LabelMaker_BP_DBContext repositoryContext)
				: base(repositoryContext)
			{
			}

		}
	}
