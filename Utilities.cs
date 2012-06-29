﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace CoverMyMeds.Claims
{
    /// <summary>
    /// Basic utility class for sending claims to CoverMyMeds
    /// </summary>
    class Utilities
    {
        /// <summary>
        /// Function to send URL encoded parameters over an HTTP Post to the CoverMyMeds Claims Service
        /// </summary>
        /// <param name="api_endpoint">URI for submitting HTTP Posts to CoverMyMeds Claims Service</param>
        /// <param name="claim_data">URL Encoded byte stream containing parameters for claim</param>
        /// <returns>HTTP WebResponse with CoverMyMeds URL to request generated by claim</returns>
        public static WebResponse RequestClaim(string api_endpoint, byte[] claim_data)
        {
            WebRequest ClaimRequest = (HttpWebRequest)WebRequest.Create(api_endpoint);

            ClaimRequest.Method = "POST";
            ClaimRequest.ContentType = "application/x-www-form-urlencoded";
            ClaimRequest.ContentLength = claim_data.Length;
            ClaimRequest.GetRequestStream().Write(claim_data, 0, claim_data.Length);
            ClaimRequest.GetRequestStream().Close();

            return ClaimRequest.GetResponse();
        }

        /// <summary>
        /// Function to generate a URL encoded byte array to send over an HTTP Post to the CoverMyMeds Claim API Server
        /// </summary>
        /// <param name="username">CoverMyMeds Account</param>
        /// <param name="password">Password for CoverMyMedsAccount</param>
        /// <param name="api_key">API Key for submitting claims. This is provided upon request from CoverMyMeds</param>
        /// <param name="claim">NCPDP Claim D0</param>
        /// <param name="optional_post_variables">Optional data that can be submitted with a claim that does not have a defined place in the NCPDP claim</param>
        /// <returns>URL Encoded Byte array for HTTP Post variables</returns>
        public static byte[] ConstructClaimPostData(string username, string password, string api_key, string claim, List<KeyValuePair<string, string>> optional_post_variables)
        {
            // Build a collection of KeyValuePairs to load into query string
            List<KeyValuePair<string, string>> lsParameters = new List<KeyValuePair<string, string>>();
            lsParameters.Add(new KeyValuePair<string, string>("username", username));
            lsParameters.Add(new KeyValuePair<string, string>("password", password));
            lsParameters.Add(new KeyValuePair<string, string>("api_key", api_key));
            lsParameters.Add(new KeyValuePair<string, string>("ncpdp_claim", System.Web.HttpUtility.UrlEncode(claim)));
            if (optional_post_variables != null) lsParameters.AddRange(optional_post_variables);

            // Create string containing all required and optional variables
            string PostData = "";
            foreach (KeyValuePair<string, string> kvp in lsParameters)
            {
                PostData += string.Format("{0}={1}&", kvp.Key, kvp.Value);
            }
            PostData.TrimEnd(char.Parse("&"));

            // URL Encode and return byte array
            return Encoding.ASCII.GetBytes(PostData);
        }
    }
}