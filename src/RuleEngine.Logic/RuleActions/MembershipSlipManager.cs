using System.Threading.Tasks;
using RuleEngine.Domain.Models;
using RuleEngine.Logic.DbContext;
using System.Collections.Generic;
using RuleEngine.Domain.RequestResponseDto;

namespace RuleEngine.Logic.RuleActions
{
    internal class MembershipSlipManager
    {
        private readonly AfterPaymentExecutionRequest _request;
        private readonly CustomersCollection _customersCollection;
        private readonly bool _isUpgradeRequest;


        private bool isValidRequest;
        private bool membershipActivated;
        private bool membershipUpgraded;
        private CustomerDetails customerDetails;

        public MembershipSlipManager(AfterPaymentExecutionRequest request, AllProducts products, bool isUpgradeRequest)
        {
            _request = request;
            _isUpgradeRequest = isUpgradeRequest;
            _customersCollection = new CustomersCollection();
        }

        public async Task<List<string>> Create()
        {
            return await Task.Run(() =>
            {
                return ValidateReqquest()
                        .GetCustomerDetails()
                        .ActivateOrUpgradeMembership()
                        .SentEmail();
            });
        }

        private MembershipSlipManager ActivateOrUpgradeMembership()
        {
            if (!this.isValidRequest)
                return this;

            if (this.customerDetails == null || string.IsNullOrEmpty(this.customerDetails.CustomerEmail))
            {
                this.isValidRequest = false;
                return this;
            }

            //For simplicty and avoid any DB transaction considering that status update for that user as active member is solve the purpose
            var customer = _customersCollection.GetCustomer(_request.CustomerId);
            this.customerDetails.IsActiveMember = customer != null ? true : false; // funny logic 
            this.membershipActivated = true;
            if (_isUpgradeRequest)
            {
                // Upgrade membership
                this.customerDetails.MembershipSlot = 2;  // Say membership status is 2 Can do this using enum
                this.membershipUpgraded = true;
            }

            return this;
        }

        private MembershipSlipManager ValidateReqquest()
        {
            this.isValidRequest = _request != null && _request.CustomerId > 0;
            return this;
        }

        private MembershipSlipManager GetCustomerDetails()
        {
            if (!this.isValidRequest)
                return this;

            this.customerDetails = _customersCollection.GetCustomer(_request.CustomerId);
            return this;
        }

        private List<string> SentEmail()
        {
            if (!this.isValidRequest)
                return null;

            if (this.customerDetails == null || string.IsNullOrEmpty(this.customerDetails.CustomerEmail))
            {
                this.isValidRequest = false;
                return null;
            }

            // Didn't implemented any sent email functionality for simplicity

            var msg = (_isUpgradeRequest ? "Account upgraded and email sent to the user " : "Account activated and email sent to user ") + customerDetails.CustomerName;
            return new List<string>() { msg };
        }
    }
}
