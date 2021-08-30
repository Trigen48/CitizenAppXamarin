using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Security.Cryptography;
using Newtonsoft.Json;
using System.IO;
namespace CitizenApp.Common
{
    public class Server
    {
        public static String SERVER_URL = "http://192.168.8.100/citizen/";


       //public static String SERVER_URL = "https://scenevoid.co.za/citizen/";

        // public static String SERVER_URL = "http://192.168.43.139/citizen/";

        private static Account account;
        private static List<Contact> contacts = new List<Contact>();
        public static double w;
        public static string contactRequest = "";

   
        /// <summary>
        /// Get our account
        /// </summary>
        /// <returns></returns>
        public static Account getAccount()
        {
            return account;
        }

        /// <summary>
        /// Get our android application path, used to figure where we will store our files
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string filePath(string file)
        {
            return Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData), file);
        }

        /// <summary>
        /// Try to load our account object by reading the account json file
        /// </summary>
        /// <returns></returns>
        public static bool loadAccount()
        {

            string fname = filePath("account.json");

            try
            {

                if (File.Exists(fname))
                {
                    string data = System.IO.File.ReadAllText(fname);

                    account = JsonConvert.DeserializeObject<Account>(data);

                    return account != null && account.AccountKey != ""; ;
                }


            }
            catch
            {

            }

            return false;
        }

        /// <summary>
        /// Load our contacts from our contact json file
        /// </summary>
        /// <returns></returns>
        public static bool loadContacts()
        {
            string fname = filePath("contacts.json");

            try
            {

                if (File.Exists(fname))
                {
                    string data = System.IO.File.ReadAllText(fname);

                    contacts = JsonConvert.DeserializeObject<List<Contact>>(data);

                    return contacts != null && contacts.Count > 0;
                }


            }
            catch
            {

            }
            return false;
        }

        /// <summary>
        /// save our contact information
        /// </summary>
        /// <param name="contactData"></param>
        /// <returns></returns>
        public static bool SaveContacts(Contact[] contactData)
        {
            //account = response.AccountData;
            contacts.Clear();
            if(contactData!=null)
            {
                contacts.AddRange(contactData);// = contactData; 
            }
           

            return SaveContactsData();
        }

        /// <summary>
        /// save our contact information to a file
        /// </summary>
        /// <returns></returns>
        public static bool SaveContactsData()
        {
            string fname = filePath("contacts.json");

            try
            {
                File.WriteAllText(fname, JsonConvert.SerializeObject(contacts));

                return true;
            }
            catch
            {

            }

            return false;
        }

        /// <summary>
        /// check if contacts are available
        /// </summary>
        /// <returns></returns>
        public static bool hasContacts()
        {
            return contacts.Count>0;
        }

        /// <summary>
        /// return all available contacts
        /// </summary>
        /// <returns></returns>
        public static List<Contact> GetContacts()
        {
            return contacts;
        }

        /// <summary>
        /// save our account to a json object
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public static bool saveAccountByLogin(Common.Net.Response.LoginResponse response)
        {
            account = response.AccountData;

            string fname = filePath("account.json");

            try
            {
                File.WriteAllText(fname, JsonConvert.SerializeObject(account));

                return true;
            }
            catch
            {

            }

            return false;
        }

        /// <summary>
        /// save our account to a json object , using register
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public static bool saveAccountByRegister(Common.Net.Response.RegisterResponse response)
        {
            account = response.AccountData;

            string fname = filePath("account.json");

            try
            {
                File.WriteAllText(fname, JsonConvert.SerializeObject(account));

                return true;
            }
            catch
            {

            }

            return false;
        }

        /// <summary>
        /// save the account data to a file
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public static bool saveAccount(Common.Net.Response.EditProfileResponse response)
        {
            account = response.AccountData;

            string fname = filePath("account.json");

            try
            {
                File.WriteAllText(fname, JsonConvert.SerializeObject(account));

                return true;
            }
            catch
            {

            }

            return false;
        }

        /// <summary>
        /// clear all saved data
        /// </summary>
        public static void ClearAll()
        {
            string fname;
            try
            {
                fname = filePath("account.json");
                if (File.Exists(fname))
                {
                    File.Delete(fname);
                }
            }
            catch
            {

            }


            try
            {
                fname = filePath("contacts.json");
                if (File.Exists(fname))
                {
                    File.Delete(fname);
                }
            }
            catch
            {

            }

            account = null;

            contacts.Clear();
        }

        /// <summary>
        /// get specific account deails
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static Contact getContact(String key)
        {


            int index = ContactIndex(key);
            if (index != -1)
            {
                return contacts[index];
            }



            return null;
        }

        /// <summary>
        /// save single account detail to the list
        /// </summary>
        /// <param name="contact"></param>
        /// <returns></returns>
        public static bool saveContact(Contact contact)
        {

            contacts.Add(contact);


            contacts.Sort((x, y) => x.Fullname.CompareTo(y.Fullname));

            return SaveContactsData();


        }

        /// <summary>
        /// get contact as index
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private static int ContactIndex(string key)
        {
            return  contacts.FindIndex(delegate (Contact item) { return item.AccountKey == key; });

        }

        /// <summary>
        /// remove contact from the list
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool removeContact(String key)
        {

            int index = ContactIndex(key);
            if (index!=-1)
            {
                contacts.RemoveAt(index);

                return SaveContactsData();
            }

            return false;
        }

        /// <summary>
        /// get session from our account
        /// </summary>
        /// <returns></returns>
        public static Session getSession()
        {
            return account.AccountSession;
        }

        /// <summary>
        /// function to send our object as a serialized string to the server and receive a object response
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="RequestObject"></param>
        /// <returns></returns>
        public static T SendRequest<T>(String url, Object RequestObject)
        {
            WebClient client = new WebClient();

            var col = new System.Collections.Specialized.NameValueCollection();

            if (RequestObject != null)
            {
                col.Add("jdata", Newtonsoft.Json.JsonConvert.SerializeObject(RequestObject));
            }

            if (account != null)
            {
                col.Add("sdata", Newtonsoft.Json.JsonConvert.SerializeObject(getSession()));
            }

            try
            {
                byte[] outData = client.UploadValues(SERVER_URL + url, "POST", col);
                string mData = Encoding.UTF8.GetString(outData);
                T myObject = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(mData);
                return myObject;
            }
            catch (Exception ex)
            {

            }

            throw new Exception("Could not receive data");
        }

        /// <summary>
        /// generate qr code
        /// </summary>
        /// <returns></returns>
        public static string getQRJoin()
        {
            return account.AccountKey + "|" + ComputeSHA256(DateTime.Now.ToString());
        }

        /// <summary>
        /// compute sha 256 hash
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ComputeSHA256(string input)
        {
            SHA256 sha256Hash = SHA256.Create();
            byte[] data = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));


            var sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("X2"));
            }


            return sBuilder.ToString();
        }

      

    }
}
