using EconomicTrendsPolLESSIRE.Contracts.Enums;
using Dapper;
using System.Data;

namespace EconomicTrendsPolLESSIRE.Infrastructure
{
    public class RoleTypeHandler : SqlMapper.TypeHandler<UserRole>
    {
        public override void SetValue(IDbDataParameter parameter, UserRole value)
        {
            parameter.Value = (int)value; // stored in int
        }

        public override UserRole Parse(object value)
        {
            if (value == null || value == DBNull.Value)
                return UserRole.User; // fallback

            return Enum.IsDefined(typeof(UserRole), (int)value) ? (UserRole)(int)value : UserRole.User; // fallback
        }
    }
}









































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.