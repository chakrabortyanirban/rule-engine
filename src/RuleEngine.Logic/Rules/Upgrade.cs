﻿using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using RuleEngine.Domain;
using RuleEngine.Domain.RequestResponseDto;
using RuleEngine.Logic.RuleActions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RuleEngine.Logic.Rules
{
    public class Upgrade : RuleContext, IPackingSlip
    {
        private readonly MembershipSlipManager _packingSlipManager;

        public Upgrade(AfterPaymentExecutionRequest request, IWebHostEnvironment webHostEnvironment)
        {
            _packingSlipManager = new MembershipSlipManager(request, true);
        }

        public string PackingSlipTemplate { get { return "SlipTemplates/Membership.html"; } } // not in use in this scenario

        public override async Task<AfterPaymentExecutionResponse> ExecuteAction()
        {
            var response = new AfterPaymentExecutionResponse
            {
                SlipHtml = await GeneratePackaingSlip(false)
            };
            return response;
        }

        public async Task<List<string>> GeneratePackaingSlip(bool requiredDuplicate)
        {
            return await _packingSlipManager.Create();
        }
    }
}
