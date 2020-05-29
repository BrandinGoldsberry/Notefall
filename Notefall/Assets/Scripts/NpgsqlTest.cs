using UnityEngine;
using System.Collections;
using System;
using System.Data;
using Npgsql;

public class NpgsqlTest
{
    async public void RunTest()
    {
        var connString = "Host=192.168.1.14;Username=postgres;Password=pa55w0rd;Database=notefallDatabase";

        var conn = new NpgsqlConnection(connString);
        await conn.OpenAsync();

        Console.WriteLine("Connection Open");

        //// Insert some data
        //await using (var cmd = new NpgsqlCommand("INSERT INTO data (some_field) VALUES (@p)", conn))
        //{
        //    cmd.Parameters.AddWithValue("p", "Hello world");
        //    await cmd.ExecuteNonQueryAsync();
        //}

        //// Retrieve all rows
        //await using (var cmd = new NpgsqlCommand("SELECT some_field FROM data", conn))
        //await using (var reader = await cmd.ExecuteReaderAsync())
        //    while (await reader.ReadAsync())
        //        Console.WriteLine(reader.GetString(0));
    }
}
