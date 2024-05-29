namespace bitirmeProje.Dto
{
    public class UserRoleDto
    {
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string RoleName { get; set; }
        public string ImageUrl { get; set; }
    }
}
