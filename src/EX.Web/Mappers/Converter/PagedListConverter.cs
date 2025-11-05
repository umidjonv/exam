using AutoMapper;
using EX.Common.Core;
using X.PagedList;

namespace EX.Web.Mappers.Converter
{
    public class PagedListConverter<TEntity, TModel> : ITypeConverter<IPagedList<TEntity>, IPagedList<TModel>>
        where TEntity : BaseEntity
        where TModel : class
    {
        public IPagedList<TModel> Convert(IPagedList<TEntity> source, IPagedList<TModel> destination, ResolutionContext context)
        {
            var models = source.Select(m => context.Mapper.Map<TEntity, TModel>(m));

            return new PagedList<TModel>(models, source.PageNumber, source.PageSize);
        }
    }
}
