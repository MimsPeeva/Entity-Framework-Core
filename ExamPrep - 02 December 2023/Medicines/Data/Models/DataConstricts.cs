using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medicines.Data.Models
{
    public class DataConstricts
    {
        //Pharmacy
        public const byte PharmacyNameMinLength = 2;
        public const byte PharmacyNameMaxLength = 50;
        public const byte PhoneNumberMaxLength = 14;
        public const string PharmacyPhoneNumberRegex = @"^\(\d{3}\) \d{3}-\d{4}$";
        public const string PharmacyBooleanRegex = @"^(true|false)$";

        //Medicine
        public const byte MedicineNameMinLength = 3;
        public const byte MedicineNameMaxLength = 150;
        public const double MedicinePriceMinValue = 0.01;
        public const double MedicinePriceMaxValue = 1000.00;
        public const byte MedicineProducerMinLength = 3;
        public const byte MedicineProducerMaxLength = 100;
        public const byte MedicineCategoryMinValue = 0;
        public const byte MedicineCategoryMaxValue = 4;


        //Patient
        public const int PatientFullNameMinLength = 5;
        public const int PatientFullNameMaxLength = 100;

    }
}
