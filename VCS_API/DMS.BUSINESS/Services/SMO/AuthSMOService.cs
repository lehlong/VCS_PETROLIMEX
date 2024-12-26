using DMS.BUSINESS.Dtos.Auth;
using DMS.BUSINESS.Dtos.SMO;
using Microsoft.AspNetCore.Identity.Data;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace DMS.BUSINESS.Services.SMO
{
    public class AuthSMOService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://smoapiuat.petrolimex.com.vn/api/Authorize/Login";

        private const string DefaultUserName = "smoapi";
        private const string DefaultPassword = "smoapi123";

        public AuthSMOService()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<string> Login()
        {
            try
            {
                var apiUrlWithParams = $"{BaseUrl}?username={DefaultUserName}&password={DefaultPassword}";
                var response = await _httpClient.GetAsync(apiUrlWithParams);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var jsonDocument = JsonDocument.Parse(responseContent);
                    if (jsonDocument.RootElement.TryGetProperty("DATA", out var dataProperty))
                    {
                        string token = dataProperty.GetString();
                        LoginSMOResponse.JWTToken = token;
                        return token; 
                    }
                    return null;
                }

                return null; 
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
