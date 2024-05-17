namespace bitirmeProje.Dto
{
    public class UserInfoDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Username { get; set; }
        public string Mail { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime Birthday { get; set; }
        public Guid DistrictId { get; set; }
        public Guid GenderId { get; set; }
        public string DistrictName { get; set; }
        public Guid CityId { get; set; }
        public Guid CountryId { get; set; }
        public string GenderName { get; set; }
        public string CityName { get; set; }
        public string CountryName { get; set; }

    }
}
