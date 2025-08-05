using EngagerMark4.ApplicationCore.Entities;
using EngagerMark4.ApplicationCore.Entities.Application;
using EngagerMark4.Common.Configs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace EngagerMark4.HtmlExtension
{
    public static class HtmlExtenstionMethods
    {
        public static HtmlString GetSorterScript(this HtmlHelper html)
        {
            return new HtmlString("$('.sorter').click(function (e) {" +
                            "e.preventDefault();" +
                            "$('#result').load($(this).attr('href'));" +
                            "});");
        }

        public static HtmlString GetPaginationScript(this HtmlHelper html)
        {
            return new HtmlString("$('.pagination a').click(function (e) {" +
                            "e.preventDefault();" +
                            "$('#result').load($(this).attr('href'));" +
                            "});");
        }

        public static HtmlString GetDeleteButtonScript(this HtmlHelper html)
        {
            return new HtmlString("$('.btn-danger').click(function (e) {" +
                            "e.preventDefault();" +
                            "var entityId = $(this).attr('entityId');" +
                            "$('#DeleteFormId').val(entityId);" +
                            "$('#divRecordDelete').modal('show');" +
                            "});");
        }

        public static IHtmlString LinkToAddNestedForm<TModel>(this HtmlHelper<TModel> htmlhelper, string linkText, string containerElement, string counterElement, string collectionProperty, Type nestedType, object htmlAttributes = null, object[] args = null)
        {
            var ticks = DateTime.UtcNow.Ticks;
            var nestedObject = args == null ? Activator.CreateInstance(nestedType) : Activator.CreateInstance(nestedType, args);
            var partial = htmlhelper.EditorFor(x => nestedObject).ToHtmlString().JsEncode();
            partial = partial.Replace("nestedObject_", collectionProperty + "_" + ticks + "_" + "_");
            partial = partial.Replace("nestedObject.", collectionProperty + "[" + ticks + "]" + ".");
            partial = partial.Replace("index=\\\"nestedObject", "index=\\\"" + collectionProperty + "_" + ticks + "_");
            var js = string.Format("javascript:addNestedForm('{0}','{1}','{2}','{3}'); return false;", containerElement, counterElement, ticks, partial);
            var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);

            TagBuilder tb = new TagBuilder("a");
            tb.Attributes.Add("href", "#");
            tb.Attributes.Add("onclick", js);
            tb.InnerHtml = linkText;
            if (attributes.ContainsKey("class"))
            {
                tb.AddCssClass(attributes["class"].ToString());
            }
            var tag = tb.ToString(TagRenderMode.Normal);
            return MvcHtmlString.Create(tag);
        }

        public static IHtmlString LinkToRemoveNestedForm(this HtmlHelper htmlHelper, string linkText, string container, string deleteElement, object htmlAttributes = null)
        {
            var js = string.Format("javascript:removeNestedForm(this,'{0}','{1}');return false;", container, deleteElement);
            TagBuilder tb = new TagBuilder("a");
            tb.Attributes.Add("href", "#");
            tb.Attributes.Add("onclick", js);
            tb.InnerHtml = linkText;
            var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            if (attributes.ContainsKey("class"))
            {
                tb.AddCssClass(attributes["class"].ToString());
            }
            if(attributes.ContainsKey("style"))
            {
                tb.Attributes.Add("style", attributes["style"].ToString());
            }
            var tag = tb.ToString(TagRenderMode.Normal);
            return MvcHtmlString.Create(tag);
        }

        public static string JsEncode(this string s)
        {
            if (string.IsNullOrEmpty(s)) return string.Empty;
            int i;
            int len = s.Length;

            StringBuilder sb = new StringBuilder(len + 4);

            string t;
            for (i = 0; i < len; i++)
            {
                char c = s[i];
                switch (c)
                {
                    case '>':
                    case '"':
                    case '\\':
                        sb.Append('\\');
                        sb.Append(c);
                        break;
                    case '\b':
                        sb.Append("\\b");
                        break;
                    case '\n':
                        break;
                    case '\t':
                        sb.Append("\\t");
                        break;
                    case '\f':
                        sb.Append("\\f");
                        break;
                    case '\r':
                        break;
                    default:
                        if (c < ' ')
                        {
                            string tmp = new string(c, 1);
                            t = "000" + int.Parse(tmp, System.Globalization.NumberStyles.HexNumber);
                            sb.Append("\\u" + t.Substring(t.Length - 4));
                        }
                        else
                        {
                            sb.Append(c);
                        }
                        break;

                }
            }
            return sb.ToString();
        }

        public static HtmlString GenerateSetupMenu(this HtmlHelper html)
        {
            List<Module> modules = HttpContext.Current.Items[AppKeyConfig.MODULE] as List<Module>;

            List<Function> currentUserFunctions = HttpContext.Current.Items[AppKeyConfig.FUNCTION] as List<Function>;

            if (modules == null)
                modules = new List<Module>();

            String menuString = GenerateModulesAndSubMenu(modules, currentUserFunctions, "Setup", "cog", FunctionType.Setup);

            return new HtmlString(menuString);
        }

        public static HtmlString GenerateTransactionMenu(this HtmlHelper html)
        {
            List<Module> modules = HttpContext.Current.Items[AppKeyConfig.MODULE] as List<Module>;

            List<Function> currentUserFunctions = HttpContext.Current.Items[AppKeyConfig.FUNCTION] as List<Function>;

            if (modules == null)
                modules = new List<Module>();

            String menuString = GenerateModulesAndSubMenu(modules, currentUserFunctions, "Ops", "car", FunctionType.Transaction);

            return new HtmlString(menuString);
        }

        public static HtmlString GenerateAccountingMenu(this HtmlHelper html)
        {
            List<Module> modules = HttpContext.Current.Items[AppKeyConfig.MODULE] as List<Module>;

            List<Function> currentUserFunctions = HttpContext.Current.Items[AppKeyConfig.FUNCTION] as List<Function>;

            if (modules == null)
                modules = new List<Module>();

            String menuString = GenerateModulesAndSubMenu(modules, currentUserFunctions, "Accounts", "money", FunctionType.Accounting);

            return new HtmlString(menuString);
        }

        public static HtmlString GenerateReportMenu(this HtmlHelper html)
        {
            List<Module> modules = HttpContext.Current.Items[AppKeyConfig.MODULE] as List<Module>;

            List<Function> currentUserFunctions = HttpContext.Current.Items[AppKeyConfig.FUNCTION] as List<Function>;

            if (modules == null)
                modules = new List<Module>();

            String menuString = GenerateModulesAndSubMenu(modules, currentUserFunctions, "Reports", "book", FunctionType.Report);

            return new HtmlString(menuString);
        }

        public static HtmlString GenerateImportMenu(this HtmlHelper html)
        {
            List<Module> modules = HttpContext.Current.Items[AppKeyConfig.MODULE] as List<Module>;

            List<Function> currentUserFunctions = HttpContext.Current.Items[AppKeyConfig.FUNCTION] as List<Function>;

            if (modules == null)
                modules = new List<Module>();

            String menuString = GenerateModulesAndSubMenu(modules, currentUserFunctions, "Import", "upload", FunctionType.Import);

            return new HtmlString(menuString);
        }

        public static HtmlString GenerateExportMenu(this HtmlHelper html)
        {
            List<Module> modules = HttpContext.Current.Items[AppKeyConfig.MODULE] as List<Module>;

            List<Function> currentUserFunctions = HttpContext.Current.Items[AppKeyConfig.FUNCTION] as List<Function>;

            if (modules == null)
                modules = new List<Module>();

            return new HtmlString(GenerateModulesAndSubMenu(modules, currentUserFunctions, "Export", "download", FunctionType.Export));
        }

        public static string GenerateModulesAndSubMenu(List<Module> modules, List<Function> currentUserFunctions, string mainModuleName, string mainModuleIconName, FunctionType functionType)
        {
            String menu = "";

            int functionCount = 0;

            foreach (var module in modules.OrderBy(x => x.SerialNo))
            {
                var currentUserPermissions = module.FunctionList.Select(x => x.PermissionId).Intersect(currentUserFunctions.Where(x => x.ShowOnMenu).Select(x => x.Id));

                if (module.FunctionList == null || module.FunctionList.Count == 0 || module.FunctionList.Where(x => x.Permission.Type == functionType).ToList().Count == 0 || currentUserPermissions.Count() <= 0) continue;
                menu += "<li class='dropdown-submenu'>";
                menu += $"<a tabindex='0'> <i class='fa {module.IconNameStr} fa-fw'></i> {module.Name} </a>";
                menu += "<ul class='dropdown-menu'>";

                if (module.FunctionList == null) module.FunctionList = new List<ModulePermission>();

                foreach (var function in module.FunctionList.Where(x => x.Permission.Type == functionType && x.Permission.ShowOnMenu == true).OrderBy(x => x.Permission.SerialNo))
                {
                    if (currentUserFunctions.Where(x => x.Id == function.PermissionId).FirstOrDefault() != null)
                    {
                        menu += $"<li><a href='/{function.Permission.Controller.Replace("Controller", "")}'> <i class='fa {function.Permission.IconNameStr} fa-fw'></i> {function.Permission.Name} </a></li>";
                        functionCount++;
                    }
                }
                menu += "</ul>";
                menu += "</li>";
            }

            String fullMenuString = "";

            if (functionCount > 0)
            {
                fullMenuString += "<li class='dropdown'>" +
                                  "<a tabindex = '0' data-toggle='dropdown' data-submenu>" +
                                  "<i class='fa fa-" +
                                   mainModuleIconName +
                                  " fa-fw'></i>&nbsp;" +
                                  mainModuleName +
                                  "&nbsp;<span class='caret'></span>" +
                                  "</a>";
                fullMenuString += "<ul class='dropdown-menu'>";
                fullMenuString += menu;
                fullMenuString += "</ul> </li>";
            }


            return fullMenuString;
        }

        public static HtmlString GetFileUploadIFrame(this HtmlHelper htmlHelper, string tempFolder = "", EntityConfig.EntityType type = EntityConfig.EntityType.Address, Int64 referenceId = 0, BlobFile.BlobFileStatus status = BlobFile.BlobFileStatus.All, bool findByReference = false, int width = 40, int height = 30)
        {
            //return new HtmlString("<iframe id='fileUploadIFrame' onLoad='iframeLoaded()' src='/FileUpload/Index?tempFolder=" + tempFolder + "&findByReference=" + findByReference + "&entityType=" + type + "&referenceId=" + referenceId.ToString() + "&status=" + status + "' style='border:none;height:" + height.ToString() + "em;' class='col-sm-10'></iframe>");
            return new HtmlString("<iframe id='fileUploadIFrame' class='embed-responsive-item' onLoad='iframeLoaded()' src='/FileUpload/Index?tempFolder=" + tempFolder + "&findByReference=" + findByReference + "&entityType=" + type + "&referenceId=" + referenceId.ToString() + "&status=" + status + "'></iframe>");
        }
    }
}
