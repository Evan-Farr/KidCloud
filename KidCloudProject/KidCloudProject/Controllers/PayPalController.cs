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
                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority +
                                "/Paypal/PaymentWithPayPal?";

                    var guid = Convert.ToString((new Random()).Next(100000));

                    var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid);

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
            ResetBalanceFromSuccess();
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
            
            var payer = new Payer() { payment_method = "paypal" };

            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl,
                return_url = redirectUrl
            };

            var amount = new Amount()
            {
                currency = "USD",
                total = customer.MoneyOwed.ToString(),
            };

            var transactionList = new List<Transaction>();

            transactionList.Add(new Transaction()
            {
                description = customer.FirstName + " " + customer.LastName + " payment for child care services. " + todaysDate + "",
                invoice_number = customer.Id + "-INV" + Guid.NewGuid().ToString(),
                amount = amount,
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

        public void ResetBalanceFromSuccess()
        {
            var user = User.Identity.GetUserId();
            Parent parent = db.Parents.Where(a => a.UserId.Id == user).FirstOrDefault();
            parent.MoneyOwed = 0M;
            db.SaveChanges();
        }

    }
}