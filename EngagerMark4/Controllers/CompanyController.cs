using EngagerMark4.ApplicationCore.Cris;
using EngagerMark4.ApplicationCore.Entities;
using EngagerMark4.ApplicationCore.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using EngagerMark4.ApplicationCore.Entities.Users;
using EngagerMark4.Common.Configs;
using EngagerMark4.ApplicationCore.IService.Users;
using EngagerMark4.ApplicationCore.Cris.Users;
using EngagerMark4.Filters;
using EngagerMark4.Common.Utilities;
using EngagerMark4.DTO;

namespace EngagerMark4.Controllers
{
    [CompanyNavigation]
    public class CompanyController : BaseController<CompanyCri,Company,ICompanyService>
    {
        IRoleService _roleService;
        IUserService _userService;

        public ApplicationUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }
        public CompanyController(ICompanyService service,
            IRoleService roleService,
            IUserService userService) : base(service)
        {
            _defaultColumn = "Name";
            this._roleService = roleService;
            this._userService = userService;
        }


        protected async override Task LoadReferences(Company entity)
        {
            if(entity.Id==0)
            {
                entity.Addresses = new HashSet<CompanyAddress>();
                entity.Addresses.Add(new CompanyAddress());
            }
            await base.LoadReferences(entity);
        }

        protected async override Task SaveEntity(Company aEntity)
        {
            if (aEntity.Id == 0) aEntity.Addresses.ToList()[0].State = BaseEntity.ModelState.Added;
            else {
                aEntity.State = BaseEntity.ModelState.Modified;
                aEntity.Addresses.ToList()[0].State = BaseEntity.ModelState.Modified;
            }
            #region Save the uploaded image into the local file system
            foreach (string file in Request.Files)
            {
                if (file.Equals("logo"))
                {
                    HttpPostedFileBase posted_file = Request.Files[file] as HttpPostedFileBase;
                    if (posted_file.ContentLength == 0)
                        continue;
                    if (posted_file.ContentLength > 0)
                    {
                        ImageUpload imageUpload = new ImageUpload { Width = aEntity.Width, UploadPath = ImageConfig.TYPE_COMPANY_LOGO_LOCATION };
                        ImageResult imageResult = imageUpload.RenameUploadFile(posted_file);
                        if (imageResult.Success)
                            aEntity.Logo = imageResult.ImageName;
                        else
                        {
                            ModelState.AddModelError("", "Error Upload Image");
                        }
                    }
                }
                else if (file.Equals("reportLogo"))
                {
                    HttpPostedFileBase posted_file = Request.Files[file] as HttpPostedFileBase;
                    if (posted_file.ContentLength == 0)
                        continue;
                    if (posted_file.ContentLength > 0)
                    {
                        ImageUpload imageUpload = new ImageUpload { Width = 1522, Height = 322, UploadPath = ImageConfig.TYPE_COMPANY_LOGO_LOCATION };
                        ImageResult imageResult = imageUpload.RenameUploadFile(posted_file);
                        if (imageResult.Success)
                            aEntity.ReportHeaderLogo = imageResult.ImageName;
                        else
                        {
                            ModelState.AddModelError("", "Error Upload Image");
                        }
                    }
                }
                else if (file.Equals("transferVoucherLogo"))
                {
                    HttpPostedFileBase posted_file = Request.Files[file] as HttpPostedFileBase;
                    if (posted_file.ContentLength == 0)
                        continue;
                    if (posted_file.ContentLength > 0)
                    {
                        ImageUpload imageUpload = new ImageUpload { Width = 1522, Height = 150, UploadPath = ImageConfig.TYPE_COMPANY_LOGO_LOCATION };
                        ImageResult imageResult = imageUpload.RenameUploadFile(posted_file);
                        if (imageResult.Success)
                            aEntity.TransferVoucherLogo = imageResult.ImageName;
                        else
                        {
                            ModelState.AddModelError("", "Error Upload Image");
                        }
                    }
                }
                else if (file.Equals("FooterLogo"))
                {
                    HttpPostedFileBase posted_file = Request.Files[file] as HttpPostedFileBase;
                    if (posted_file.ContentLength == 0)
                        continue;
                    if (posted_file.ContentLength > 0)
                    {
                        ImageUpload imageUpload = new ImageUpload { Width = 1380, Height = 140, UploadPath = ImageConfig.TYPE_COMPANY_LOGO_LOCATION };
                        ImageResult imageResult = imageUpload.RenameUploadFile(posted_file);
                        if (imageResult.Success)
                            aEntity.ReportFooterLogo = imageResult.ImageName;
                        else
                        {
                            ModelState.AddModelError("", "Error Upload Image");
                        }
                    }
                }
            }
            #endregion

            await this._service.SaveWithGraph(aEntity);
            var admin = new ApplicationUser { UserName = SecurityConfig.ADMIN + aEntity.Domain, Email = SecurityConfig.ADMIN + aEntity.Domain };
            var result = await UserManager.CreateAsync(admin, SecurityConfig.ADMIN_PASSWORD);
            if(result.Succeeded)
            {
                EngagerMark4.ApplicationCore.Entities.Users.User user = new ApplicationCore.Entities.Users.User
                {
                    ApplicationUserId = admin.Id,
                    FirstName = "Admin",
                    LastName = aEntity.Domain,
                    UserName = admin.UserName,
                    Email = admin.Email,
                };
                var cri = new RoleCri();
                cri.StringCris["Code"] = new StringValue { ComparisonOperator = BaseCri.StringComparisonOperator.Equal, Value = SecurityConfig.GENERAL_ADMIN_ROLE };
                var generalAdmin = (await _roleService.GetByCri(cri)).FirstOrDefault();
                if(generalAdmin!=null)
                {
                    user.UserRoleList = new List<UserRole>();
                    user.UserRoleList.Add(new UserRole { RoleId = generalAdmin.Id });
                }
                await this._userService.Save(user);
            }
        }
    }
}