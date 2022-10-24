using Cargo4You.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Net.Http.Headers;

namespace Cargo4You.Controllers
{
    public class CalculationController : Controller
    {

        //Takes the input data from the client, call methods for calculate results, get api
        //Returns a view with model dictionary results
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Result(InputDataModel obj)
        {

            if (ModelState.IsValid)
            {
                //Take all the data from the client input
                double DimensionResult = CalculateVolume(obj.Depth, obj.Width, obj.Height);
                double Weight = obj.Weight;

                //Create model object for our company cargo
                //Make the calculation and set property FinalPrice
                Cargo4YouModel cargo4YouObj = new Cargo4YouModel();
                cargo4YouObj.FinalPrice = Cargo4YouCalculation(cargo4YouObj, DimensionResult, Weight);

                //Create list of partners, objects from partner model class
                var partners = new List<PartnerModel>() {
             new PartnerModel(){ Name="Ship Faster",
                 BaseUrl="http://localhost:5164/",
                 ApiControler="Api"},
             new PartnerModel(){ Name="Malta Ship",
                 BaseUrl="http://localhost:5163/",
                 ApiControler="Api"}
            };

                //All the results goes in Dictionary (name, price) - we want to retrieve name and price in the view
                Dictionary<string, double> results = new Dictionary<string, double>();
                results.Add("Cargo 4 You", cargo4YouObj.FinalPrice);


                //foreach loop - go throu all the partners, call Api request, put the result in the dictionary 
                foreach (var partner in partners)
                {
                    double resultApi = CallingWebApi(partner.BaseUrl, partner.ApiControler, DimensionResult, Weight).Result;
                    results.Add(partner.Name, resultApi);
                }


                //return a view, bind the view with the dictionary - we want to retrieve name and price in the view
                return View(results);
            }

            

           return RedirectToAction("Index","Home");
        }




        //method for calculate volume for the parcel
        public double CalculateVolume(double depth, double width, double height)
        {
            return depth * width * height;
        }



        //method GetFinalPrice - return the biggest price between price based on dimension and weight
        public double GetFinalPrice(double price1, double price2)
        {
            double result = 0;

            if (price1 > price2) { result += price1; }
            else { result += price2; }

            return result;
        }


        //Cargo4You calculation for the price
        public double Cargo4YouCalculation(Cargo4YouModel cargo4YouObj, double dimensionResult, double weight)
        {
            double DimensionResult = dimensionResult;
            double Weight = weight;
            
            //validation
            if (Weight <= 20 && DimensionResult <= 2000)
            {
                //based on dimension
                if (DimensionResult <= 1000)
                {
                    cargo4YouObj.DimensionPrice = 10;
                }
                else if (DimensionResult > 1000 && DimensionResult <= 2000)
                {
                    cargo4YouObj.DimensionPrice = 20;
                }

                //based on weight
                if (Weight <= 2)
                { cargo4YouObj.WeightPrice = 15; }
                else if (Weight > 2 && Weight <= 15)
                { cargo4YouObj.WeightPrice = 18; }
                else if (Weight > 15 && Weight <= 20)
                { cargo4YouObj.WeightPrice = 35; }

                //final price
                cargo4YouObj.FinalPrice = GetFinalPrice(cargo4YouObj.DimensionPrice, cargo4YouObj.WeightPrice);
                
            }
            return cargo4YouObj.FinalPrice;
          
         

        }



       //Call the web api and return total price
        public async Task<double> CallingWebApi(string urlApi, string controlerApi, double Dimension, double Weight)
        {
            double resultApi=0;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(urlApi);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage getData = await client.GetAsync(controlerApi+ "/"+Dimension+"/"+Weight);

                if (getData.IsSuccessStatusCode)
                {
                    string result = getData.Content.ReadAsStringAsync().Result;
                    resultApi = JsonConvert.DeserializeObject<double>(result);
                }
                else
                {
                    Console.WriteLine("Error calling web API");
                 
                }
            
            
            }

            return resultApi;
        }



     }
}


    
