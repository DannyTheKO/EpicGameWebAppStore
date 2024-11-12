using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Repository
{
	public interface IAccountGameRepository
	{
		Task Add(AccountGame accountGame);

		Task<IEnumerable<AccountGame>> GetAll();

		Task Update(AccountGame accountGame);

		Task Delete(AccountGame accountGame);
	}
}
