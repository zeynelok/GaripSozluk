using GaripSozluk.Business.Interfaces;
using GaripSozluk.Common.ViewModels;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;

namespace GaripSozluk.Business.Services
{
    public class ApiService : IApiService
    {
        // API'ye sorgu atıp veri çekme 
        public ApiRowVM GetApi(string searchText, int? option = null)
        {
            var asd = new ApiRowVM();
            if (searchText != "")
            {
                //bool isTherebook = false;
                //var index = searchText.IndexOf("(Kitap)");
                var index = -1;
                if (searchText.EndsWith("(Yazar)"))
                {
                    index = searchText.IndexOf("(Yazar)");
                    searchText = searchText.Substring(0, index);
                    option = 1;
                }
                else if (searchText.EndsWith("(Kitap)"))
                {
                    index = searchText.IndexOf("(Kitap)");
                    searchText = searchText.Substring(0, index);
                    option = 2;

                }


                if (option == 1)
                {
                    var client = new RestClient($"http://openlibrary.org/search.json?author=" + searchText);
                    var request = new RestRequest(Method.GET);
                    IRestResponse response = client.ExecuteAsync(request).Result;
                    if (response.IsSuccessful)
                    {
                        var content = JsonConvert.DeserializeObject<ApiRowVM>(response.Content);
                        content.docs = content.docs.GroupBy(x => new { x.title }).Select(a => a.First()).OrderByDescending(x => x.first_publish_year).ToArray();
                        if (index > -1)
                        {
                            content.docs = content.docs.Take(5).ToArray();
                        }
                        return content;
                    }
                }
                else
                {
                    var client = new RestClient($"http://openlibrary.org/search.json?title=" + searchText);
                    var request = new RestRequest(Method.GET);
                    IRestResponse response = client.ExecuteAsync(request).Result;

                    if (response.IsSuccessful)
                    {
                        var content = JsonConvert.DeserializeObject<ApiRowVM>(response.Content);
                        content.docs = content.docs.GroupBy(x => new { x.title }).Select(a => a.First()).OrderByDescending(x => x.first_publish_year).ToArray();
                        if (index>-1)
                        {
                            content.docs = content.docs.Take(1).ToArray();
                        }
                     
                        return content;
                    }
                }
            }

            return asd;
        }
    }
}
