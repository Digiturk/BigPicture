using BigPicture.Core.Repository;
using BigPicture.Core.Resolver;
using BigPicture.Resolver.Redis.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigPicture.Resolver.CSharp.CodeToDatabaseResolvers
{
    public class ExecuteNonQueryRedisResolver : IResolver<DbCallBlock>
    {
        private readonly List<string> _keywords = new List<string>
        {
        "UserLogin", "UserCredential", "UserCredentialExt", "UserCredentialExtHash",
        "UserCredentialNotVerified", "UserCredentialHash", "UserCredentialBehavior",
        "UserCredentialBehaviorHash", "UserDeny", "UserDenyHash", "UserPassword",
        "UserPasswordHash", "UserSession", "UserSessionExt", "UserSessionExtHash",
        "UserInfo", "ResetKey", "ResetKeyByCredential", "VerificationKeyByCredential",
        "AuthCode", "OAuthApplication", "OAuthWhiteList", "OAuthWhiteListHash",
        "OAuthAccessCode", "SessionKeyByAuthCode", "UserFingerPrintInfo", "UserPincode",
        "UserDeviceHash", "UserProfileHash"
        };
        private IRepository _Repository { get; set; }

        public ExecuteNonQueryRedisResolver(IRepository repository)
        {
            this._Repository = repository;
        }
        public void Resolve(DbCallBlock obj)
        {
            var redisTableName = this.GetRedisTableName(obj);
            if (String.IsNullOrEmpty(redisTableName))
            {
                return;
            }
            var redisTable = this.GetRedisTable(redisTableName);
            if (redisTable == null)
            {
                return;
            }

            this._Repository.ControlAndCreateRelationship(obj.Code.Id, redisTable.Id, "ACCESS");
        }

        private string GetRedisTableName(DbCallBlock obj)
        {
            List<string> paramValues = obj.Calls.ParamValues;
            List<string> paramCodes = obj.Calls.ParamCodes;

            if (paramValues.Count > 0 && paramCodes.Count > 0 &&
                (paramValues[0].StartsWith("String.Format", StringComparison.OrdinalIgnoreCase) ||
                 paramCodes[0].StartsWith("String.Format", StringComparison.OrdinalIgnoreCase) ||
                 paramValues[0].StartsWith("string.Format", StringComparison.OrdinalIgnoreCase) ||
                 paramCodes[0].StartsWith("string.Format", StringComparison.OrdinalIgnoreCase)))
            {

                foreach (string keyword in _keywords)
                {
                    if (paramValues[0].IndexOf($"{keyword}Format", StringComparison.OrdinalIgnoreCase) >= 0 ||
                        paramCodes[0].IndexOf($"{keyword}Format", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        return GetRedisTableSuffix(keyword);
                    }
                }
            }

            return string.Empty;
        }

        private string GetRedisTableSuffix(string keyword)
        {
            switch (keyword)
            {
                case "UserLogin": return "ULogin";
                case "UserCredential": return "UCred";
                case "UserCredentialExt": return "UCredExt";
                case "UserCredentialExtHash": return "CredExtKey";
                case "UserCredentialNotVerified": return "UCredNotVrfy";
                case "UserCredentialHash": return "PartyRoleId";
                case "UserCredentialBehavior": return "UCredBhvr";
                case "UserCredentialBehaviorHash": return "ApplicationId";
                case "UserDeny": return "UDeny";
                case "UserDenyHash": return "UserDenyKey";
                case "UserPassword": return "UPass";
                case "UserPasswordHash": return "UserPasswordKey";
                case "UserSession": return "USes";
                case "UserSessionExt": return "USesExt";
                case "UserSessionExtHash": return "ExtKey";
                case "UserInfo": return "UInfo";
                case "ResetKey": return "ResetKey";
                case "ResetKeyByCredential": return "ResetKeyByCredential";
                case "VerificationKeyByCredential": return "VerificationKeyByCredential";
                case "AuthCode": return "AuthCode";
                case "OAuthApplication": return "OAuthApp";
                case "OAuthWhiteList": return "OAuthWhiteList";
                case "OAuthWhiteListHash": return "OAuthWhiteListKey";
                case "OAuthAccessCode": return "OAuthAccessCode";
                case "SessionKeyByAuthCode": return "SessionKeyByAuthCode";
                case "UserFingerPrintInfo": return "UserFingerPrintInfo";
                case "UserPincode": return "UPin";
                case "UserDeviceHash": return "UDevice";
                case "UserProfileHash": return "UProfile";
                default: return string.Empty;
            }
        }
        private RedisTable GetRedisTable(String redisTableName)
        {
            RedisTable obj = null;


                var filter = new
                {
                    Name = redisTableName
                };

                obj = this._Repository.FindNode<RedisTable>(filter, "RedisTable");

            return obj;
        }
    }
}
