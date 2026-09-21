// Copyright © Erickson Lopez. MIT License.
using System;

namespace EricksonLopez.ValueObjects.Fiscal.DominicanRepublic;

/// <summary>
/// DGII taxpayer classification for Dominican Republic fiscal reporting purposes.
/// Determines applicable fiscal obligations and permitted document types.
/// </summary>
public enum DgiiClassification
{
    /// <summary>Legal entity with RNC (Persona Jurídica).</summary>
    PersonaJuridica = 1,

    /// <summary>Individual with Cédula (Persona Física).</summary>
    PersonaFisica = 2,

    /// <summary>Government entity (exento de ITBIS).</summary>
    Gubernamental = 3,

    /// <summary>Special taxation regime (Zona Franca, PROINDUSTRIA).</summary>
    RegimenesEspeciales = 4,

    /// <summary>Informal taxpayer (sin RNC, proveedores informales).</summary>
    ContribuyenteInformal = 5
}
