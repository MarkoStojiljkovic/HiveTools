using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Threading;
using System.Net.NetworkInformation;
using System.Net;
using System.Threading.Tasks;

namespace HiveTools
{
    class HTTPClientClass
    {
        private static int timeoutSec = 180; // TODO: make it configurable

        /// <summary>
        ///  Send post request asynchronously, for now print resulting data to console
        /// </summary>
        /// <param name="url">Full url name with protocol included, like http://www.example.com </param>
        /// <param name="pairs">Key value pairs that go with post request</param>
        //async public static void PostRequest(string url, List<KeyValuePair<string, string>> pairs, Action<string> callback)
        //{   
        //    try
        //    {
        //        var client = new HttpClient();
        //        client.Timeout = TimeSpan.FromSeconds(timeoutSec);

        //        var postValues = new FormUrlEncodedContent(pairs);

        //        //Console.WriteLine("Before await, Thread ID: " + Thread.CurrentThread.ManagedThreadId.ToString());
        //        HttpResponseMessage response = await client.PostAsync(url, postValues);

        //        HttpContent content = response.Content;
        //        string mycontent = "";
        //        if (response.IsSuccessStatusCode)
        //        {
        //            mycontent = await content.ReadAsStringAsync();
        //        }

        //        //Console.WriteLine(mycontent);
        //        //System.Windows.Forms.MessageBox.Show(mycontent);
        //        callback(mycontent); // Execute callback with result
                
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine("Exception at PostRequest()");
        //        System.Windows.Forms.MessageBox.Show("Exception at PostRequest()" + e.Message);
        //        throw;
        //    }

        //}

        async public static Task<string> PostRequest(string url, List<KeyValuePair<string, string>> pairs)
        {
            var client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(timeoutSec);

            var postValues = new FormUrlEncodedContent(pairs);

            //Console.WriteLine("Before await, Thread ID: " + Thread.CurrentThread.ManagedThreadId.ToString());
            HttpResponseMessage response = await client.PostAsync(url, postValues);

            HttpContent content = response.Content;
            string mycontent = "";
            if (response.IsSuccessStatusCode)
            {
                mycontent = await content.ReadAsStringAsync();
            }

            return mycontent;
        }



        //async public static void GetRequest(string url, Action<string> callback)
        //{
        //    try
        //    {
        //        var client = new HttpClient();
        //        client.Timeout = TimeSpan.FromSeconds(timeoutSec);
                

        //        //Console.WriteLine("Before await, Thread ID: " + Thread.CurrentThread.ManagedThreadId.ToString());
        //        HttpResponseMessage response = await client.GetAsync(url);

        //        HttpContent content = response.Content;
        //        string mycontent = "";
        //        if (response.IsSuccessStatusCode)
        //        {
        //            mycontent = await content.ReadAsStringAsync();
        //        }
        //        callback(mycontent); // Execute callback with result

        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine("Exception at GetRequest()");
        //        System.Windows.Forms.MessageBox.Show("Exception at GetRequest()" + e.Message);
        //        throw;
        //    }
        //}

        async public static Task<string> GetRequest(string url)
        {
            var client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(timeoutSec);

            FormCustomConsole.WriteLine(url);
            //Console.WriteLine("Before await, Thread ID: " + Thread.CurrentThread.ManagedThreadId.ToString());
            HttpResponseMessage response = await client.GetAsync(url);

            HttpContent content = response.Content;
            string mycontent = "";
            if (response.IsSuccessStatusCode)
            {
                mycontent = await content.ReadAsStringAsync();
            }
            return mycontent;
        }


    }
}
