// Copyright © Erickson Lopez. MIT License.
using System;

namespace EricksonLopez.ValueObjects.Fiscal.DominicanRepublic;

/// <summary>
/// High-performance, zero-allocation implementation of the Modulo 10 check-digit
/// verification algorithm for 9-digit Dominican Social Security Numbers (NSS - Tesorería de la Seguridad Social).
/// </summary>
public static class NssChecksum
{
    private static readonly int[] Weights = [1, 2, 1, 2, 1, 2, 1, 2];

    /// <summary>
    /// Validates whether the 9-digit sequence has a valid TSS Modulo 10 check digit.
    /// </summary>
    /// <param name="digits">Exact 9 numeric characters.</param>
    /// <returns><see langword="true"/> if the check digit matches official TSS specification; otherwise <see langword="false"/>.</returns>
    public static bool ValidateNss(ReadOnlySpan<char> digits)
    {
        if (digits.Length != 9)
        {
            return false;
        }

        int sum = 0;
        for (int i = 0; i < 8; i++)
        {
            char c = digits[i];
            if (c is < '0' or > '9')
            {
                return false;
            }

            int product = (c - '0') * Weights[i];
            sum += product > 9 ? (product / 10) + (product % 10) : product;
        }

        int remainder = sum % 10;
        int expectedDigit = (10 - remainder) % 10;

        return (digits[8] - '0') == expectedDigit;
    }
}
