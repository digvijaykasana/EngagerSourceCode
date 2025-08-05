using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using EngagerMark4.ApplicationCore.IService;
using EngagerMark4.ApplicationCore.Cris;
using EngagerMark4.ApplicationCore.Entities;
using System.Threading.Tasks;
using PagedList;
using EngagerMark4.Common.Configs;
using System.Data.Entity.Infrastructure;
using EngagerMark4.Common.Exceptions;
using System.IO;
using EngagerMark4.DocumentProcessor;
using EngagerMark4.ApplicationCore.IRepository;
using EngagerMark4.Infrasturcture.EFContext;
using EngagerMark4.Infrasturcture.Repository;
using EngagerMark4.Common.Utilities;
using EngagerMark4.ApplicationCore.Entities.Application;
using EngagerMark4.ApplicationCore.Entities.Users;

namespace EngagerMark4.Controllers
{
    public abstract class BaseController<Cri, Ent, Service> : Controller
        where Cri : BaseCri, new()
        where Ent : BasicEntity, new()
        where Service : IBaseService<Cri, Ent>
    {
        protected Service _service;
        protected string _column;
        protected string _orderBy;
        protected string _defaultOrderBy = BaseCri.EntityOrderBy.Asc.ToString();
        protected string _defaultDataType = BaseCri.DataType.String.ToString();
        protected string _dataType;
        protected string _defaultColumn = "Code";
        IAuditRepository _auditRepository;
        public string _controller;

        public Int64 _currentEntityId;

        public bool IsForFirstTime = false;
        private bool AllAccess = true;

        List<Audit> audits = new List<Audit>();
        List<Function> functionList = new List<Function>();
        List<Role> roles = new List<Role>();
        Function currentFunction = new Function();
        List<RolePermissionDetails> rolePermissionsDetails = new List<RolePermissionDetails>();

        protected bool ValidateToken()
        {
            var token = Request[ApiConfig.TOKEN_NAME];

            if (string.IsNullOrEmpty(token))
                return false;

            if (!token.Equals(ApiConfig.KEY))
                return false;

            return true;
        }

        public BaseController(Service service)
        {
            this._service = service;
            
        }

        private void LoadCompanyInfo()
        {

        }

        private void LoadAccessInfo()
        {
            this._controller = RouteData.Values["controller"].ToString() + "Controller";
            this.functionList = HttpContext.Items[AppKeyConfig.FUNCTION] as List<Function>;
            if (this.functionList == null) this.functionList = new List<Function>();
            this.roles = HttpContext.Items[AppKeyConfig.ROLE] as List<Role>;
            if (this.roles == null) this.roles = new List<Role>();
            this.currentFunction = this.functionList.FirstOrDefault(x => x.Controller.Equals(_controller));
            if (this.currentFunction == null) this.currentFunction = new Function();
            this.AllAccess =(bool)HttpContext.Items[AppKeyConfig.ALL_ACCESS];
            this.rolePermissionsDetails = HttpContext.Items[AppKeyConfig.DETAILS_PERMISSION] as List<RolePermissionDetails>;
        }

#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        protected async virtual Task LoadReferences(Ent entity)
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        {

        }

        protected async virtual Task LoadReferencesForList(Cri aCri)
        {
            await Task.Delay(0);
        }

        public async virtual Task<ActionResult> Index(Cri cri)
        {
            LoadAccessInfo();

            await LoadReferencesForList(cri);

            var entity = RouteData.Values["controller"].ToString().Replace("Controller", string.Empty);

            try
            {
                Audit audit = new Audit
                {
                    Description = $"{entity} accessed",
                    StartProcessingTime = TimeUtil.GetLocalTime(),
                    Type = Audit.AuditType.Normal
                };

                audits.Add(audit);
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch(Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

            }

            return View(cri);
        }

        protected virtual Cri GetCri()
        {
            return new Cri();
        }

        protected virtual Cri GetOrderBy(Cri aCri)
        {
            if (aCri == null)
                aCri = new Cri();

            if (!string.IsNullOrEmpty(_column))
            {
                aCri.OrderBys = new Dictionary<string, Dictionary<ApplicationCore.Cris.BaseCri.DataType, ApplicationCore.Cris.BaseCri.EntityOrderBy>>();
                var orderValues = new Dictionary<BaseCri.DataType, BaseCri.EntityOrderBy>();
                orderValues[(BaseCri.DataType)Enum.Parse(typeof(BaseCri.DataType), _dataType)] = (BaseCri.EntityOrderBy)Enum.Parse(typeof(BaseCri.EntityOrderBy), _orderBy);
                aCri.OrderBys[_column] = orderValues;
            }
            return aCri;
        }

        protected Cri cri;

        protected async virtual Task<IPagedList<Ent>> GetEntities(Cri aCri)
        {
            try
            {
                cri = GetCri();

                var cri2 = GetOrderBy(cri);

                var entities = await GetData(cri2);

                return entities.ToPagedList(aCri.CurrentPage, aCri.NoOfPage);
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                return null;
            }
        }

        protected async virtual Task<IEnumerable<Ent>> GetData(Cri cri)
        {
            return await _service.GetByCri(cri);
        }

        public async virtual Task<ActionResult> List(Cri aCri)
        {
           // ViewBag.OrderBy = aCri.OrderBy;

            _column = Request["Column"];
            _orderBy = Request["OrderBy"];
            _dataType = Request["DataType"];

            _column = string.IsNullOrEmpty(_column) ? _defaultColumn : _column;
            _orderBy = string.IsNullOrEmpty(_orderBy) ? _defaultOrderBy : _orderBy;
            _dataType = string.IsNullOrEmpty(_dataType) ? _defaultDataType : _dataType;

            ViewBag.Column = _column;
            ViewBag.OrderBy = _orderBy;
            ViewBag.DataType = _dataType;

            cri = aCri;

            var entities = await GetEntities(aCri);

            var entity = RouteData.Values["controller"].ToString().Replace("Controller", string.Empty);

            Audit audit = new Audit
            {
                Description = $"{entity} list page {aCri.CurrentPage} accessed",
                StartProcessingTime = TimeUtil.GetLocalTime(),
                Type = Audit.AuditType.Normal
            };

            audits.Add(audit);

            return PartialView(entities);
        }

        protected virtual void ValidateEntity(Ent aEntity)
        {

        }

        protected async virtual Task SaveEntity(Ent aEntity)
        {
                await _service.Save(aEntity);            
        }

        protected virtual void AfterSaveMessage()
        {
            TempData["color"] = ApplicationConfig.MESSAGE_SAVE_COLOR;
            TempData["message"] = ApplicationConfig.MESSAGE_SAVE_MESSAGE;
        }

        protected virtual void AfterSaveErrorMessage()
        {
            TempData["color"] = ApplicationConfig.MESSAGE_DELETE_COLOR;
            TempData["message"] = ApplicationConfig.MESSAGE_RECORD_CANNOT_SAVE;
        }

        protected virtual void AfterDeleteMessage()
        {
            TempData["color"] = ApplicationConfig.MESSAGE_DELETE_COLOR;
            TempData["message"] = ApplicationConfig.MESSAGE_DELETE_MESSAGE;
        }

        protected virtual void AfterDeleteErrorMessage()
        {
            TempData["color"] = ApplicationConfig.MESSAGE_DELETE_COLOR;
            TempData["message"] = ApplicationConfig.MESSAGE_RECORD_CANNOT_DELETE;
        }

        protected async virtual Task<bool> DeleteEntity(object id)
        {
            var entity = new Ent();
            entity = await _service.GetById(id);
            entity.Id = (Int64)id;
            var result = await _service.Delete(entity);

            var entityStr = RouteData.Values["controller"].ToString().Replace("Controller", string.Empty);

            if(result)
            {
                audits.Add(new Audit
                {
                    Description = $"{entityStr} {entity.ToString()} deleted",
                    StartProcessingTime = TimeUtil.GetLocalTime(),
                    Type = Audit.AuditType.Normal
                });
            }
            else
            {
                audits.Add(new Audit
                {
                    Description = $"{entityStr} {entity.ToString()} delete failed. Item is referenced by another table.",
                    StartProcessingTime = TimeUtil.GetLocalTime(),
                    Type = Audit.AuditType.Normal
                });
            }

            return result;
        }

        public async virtual Task<ActionResult> Details(Int64 id = 0)
        {
            Ent entity = new Ent();
            try
            {
                if (id != 0)
                    entity = await _service.GetById(id);

                await LoadReferences(entity);

                var entityStr = RouteData.Values["controller"].ToString().Replace("Controller", string.Empty);
                audits.Add(new Audit
                {
                    Description = $"{entityStr} {entity.ToString()} accessed",
                    StartProcessingTime = TimeUtil.GetLocalTime(),
                    Type = Audit.AuditType.Normal
                });

                return View(entity);
            }
            catch(Exception ex)
            {
                return View(entity);
            }
        }

#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        public async virtual Task PushNotification(Ent aEntity)
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        {

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async virtual Task<ActionResult> Details(Ent aEntity)
        {
            LoadAccessInfo();

            try
            {
                ValidateEntity(aEntity);

                if(ModelState.IsValid)
                {
                    if (aEntity.Id == 0) IsForFirstTime = true;
                    if (IsForFirstTime)
                    {
                        if (AllAccess == false)
                        {
                            var create = this.rolePermissionsDetails.FirstOrDefault(x => x.Type == RolePermissionDetails.PermissionType.Create);
                            if (create == null)
                            {
                                ModelState.AddModelError("", "No permission to create record!");
                                await LoadReferences(aEntity);
                                return View(aEntity);
                            }
                        }
                        
                    }
                    else
                    {
                        if (AllAccess == false)
                        {
                            var update = this.rolePermissionsDetails.FirstOrDefault(x => x.Type == RolePermissionDetails.PermissionType.Edit);
                            if (update == null)
                            {
                                ModelState.AddModelError("", "No permission to update record!");
                                await LoadReferences(aEntity);
                                return View(aEntity);
                            }
                        }
                        
                    }
                    await SaveEntity(aEntity);

                    AfterSaveMessage();

                    await PushNotification(aEntity);

                    _currentEntityId = aEntity.Id;

                    var returnUrl = Request["ReturnUrl"];

                    if (IsForFirstTime)
                    {
                        var entityStr = RouteData.Values["controller"].ToString().Replace("Controller", string.Empty);
                        audits.Add(new Audit
                        {
                            Description = $"{entityStr} {aEntity.ToString()} created",
                            StartProcessingTime = TimeUtil.GetLocalTime(),
                            Type = Audit.AuditType.Normal
                        });
                    }else
                    {
                        var entityStr = RouteData.Values["controller"].ToString().Replace("Controller", string.Empty);
                        audits.Add(new Audit
                        {
                            Description = $"{entityStr} {aEntity.ToString()} updated",
                            StartProcessingTime = TimeUtil.GetLocalTime(),
                            Type = Audit.AuditType.Normal
                        });
                    }

                    return DetailRedirect(returnUrl);
                }
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch(CannotAddException ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                ModelState.AddModelError("", "Wrong Data Inserted! Record cannot be saved!");
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Data.Entity.Infrastructure.DbUpdateException ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                ModelState.AddModelError("", "Cannot insert dupliate record!");
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", "Issue encountered! " + ex.Message);
            }

            await LoadReferences(aEntity);
            return View(aEntity);
        }

        protected virtual ActionResult DetailRedirect(string returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl))
                return RedirectToAction(nameof(Index));
            else
                return Redirect(returnUrl);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(Int64 id)
        {
            LoadAccessInfo();

            try
            {
                if(AllAccess == false)
                {
                    var delete = this.rolePermissionsDetails.FirstOrDefault(x => x.Type == RolePermissionDetails.PermissionType.Delete);
                    if (delete == null)
                    {
                        TempData["color"] = ApplicationConfig.MESSAGE_DELETE_COLOR;
                        TempData["message"] = "No permission to delete";
                        return RedirectToAction(nameof(Index));
                    }
                }
                var aEntity = await this._service.GetById(id);

                var result = await DeleteEntity(id);

                var entityStr = RouteData.Values["controller"].ToString().Replace("Controller", string.Empty);

                if (result)
                {
                    AfterDeleteMessage();
                    audits.Add(new Audit
                    {
                        Description = $"{entityStr} {aEntity.ToString()} deleted",
                        StartProcessingTime = TimeUtil.GetLocalTime(),
                        Type = Audit.AuditType.Normal
                    });
                }
                else
                {
                    AfterDeleteErrorMessage();
                    audits.Add(new Audit
                    {
                        Description = $"{entityStr} {aEntity.ToString()} delete failed. Item is referenced by another table.",
                        StartProcessingTime = TimeUtil.GetLocalTime(),
                        Type = Audit.AuditType.Normal
                    });
                }

                return RedirectToAction(nameof(Index));
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch(CannotDeleteException ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                AfterDeleteErrorMessage();

                return RedirectToAction(nameof(Index));
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch(DbUpdateException ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                return RedirectToAction(nameof(Index));
            }
        }

        protected override void Dispose(bool disposing)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                if (audits != null)
                {
                    foreach (var audit in audits)
                    {
                        audit.EndProcessingTime = TimeUtil.GetLocalTime();
                    }
                    this._auditRepository = new AuditRepository(db);
                    this._auditRepository.Saves(audits);
                }
            }
            base.Dispose(disposing);
        }

        protected const string CONTENT_DISPOSITION = "content-disposition";
        protected const string CONTENT_JPEG = "image/jpeg";
        protected const string APPLICATION_ZIP = "application/zip";

        protected string GetDisplayTemplate<T>(T entity)
        {
            TextWriter tw = TextWriter.Null;
            HtmlHelper<T> htmlHelper = new HtmlHelper<T>(new ViewContext(ControllerContext,
                new RazorView(ControllerContext, "asd", "sdsd", false, null),
                new ViewDataDictionary(),
                new TempDataDictionary(), tw), new ViewPage());
            var htmlString = htmlHelper.DisplayFor(x => entity);
            return htmlString.ToHtmlString();
        }

        protected string GetEditorTemplate<T>(T entity)
        {
            TextWriter tw = TextWriter.Null;
            HtmlHelper<T> htmlHelper = new HtmlHelper<T>(new ViewContext(ControllerContext,
                new RazorView(ControllerContext, "asd", "sdsd", false, null),
                new ViewDataDictionary(),
                new TempDataDictionary(), tw), new ViewPage());
            var htmlString = htmlHelper.EditorFor(x => entity);
            return htmlString.ToHtmlString();
        }

        protected string GeneratePDF<T>(T entity, string savepath, string fileName)
        {
            PDFProcessor poProcessor = new PDFProcessor();
            var htmlstring = GetDisplayTemplate<T>(entity);
            return poProcessor.GeneratePDF(htmlstring, Server.MapPath(savepath), fileName);
        }

        protected string GetUrl(string action, string controller, object aObject)
        {
            return Url.Action(action, controller, aObject, Request.Url.Scheme);
        }
    }
}