using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace NKolayTahmin.RestClient
{
    public class Client
    {
        private Client _instance;
        private readonly HttpClient _client = new HttpClient();
        private string apiUrl = "http://10.70.0.11";
        private KeyValuePair<bool, string> _genericEerror = new KeyValuePair<bool, string>(false, "Bir hata oluştu, biz de merak ediyoruz :(");

        public Client Instance {
            get
            {
                if (_instance == null)
                    _instance = new Client();
                return _instance;
            }
        }

        public async Task<KeyValuePair<bool,string>> getTodaysQuestion()
        {
            return await _instance._getData("/todaysquestion");
        }

        public async Task<KeyValuePair<bool, string>> StartSession(string email)
        {
            var value = new Dictionary<string, string> { { "email", email } };

            return await _postData("/startsession", value);
        }

        public async Task<KeyValuePair<bool,string>> PostAnswer(int answer)
        {
            var value = new Dictionary<string, string> { { "answer", answer.ToString() } };

            return await _postData("/postanswer", value);
        }

        private async Task<KeyValuePair<bool, string>> _postData(string method, Dictionary<string, string> parameters)
        {
            var content = new FormUrlEncodedContent(parameters);

            var response = await _client.PostAsync(apiUrl + method, content);
            if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return new KeyValuePair<bool, string>(true, response.Content.ToString());
            }
            else
                return _genericEerror;
        }

        private async Task<KeyValuePair<bool, string>> _getData(string method)
        {
            var response = await _client.GetAsync(apiUrl + method);
            if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return new KeyValuePair<bool, string>(true, response.Content.ToString());
            }
            else
                return _genericEerror;
        }

        private Client()
        {
            throw new Exception("Bu bir singleton objesidir. Objeyi Instance ile çağır!");
        }

        //public Client()
        //{
        //    throw new Exception("Bu bir singleton objesidir. Objeyi Instance ile çağır!");
        //}
    }
}
