using Geocoding;
using Geocoding.Google;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EngagerMark4.Common.Utilities
{
    public static class GoogleMapUtil
    {
        public static string GetCoordinateofLocation(string aPostalCode, string aKey, string aUrl)
        {
            if (string.IsNullOrEmpty(aPostalCode)) return null;
            if (string.IsNullOrEmpty(aKey)) return null;
            if (string.IsNullOrEmpty(aUrl)) return null;

            string l_googleApi_key = aKey.Trim();
            aPostalCode = aPostalCode.Trim();
            string url = aUrl.Trim().Replace("[postalCode]", aPostalCode).Replace("[key]", aKey);
            WebRequest request = WebRequest.Create(url);
            using (WebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    DataSet dsResult = new DataSet();
                    dsResult.ReadXml(reader);
                    DataTable dtCoordinates = new DataTable();
                    dtCoordinates.Columns.AddRange(new DataColumn[4] { new DataColumn("Id", typeof(int)),
                        new DataColumn("Address", typeof(string)),
                        new DataColumn("Latitude",typeof(string)),
                        new DataColumn("Longitude",typeof(string)) });

                    var dataRowsTable = dsResult.Tables["result"];

                    //var dataRows = dsResult.Tables["result"].Rows;

                    if (dataRowsTable == null) return null;

                    foreach (DataRow row in dsResult.Tables["result"].Rows)
                    {
                        string geometry_id = dsResult.Tables["geometry"].Select("result_id = " + row["result_id"].ToString())[0]["geometry_id"].ToString();
                        DataRow location = dsResult.Tables["location"].Select("geometry_id = " + geometry_id)[0];
                        return location["lat"] + "," + location["lng"];
                    }
                    return null;
                }
            }
        }

        public static string GenerateAddressFromLongAndLati(double longitude, double latitude)
        {
            try
            {
                //IGeoCoder geocoder = new GoogleGeoCoder();

                //IEnumerable<GeoCoding.Address> addresses = geocoder.ReverseGeocode(latitude, longitude);
                string strAddress = "";
                //int i = 0;
                //foreach (GeoCoding.Address address in addresses)
                //{
                //    i++;
                //    if (i == 2)
                //        return strAddress;
                //    strAddress += address.FormattedAddress + " ";
                //    return strAddress;
                //}
                return strAddress;
            }
            catch (Exception ex)
            {
                return "Geo Code error," + latitude.ToString() + "," + longitude.ToString();
            }
        }
    }
}
