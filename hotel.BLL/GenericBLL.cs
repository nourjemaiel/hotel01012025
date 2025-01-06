using hotel.BLL.Contract;
using hotel.DAL.Contract;
using System.Linq.Expressions;

namespace hotel.BLL
{
    public class GenericBLL<T>:IGenericBLL<T> where T : class
    {
        IUnitOfWork _unitOfWork;
        IRepository<T> _repo;

        public GenericBLL(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _repo = (IRepository<T>)_unitOfWork.GetRepository<T>();
        }

        public void Add(T entity)
        {
            _repo.Add(entity);
        }


        public void Delete(T entity)
        {
            _repo.Delete(entity);
        }


        public T GetById(params object[] id)
        {
            return _repo.GetById(id);
        }

        public IEnumerable<T> GetMany(Expression<Func<T, bool>>? predicate = null, params Expression<Func<T, object>>[] includeProperties)
        {
            return _repo.GetMany(predicate, includeProperties);
        }

        public void Submit()
        {
            _repo.Submit();
        }

        public void Update(T entity)
        {
            _repo.Update(entity);
        }
    }
}