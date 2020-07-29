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
    public class WorkersHelper
    {
        FirebaseClient client = new FirebaseClient("https://freshfish-bf927.firebaseio.com");//поле для зв'язку з віддаленим сервером Firebase

        //отримання всіх даних працівників з серверу
        public async Task<List<Workers>> GetAllWorkersAsync()
        {
            return (await client
                .Child("workers")
                .OnceAsync<Workers>()).Select(item => new Workers
                {
                    w_id = item.Object.w_id,
                    name = item.Object.name,
                    surname = item.Object.surname,
                    patronymic = item.Object.patronymic,
                    position = item.Object.position,
                    salary = item.Object.salary,
                    phonenumber = item.Object.phonenumber,
                    address = item.Object.address,
                    additioninfo = item.Object.additioninfo
                }).ToList();
        }

        //додавання нового працівника
        public async Task AddWorker(
            string Name,
            string Surname,
            string Patronymic,
            string Position,
            string Salary,
            string Phonenumber,
            string Address,
            string Additioninfo
            )
        {

            await client
                .Child("workers/")
                .PostAsync(new Workers()
                {
                    w_id = GetRandomId(),//отримуємо нове згенероване айді
                    name = Name,
                    surname = Surname,
                    patronymic = Patronymic,
                    position = Position,
                    salary = Salary,
                    phonenumber = Phonenumber,
                    address = Address,
                    additioninfo = Additioninfo
                });
        }

        //отримання конкретного працівника за айді
        public async Task<Workers> GetWorker(string ID)
        {
            var allWorkers = await GetAllWorkersAsync();
            await client
                .Child("workers")
                .OnceAsync<Workers>();

            return allWorkers.Where(w => w.w_id == ID).FirstOrDefault();
        }

        //оновлення даних конкретного працівника
        public async Task UpdateWorker(
            string ID,
            string Name,
            string Surname,
            string Patronymic,
            string Position,
            string Salary,
            string Phonenumber,
            string Address,
            string Additioninfo)
        {
            var toUpdateProduct = (await client
               .Child("workers")
               .OnceAsync<Workers>()).Where(a => a.Object.w_id == ID).FirstOrDefault();

            await client
                .Child("workers")
                .Child(toUpdateProduct.Key)
                .PutAsync(new Workers
                {
                    w_id = ID,
                    name = Name,
                    surname = Surname,
                    patronymic = Patronymic,
                    position = Position,
                    salary = Salary,
                    phonenumber = Phonenumber,
                    address = Address,
                    additioninfo = Additioninfo

                });
        }
        //видалення конкретного працівника за айді
        public async Task DeleteWorker(string ID)
        {
            var toDeleteWorker = (await client
                .Child("workers")
                .OnceAsync<Workers>()).Where(w => w.Object.w_id == ID).FirstOrDefault();
            await client.Child("workers").Child(toDeleteWorker.Key).DeleteAsync();
        }


        //генерування нового айді
        #region Random ID FOR WORKERS
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
                "w";

        }
        #endregion
    }
}
