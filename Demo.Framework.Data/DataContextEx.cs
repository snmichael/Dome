//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using JinRi.Common;
//using System.Linq.Expressions;
//using System.Data.Entity;
//using System.Data.Objects;
//using System.Reflection;
//using System.Runtime.CompilerServices;
//using System.Data.Entity.Infrastructure;
//using System.Text.RegularExpressions;
//using System.Data.Common;

//namespace JinRi.EFRepositories
//{
//    public static class DataContextEx
//    {
//        public static int Delete(this DbContext dc, IQueryable query)
//        {
//            //dc.Database.ExecuteSqlCommand

//            var sql = query.ToString().Replace(Environment.NewLine," ");
//            var reg = new Regex(@"^SELECT[\s]*(?<Fields>.*)[\s]*FROM[\s]*(?<Table>.*)[\s]*AS[\s]*(?<TableAlias>.*)[\s]*WHERE[\s]*(?<Condition>.*)",
//                                    RegexOptions.IgnoreCase | RegexOptions.Multiline);
 
//            Match match = reg.Match(sql);

//            if (!match.Success)
//                throw new ArgumentException("Cannot delete this type of collection");

//            string table = match.Groups["Table"].Value.Trim();
//            string tableAlias = match.Groups["TableAlias"].Value.Trim();
//            string condition = match.Groups["Condition"].Value.Trim().Replace(tableAlias, table);

//            var delCommand = string.Format("DELETE FROM {0} WHERE {1}", table, condition);
//            return dc.Database.ExecuteSqlCommand(delCommand);
            
//        }


//        //public static int DeleteAll<TEntity>(this DbSet<TEntity> table, Expression<Func<TEntity, bool>> predicate)
//        //    where TEntity : class
//        //{
//        //    IQueryable query = table.Where(predicate);

//        //    return table.con.DeleteAll(query);
//        //}
//    }


//}
