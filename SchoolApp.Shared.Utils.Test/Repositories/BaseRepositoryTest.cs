using Microsoft.EntityFrameworkCore;
using Moq;
using SchoolApp.Shared.Utils.Sql.Base;
using SchoolApp.Shared.Utils.Sql.Contexts;
using SchoolApp.Shared.Utils.Sql.Interfaces;
using Xunit;

namespace SchoolApp.Shared.Utils.Test.Repositories;

public class BaseRepositoryTest<TDto, TDomain, TContext, TRepository> where TDto : class, IIdentityEntity
                                                                      where TDomain : class
                                                                      where TContext : SchoolAppContext
                                                                      where TRepository : BaseCrudRepository<TDto, TDomain, TContext>
{
    protected readonly Mock<DbSet<TDto>> _mockSet;
    protected readonly Mock<TContext> _mockContext;

    public BaseRepositoryTest()
    {
        _mockSet = new Mock<DbSet<TDto>>();
        _mockContext = new Mock<TContext>(new DbContextOptions<TContext>());
        _mockContext.Setup(x => x.Set<TDto>()).Returns(_mockSet.Object);
        _mockContext.Setup(x => x.DetachedItem(It.IsAny<object>()));
    }

    public async Task InternalInsertTestAsync(TRepository repository, TDomain newItem)
    {
        // Act
        var result = await repository.InsertAsync(newItem);

        // Assert
        _mockSet.Verify(x => x.AddAsync(It.IsAny<TDto>(), default), Times.Once);
        _mockContext.Verify(x => x.SaveChangesAsync(default), Times.Once);

        foreach (var property in typeof(TDomain).GetProperties().Where(x => x.Name != "Id"))
        {
            Assert.Equal(property.GetValue(newItem), property.GetValue(result));
        }
    }

    public async Task InternalUpdateTestAsync(TRepository repository, TDomain newItem)
    {
        // Act 
        var result = await repository.UpdateAsync(newItem);

        // Assert
        _mockSet.Verify(x => x.Update(It.IsAny<TDto>()), Times.Once);
        _mockContext.Verify(x => x.SaveChangesAsync(default), Times.Once);
        _mockContext.Verify(x => x.DetachedItem(It.IsAny<object>()), Times.Once);

        foreach (var property in typeof(TDomain).GetProperties())
        {
            Assert.Equal(property.GetValue(newItem), property.GetValue(result));
        }
    }

    public virtual async Task InternalDeleteTestAsync(TRepository repository, IQueryable<TDto> data)
    {
        _mockContext.Setup(x => x.GetQueryable(_mockSet.Object)).Returns(data);

        // Act 
        await repository.DeleteAsync(1);

        // Assert
        _mockSet.Verify(x => x.Remove(It.IsAny<TDto>()), Times.Once);
        _mockContext.Verify(x => x.SaveChangesAsync(default), Times.Once);
        _mockContext.Verify(x => x.DetachedItem(It.IsAny<object>()), Times.Once);
        _mockContext.Verify(x => x.GetQueryable(It.IsAny<DbSet<TDto>>()), Times.Once);
    }

    public virtual async Task InternalDeleteTest_TryToDeleteANonExistentItemAsync(TRepository repository)
    {
        // Arrange
        var data = new List<TDto>().AsQueryable();
        _mockContext.Setup(x => x.GetQueryable(_mockSet.Object)).Returns(data);

        // Act 
        await repository.DeleteAsync(1);

        // Assert
        _mockSet.Verify(x => x.Remove(It.IsAny<TDto>()), Times.Never);
        _mockContext.Verify(x => x.SaveChangesAsync(default), Times.Never);
        _mockContext.Verify(x => x.DetachedItem(It.IsAny<object>()), Times.Never);
        _mockContext.Verify(x => x.GetQueryable(It.IsAny<DbSet<TDto>>()), Times.Once);
    }

    public void InternalGetOneByIdTest(TRepository repository, IQueryable<TDto> data, int id)
    {
        // Arrange
        _mockContext.Setup(x => x.GetQueryable(_mockSet.Object)).Returns(data);

        // Act 
        var result = repository.GetOneById(id);

        // Assert
        _mockContext.Verify(x => x.GetQueryable(It.IsAny<DbSet<TDto>>()), Times.Once);
        Assert.NotNull(result);
    }

    public void InternalGetOneByIdTest_NotFound(TRepository repository, int id)
    {
        // Arrange
        var data = new List<TDto>().AsQueryable();
        _mockContext.Setup(x => x.GetQueryable(_mockSet.Object)).Returns(data);

        // Act 
        var result = repository.GetOneById(id);

        // Assert
        _mockContext.Verify(x => x.GetQueryable(It.IsAny<DbSet<TDto>>()), Times.Once);
        Assert.Null(result);
    }
}
