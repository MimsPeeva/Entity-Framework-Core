using System.Diagnostics;
using System.Globalization;
using System.Text;
using Medicines.Data.Models;
using Medicines.Data.Models.Enums;
using Medicines.DataProcessor.ImportDtos;
using Medicines.Utilities;
using Newtonsoft.Json;

namespace Medicines.DataProcessor
{
    using Medicines.Data;
    using System.ComponentModel.DataAnnotations;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid Data!";
        private const string SuccessfullyImportedPharmacy = "Successfully imported pharmacy - {0} with {1} medicines.";
        private const string SuccessfullyImportedPatient = "Successfully imported patient - {0} with {1} medicines.";

        public static string ImportPatients(MedicinesContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            int counter = 0;

            ImportPatientsDto[] patientDtos = JsonConvert.DeserializeObject<ImportPatientsDto[]>(jsonString);

            ICollection<Patient> patients = new List<Patient>();

            foreach (ImportPatientsDto pDto in patientDtos)
            {
                if (!IsValid(pDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Patient patient = new Patient()
                {
                    FullName = pDto.FullName,
                    AgeGroup = (AgeGroup)pDto.AgeGroup,
                    Gender = (Gender)pDto.Gender,
                };

                foreach (int medId in pDto.Medicines)
                {
                    if (patient.PatientsMedicines.Any(x => x.MedicineId == medId))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    PatientMedicine patientMedicine = new PatientMedicine()
                    {
                        Patient = patient,
                        MedicineId = medId,
                    };

                    patient.PatientsMedicines.Add(patientMedicine);
                }

                counter += patient.PatientsMedicines.Count;
                patients.Add(patient);
                sb.AppendLine(string.Format(SuccessfullyImportedPatient, patient.FullName, patient.PatientsMedicines.Count));
            }
            context.Patients.AddRange(patients);
            context.SaveChanges();

            return sb.ToString().Trim();
        }

        public static string ImportPharmacies(MedicinesContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();
            XmlHelper helper = new XmlHelper();

            int medCounter = 0;

            ImportPharmaciesDto[] pharmacyDtos = helper.Deserialize<ImportPharmaciesDto[]>(xmlString, "Pharmacies");

            ICollection<Pharmacy> validPharmacies = new List<Pharmacy>();

            foreach (var phDto in pharmacyDtos)
            {
                if (!IsValid(phDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Pharmacy pharmacy = new Pharmacy()
                {
                    Name = phDto.Name,
                    PhoneNumber = phDto.PhoneNumber,
                    IsNonStop = bool.Parse(phDto.IsNonStop),
                };

                foreach (var medDto in phDto.Medicines)
                {
                    if (!IsValid(medDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    DateTime medicineProductionDate;
                    bool isProductionDateValid = DateTime
                        .TryParseExact(medDto.ProductionDate, "yyyy-MM-dd", CultureInfo
                        .InvariantCulture, DateTimeStyles.None, out medicineProductionDate);

                    if (!isProductionDateValid)
                    {
                        sb.Append(ErrorMessage);
                        continue;
                    }

                    DateTime medicineExpityDate;
                    bool isExpityDateValid = DateTime
                        .TryParseExact(medDto.ExpiryDate, "yyyy-MM-dd", CultureInfo
                        .InvariantCulture, DateTimeStyles.None, out medicineExpityDate);

                    if (!isExpityDateValid)
                    {
                        sb.Append(ErrorMessage);
                        continue;
                    }

                    if (medicineProductionDate >= medicineExpityDate)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (pharmacy.Medicines.Any(x => x.Name == medDto.Name && x.Producer == medDto.Producer))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Medicine medicine = new Medicine()
                    {
                        Name = medDto.Name,
                        Price = (decimal)medDto.Price,
                        Category = (Category)medDto.Category,
                        ProductionDate = medicineProductionDate,
                        ExpiryDate = medicineExpityDate,
                        Producer = medDto.Producer,
                    };

                    medCounter++;

                    pharmacy.Medicines.Add(medicine);
                }

                validPharmacies.Add(pharmacy);
                sb.AppendLine(string
                    .Format(SuccessfullyImportedPharmacy, pharmacy.Name, pharmacy.Medicines.Count));
            }
            context.Pharmacies.AddRange(validPharmacies);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}
