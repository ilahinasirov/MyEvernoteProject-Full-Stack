using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace MyEvernote.Core.DataAccess
{
    // Bu Klası on görə yaradırıq ki, gələcəkdə Sql dən MySql vəya Oracle- a keçdikdə çətinlik olmasın.
    public interface IDataAccess<T>
    {
         List<T> List();

         IQueryable<T> QueryableList();

        // Bütöv listi deyildə, sadəcə istədiyimiz və seçdiklərimizi lambda və linq sorğuları ilə
        // listelemek üçün aşağıdakı List Expression metodunu yazırıq.

         List<T> List(Expression<Func<T, bool>> where);


         int Insert(T obj);



         int Update(T obj);


         int Delete(T obj);

         int Save();         //neche eded save edildiyini gostermek uchun int veririk.

         T Find(Expression<Func<T, bool>> where); // Tek 1 element donduyu uchun T yaziriq List<T> yazmiriq.

    }
}
