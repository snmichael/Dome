using System;

namespace Demo.Component.Entity
{
    [Serializable]
    public class CurrentUser
    {
        public string UserId { get; set; }
        public string EmployeeNo { get; set; }
        public string ChineseName { get; set; }
        public int RoleGrId { get; set; }
        public string PositionCode { get; set; }
        public string DepartmentCode { get; set; }
        public string RoleType { get; set; }


   

        //public IList<PERMISSION> FunList { get; set; }


        ///// <summary>
        ///// 验证当前用户是否权限
        ///// </summary>
        ///// <param name="funcID"></param>
        ///// <returns></returns>
        //public bool CheckPermission(int funcID)
        //{
        //    return FunList.Any(_ => _.FuncID == funcID);
        //}
    }
}
