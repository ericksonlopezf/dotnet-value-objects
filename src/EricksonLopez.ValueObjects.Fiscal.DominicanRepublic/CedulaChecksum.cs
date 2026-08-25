// Copyright © Erickson Lopez. MIT License.
using System;

namespace EricksonLopez.ValueObjects.Fiscal.DominicanRepublic;

/// <summary>
/// High-performance, zero-allocation implementation of the Modulo 10 (Luhn variant)
/// check-digit verification algorithm for 11-digit Dominican Cédulas.
/// </summary>
public static class CedulaChecksum
{
    /// <summary>
    /// Validates whether the 11-digit sequence has a valid Modulo 10 check digit.
    /// </summary>
    /// <param name="digits">Exact 11 numeric characters.</param>
    /// <returns><c>true</c> if the check digit matches official JCE/DGII specification; otherwise <c>false</c>.</returns>
    public static bool ValidateCedula(ReadOnlySpan<char> digits)
    {
        if (digits.Length != 11)
        {
            return false;
        }

        ReadOnlySpan<int> cedulaWeights = [1, 2, 1, 2, 1, 2, 1, 2, 1, 2];

        int sum = 0;
        for (int i = 0; i < 10; i++)
        {
            char c = digits[i];
            if (c is < '0' or > '9')
            {
                return false;
            }

            int product = (c - '0') * cedulaWeights[i];
            sum += (product / 10) + (product % 10);
        }

        int remainder = sum % 10;
        int expectedCheckDigit = (10 - remainder) % 10;

        return (digits[10] - '0') == expectedCheckDigit;
    }
}


