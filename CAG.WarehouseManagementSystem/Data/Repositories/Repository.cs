using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;


public class Repository<T> : IRepository<T> where T : class
{
	protected readonly WareHouseDbContext _context;
	protected readonly DbSet<T> _dbSet;

	public Repository(WareHouseDbContext context)
	{
		_context = context;
		_dbSet = _context.Set<T>();
	}

	public async Task<IEnumerable<T>> GetAllAsync()
	{
		return await _dbSet.ToListAsync();
	}

	public async Task<T?> GetByIdAsync(int id)
	{
		return await _dbSet.FindAsync(id);
	}

	public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
	{
		return await _dbSet.Where(predicate).ToListAsync();
	}

	public async Task<T> AddAsync(T entity)
	{
		_dbSet.Add(entity);
		await _context.SaveChangesAsync();
		return entity;
	}

	public async Task<bool> UpdateAsync(T entity)
	{
		if (!_dbSet.Local.Any(e => e == entity))
		{
			_dbSet.Attach(entity);
		}

		_context.Entry(entity).State = EntityState.Modified;

		try
		{
			await _context.SaveChangesAsync();
			return true;
		}
		catch (DbUpdateConcurrencyException)
		{
			return false;
		}
	}

	public async Task<bool> DeleteAsync(int id)
	{
		var entity = await _dbSet.FindAsync(id);
		if (entity == null)
			return false;

		_dbSet.Remove(entity);
		await _context.SaveChangesAsync();
		return true;
	}
}
