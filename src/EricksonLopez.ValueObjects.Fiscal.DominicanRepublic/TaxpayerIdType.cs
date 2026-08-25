// Copyright © Erickson Lopez. MIT License.
using System;

namespace EricksonLopez.ValueObjects.Fiscal.DominicanRepublic;

/// <summary>
/// Specifies the type of Dominican taxpayer identity.
/// </summary>
public enum TaxpayerIdType
{
    /// <summary>Registro Nacional de Contribuyentes (9 digits for legal entities / business entities).</summary>
    Rnc = 1,

    /// <summary>Cédula de Identidad y Electoral (11 digits for natural persons).</summary>
    Cedula = 2
}


