﻿using bitirmeProje.Domain.Entities;
using bitirmeProje.Domain.IRepositories;
using bitirmeProje.Dto;
using bitirmeProje.Helper.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace bitirmeProje.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly ILoginUserHelper _loginUserHelper;
        private readonly IRoleRepository _roleRepository;
        private readonly IGroupUserRepository _groupUserRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly IGenderRepository _genderRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;



        public UserController(IUserRepository userRepository, ILoginUserHelper loginUserHelper, IRoleRepository roleRepository, IGroupUserRepository groupUserRepository, IGroupRepository groupRepository,  IGenderRepository genderRepository,  IWebHostEnvironment webHostEnvironment)
        {
            _userRepository = userRepository;
            _loginUserHelper = loginUserHelper;
            _roleRepository = roleRepository;
            _groupUserRepository = groupUserRepository;
            _groupRepository = groupRepository;
            _genderRepository = genderRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task <IActionResult> Index()
        {
            var userId = _loginUserHelper.GetLoginUserId();
            ViewBag.UserInfo=await _userRepository.FirstOrDefaultAsync(x=>x.Id==userId);
            ViewBag.GroupsManagedBy =await GroupsManagedBy(userId);
            ViewBag.GroupMemberInfo = await GroupMember(userId);
            return View();
        }
        public async Task<List<Group>> GroupsManagedBy(Guid userId)
        {
            //Yönetici rolünün id'sini çekiyoruz
            var adminRoleId=await _roleRepository.FirstOrDefaultAsync(x=>x.RoleName=="Yönetici");
            // Kullanıcının yönetici olduğu grupların ID'lerini seçiyoruz
            var groupsId = await _groupUserRepository.GetAll(x => x.UserId == userId && x.RoleId == adminRoleId.Id)
                .Select(x => x.GroupId)
                .ToListAsync();
           var groupInfo= new List<Group>();  // Grup bilgilerini saklamak için bir liste
            foreach (var groupId in groupsId)
            {
              var group= await  _groupRepository.FirstOrDefaultAsync(x => x.Id == groupId);
                if(group != null)
                {
                    groupInfo.Add(new Group // Grup bilgilerini listeye ekliyoruz
                    {
                        Id = groupId,
                        GroupName= group.GroupName,
                        ImageUrl= group.ImageUrl,
                        GroupDescription=group.GroupDescription,
                    });
                }
            }
            return groupInfo; // Grup bilgilerini döndürüyoruz
        }


        // kullanıcının üye olduğu grupları döndürür.
        public async Task<List<Group>> GroupMember(Guid userId)
        {
            //"Üye" rolünün ID'sini çekiyoruz
            var adminRoleId = await _roleRepository.FirstOrDefaultAsync(x => x.RoleName == "Üye");
            var groupsId = await _groupUserRepository.GetAll(x => x.UserId == userId && x.RoleId == adminRoleId.Id)
               .Select(x => x.GroupId)
               .ToListAsync();// Üye olduğu grupların ID'lerini seçiyoruz
            var groupMemberInfo = new List<Group>(); //liste oluşturuyoruz
            foreach (var groupId in groupsId)
            {   // Grup bilgilerini alıyoruz
                var group = await _groupRepository.FirstOrDefaultAsync(x => x.Id == groupId);
                if (group != null)
                {
                    groupMemberInfo.Add(new Group  // Grup bilgilerini listeye ekliyoruz
                    {
                        Id = groupId,
                        GroupName = group.GroupName,
                        ImageUrl = group.ImageUrl,
                        GroupDescription = group.GroupDescription,
                    });
                }
            }
            return groupMemberInfo; // Grup bilgilerini döndürüyoruz
        }
        [HttpPost]
        public async Task<IActionResult> UserUpdate(IFormFile img, User user)
        {
            var userId = _loginUserHelper.GetLoginUserId();
            var path = "";
            if (img != null)
            {
                // wwwroot yolunu alır
                var rootPath = _webHostEnvironment.WebRootPath;
                var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(img.FileName); // Uzantı olmadan dosya adı
                var fileExtension = Path.GetExtension(img.FileName); // Dosya uzantısı
                // Dosyanın kaydedileceği yolu oluşturur                                            
                var filePath = Path.Combine(rootPath, "userImage", fileNameWithoutExtension + "-" + userId.ToString() + fileExtension);
                // Dosya dizininin var olduğundan emin olur
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                // Dosya akışı oluşturur ve dosyayı kaydeder
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await img.CopyToAsync(stream);// Dosyayı asenkron olarak kopyalar
                }
                // Grup resmi URL'sini ayarlar
                path = "/userImage/" + fileNameWithoutExtension + "-" + userId.ToString() + fileExtension;
            }
            //Diğer güncellenen bilgileri ekler ve UpdateAsync ile güncelleme işlemini tamamlar.
            var data =await _userRepository.FirstOrDefaultAsync(x => x.Id == userId);
            if (data != null)
            {
                data.Name= user.Name;
                data.Surname = user.Surname;
                data.PhoneNumber = user.PhoneNumber;
                data.Birthday = user.Birthday;
                data.Mail = user.Mail;
                data.ImageUrl = img != null ? path : user.ImageUrl;
               await _userRepository.UpdateAsync(data);
            }
            return Json(new Response<User>
            {
                Success = true,
                Message = "Kayıt Başarılı",
                Result = data
            });
        }
    }
}
