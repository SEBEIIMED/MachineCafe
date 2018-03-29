using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Generics.Security
{
    /// <summary>
    /// classe JsonWebToken
    /// </summary>
    public class JsonWebToken
    {
        public JsonWebTokenHeader Header { get; set; }

        public JsonWebTokenPayload Payload { get; set; }

        public string Signature { get; set; }

    }
    /// <summary>
    /// classe JsonWebTokenHeader
    /// </summary>
    public class JsonWebTokenHeader
    {
        /// <summary>
        /// Identifiant de l'algorithme utilisé pour signer le token
        /// Exemple: HS256 pour l'algorithme HMAC + SHA 256
        /// </summary>
        public string alg { get; set; }

        /// <summary>
        /// Media Type du jeton. Doit être "JWT".
        /// </summary>
        public string typ { get; set; }
    }

    public class JsonWebTokenPayload
    {

        private Dictionary<string, object> datas = new Dictionary<string, object>();


        /// <summary>
        /// Identifiant applicatif 
        /// </summary>
        public string appId { get;  set;  }

        /// <summary>
        /// Issued at Time
        /// Date de création du token au format "secondes depuis 01/01/1970" comme défini par POSIX
        /// Exemple: 1476872266748
        /// </summary>
        public long iat { get; set; }

        /// <summary>
        /// Url absolue de la requete
        /// </summary>
        public string url { get; set; }



        /// <summary>
        /// Identificant d'unicité du jeton
        /// </summary>
        public string jti { get; set; }
        public Dictionary<string, object> GetDictionary()
        {
            return this.datas;
        }
        public bool SetObject(string nameValue, object obj)
        {
            if (string.IsNullOrWhiteSpace(nameValue)) return false;
            nameValue = nameValue.ToLower();
            if (datas.ContainsKey(nameValue)) datas[nameValue] = obj;
            else datas.Add(nameValue, obj);
            return true;

        }

        public void SetDictionary(Dictionary<string, object> datad)
        {
            foreach (var item in datad)
            {
                this.SetObject(item.Key, item.Value);
            }
        }
        

    }
}