using Microsoft.EntityFrameworkCore;
using Moq;
using SchoolApp.Shared.Utils.Sql.Base;
using SchoolApp.Shared.Utils.Sql.Contexts;
using SchoolApp.Shared.Utils.Sql.Interfaces;

namespace SchoolApp.Shared.Utils.Test.Repositories;

public class BaseMainEntityRepositoryTest<TDto, TDomain, TContext, TRepository> : BaseRepositoryTest<TDto, TDomain, TContext, TRepository> where TDto : class, IIdentityEntity, IAccountEntity, ISoftDeleteEntity
                                                                                                                                           where TDomain : class
                                                                                                                                           where TContext : SchoolAppContext
                                                                                                                                           where TRepository : BaseCrudRepository<TDto, TDomain, TContext>
{

    public override async Task InternalDeleteTestAsync(TRepository repository, IQueryable<TDto> data)
    {
        _mockContext.Setup(x => x.GetQueryable(_mockSet.Object)).Returns(data);

        // Act 
        await repository.DeleteAsync(1);

        // Assert
        _mockSet.Verify(x => x.Attach(It.IsAny<TDto>()), Times.Once);
        _mockContext.Verify(x => x.SaveChangesAsync(default), Times.Once);
        _mockContext.Verify(x => x.SetModifiedProperty(It.IsAny<object>(), "Deleted"), Times.Once);
        _mockContext.Verify(x => x.DetachedItem(It.IsAny<object>()), Times.Once);
        _mockContext.Verify(x => x.GetQueryable(It.IsAny<DbSet<TDto>>()), Times.Once);
    }

    public override async Task InternalDeleteTest_TryToDeleteANonExistentItemAsync(TRepository repository)
    {
        // Arrange
        var data = new List<TDto>().AsQueryable();
        _mockContext.Setup(x => x.GetQueryable(_mockSet.Object)).Returns(data);

        // Act 
        await repository.DeleteAsync(1);

        // Assert
        _mockSet.Verify(x => x.Attach(It.IsAny<TDto>()), Times.Never);
        _mockContext.Verify(x => x.SaveChangesAsync(default), Times.Never);
        _mockContext.Verify(x => x.SetModifiedProperty(It.IsAny<object>(), "Deleted"), Times.Never);
        _mockContext.Verify(x => x.DetachedItem(It.IsAny<object>()), Times.Never);
        _mockContext.Verify(x => x.GetQueryable(It.IsAny<DbSet<TDto>>()), Times.Once);
    }
}
