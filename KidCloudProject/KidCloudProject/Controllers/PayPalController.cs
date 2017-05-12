using PayPal.Api;
using KidCloudProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace KidCloudProject.Controllers
{
    public class PaypalController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        // GET: PayPal
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PaymentWithPaypal()
        {
            //getting the apiContext as earlier
            APIContext apiContext = Configuration.GetAPIContext();

            try
            {
                string payerId = Request.Params["PayerID"];

                if (string.IsNullOrEmpty(payerId))
                {
                    //this section will be executed first because PayerID doesn't exist
                    //it is returned by the create function call of the payment class

                    // Creating a payment
                    // baseURL is the url on which paypal sendsback the data.
                    // So we have provided URL of this controller only
                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority +
                                "/Paypal/PaymentWithPayPal?";

                    //guid we are generating for storing the paymentID received in session
                    //after calling the create function and it is used in the payment execution

                    var guid = Convert.ToString((new Random()).Next(100000));

                    //CreatePayment function gives us the payment approval url
                    //on which payer is redirected for paypal account payment

                    var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid);

                    //get links returned from paypal in response to Create function call

                    var links = createdPayment.links.GetEnumerator();

                    string paypalRedirectUrl = null;

                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;

                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            //saving the payapalredirect URL to which user will be redirected for payment
                            paypalRedirectUrl = lnk.href;
                        }
                    }

                    // saving the paymentID in the key guid
                    Session.Add(guid, createdPayment.id);

                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    // This section is executed when we have received all the payments parameters

                    // from the previous call to the function Create

                    // Executing a payment

                    var guid = Request.Params["guid"];

                    var executedPayment = ExecutePayment(apiContext, payerId, Session[guid] as string);

                    if (executedPayment.state.ToLower() != "approved")
                    {
                        return View("FailureView");
                    }
                }
            }
            catch (Exception)
            {
                return View("FailureView");
            }

            return View("SuccessView");
        }

        private PayPal.Api.Payment payment;

        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution() { payer_id = payerId };
            this.payment = new Payment() { id = paymentId };
            return this.payment.Execute(apiContext, paymentExecution);
        }

        private Payment CreatePayment(APIContext apiContext, string redirectUrl)
        {
            var user = User.Identity.GetUserId();
            var customer = db.Parents.Where(p => p.UserId.Id == user).First();
            DateTime date = DateTime.Now;
            string todaysDate = date.ToLongDateString();
            //similar to credit card create itemlist and add item objects to it
            //var itemList = new ItemList() { items = new List<Item>() };

            //itemList.items.Add(new Item()
            //{
            //    name = "DayCare Account Balance",
            //    currency = "USD",
            //    price = "25",
            //    quantity = "1",
            //    //sku = "sku"
            //});

            var payer = new Payer() { payment_method = "paypal" };

            // Configure Redirect Urls here with RedirectUrls object
            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl,
                return_url = redirectUrl
            };

            // similar as we did for credit card, do here and create details object
            //var details = new Details()
            //{
            //    tax = "1",
            //    shipping = "1",
            //    subtotal = "5"
            //};

            // similar as we did for credit card, do here and create amount object
            var amount = new Amount()
            {
                currency = "USD",
                total = customer.MoneyOwed.ToString(), // Total must be equal to sum of shipping, tax and subtotal.
                //details = details
            };

            var transactionList = new List<Transaction>();

            transactionList.Add(new Transaction()
            {
                description = customer.FirstName + " " + customer.LastName + " payment for child care services. " + todaysDate + "",
                invoice_number = customer.Id + "-INV" + Guid.NewGuid().ToString(),
                amount = amount,
                //item_list = itemList
            });

            this.payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };

            // Create a payment using a APIContext
            return this.payment.Create(apiContext);

        }

    }
}