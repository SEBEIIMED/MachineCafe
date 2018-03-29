
using Generics.Security;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MachineCafeIHM.ApiWrapper
{
    
        public abstract class BaseApiWrapper
        {
            protected RestClient RestClient;

            protected static string BaseUrl;

            public string ErrorPocoSchema { get; set; }

            protected BaseApiWrapper(string baseUrl, string appId, string appSecret)
            {
                RestClient = new RestClient(baseUrl);
                BaseUrl = baseUrl;
            }

            protected BaseApiWrapper(string baseUrl)
            {
                RestClient = new RestClient(baseUrl);
                BaseUrl = baseUrl;
            }

            protected string CreateToken(string url)
            {
                ITokenManager tokenmanager = new TokenManager();
                JsonWebToken token = tokenmanager.PrepareTokenData("demoid");
                return tokenmanager.GenerateToken(token, "demosecret");
            }

        #region Get

            public T GetFromApi<T>(string path)
            {
                T clientPoco = default(T);
                RestRequest request = new RestRequest(path, Method.GET);
                request.AddHeader("Authorization", "JWT " + CreateToken(BaseUrl + path));
                IRestResponse response = RestClient.Execute(request);

                if (!((int)response.StatusCode / 100 == 2))
                    throw new Exception(string.Format("API ERROR{0} : {1}", response.StatusCode, response.Content));

                string jsoncontent = response.Content;
                clientPoco = JsonConvert.DeserializeObject<T>(jsoncontent);

                return clientPoco;
            }
            
            public byte[] GetFromApiAsByte(string path)
            {
                RestRequest request = new RestRequest(path, Method.GET);
                request.AddHeader("Authorization", "JWT " + CreateToken(BaseUrl + path));
                IRestResponse response = RestClient.Execute(request);

                if (!((int)response.StatusCode / 100 == 2))
                    throw new Exception(string.Format("API ERROR{0} : {1}", response.StatusCode, response.Content));


                return response.RawBytes;


                return null;
            }

            public RestRequest BuildRequest(string path, Method method = Method.GET)
            {
                RestRequest request = new RestRequest(path, method);
                request.AddHeader("Authorization", "JWT " + CreateToken(BaseUrl + path));
                return request;
            }


            public IRestResponse ExecuteRequest(RestRequest request)
            {
                IRestResponse response = RestClient.Execute(request);

                if (!((int)response.StatusCode / 100 == 2))
                    throw new Exception(string.Format("API ERROR{0} : {1}", response.StatusCode, response.Content));
                return response;
            }


            #endregion

            #region Post

            public T2 PostFromApi<T1, T2>(string path, T1 poco, out string errorMessage, bool safe = true)
            {
                errorMessage = null;
                RestRequest request = new RestRequest(path, Method.POST);
                request.AddHeader("Authorization", "JWT " + CreateToken(BaseUrl + path));
                request.RequestFormat = DataFormat.Json;
                request.AddBody(poco);

                IRestResponse response = RestClient.Execute(request);

                if (!((int)response.StatusCode / 100 == 2))
                    throw new Exception(string.Format("API ERROR{0} : {1}", response.StatusCode, response.Content));

                string jsoncontent = response.Content;

                return JsonConvert.DeserializeObject<T2>(jsoncontent);
            }

            public T PostFromApi<T>(string path, T poco, bool safe = true)
            {
                string errorMessage;
                return PostFromApi<T, T>(path, poco, out errorMessage, safe);
            }

            public T PostFromApi<T>(string path, T poco, out string errorMessage, bool safe = true)
            {
                return PostFromApi<T, T>(path, poco, out errorMessage, safe);
            }

            #endregion
    }

}