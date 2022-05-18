using MySql.Data.MySqlClient;
using Sources.Tools.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Sources.Tools.Factory
{
    public class DBManagementMySql : IDBManagement
    {
        private readonly string _connectionString;
        public DBManagementMySql(string connectionString)
        {
            _connectionString = connectionString;
        }
        public async Task<Drink> GetDrink(int drinkId)
        {
            Drink response = null;
            try
            {
                using MySqlConnection connection = new MySqlConnection(_connectionString);
                await connection.OpenAsync();
                MySqlCommand cmd = new MySqlCommand("Sp_GetDrink", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("drinkIdParam", drinkId);
                var dr = await cmd.ExecuteReaderAsync();
                while (await dr.ReadAsync())
                {
                    response = new Drink
                    {
                        Name = dr["Name"].ToString(),
                        DrinkId = (int)dr["DrinkId"],
                        Description = dr["Description"].ToString(),
                        AlcoholicGrade = (decimal)dr["AlcoholicGrade"],
                        OriginCountry = dr["OriginCountry"].ToString(),
                        History = dr["History"].ToString(),
                        WorldWideRanking = (decimal)dr["WorldWideRanking"],
                        SizeBottle = dr["SizeBottle"].ToString(),
                        Price = (double)dr["Price"],
                        InStock = (bool)dr["InStock"]
                    };
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                response = null;
            }
            return response;
        }

        public async Task<List<Drink>> GetDrinks()
        {
            List<Drink> response = new List<Drink>();
            try
            {
                using (MySqlConnection connection = new MySqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    MySqlCommand cmd = new MySqlCommand("Sp_GetDrinks", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    var dr = await cmd.ExecuteReaderAsync();
                    while (await dr.ReadAsync())
                    {
                        response.Add(new Drink
                        {
                            Name = dr["Name"].ToString(),
                            DrinkId = (int)dr["DrinkId"],
                            Description = dr["Description"].ToString(),
                            AlcoholicGrade = (decimal)dr["AlcoholicGrade"],
                            OriginCountry = dr["OriginCountry"].ToString(),
                            History = dr["History"].ToString(),
                            WorldWideRanking = (decimal)dr["WorldWideRanking"],
                            SizeBottle = dr["SizeBottle"].ToString(),
                            Price = (double)dr["Price"],
                            InStock = (bool)dr["InStock"]
                        });
                    }
                    dr.Close();
                }
            }
            catch (Exception e)
            {
                response = null;
            }
            return response;
        }

        public async Task<Drink> InsertDrink(Drink drink)
        {
            Drink response = new Drink();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    MySqlCommand cmd = new MySqlCommand("Sp_InsertDrinks", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("NameParam", drink.Name);
                    cmd.Parameters.AddWithValue("DescriptionParam", drink.Description);
                    cmd.Parameters.AddWithValue("AlcoholicGradeParam", drink.AlcoholicGrade);
                    cmd.Parameters.AddWithValue("OriginCountryParam", drink.OriginCountry);
                    cmd.Parameters.AddWithValue("HistoryParam", drink.History);
                    cmd.Parameters.AddWithValue("WorldWideRankingParam", drink.WorldWideRanking);
                    cmd.Parameters.AddWithValue("SizeBottleParam", drink.SizeBottle);
                    cmd.Parameters.AddWithValue("PriceParam", drink.Price);
                    cmd.Parameters.AddWithValue("InStockParam", drink.InStock);
                    var insertedResponse = await cmd.ExecuteScalarAsync();
                    int insertedDrinkId = (int)(ulong)insertedResponse;
                    if (insertedDrinkId > 0)
                    {
                        response = await GetDrink(insertedDrinkId);
                    }
                }
            }
            catch (Exception exe)
            { 
            }
            return response;
        }

        public async Task<bool> UpdateDrink(Drink drinkObj)
        {
            bool response = false;
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                MySqlCommand cmd = new MySqlCommand("Sp_UpdateDrinks", connection);
                cmd.Parameters.AddWithValue("DrinkIdParam", drinkObj.DrinkId);
                cmd.Parameters.AddWithValue("NameParam", drinkObj.Name);
                cmd.Parameters.AddWithValue("DescriptionParam", drinkObj.Description);
                cmd.Parameters.AddWithValue("AlcoholicGradeParam", drinkObj.AlcoholicGrade);
                cmd.Parameters.AddWithValue("OriginCountryParam", drinkObj.OriginCountry);
                cmd.Parameters.AddWithValue("HistoryParam", drinkObj.History);
                cmd.Parameters.AddWithValue("WorldWideRankingParam", drinkObj.WorldWideRanking);
                cmd.Parameters.AddWithValue("SizeBottleParam", drinkObj.SizeBottle);
                cmd.Parameters.AddWithValue("PriceParam", drinkObj.Price);
                cmd.Parameters.AddWithValue("InStockParam", drinkObj.InStock);
                cmd.CommandType = CommandType.StoredProcedure;
                var dr = await cmd.ExecuteNonQueryAsync();
                response = dr > 0;
            }
            return response;
        }

        public async Task<bool> DeleteDrink(int idDrink)
        {
            bool response = false;
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                MySqlCommand cmd = new MySqlCommand("Sp_DeleteDrinks", connection);
                cmd.Parameters.AddWithValue("@IdDrink", idDrink);
                cmd.CommandType = CommandType.StoredProcedure;
                var dr = await cmd.ExecuteNonQueryAsync();
                response = dr > 0;
            }
            return response;
        }


        public async Task<User> GetUserByUserName(string userName)
        {
            User response = new User();
            try
            {
                using (MySqlConnection connection = new MySqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    MySqlCommand cmd = new MySqlCommand("Sp_GetUserByUserName", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("UserNameParam", userName);
                    var dr = await cmd.ExecuteReaderAsync();
                    while (await dr.ReadAsync())
                    {

                        response = new User
                        {
                            UserId = (int)dr["UserId"],
                            UserName = dr["UserName"].ToString(),
                            Password = dr["Password"].ToString(),
                            Email = dr["Email"].ToString(),
                            Name = dr["Name"].ToString(),
                            LastName = dr["LastName"].ToString(),
                            IsActive = (bool)dr["IsActive"]
                        };
                    }
                    dr.Close();
                }

            }
            catch (Exception e)
            {
                response = null;
            }
            return response;
        }

    }
}
