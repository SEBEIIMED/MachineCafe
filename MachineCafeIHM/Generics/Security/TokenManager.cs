using Jose;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Generics.Security
{
    public class TokenManager : ITokenManager
    {

        private static int _expirationMinutes = 10;
        /// <summary>
        /// Prepare Token data
        /// </summary>
        /// <param name="appid">appid</param>
        /// <returns><see cref="JsonWebToken"/></returns>
        public JsonWebToken PrepareTokenData(string appid)
        {
            JsonWebToken tokendata = new JsonWebToken();
            tokendata.Payload = new JsonWebTokenPayload();
            tokendata.Payload.appId = appid;

            return tokendata;
        }

        /// <summary>
        /// Génération de token.
        /// </summary>
        /// <param name="tokendata"></param>
        /// <param name="appsecret"></param>
        /// <returns>retourne un jeton</returns>
        public string GenerateToken(JsonWebToken tokendata, string appsecret)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(appsecret)) throw new Exception("appsecret empty");

                // completer le token
                if (tokendata.Header == null)
                    tokendata.Header = new JsonWebTokenHeader();
                if (string.IsNullOrWhiteSpace(tokendata.Header.alg))
                    tokendata.Header.alg = "HS256";
                if (string.IsNullOrWhiteSpace(tokendata.Header.alg))
                    tokendata.Header.typ = "JWT";

                if (tokendata.Payload == null)
                    tokendata.Payload = new JsonWebTokenPayload();
                if (string.IsNullOrWhiteSpace(tokendata.Payload.appId)) throw new Exception("appid empty");
                if (tokendata.Payload.iat == 0)
                    tokendata.Payload.iat = (long)(DateTime.UtcNow - (new DateTime(1970, 1, 1))).TotalSeconds;
                if (string.IsNullOrWhiteSpace(tokendata.Payload.jti))
                    tokendata.Payload.jti = Guid.NewGuid().ToString();

                string token = GenerateFinalToken(tokendata, appsecret);
                return token;

            }
            catch (Exception ex)
            {
                throw new Exception("GenerateToken " + ex.Message, ex);
            }

        }
        /// <summary>
        /// Generates a token to be used in API calls.
        /// </summary>
        public static string GenerateFinalToken(JsonWebToken jeton, string appSecret)
        {
            Dictionary<string, object> header = new Dictionary<string, object>()
                {
                    { "alg", "HS256"},
                    { "typ", "JWT"},
                    { "dtyp", "DOCAJWT1"}
                };

            string token;
            token = Jose.JWT.Encode(jeton.Payload.GetDictionary(), Encoding.UTF8.GetBytes(appSecret), JwsAlgorithm.HS256, extraHeaders: header);
            return token;
        }

        public static JsonWebToken ReadToken(string token)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(token))
                    throw new Exception("Token NotFound");

                if (token.Count(c => c == '.') != 2)
                    throw new Exception("Is not JWT Token");

                JsonWebToken tokenDecrypt = new JsonWebToken();
                tokenDecrypt.Header = Jose.JWT.Headers<JsonWebTokenHeader>(token);


                string payloadstring = Jose.JWT.Payload(token);
                Dictionary<string, object> payloaddatas = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(payloadstring);
                tokenDecrypt.Payload = new JsonWebTokenPayload();
                tokenDecrypt.Payload.SetDictionary(payloaddatas);
                //tokenDecrypt.Payload = Jose.JWT.Payload<DocaJsonWebTokenPayload>(token);

                return tokenDecrypt;
            }
            catch (Exception ex)
            {
                throw new Exception("ReadToken " + ex.Message);
            }
        }
        public static bool ValidateTokenClient(JsonWebToken tokenDecrypt, string tokenString, string url, string mysecret, bool safe = true)
        {
            try
            {
                //appid not found



               
                CheckTimeLife(tokenDecrypt.Payload, TimeSpan.FromMinutes(_expirationMinutes));
                CheckUrl(tokenDecrypt.Payload, url);


                //if (tokenDecrypt.Payload.appId != myappId) throw new Exception("appid invalide");

                //Vérification de la signature et du token
                string deoce = Jose.JWT.Decode(tokenString, Encoding.UTF8.GetBytes(mysecret), JwsAlgorithm.HS256);
                if (string.IsNullOrWhiteSpace(deoce))
                    throw new Exception("signature invalide");

                // if (!string.IsNullOrWhiteSpace(tokenDecrypt.Payload.jti))
              //  CheckUnique(tokenDecrypt, tokenString, true);


                return true;
            }
            catch (Exception ex)
            {
                if (safe) return false;
                else throw;
            }
        }



      



       


        private static void CheckUrl(JsonWebTokenPayload payload, string url)
        {
            //UPDATE : Maintenant l'url est facultative
            if (!string.IsNullOrWhiteSpace(payload.url))
                //Si l'url présente dans le payload est différente de l'url cible, on annule
                if (!(payload.url !=  url))
                    //On fait ça au début parce que ça ne coute rien et ça évite les calculs suivants
                    throw new Exception(string.Format("claims url invalid ({0}) NOT EQUAL ({1})", payload.url, url));
        }


        private static void CheckTimeLife(JsonWebTokenPayload payload, TimeSpan tokenLifeSpan)
        {
            DateTime timeStamp = new DateTime(1970, 1, 1, 0, 0, 0);
            timeStamp = timeStamp.AddSeconds(payload.iat);

            // Ensure the timestamp is valid.
            bool expired = Math.Abs((DateTime.UtcNow - timeStamp).TotalMinutes) > _expirationMinutes;
            if (expired) throw new Exception("Token iat is expired");
        }


        //private static bool Authorize(HttpContext actionContext)
        //{
        //    string token = null;
        //    JsonWebToken tokendata = null;
        //    string noStrictTokenError = null;
        //    string url = "";
        //    try
        //    {

        //        url = actionContext.Request.ToString();


        //        token = GetTokenFromRequest(actionContext.Request);
        //        if (string.IsNullOrWhiteSpace(token)) throw new Exception("token not found");

        //        // Obtenir les informations du token 
        //        tokendata = TokenManager.ReadToken(token);
        //        if (tokendata == null || tokendata.Payload == null) throw new Exception("Read tokendata null");

        //        if (tokendata.Payload.appId != "machinecafe_demo") return false;

        //        try
        //        {
        //            bool isValid = TokenManager.ValidateTokenClient(tokendata, token, url, "machinecafe_demo", false);
        //            if (!isValid) return false;
        //        }
        //        catch (Exception ex)
        //        {
        //            return false;
        //        }


        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}
        //public static string GetTokenFromRequest(HttpRequest request)
        //{
        //    try
        //    {
        //        string token = null;
        //        //var authorisation = request.Headers["Authorization"] as Authorisation;

        //        //if (authorisation != null)
        //        //    return authorizationheader.Scheme == "JWT" ? authorizationheader.Parameter : authorizationheader.Scheme;

        //        //token = request.(_securityToken);

        //        //if (token != null)
        //        //    return token;

        //        //NameValueCollection res = request.Content.ReadAsFormDataAsync().Result;
        //        //return res[_securityToken];

        //        return token;

        //    }
        //    catch (Exception ex)
        //    {

        //        throw new Exception("GetTokenFromRequest " + ex.Message);
        //    }

        //    //string jsoncontent = request.Content.ReadAsStringAsync().Result;
        //    //JObject content = JObject.Parse(jsoncontent);
        //    //if (content.HasValues)
        //    //    return (string) content["token"];

        //}


    }
}