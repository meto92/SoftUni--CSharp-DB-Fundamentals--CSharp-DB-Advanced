namespace PetClinic.DataProcessor
{
    using System.Linq;
    using PetClinic.Data;
    using PetClinic.Models;

    public class Bonus
    {
        public static string UpdateVetProfession(PetClinicContext context, string phoneNumber, string newProfession)
        {
            Vet vet = context.Vets
                .Where(v => v.PhoneNumber == phoneNumber)
                .FirstOrDefault();

            string result = $"Vet with phone number {phoneNumber} not found!";

            if (vet == null)
            {
                return result;
            }

            string oldProfession = vet.Profession;

            vet.Profession = newProfession;

            context.SaveChanges();

            result = $"{vet.Name}'s profession updated from {oldProfession} to {newProfession}.";
                
            return result;
        }
    }
}