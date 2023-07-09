using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using MyEvernote.Common;
using MyEvernote.Core.DataAccess;
using MyEvernote.Entities;

namespace MyEvernote.DataAccessLayer.EntityFramework
{
    public class Repository<T> :RepositoryBase, IDataAccess<T> where T : class //T-nin klas tipdə olduğunu mütləq qeyd etməliyik.
    {


        // Hər dəfə metodlarda Set<T> metodunu tapıb çağırmamaq üçün aşağədakı kimi ctor ilə
        // 1 dəfə tanımlayırıq və istənilən zaman istifadə edirik.
        private  DbSet<T> _objectSet;
        public Repository()
        {
            _objectSet = context.Set<T>();
        }


        public List<T> List()
        {
            return _objectSet.ToList();
            
        } 
        public IQueryable<T> QueryableList()
        {
            return _objectSet.AsQueryable<T>();
            
        }

        // Bütöv listi deyildə, sadəcə istədiyimiz və seçdiklərimizi lambda və linq sorğuları ilə
        // listelemek üçün aşağıdakı List Expression metodunu yazırıq.

        public List<T> List(Expression<Func<T, bool>> where)
        {
            return _objectSet.Where(where).ToList();
        }

        public int Insert(T obj)
        {
            _objectSet.Add(obj);
            if (obj is MyEntityBase)
            {
                MyEntityBase o = obj as MyEntityBase;
                DateTime now = DateTime.Now;
                o.CreatedOn = now;
                o.ModifiedOn= now ;
                o.ModifiedUserName =App.Common.GetCurrentUsername(); 

            }
            return Save();
        }


        public int Update(T obj)
        {
            if (obj is MyEntityBase)
            {
                MyEntityBase o = obj as MyEntityBase;
                o.ModifiedOn = DateTime.Now;
                o.ModifiedUserName = App.Common.GetCurrentUsername() ;

            }
             
            return Save();
        }

        public int Delete(T obj)
        {
            _objectSet.Remove(obj);
            return Save();
        }


        

        public int Save()           //neche eded save edildiyini gostermek uchun int veririk.
        {
            return context.SaveChanges();
        }
        public T Find(Expression<Func<T, bool>> where) // Tek 1 element donduyu uchun T yaziriq List<T> yazmiriq.
        {
            return _objectSet.FirstOrDefault(where);
        }
    }
}
