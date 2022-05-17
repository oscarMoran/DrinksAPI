using Sources.Tools.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sources.Tools.Factory
{
    public  interface IDBManagement
    {
        public Task<Drink> GetDrink(int drinkId);
        public Task<List<Drink>> GetDrinks();
        public Task<bool> UpdateDrink(Drink drink);
        public Task<bool> DeleteDrink(int drinkId);
        public Task<Drink> InsertDrink(Drink drink);
        public Task<User> GetUserByUserName(string userName);
    }
}
