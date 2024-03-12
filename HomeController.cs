using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Ass_2.Models;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http; 



namespace Ass_2.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Submit(string email)
    {
        Dictionary<string, string> parameters = new Dictionary<string, string>();
        parameters.Add("email", email);
        var _data = ExecuteProcedure("GetData", parameters);
        return new JsonResult(_data);
    }

    public object ExecuteProcedure(string procedureName, Dictionary<string, string> parameters)
    {
        string connectionString = "Data Source=OMII-SAII\\SQLEXPRESS02;Initial Catalog=Self;Integrated Security=true;";
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            using (SqlCommand data = new SqlCommand(procedureName, connection))
            {
                data.CommandType = CommandType.StoredProcedure;
                foreach (KeyValuePair<string, string> parameter in parameters)
                {
                    data.Parameters.AddWithValue("@" + parameter.Key, parameter.Value);
                }

                using (SqlDataReader reader = data.ExecuteReader())
                {
                    DataTable dataTable = new DataTable();
                    dataTable.Load(reader);
                    
                    // Convert DataTable to List<Dictionary<string, object>> for serialization
                    var result = new List<Dictionary<string, object>>();
                    foreach (DataRow row in dataTable.Rows)
                    {
                        var rowData = new Dictionary<string, object>();
                        foreach (DataColumn column in dataTable.Columns)
                        {
                            rowData.Add(column.ColumnName, row[column]);
                        }
                        result.Add(rowData);
                    }
                    return result;
                }
            }
        }
    }

    

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
