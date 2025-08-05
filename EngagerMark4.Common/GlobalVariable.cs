using EngagerMark4.Common.Configs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace EngagerMark4.Common
{
    public static class GlobalVariable
    {
        public static Dictionary<string, Int64> DOMAIN_KEYS = null;

        public static Dictionary<string, string> DOMAIN_PREFIX = null;

        public static Dictionary<string, string> USER_ROLES = null;

        public static Dictionary<string, string> USER_NAMES = new Dictionary<string, string>();

        public static string mobile_userId = String.Empty;
        public static string mobile_userName = String.Empty;

        public static void AddUserRole(string userName, string role)
        {
            if (USER_ROLES == null) USER_ROLES = new Dictionary<string, string>();

            USER_ROLES[userName] = role;
        }

        private static Int64 _companyId = 0;

        public static Int64 COMPANY_ID
        {
            get
            {
                if (HttpContext.Current == null) return _companyId;

                string l_domain = GetDomain(HttpContext.Current.User == null ? "" : HttpContext.Current.User.Identity.Name);

                if (DOMAIN_KEYS == null) DOMAIN_KEYS = new Dictionary<string, Int64>();

                if (DOMAIN_KEYS.ContainsKey(l_domain))
                {
                    return DOMAIN_KEYS[l_domain];
                }
                return _companyId;
            }
            set
            {
                _companyId = value;
            }
        }

        private static string _company_prefix = "";
        public static string COMAPNY_PREFIX
        {
            get
            {
                if (HttpContext.Current == null) return _company_prefix;

                string l_domain = GetDomain(HttpContext.Current.User == null ? "" : HttpContext.Current.User.Identity.Name);

                if (DOMAIN_PREFIX == null) DOMAIN_PREFIX = new Dictionary<string, string>();

                if (DOMAIN_PREFIX.ContainsKey(l_domain))
                    return DOMAIN_PREFIX[l_domain];
                return _company_prefix;
            }
            set
            {
                _company_prefix = value;
            }
        }

        public static string GetDomainWithout()
        {
            if (HttpContext.Current == null) return "";
            string domain = "";
            if (HttpContext.Current.User == null)
            {
                domain = ConfigurationManager.AppSettings[AppSettingKey.DOMAIN];
            }
            else
            {
                domain = GetDomain(HttpContext.Current.User.Identity.Name);
            }

            if (domain.Contains('@'))
            {
                domain = domain.Replace("@", "");
            }
            return domain;
        }

        public static String DOMAIN = "";

        public static void AddCompanyId(Int64 companyId, string userName)
        {
            string l_domain = GetDomain(userName);
            if (DOMAIN_KEYS == null) DOMAIN_KEYS = new Dictionary<string, Int64>();
            if (!DOMAIN_KEYS.ContainsKey(l_domain))
            {
                DOMAIN_KEYS[l_domain] = companyId;
            }
        }

        public static void AddPrefix(string prefix, string userName)
        {
            string l_domain = GetDomain(userName);
            if (DOMAIN_PREFIX == null) DOMAIN_PREFIX = new Dictionary<string, string>();
            if (!DOMAIN_PREFIX.ContainsKey(l_domain))
            {
                DOMAIN_PREFIX[l_domain] = prefix;
            }
        }

        public static string GetDomain(string aUserName)
        {
            try
            {
                //return "@" + aUserName.Split('@')[1];
                return aUserName.Split('@')[1].ToLower();
            }
            catch
            {
                return String.Empty;
            }
        }
    }
}
