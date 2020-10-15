using GaripSozluk.Business.Interfaces;
using GaripSozluk.Common.ViewModels;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace GaripSozluk.Business.Services
{
    public class ApiService : IApiService
    {
        public ApiRowVM GetApi(string searchText,int option)
        {
            var asd = new ApiRowVM();
            var client = new RestClient($"http://openlibrary.org/search.json?author=" + searchText);
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.ExecuteAsync(request).Result;
            if (response.IsSuccessful)
            {
                var content = JsonConvert.DeserializeObject<ApiRowVM>(response.Content);

                return content;
            }


            return asd;

        }
    }
}
