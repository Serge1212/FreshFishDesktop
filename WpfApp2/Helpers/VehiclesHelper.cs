using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp2.Models;

namespace WpfApp2.Helpers
{
    public class VehiclesHelper
    {
        FirebaseClient client = new FirebaseClient("https://freshfish-bf927.firebaseio.com");//поле для зв'язку з віддаленим сервером Firebase

        //отримання всіх даних працівників з серверу
        public async Task<List<Vehicles>> GetAllVehiclesAsync()
        {
            return (await client
                .Child("vehicles")
                .OnceAsync<Vehicles>()).Select(item => new Vehicles
                {
                    ID = item.Object.ID,
                    Model = item.Object.Model,
                    Mark = item.Object.Mark,
                    ManufactureDate = item.Object.ManufactureDate,
                    Mileage = item.Object.Mileage,
                    FuelConsumption = item.Object.FuelConsumption
                }).ToList();
        }

        //додавання нового працівника
        public async Task AddVehicles(
           string model,
            string mark,
            string manufactureDate,
            string mileage,
            string fuelConsumption
            )
        {

            await client
                .Child("vehicles/")
                .PostAsync(new Vehicles()
                {
                    ID = GetRandomId(),
                    Model = model,
                    Mark = mark,
                    ManufactureDate = manufactureDate,
                    Mileage = mileage,
                    FuelConsumption = fuelConsumption
                });
        }

        //отримання конкретного працівника за айді
        public async Task<Vehicles> GetVehicle(string id)
        {
            var allVehicles = await GetAllVehiclesAsync();
            await client
                .Child("vehicles")
                .OnceAsync<Vehicles>();

            return allVehicles.Where(w => w.ID == id).FirstOrDefault();
        }

  
        public async Task UpdateVehicle(
            string id,
            string model,
            string mark,
            string manufactureDate,
            string mileage,
            string fuelConsumption)
        {
            var toUpdateVehicle = (await client
               .Child("vehicles")
               .OnceAsync<Vehicles>()).Where(a => a.Object.ID == id).FirstOrDefault();

            await client
                .Child("vehicles")
                .Child(toUpdateVehicle.Key)
                .PutAsync(new Vehicles
                {
                    ID = id,
                    Model = model,
                    Mark = mark,
                    ManufactureDate = manufactureDate,
                    Mileage = mileage,
                    FuelConsumption = fuelConsumption

                });
        }
   
        public async Task DeleteVehicle(string id)
        {
            var toDeleteVehicle = (await client
                .Child("vehicles")
                .OnceAsync<Vehicles>()).Where(v => v.Object.ID == id).FirstOrDefault();
            await client.Child("vehicles").Child(toDeleteVehicle.Key).DeleteAsync();
        }


  
        #region Random ID FOR VEHICLES
        string GetRandomId()
        {
            Random rnd = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string x = new string(Enumerable.Repeat(chars, 4)
                .Select(s => s[rnd.Next(s.Length)]).ToArray());
            const string nums = "123456789";
            string y = new string(Enumerable.Repeat(nums, 4)
                .Select(s => s[rnd.Next(s.Length)]).ToArray());

            string sDate = DateTime.Now.ToString();
            DateTime value = (Convert.ToDateTime(sDate.ToString()));

            return x + y +
                value.Day.ToString() +
                value.Month.ToString() +
                value.Year.ToString() +
                value.Minute.ToString() +
                value.Hour.ToString() +
                value.Second.ToString() +
                "v";

        }
        #endregion
    }
}
