using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace Pumpwatch.Models
{
    //Class for basic databaseopperation. I only have chosen to put LoadData(), basic HTTP GET, in here to 
    //demonstrate generic method
    public class DatabaseOperator
    {
        /// <summary>
        /// Loads the data from database. Basic HTTP GET method to demonstrate generic
        /// that can take any type T in and a string uri to add to the client baseadress 
        /// to get the given object from the database and store it in an array and then returning the array of objects
        /// for use in the application
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uri">The URI.</param>
        /// <returns></returns>
        public async Task<T[]>LoadData<T>(string uri)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(@"http://localhost:50562/api/");

                    var result = await client.GetStringAsync(uri);

                    T[] data = JsonConvert.DeserializeObject<T[]>(result);
                    return data;
                }
            }
            catch (HttpRequestException)
            {
                MessageDialog msg = new MessageDialog("Error loading data from database");
                await msg.ShowAsync();
                return null;
            }
        }
    }
}
