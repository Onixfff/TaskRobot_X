using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Solution1
{

    public class Card
    {
        public long cardCode { get; private set; }
        public DateTime? startDate { get; private set; }
        public DateTime? finishDate { get; private set; }
        public string lastName { get; private set; }
        public string firstName { get; private set; }
        public string surName { get; private set; }
        public string fullName { get; private set; }
        public int genderId { get; private set; }
        public DateTime? birthday { get; private set; }
        public string phoneHome { get; private set; }
        public string phoneMobil { get; private set; }
        public string email { get; private set; }
        public string city { get; private set; }
        public string street { get; private set; }
        public string house { get; private set; }
        public string apartment { get; private set; }
        public string alltaddress { get; private set; }
        public string cardType { get; private set; }
        public string ownerguId { get; private set; }
        public long cardper { get; private set; }
        public double turnover { get; private set; }

        public Card(long cardCode, DateTime? startDate, DateTime? finishDate, string lastName, string firstName, string surName, string fullName, int genderId, DateTime? birthday, string phoneHome, string phoneMobil, string email, string city, string street, string house, string apartment, string alltaddress, string cardType, string ownerguId, long cardper, double turnover)
        {
            this.cardCode = cardCode;
            this.startDate = startDate;
            this.finishDate = finishDate;
            this.lastName = lastName;
            this.firstName = firstName;
            this.surName = surName;
            this.fullName = fullName;
            this.genderId = genderId;
            this.birthday = birthday;
            this.phoneHome = phoneHome;
            this.phoneMobil = phoneMobil;
            this.email = email;
            this.city = city;
            this.street = street;
            this.house = house;
            this.apartment = apartment;
            this.alltaddress = alltaddress;
            this.cardType = cardType;
            this.ownerguId = ownerguId;
            this.cardper = cardper;
            this.turnover = turnover;
        }
    }
}
