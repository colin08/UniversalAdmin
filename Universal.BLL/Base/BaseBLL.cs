using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Universal.DataCore;
using EntityFramework.Extensions;
using EntityFramework.Caching;

namespace Universal.BLL
{
    /// <summary>
    /// MSSql 数据库 数据层 父类 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseBLL<T> where T : class, new()
    {
        /// <summary>
        /// 上下文
        /// </summary>
        DbContext db = new EFDBContext();
        private static readonly int CacheTime = 10;

        //增
        #region 1.0 新增实体，返回受影响的行数 +  int Add(T model)
        /// <summary>
        /// 1.0 新增实体，返回受影响的行数
        /// </summary>
        /// <param name="model"></param>
        /// <returns>返回受影响的行数</returns>
        public int Add(T model)
        {
            db.Set<T>().Add(model);
            return db.SaveChanges();
        }
        #endregion

        #region 1.1 新增实体，返回对应的实体对象 + T AddReturnModel(T model)
        /// <summary>
        /// 1.1 新增实体，返回对应的实体对象
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public T AddReturnModel(T model)
        {
            db.Set<T>().Add(model);
            db.SaveChanges();
            return model;
        }
        #endregion

        //删
        #region 2.0 根据id删除 +  int Del(T model)
        /// <summary>
        /// 2.0 根据id删除
        /// </summary>
        /// <param name="model">必须包含要删除id的对象</param>
        /// <returns></returns>
        public int Del(T model)
        {
            db.Set<T>().Attach(model);
            db.Set<T>().Remove(model);
            return db.SaveChanges();
        }
        #endregion

        #region 2.1 根据条件删除 + void DelBy(List<FilterSearch> delWhere)
        /// <summary>
        /// 2.1 根据条件删除
        /// </summary>
        /// <param name="delWhere"></param>
        /// <returns>返回受影响的行数</returns>
        public void DelBy(List<FilterSearch> delWhere)
        {
            db.Set<T>().WhereCustom(delWhere).Delete();
            db.SaveChanges();
        }
        #endregion

        #region 2.2 根据条件删除 + void DelBy(Expression<Func<T, bool>> delWhere)
        /// <summary>
        /// 2.2 根据条件删除
        /// </summary>
        /// <param name="delWhere"></param>
        /// <returns>返回受影响的行数</returns>
        public void DelBy(Expression<Func<T, bool>> delWhere)
        {
            db.Set<T>().Where(delWhere).Delete();
            db.SaveChanges();
        }
        #endregion

        //改
        #region 3.0 修改实体 +  int Modify(T model)
        /// <summary>
        /// 修改实体
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Modify(T model)
        {
            DbEntityEntry entry = db.Entry<T>(model);
            entry.State = EntityState.Modified;
            return db.SaveChanges();
        }
        #endregion

        #region 3.1 修改实体，可修改指定属性 + int Modify(T model, params string[] propertyNames)
        /// <summary>
        /// 3.1 修改实体，可修改指定属性
        /// </summary>
        /// <param name="model"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public int Modify(T model, params string[] propertyNames)
        {
            //3.1.1 将对象添加到EF中
            DbEntityEntry entry = db.Entry<T>(model);
            //3.1.2 先设置对象的包装状态为 Unchanged
            entry.State = EntityState.Unchanged;
            //3.1.3 循环被修改的属性名数组
            foreach (string propertyName in propertyNames)
            {
                //将每个被修改的属性的状态设置为已修改状态；这样在后面生成的修改语句时，就只为标识为已修改的属性更新
                entry.Property(propertyName).IsModified = true;
            }
            return db.SaveChanges();
        }
        #endregion

        #region 3.2 批量修改 + int ModifyBy(T model, List<FilterSearch> where, params string[] modifiedPropertyNames)
        /// <summary>
        /// 3.2 批量修改
        /// </summary>
        /// <param name="model"></param>
        /// <param name="where"></param>
        /// <param name="modifiedPropertyNames"></param>
        /// <returns></returns>
        public int ModifyBy(T model, List<FilterSearch> where, params string[] modifiedPropertyNames)
        {
            //3.2.1 查询要修改的数据
            List<T> listModifing = db.Set<T>().WhereCustom(where).ToList();
            //3.2.2 获取实体类类型对象
            Type t = typeof(T);
            //3.2.3 获取实体类所有的公共属性
            List<PropertyInfo> propertyInfos = t.GetProperties(BindingFlags.Instance | BindingFlags.Public).ToList();
            //3.2.4 创建实体属性字典集合
            Dictionary<string, PropertyInfo> dicPropertys = new Dictionary<string, PropertyInfo>();
            //3.2.5 将实体属性中要修改的属性名 添加到字典集合中  键：属性名  值：属性对象
            propertyInfos.ForEach(p =>
            {
                if (modifiedPropertyNames.Contains(p.Name))
                {
                    dicPropertys.Add(p.Name, p);
                }
            });
            //3.2.6 循环要修改的属性名
            foreach (string propertyName in modifiedPropertyNames)
            {
                //判断要修改的属性名是否在实体类的属性集合中存在
                if (dicPropertys.ContainsKey(propertyName))
                {
                    //如果存在，则取出要修改的属性对象
                    PropertyInfo proInfo = dicPropertys[propertyName];
                    //取出要修改的值
                    object newValue = proInfo.GetValue(model, null);
                    //批量设置要修改对象的属性
                    foreach (T item in listModifing)
                    {
                        //为要修改的对象的要修改的属性设置新的值
                        proInfo.SetValue(item, newValue, null);
                    }
                }
            }
            //一次性生成sql语句 到数据库执行
            return db.SaveChanges();
        }
        #endregion

        #region 3.3 批量修改 + int ModifyBy(T model, Expression<Func<T,bool>> whereLambda, params string[] modifiedPropertyNames)
        /// <summary>
        /// 3.3 批量修改
        /// </summary>
        /// <param name="model"></param>
        /// <param name="whereLambda"></param>
        /// <param name="modifiedPropertyNames"></param>
        /// <returns></returns>
        public int ModifyBy(T model, Expression<Func<T, bool>> whereLambda, params string[] modifiedPropertyNames)
        {
            //3.2.1 查询要修改的数据
            List<T> listModifing = db.Set<T>().Where(whereLambda).ToList();
            //3.2.2 获取实体类类型对象
            Type t = typeof(T);
            //3.2.3 获取实体类所有的公共属性
            List<PropertyInfo> propertyInfos = t.GetProperties(BindingFlags.Instance | BindingFlags.Public).ToList();
            //3.2.4 创建实体属性字典集合
            Dictionary<string, PropertyInfo> dicPropertys = new Dictionary<string, PropertyInfo>();
            //3.2.5 将实体属性中要修改的属性名 添加到字典集合中  键：属性名  值：属性对象
            propertyInfos.ForEach(p =>
            {
                if (modifiedPropertyNames.Contains(p.Name))
                {
                    dicPropertys.Add(p.Name, p);
                }
            });
            //3.2.6 循环要修改的属性名
            foreach (string propertyName in modifiedPropertyNames)
            {
                //判断要修改的属性名是否在实体类的属性集合中存在
                if (dicPropertys.ContainsKey(propertyName))
                {
                    //如果存在，则取出要修改的属性对象
                    PropertyInfo proInfo = dicPropertys[propertyName];
                    //取出要修改的值
                    object newValue = proInfo.GetValue(model, null);
                    //批量设置要修改对象的属性
                    foreach (T item in listModifing)
                    {
                        //为要修改的对象的要修改的属性设置新的值
                        proInfo.SetValue(item, newValue, null);
                    }
                }
            }
            //一次性生成sql语句 到数据库执行
            return db.SaveChanges();
        }
        #endregion


        //查，查单个model
        #region 4.0 根据条件查询单个model + T GetModel(List<FilterSearch> where,string orderby="")
        /// <summary>
        /// 4.0 根据条件查询单个model
        /// </summary>
        /// <param name="where">Where条件</param>
        /// <param name="orderby">排序</param>
        /// <returns></returns>
        public T GetModel(List<FilterSearch> where, string orderby = "")
        {
            if (string.IsNullOrWhiteSpace(orderby))
                return db.Set<T>().WhereCustom(where).AsNoTracking().FirstOrDefault();
            else
                return db.Set<T>().WhereCustom(where).OrderByCustom(orderby).AsNoTracking().FirstOrDefault();
        }
        #endregion

        #region 4.1 根据条件查询单个model + T GetModel(Expression<Func<T, bool>> whereLambda, string orderby = "")
        /// <summary>
        /// 4.1 根据条件查询单个model
        /// </summary>
        /// <param name="whereLambda">Where条件</param>
        /// <param name="orderby">排序</param>
        /// <returns></returns>
        public T GetModel(Expression<Func<T, bool>> whereLambda, string orderby = "")
        {
            if (string.IsNullOrWhiteSpace(orderby))
                return db.Set<T>().Where(whereLambda).AsNoTracking().FirstOrDefault();
            else
                return db.Set<T>().Where(whereLambda).OrderByCustom(orderby).AsNoTracking().FirstOrDefault();
        }
        #endregion

        #region 4.2 根据条件查询单个model，使用Incloud + T GetModel<TKey>(List<FilterSearch> where, Expression<Func<T, TKey>> incloudLambda, string orderby = "")
        /// <summary>
        /// 4.2 根据条件查询单个model，使用Incloud
        /// </summary>
        /// <param name="where">Where条件</param>
        /// <param name="incloudLambda">包含条件</param>
        /// <param name="orderby">排序</param>
        /// <returns></returns>
        public T GetModel<TKey>(List<FilterSearch> where, Expression<Func<T, TKey>> incloudLambda, string orderby = "")
        {
            if (string.IsNullOrWhiteSpace(orderby))
                return db.Set<T>().WhereCustom(where).Include(incloudLambda).AsNoTracking().FirstOrDefault();
            else
                return db.Set<T>().WhereCustom(where).Include(incloudLambda).OrderByCustom(orderby).AsNoTracking().FirstOrDefault();
        }
        #endregion

        #region 4.3 根据条件查询单个model，使用Incloud + T GetModel<TKey>(Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey>> incloudLambda, string ordery = "")
        /// <summary>
        /// 4.3 根据条件查询单个model，使用Incloud
        /// </summary>
        /// <param name="whereLambda">Where条件</param>
        /// <param name="incloudLambda">包含条件</param>
        /// <param name="orderby">排序</param>
        /// <returns></returns>
        public T GetModel<TKey>(Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey>> incloudLambda, string ordery = "")
        {
            if (string.IsNullOrWhiteSpace(ordery))
                return db.Set<T>().Where(whereLambda).Include(incloudLambda).AsNoTracking().FirstOrDefault();
            else
                return db.Set<T>().Where(whereLambda).Include(incloudLambda).OrderByCustom(ordery).AsNoTracking().FirstOrDefault();
        }
        #endregion

        #region 7.0 根据条件查询数量 + T GetCount(Expression<Func<T,bool>> whereLambda)
        /// <summary>
        /// 7.0 根据条件查询数量
        /// </summary>
        /// <param name="whereLambda">Where条件</param>
        /// <returns></returns>
        public int GetCount(Expression<Func<T, bool>> whereLambda)
        {
            return db.Set<T>().Count(whereLambda);
        }
        #endregion

        #region 7.1 根据条件查询数量 + T GetCount(List<FilterSearch> where)
        /// <summary>
        /// 7.1 根据条件查询数量
        /// </summary>
        /// <param name="where">Where条件</param>
        /// <returns></returns>
        public int GetCount(List<FilterSearch> where)
        {
            return db.Set<T>().CountCustom(where);
        }
        #endregion

        #region 8.0 判断是否存在 + T Exists(Expression<Func<T, bool>> whereLambda)
        /// <summary>
        /// 8.0 判断是否存在
        /// </summary>
        /// <param name="whereLambda">Where条件</param>
        /// <returns></returns>
        public bool Exists(Expression<Func<T, bool>> whereLambda)
        {
            return db.Set<T>().Any(whereLambda);
        }
        #endregion

        #region 8.1 判断是否存在 + T Exists(List<FilterSearch> where)
        /// <summary>
        /// 8.1 判断是否存在
        /// </summary>
        /// <param name="where">Where条件</param>
        /// <returns></returns>
        public bool Exists(List<FilterSearch> where)
        {
            return db.Set<T>().AnyCustom(where);
        }
        #endregion

        //查，查List
        #region  5.0 根据条件查询 + List<T> GetListBy(int top, List<FilterSearch> where,string orderby, bool isCache = false)
        /// <summary>
        /// 5.0 根据条件查询
        /// </summary>
        /// <param name="top">0为所有</param>
        /// <param name="where"></param>
        /// <param name="orderby">排序</param>
        /// <param name="isCache">是否缓存</param>
        /// <returns></returns>
        public List<T> GetListBy(int top, List<FilterSearch> where, string orderby, bool isCache = false)
        {

            if (string.IsNullOrWhiteSpace(orderby))
            {
                if (isCache)
                {
                    if (top <= 0)
                        return db.Set<T>().WhereCustom(where).FromCache(CachePolicy.WithDurationExpiration(TimeSpan.FromMinutes(CacheTime))).ToList();
                    else
                        return db.Set<T>().WhereCustom(where).Take(top).FromCache(CachePolicy.WithDurationExpiration(TimeSpan.FromMinutes(CacheTime))).ToList();
                }
                else
                {
                    if (top <= 0)
                        return db.Set<T>().WhereCustom(where).AsNoTracking().ToList();
                    else
                        return db.Set<T>().WhereCustom(where).Take(top).AsNoTracking().ToList();
                }
            }
            else
            {
                if (isCache)
                {
                    if (top <= 0)
                        return db.Set<T>().WhereCustom(where).OrderByCustom(orderby).FromCache(CachePolicy.WithDurationExpiration(TimeSpan.FromMinutes(CacheTime))).ToList();
                    else
                        return db.Set<T>().WhereCustom(where).OrderByCustom(orderby).Take(top).FromCache(CachePolicy.WithDurationExpiration(TimeSpan.FromMinutes(CacheTime))).ToList();
                }
                else
                {
                    if (top <= 0)
                        return db.Set<T>().WhereCustom(where).OrderByCustom(orderby).AsNoTracking().ToList();
                    else
                        return db.Set<T>().WhereCustom(where).Take(top).OrderByCustom(orderby).AsNoTracking().ToList();
                }
            }

        }
        #endregion

        #region  5.1 根据条件查询 + List<T> GetListBy(int top, Expression<Func<T, bool>> whereLambda,string orderby, bool isCache = false)
        /// <summary>
        /// 5.1 根据条件查询
        /// </summary>
        /// <param name="top">0为所有</param>
        /// <param name="whereLambda"></param>
        /// <param name="orderby">排序</param>
        /// <param name="isCache">是否缓存</param>
        /// <returns></returns>
        public List<T> GetListBy(int top, Expression<Func<T, bool>> whereLambda, string orderby, bool isCache = false)
        {
            if (string.IsNullOrWhiteSpace(orderby))
            {
                if (isCache)
                {
                    if (top <= 0)
                        return db.Set<T>().Where(whereLambda).FromCache(CachePolicy.WithDurationExpiration(TimeSpan.FromMinutes(CacheTime))).ToList();
                    else
                        return db.Set<T>().Where(whereLambda).Take(top).FromCache(CachePolicy.WithDurationExpiration(TimeSpan.FromMinutes(CacheTime))).ToList();
                }
                else
                {
                    if (top <= 0)
                        return db.Set<T>().Where(whereLambda).AsNoTracking().ToList();
                    else
                        return db.Set<T>().Where(whereLambda).Take(top).AsNoTracking().ToList();
                }
            }
            else
            {
                if (isCache)
                {
                    if (top <= 0)
                        return db.Set<T>().Where(whereLambda).OrderByCustom(orderby).FromCache(CachePolicy.WithDurationExpiration(TimeSpan.FromMinutes(CacheTime))).ToList();
                    else
                        return db.Set<T>().Where(whereLambda).OrderByCustom(orderby).Take(top).FromCache(CachePolicy.WithDurationExpiration(TimeSpan.FromMinutes(CacheTime))).ToList();
                }
                else
                {
                    if (top <= 0)
                        return db.Set<T>().Where(whereLambda).OrderByCustom(orderby).AsNoTracking().ToList();
                    else
                        return db.Set<T>().Where(whereLambda).Take(top).OrderByCustom(orderby).AsNoTracking().ToList();
                }
            }
        }
        #endregion

        //查，带分页查询
        #region 6.0分页查询 不带InCloud +List<T> GetPagedList(int pageIndex, int pageSize, ref int rowCount, List<FilterSearch> where, string orderby)
        /// <summary>
        /// 分页查询 不带InCloud
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="rowCount"></param>
        /// <param name="where"></param>
        /// <param name="orderby"></param>
        /// <returns></returns>
        public List<T> GetPagedList(int pageIndex, int pageSize, ref int rowCount, List<FilterSearch> where, string orderby)
        {
            //使用拓展框架
            var q = db.Set<T>().WhereCustom(where);
            var q1 = q.FutureCount();
            var q3 = q.OrderByCustom(orderby).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsNoTracking().Future();
            rowCount = q1.Value;
            return q3.ToList();
        }


        #endregion

        #region 6.1分页查询 不带InCloud +List<T> GetPagedList(int pageIndex, int pageSize, ref int rowCount, Expression<Func<T, bool>> whereLambda, string orderby)
        /// <summary>
        /// 分页查询 不带InCloud
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="rowCount"></param>
        /// <param name="whereLambda"></param>
        /// <param name="orderby"></param>
        /// <returns></returns>
        public List<T> GetPagedList(int pageIndex, int pageSize, ref int rowCount, Expression<Func<T, bool>> whereLambda, string orderby)
        {
            //使用拓展框架
            var q = db.Set<T>().Where(whereLambda);
            var q1 = q.FutureCount();
            var q3 = q.OrderByCustom(orderby).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsNoTracking().Future();
            rowCount = q1.Value;
            return q3.ToList();

        }


        #endregion

        #region 6.2分页查询 带InCloud +List<T> GetPagedList<TKey>(int pageIndex, int pageSize, ref int rowCount, List<FilterSearch> where, string orderby, Expression<Func<T, TKey>> incloudLambda)
        /// <summary>
        /// 分页查询 带InCloud
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="rowCount"></param>
        /// <param name="where"></param>
        /// <param name="orderby"></param>
        /// <param name="incloudLambda"></param>
        /// <returns></returns>
        public List<T> GetPagedList<TKey>(int pageIndex, int pageSize, ref int rowCount, List<FilterSearch> where, string orderby, Expression<Func<T, TKey>> incloudLambda)
        {
            rowCount = db.Set<T>().WhereCustom(where).Count();

            if (incloudLambda != null)
                return db.Set<T>().OrderByCustom(orderby).Include(incloudLambda).WhereCustom(where).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsNoTracking().ToList();
            else
                return db.Set<T>().OrderByCustom(orderby).WhereCustom(where).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsNoTracking().ToList();

        }


        #endregion

        #region 6.3分页查询 带InCloud +List<T> GetPagedList<TKey>(int pageIndex, int pageSize, ref int rowCount, Expression<Func<T, bool>> whereLambda, string orderby, Expression<Func<T, TKey>> incloudLambda)
        /// <summary>
        /// 分页查询 带InCloud
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="rowCount"></param>
        /// <param name="whereLambda"></param>
        /// <param name="orderby"></param>
        /// <param name="incloudLambda"></param>
        /// <returns></returns>
        public List<T> GetPagedList<TKey>(int pageIndex, int pageSize, ref int rowCount, Expression<Func<T, bool>> whereLambda, string orderby, Expression<Func<T, TKey>> incloudLambda)
        {
            rowCount = db.Set<T>().Where(whereLambda).Count();

            if (incloudLambda != null)
                return db.Set<T>().OrderByCustom(orderby).Include(incloudLambda).Where(whereLambda).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsNoTracking().ToList();
            else
                return db.Set<T>().OrderByCustom(orderby).Where(whereLambda).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsNoTracking().ToList();
        }

        #endregion
    }
}
