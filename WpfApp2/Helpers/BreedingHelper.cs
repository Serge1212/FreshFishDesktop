using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2.Helpers
{
    public class BreedingHelper
    {
        public FirebaseClient client = new FirebaseClient("https://freshfish-bf927.firebaseio.com");
        public async Task<List<BreedingDetails>> GetAllBreedingDetailsAsync()
        {
            return (await client
                .Child("BreedingDetails")
                .OnceAsync<BreedingDetails>()).Select(item => new BreedingDetails
                {
                    ID = item.Object.ID,
                    InitialWaterLevel = item.Object.InitialWaterLevel,
                    Temperature = item.Object.Temperature,
                    NitrogenAmount = item.Object.NitrogenAmount,
                    FishLaunchDate = item.Object.FishLaunchDate,
                    FishVolume = item.Object.FishVolume
                }).ToList();
        }

        public async Task<List<BreedingWeeks>> GetAllBreedingWeeksAsync()
        {
            return (await client
                .Child("BreedingWeeks")
                .OnceAsync<BreedingWeeks>()).Select(item => new BreedingWeeks
                {
                    ID = item.Object.ID,
                    WeekNumber = item.Object.WeekNumber,
                    WeekDate = item.Object.WeekDate,
                    WaterLevel = item.Object.WaterLevel,
                    BreedingDetailsID = item.Object.BreedingDetailsID
                }).ToList();
        }

        public async Task AddBreedingDetails(
            string initialWaterLevel,
            string temperature,
            string nitrogenAmount,
            string fishLaunchDate,
            string fishVolume)
        {
            await client
                .Child("BreedingDetails/")
                .PostAsync(new BreedingDetails()
                {
                    ID = GetRandomId(),
                    InitialWaterLevel = initialWaterLevel,
                    Temperature = temperature,
                    NitrogenAmount = nitrogenAmount,
                    FishLaunchDate = fishLaunchDate,
                    FishVolume = fishVolume
                });
        }

        public async Task AddBreedingWeek(
            string weekNumber,
            string weekDate,
            string waterLevel,
            string breedingDetailsID)
        {
            await client
                .Child("BreedingWeeks/")
                .PostAsync(new BreedingWeeks()
                {
                    ID = GetRandomId(),
                    WeekNumber = weekNumber,
                    WeekDate = weekDate,
                    WaterLevel = waterLevel,
                    BreedingDetailsID = breedingDetailsID
                });
        }

        public async Task UpdateBreedingDetails(
           string id,
           string initialWaterLevel,
           string temperature,
           string nitrogenAmount,
           string fishLaunchDate,
           string fishVolume)
        {
            var toUpdateBreedingDetails = (await client
                .Child("BreedingDetails")
                .OnceAsync<BreedingDetails>()).Where(bd => bd.Object.ID == id).FirstOrDefault();

            await client
                .Child("BreedingDetails")
                .Child(toUpdateBreedingDetails.Key)
                .PutAsync(new BreedingDetails
                {
                    ID = id,
                    InitialWaterLevel = initialWaterLevel,
                    Temperature = temperature,
                    NitrogenAmount = nitrogenAmount,
                    FishLaunchDate = fishLaunchDate,
                    FishVolume = fishVolume
                });
        }

        public async Task UpdateBreedingWeek(
           string id,
           string weekNumber,
           string weekDate,
           string waterLevel,
           string breedingDetailsID)
        {
            var toUpdateBreedingWeek = (await client
                .Child("BreedingWeeks")
                .OnceAsync<BreedingDetails>()).Where(bw => bw.Object.ID == id).FirstOrDefault();

            await client
                .Child("BreedingWeeks")
                .Child(toUpdateBreedingWeek.Key)
                .PutAsync(new BreedingWeeks
                {
                    ID = id,
                    WeekNumber = weekNumber,
                    WeekDate = weekDate,
                    WaterLevel = waterLevel,
                    BreedingDetailsID = breedingDetailsID
                });
        }

        public async Task DeleteBreedingDelails(string id)
        {
            var toDeleteBreedingDetails = (await client
                .Child("BreedingDetails")
                .OnceAsync<BreedingDetails>()).Where(bd => bd.Object.ID == id).FirstOrDefault();
            await client.Child("BreedingDetails").Child(toDeleteBreedingDetails.Key).DeleteAsync();
        }
        public async Task DeleteBreedingWeek(string id)
        {
            var toDeleteBreedingWeek = (await client
                .Child("BreedingWeeks")
                .OnceAsync<BreedingWeeks>()).Where(bw => bw.Object.ID == id).FirstOrDefault();
            await client.Child("BreedingWeeks").Child(toDeleteBreedingWeek.Key).DeleteAsync();
        }
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
                value.Second.ToString();

        }
    }
}
