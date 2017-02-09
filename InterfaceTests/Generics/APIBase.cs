﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceTests.Generics
{
    public abstract class APIBase
    {
        public AppConfig _config; 
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string BaseURL { get; set; }
        public string APIKey { get; private set; }

        public Dictionary<string, string> EndPoints { get; set; }
        public Dictionary<string, string> Headers { get; set; }

        public APIBase(string name, string apikey)
        {
            Name = name;
            Headers = new Dictionary<string, string>(); 
            EndPoints = new Dictionary<string, string>();
            APIKey = apikey;
        }

        public APIBase(AppConfig config, string name)
        {
            Name = name; 
            _config = config;
            Headers = new Dictionary<string, string>();
            EndPoints = new Dictionary<string, string>();
            APIKey = _config.Tokens[name]; 
        }

        public virtual string VisitEndpoint(string endpoint)
        {
            HttpWebRequest req = HttpWebRequest.CreateHttp(endpoint);

            //there are a handful of headers you have to manually modify 
            if (Headers.ContainsKey("Accept"))
                req.Accept = Headers["Accept"];
            //for everything else, add it in this probably should be moved into its own method
            foreach (var pair in Headers.Where(x => x.Key != "Accept"))
                req.Headers.Add(pair.Key, pair.Value);

            HttpWebResponse response = (HttpWebResponse)req.GetResponse();
            using (var reader = new StreamReader(response.GetResponseStream()))
                return reader.ReadToEnd();
        }

        public async Task<Response<string>> VisitEndpointAsync(string endpoint)
        {
            Response<string> response = new Response<string>();
            try
            {
                HttpWebRequest req = HttpWebRequest.CreateHttp(endpoint);
                if (Headers.ContainsKey("Accept"))
                    req.Accept = Headers["Accept"];
                foreach (var pair in Headers.Where(x => x.Key != "Accept")) //messy and difficult to adapt to other apis. Should separate headers
                    req.Headers.Add(pair.Key, pair.Value);

                using (WebResponse res = await req.GetResponseAsync())
                    using (StreamReader reader = new StreamReader(res.GetResponseStream()))
                        response.Result = await reader.ReadToEndAsync();
            }
            catch (Exception ex)
            {
                response.Ex = ex;
            }
            return response;
        }

        public virtual Task<Response<string>> CacheChannelEndpointAsync(string apiData, string root)
        {
            var response = new Response<string>();
            response.Result = "Higher class not instantiated";
            response.Ex = new Exception("Call this method from the parent class");
            return Task.FromResult(response); 
        }

        public async Task<Response<MongoDB.Bson.BsonDocument>> SaveAsConfig()
        {
            //I like the idea of being able to save a template of an api object so 
            //we have the option to instantiate from a database call 
            //incase we have to query an api for something in the web application     
            
                   
            throw new NotImplementedException();
        }
    }
}