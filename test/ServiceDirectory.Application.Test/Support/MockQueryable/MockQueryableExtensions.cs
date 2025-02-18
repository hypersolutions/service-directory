namespace ServiceDirectory.Application.Test.Support.MockQueryable;

public static class MockQueryableExtensions
{
	public static IQueryable<TEntity> AsTestQueryable<TEntity>(this IEnumerable<TEntity> data) where TEntity : class
	{
		return new TestAsyncEnumerable<TEntity>(data);
	}
}