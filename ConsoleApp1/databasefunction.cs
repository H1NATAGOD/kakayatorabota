using Npgsql;
﻿using System.Runtime.InteropServices.JavaScript;

namespace ConsoleApp1;

public class databasefunction
{
    static string formattedDate = "";

    public static void (int numberID)
    {
        var querySql = $"SELECT datequest, quest, completed FROM sidequests WHERE ID_user = {numberID} ";


        using var cmd = new NpgsqlCommand(querySql, DatabaseService.GetSqlConnection());


        using var reader = cmd.ExecuteReader();


        while (reader.Read())
        {
            Console.WriteLine($"Дата: {reader[0]} \nЗадание: {reader[1]} \nЗавершено: {reader[2]}");
        }

        return default;
    }
}