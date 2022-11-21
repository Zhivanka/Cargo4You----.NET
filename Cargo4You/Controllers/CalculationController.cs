
﻿using Cargo4You.Models;
using Cargo4You.Data;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IO;

namespace Cargo4You.Controllers
{
    public class CalculationController : Controller
    {

        public CalculationController()
        {
 
        }
        //Takes the input data from the client, call all the methotds for validate and calulate
        //Returns a view with model dictionary results
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Result(InputDataModel obj)
        {

            
            if (ModelState.IsValid)
            {
                //Take all the data from the client input
                double Depth = Convert.ToDouble(obj.Depth);
                double Width = Convert.ToDouble(obj.Width);
                double Height = Convert.ToDouble(obj.Height);
                double Weight = Convert.ToDouble(obj.Weight);

                //calculate volume of the parcel
                double DimensionResult = CalculateVolume(Depth, Width, Height);
                
                //Create list of partners, read all the data from json file with GetCargoObject metod 
                List<CargoModel>? partners = GetCargoObject();

                //All the results goes in Dictionary (name, price) - we want to retrieve name and price in the view
                Dictionary<string, double> results = new();

                //first check if the partner list is not empty
                if (partners != null)
                {
                    //go throu all the partners, make the validation&calculation put the result in the dictionary 
                    foreach (var partner in partners)
                    {

                        //validation
                        bool isValidWeight = false;
                        bool isValidDimension = false;
                        for (int i = 0; i < partner.Validations.Length; i ++)
                        {
                            string validationString = "";
                            if (partner.Validations[i].Contains("kg"))
                            {
                              validationString = partner.Validations[i].Split("kg").First();
                              isValidWeight = Validate(Weight, validationString);
                            }
                            else if (partner.Validations[i].Contains("cm"))
                            {
                                validationString = partner.Validations[i].Split("cm").First();
                                isValidDimension = Validate(DimensionResult, validationString);
                            }

                        }
 
                       
                        //if is valid kg and dimension, call calculate and add results to the dictionary
                        if (isValidWeight && isValidDimension)
                        {
                            //make the calculation to get weight price
                            double WeightPrice = 0;
                            foreach (var e in partner.CalculationsWeight)
                            {
                              WeightPrice = Calculate(e.Key, e.Value, Weight);
                              if (WeightPrice != 0) { break; }
                            }

                            //make the calculation to get dimenstion price
                            double DimensionPrice = 0;
                            foreach (var e in partner.CalculationsDimension)
                            {
                                DimensionPrice = Calculate(e.Key, e.Value, DimensionResult);
                                if (DimensionPrice != 0) { break; }
                            }

                            //get the bigger price comparing the DimensionPrice and WeightPrice
                            double Price = GetFinalPrice(DimensionPrice, WeightPrice);
                            results.Add(partner.Name, Price);
                        }
                    }

                    //sort the result lower to biggest price
                    var sortedList = results.ToList();
                    sortedList.Sort((pair1, pair2) => pair1.Value.CompareTo(pair2.Value));


                    //return a view, bind the view with the dictionary - we want to retrieve name and price in the view
                    return View(sortedList);
                }
                else
                    Console.WriteLine("no results");
            }

            //if the form is not regulary fulfilled
            return RedirectToAction("Index", "Home");
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


        //validation
        public bool Validate( double weightOrDimension, string stringToValidate)
        {
            string valueToCompare;
            double dblvalueToCompare;

            if (stringToValidate.Contains("<="))
            {
                valueToCompare = stringToValidate.Split("<=").Last();
                dblvalueToCompare = double.Parse(valueToCompare);
                
                int resultValue = Math.Sign(weightOrDimension.CompareTo(dblvalueToCompare));
                if (resultValue == -1){return true;}
                else if (resultValue == 0){return true;}
                else if (resultValue == 1){return false;}
            }

            else if (stringToValidate.Contains(">="))
            {
                valueToCompare = stringToValidate.Split(">=").Last();
                dblvalueToCompare = double.Parse(valueToCompare);

                int resultValue = Math.Sign(weightOrDimension.CompareTo(dblvalueToCompare));
                if (resultValue == -1){return false;}
                else if (resultValue == 0){return true;}
                else if (resultValue == 1){return true;}
            }

            else if (stringToValidate.Contains('<'))
            {
                valueToCompare = stringToValidate.Split('<').Last();
                dblvalueToCompare = double.Parse(valueToCompare);

                int resultValue = Math.Sign(weightOrDimension.CompareTo(dblvalueToCompare));
                if (resultValue == -1){return true;}
                else if (resultValue == 0){return false;}
                else if (resultValue == 1){return false;}
            }

            else if (stringToValidate.Contains('>'))
            {
                valueToCompare = stringToValidate.Split('>').Last();
                dblvalueToCompare = double.Parse(valueToCompare);

                int resultValue = Math.Sign(weightOrDimension.CompareTo(dblvalueToCompare));
                if (resultValue == -1){return false;}
                else if (resultValue == 0){return false;}
                else if (resultValue == 1){return true;}
            }


           return false;
        }



        //calculate the result
        public double Calculate(string calculateString, string calculationPrice, double weightOrDimension)
        {
         
            double price;

                //if contains && we should check 2 cases
                if (calculateString.Contains("&&"))
                {

                    string[] b = calculateString.Split("&&");
                    bool isValid1 = Validate(weightOrDimension, b[0]);
                    bool isValid2 = Validate(weightOrDimension, b[1]);
                    if (isValid1 && isValid2)
                    {
                        price = double.Parse(calculationPrice);
                        return price;
                    }

                }

                //if contains + make the calculation for extra charge "if bigger then.."
                else if (calculationPrice.Contains('+'))
                { 
                   double extraCharge=double.Parse(calculationPrice.Split('+').Last());
                   bool isValid = Validate(weightOrDimension, calculateString);
                    if (isValid)
                    {
                        string stringMaxPrice = calculateString.Split().Last();
                        string stringPrice = calculationPrice.Split('+').First();
                        price = double.Parse(stringPrice);
                        double maxKg = double.Parse(calculateString.Split('>').Last());
                        double diferenceForExtraCharge = weightOrDimension - maxKg;
                        double finalResult = 0;
                        if (diferenceForExtraCharge < 1)
                        { finalResult = price + 0; }
                        else
                        { finalResult = price + (diferenceForExtraCharge * extraCharge); }

                        return finalResult;

                    }

                }

                //if neither of the above is true just validate the case and return the price
                else
                {
                    bool isValid = Validate(weightOrDimension, calculateString);
                    if (isValid)
                    {
                        price = double.Parse(calculationPrice);
                        return price;
                    }

                }
            
            return 0;
        }


        //reads cargo objects from json file, deserialize them into cargoModel objects
        public List<CargoModel>? GetCargoObject()
        {
            List<CargoModel>? cargoListObj = new ();
            using (StreamReader reader = new("Data/PartnersData.json"))
            {
                string json = reader.ReadToEnd();
                cargoListObj = JsonConvert.DeserializeObject<List<CargoModel>>(json);

                if(cargoListObj!=null)
                return cargoListObj;


            }

            return null;

        }





        
       

        }
    }


    



     


    
