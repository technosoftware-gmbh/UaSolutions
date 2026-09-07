#region Copyright (c) 2022-2026 Technosoftware GmbH. All rights reserved
//-----------------------------------------------------------------------------
// Copyright (c) 2022-2026 Technosoftware GmbH. All rights reserved
// Web: https://technosoftware.com 
//
// The Software is based on https://github.com/junian/Standard.Licensing. 
// The complete license agreement for that can be found in this directore in the LICENSE.txt file.
//-----------------------------------------------------------------------------
#endregion Copyright (c) 2022-2026 Technosoftware GmbH. All rights reserved

#region Using Directives
using System;
using System.Collections.Generic;
#endregion Using Directives

namespace Technosoftware.UaUtilities
{
    internal class ValidationChainBuilder : IStartValidationChain, IValidationChain
    {
        private readonly List<ILicenseValidator> m_validators;
        private ILicenseValidator m_currentValidatorChain;
        private readonly License m_license;

        public ValidationChainBuilder(License license)
        {
            m_license = license;
            m_validators = [];
        }

        public ILicenseValidator StartValidatorChain()
        {
            return m_currentValidatorChain = new LicenseValidator();
        }

        public void CompleteValidatorChain()
        {
            if (m_currentValidatorChain == null)
            {
                return;
            }

            m_validators.Add(m_currentValidatorChain);
            m_currentValidatorChain = null;
        }

        public ICompleteValidationChain When(Predicate<License> predicate)
        {
            m_currentValidatorChain.ValidateWhen = predicate;
            return this;
        }

        public IStartValidationChain And()
        {
            CompleteValidatorChain();
            return this;
        }

        public IEnumerable<IValidationFailure> AssertValidLicense()
        {
            CompleteValidatorChain();

            foreach (ILicenseValidator validator in m_validators)
            {
                if (validator.ValidateWhen != null && !validator.ValidateWhen(m_license))
                {
                    continue;
                }

                if (!validator.Validate(m_license))
                {
                    yield return validator.FailureResult
                        ?? new GeneralValidationFailure
                        {
                            Message = "License validation failed!"
                        };
                }
            }
        }
    }
}
