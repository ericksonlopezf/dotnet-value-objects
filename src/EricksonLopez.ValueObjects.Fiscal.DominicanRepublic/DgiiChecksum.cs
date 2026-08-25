// Copyright © Erickson Lopez. MIT License.
using System;

namespace EricksonLopez.ValueObjects.Fiscal.DominicanRepublic;

/// <summary>
/// High-performance, zero-allocation implementation of the DGII Modulo 11
/// check-digit verification algorithm for 9-digit Dominican RNCs.
/// </summary>
public static class DgiiChecksum
{
    /// <summary>
    /// Validates whether the 9-digit sequence has a valid DGII Modulo 11 check digit.
    /// </summary>
    /// <param name="digits">Exact 9 numeric characters.</param>
    /// <returns><see langword="true"/> if the check digit matches DGII specification; otherwise, <see langword="false"/>.</returns>
    public static bool ValidateRnc(ReadOnlySpan<char> digits)
    {
        if (digits.Length != 9)
        {
            return false;
        }

        ReadOnlySpan<int> rncWeights = [7, 9, 8, 6, 5, 4, 3, 2];

        int sum = 0;
        for (int i = 0; i < 8; i++)
        {
            int digitValue = digits[i] - '0';
            if (digitValue is < 0 or > 9)
            {
                return false;
            }

            sum += digitValue * rncWeights[i];
        }

        int remainder = sum % 11;
        int expectedCheckDigit = remainder switch
        {
            0 => 2,
            1 => 1,
            _ => 11 - remainder
        };

        return (digits[8] - '0') == expectedCheckDigit;
    }
}



