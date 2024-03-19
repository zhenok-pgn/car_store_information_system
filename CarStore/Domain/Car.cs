using CarStore.Infrastracture;
using CarStore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace CarStore.Domain
{
    class Car : ValueType<Car>
    {
        [Label("Марка")]
        public string Brand { set; get; }
        [Label("Модель")]
        public string Model { set; get; }
        [Label("Поколение")]
        public string Generation { set; get; }
        [Label("Год выпуска")]
        public int YearOfManufacture { set; get; }
        [Label("Мощность двигателя")]
        public string EnginePower { set; get; }
        [Label("Тип трансмиссии")]
        public TransmissionType TransmissionType { set; get; }
        [Label("Техническое состояние")]
        public TechnicalCondition TechnicalCondition { set; get; }
        [Label("Пробег")]
        public int Mileage { set; get; }
        [Label("Цена")]
        public int Price { set; get; }

        public Car(
            TechnicalCondition technicalCondition, 
            int price, 
            string brand, 
            string model, 
            int yearOfManufacture, 
            string generation,
            string enginePower, 
            TransmissionType transmissionType,
            int mileage)
        {
            TechnicalCondition = TechnicalCondition;
            Price = price;
            Brand = brand;
            Model = model;
            YearOfManufacture = yearOfManufacture;
            Generation = generation;
            EnginePower = enginePower;
            TransmissionType = transmissionType;
            Mileage = mileage;
        }
    }

    class CarsRepository
    {
        static string connectionString = "Data Source=DESKTOP-8BQNQK7\\SQLEXPRESS;Initial Catalog=CarStoreDB;Integrated Security=True";

        public static async void Insert(Car car)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand($"insert into cars values ({car.ToString()})", connection);
                await command.ExecuteNonQueryAsync();
            }
        }

        public static List<Car> SelectAll()
        {
            var result = new List<Car>();
            /*using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(
                    new SqlRequest()
                    .Select()
                    .From("cars")
                    .InnerJoin("characteristics")
                    .On("cars.CharacteristicID = characteristics.Id")
                    .InnerJoin("generations")
                    .On("characteristics.GenerationID = generations.Id")
                    .InnerJoin("models")
                    .On("models.Id = generations.ModelID")
                    .InnerJoin("brands")
                    .On("models.BrandID = brands.Id")
                    .Execute()
                    /*"select * from cars " +
                    "inner join characteristics on cars.CharacteristicID = characteristics.Id " +
                    "inner join generations on characteristics.GenerationID = generations.Id " +
                    "inner join models on models.Id = generations.ModelID " +
                    "inner join brands on models.BrandID = brands.Id",
                    connection);
                SqlDataReader reader = command.ExecuteReader();*/
            var reader = new SqlRequest()
                .Select("TechnicalCondition", "Price", "Brand", "ModelName", "YearOfManufacture", "GenerationName", "EnginePower", "TransmissionType", "Mileage")
                .From("cars")
                .InnerJoin("characteristics")
                .On("cars.CharacteristicID = characteristics.Id")
                .InnerJoin("generations")
                .On("characteristics.GenerationID = generations.Id")
                .InnerJoin("models")
                .On("models.Id = generations.ModelID")
                .InnerJoin("brands")
                .On("models.BrandID = brands.Id")
                .Execute();
            //if (reader.HasRows)
            //{
            //    while (reader.Read())
            //    {
            //        var car = new Car(
            //            (string)reader["TechnicalCondition"] == "New" ? TechnicalCondition.New : TechnicalCondition.Used,
            //            (int)reader["Price"],
            //            (string)reader["Brand"],
            //            (string)reader["ModelName"],
            //            (int)reader["YearOfManufacture"],
            //            (string)reader["GenerationName"],
            //            (string)reader["EnginePower"].ToString(),
            //            (string)reader["TransmissionType"] == "Manual" ? TransmissionType.Manual : TransmissionType.Automatic,
            //            (int)reader["Mileage"]
            //            );
            //        result.Add(car);
            //    }
            //}
            //reader.Close();

            if (reader.Count != 0)
            {
                foreach(var e in reader)
                {
                    var car = new Car(
                        (string)e["TechnicalCondition"] == "New" ? TechnicalCondition.New : TechnicalCondition.Used,
                        (int)e["Price"],
                        (string)e["Brand"],
                        (string)e["ModelName"],
                        (int)e["YearOfManufacture"],
                        (string)e["GenerationName"],
                        (string)e["EnginePower"].ToString(),
                        (string)e["TransmissionType"] == "Manual" ? TransmissionType.Manual : TransmissionType.Automatic,
                        (int)e["Mileage"]
                        );
                    result.Add(car);
                }
            }
            //}
            return result;
        }

        public static List<string> SelectField(string selectField, string table, string condition = null)
        {
            var list = new List<string>();
            var reader = new List<Dictionary<string, object>>();
            if (condition != null)
                reader = new SqlRequest().Select(selectField).From(table).Where(condition).Execute();
            else
                reader = new SqlRequest().Select(selectField).From(table).Execute();
            foreach(var e in reader)
            {
                list.Add(e[selectField].ToString());
            }
            return list;
        }

        /*public static List<string> SelectField(SqlRequest sqlRequest)
        {
            var list = new List<string>();
            var result = sqlRequest.Execute();
            foreach (var e in reader)
            {
                list.Add(e[selectField].ToString());
            }
            return list;
        }*/
    }
}
