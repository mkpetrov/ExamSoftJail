namespace SoftJail.DataProcessor
{

    using Data;
    using Microsoft.EntityFrameworkCore.Internal;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using SoftJail.Data.Models;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    public class Deserializer
    {
        public static string ImportDepartmentsCells(SoftJailDbContext context, string jsonString)
        {
            var departmentsCells = JsonConvert.DeserializeObject<List<Department>>(jsonString);

            var departmentsToAdd = new List<Department>();

            var sb = new StringBuilder();

            foreach (var department in departmentsCells)
            {
                if ((department.Name.Length < 3 || department.Name.Length > 25) ||
                    !department.Cells.Any() ||
                    department.Cells.ToList().Any(c => c.CellNumber < 1 || c.CellNumber > 1000))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                departmentsToAdd.Add(department);

                sb.AppendLine($"Imported {department.Name} with {department.Cells.Count} cells");
            }

            context.AddRange(departmentsToAdd);
            context.SaveChanges();

            return sb.ToString();
        }

        public static string ImportPrisonersMails(SoftJailDbContext context, string jsonString)
        {
            var format = "dd/MM/yyyy"; 
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = format, Culture = CultureInfo.InvariantCulture };

            var prisonersMails = JsonConvert.DeserializeObject<List<Prisoner>>(jsonString, dateTimeConverter);

            var prisonersToAdd = new List<Prisoner>();

            var sb = new StringBuilder();

            foreach (var prisoner in prisonersMails)
            {
                if ((string.IsNullOrEmpty(prisoner.FullName) || prisoner.FullName.Length < 3 || prisoner.FullName.Length > 20) ||
                    (prisoner.IncarcerationDate == null || prisoner.ReleaseDate == null) ||
                    (prisoner.Age < 18 || prisoner.Age > 65) ||
                    (string.IsNullOrEmpty(prisoner.Nickname) || !prisoner.Nickname.StartsWith("The")) ||
                    !prisoner.Mails.ToList().Any(x => x.Address.EndsWith("str.")) ||
                    (prisoner.IncarcerationDate == default || prisoner.ReleaseDate == default) ||
                    prisoner.Bail < 0)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                var nickNameParts = prisoner.Nickname.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (!nickNameParts.All(x => char.IsUpper(x[0])))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                prisonersToAdd.Add(prisoner);

                sb.AppendLine($"Imported {prisoner.FullName} {prisoner.Age} years old");
            }

            context.AddRange(prisonersToAdd);
            context.SaveChanges();

            return sb.ToString();
        }

        public static string ImportOfficersPrisoners(SoftJailDbContext context, string xmlString)
        {
            return string.Empty;
        }

        private static bool IsValid(object obj)
        {
            var validationContext = new ValidationContext(obj);
            var validationResult = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(obj, validationContext, validationResult, true);
            return isValid;
        }
    }
}