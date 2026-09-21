// Copyright © Erickson Lopez. MIT License.
using System;

namespace EricksonLopez.ValueObjects.Fiscal.DominicanRepublic;

/// <summary>
/// Official cost and expense classification categories for DGII Formato 606 (Norma 07-2018).
/// </summary>
public enum DgiiExpenseType
{
    /// <summary>01 - Personnel Expenses (Gastos de Personal).</summary>
    PersonnelExpenses = 1,

    /// <summary>02 - Work, Supplies and Services Expenses (Gastos por Trabajos, Suministros y Servicios).</summary>
    WorkSuppliesAndServices = 2,

    /// <summary>03 - Leasing / Rentals (Arrendamientos).</summary>
    Leasing = 3,

    /// <summary>04 - Fixed Asset Expenses (Gastos de Activos Fijos).</summary>
    FixedAssetExpenses = 4,

    /// <summary>05 - Representation Expenses (Gastos de Representación).</summary>
    RepresentationExpenses = 5,

    /// <summary>06 - Other Admitted Deductions (Otras Deducciones Admitidas).</summary>
    OtherAdmittedDeductions = 6,

    /// <summary>07 - Financial Expenses (Gastos Financieros).</summary>
    FinancialExpenses = 7,

    /// <summary>08 - Extraordinary Expenses (Gastos Extraordinarios).</summary>
    ExtraordinaryExpenses = 8,

    /// <summary>09 - Cost of Sales Purchases and Expenses (Compras y Gastos que forman parte del Costo de Venta).</summary>
    CostOfSalesPurchasesAndExpenses = 9,

    /// <summary>10 - Asset Acquisitions (Adquisiciones de Activos).</summary>
    AssetAcquisitions = 10,

    /// <summary>11 - Insurance Expenses (Gastos de Seguros).</summary>
    InsuranceExpenses = 11
}
