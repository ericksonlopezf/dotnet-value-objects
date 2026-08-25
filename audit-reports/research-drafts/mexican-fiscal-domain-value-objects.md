# **Informe de Auditoría y Diseño Arquitectónico de Value Objects para el Sistema Fiscal Mexicano en .NET 10**

## **Marco Normativo, Principios de Modelado y Taxonomía Fiscal**

El desarrollo de un ecosistema de software fiscal para México requiere un rigor formal superior al de los sistemas comerciales convencionales. La interacción con la autoridad tributaria, el Servicio de Administración Tributaria (SAT), está regida por especificaciones técnicas estrictas, leyes federales, resoluciones misceláneas y un ecosistema de catálogos dinámicos que sufren actualizaciones constantes1. Para diseñar un modelo de dominio tributario robusto, mantenible y preparado para producción en .NET 10, es imperativo establecer fronteras claras mediante la metodología Domain-Driven Design (DDD), combatiendo tanto la obsesión por los tipos primitivos (*Primitive Obsession*) como el sobre-modelado artificial (*Over-modeling*).

### **Fuentes Normativas Vigentes y Reglas Fiscales**

El modelo de dominio diseñado en este informe se deriva de las siguientes fuentes oficiales vigentes en el ordenamiento jurídico y técnico mexicano:

* **Código Fiscal de la Federación (CFF)**: Artículos 29 y 29-A, los cuales establecen las obligaciones de expedición de Comprobantes Fiscales Digitales por Internet (CFDI), requisitos de los comprobantes, reglas de validación de identidad del receptor, plazos de cancelación y la obligación de conservar la contabilidad electrónica2.  
* **Resolución Miscelánea Fiscal (RMF) y Anexo 20**: Define las reglas de carácter general y la especificación técnica oficial del CFDI en su versión 4.0, incluyendo la estructura XML, secuencia de cadena original, matrices de errores y reglas de validación1.  
* **Leyes Impositivas Federales**: Ley del Impuesto sobre la Renta (LISR), Ley del Impuesto al Valor Agregado (LIVA) y Ley del Impuesto Especial sobre Producción y Servicios (LIEPS), regulando las tasas, cuotas, bases gravables, exenciones y mecanismos de retención3.  
* **Reglas Generales de Comercio Exterior (RGCE) y Anexo 22**: Instrucciones de llenado e integración de pedimentos aduanales de importación y exportación, estructuras de patentes, aduanas y fraccionamiento arancelario6.  
* **Anexos Técnicos de Complementos CFDI**: Especificaciones para el Complemento de Recepción de Pagos (REP 2.0), Nómina (1.2), Carta Porte (3.1), Comercio Exterior (2.0) y Retenciones e Información de Pagos (2.0)1.

## **Entregable A: Taxonomía Completa de Conceptos del Fiscal Domain**

Para evitar convertir automáticamente cada tipo primitivo (string, decimal, DateTime, Guid) en un Value Object, se analiza el catálogo global de conceptos del ecosistema tributario mexicano. Cada concepto se clasifica dentro de la taxonomía del DDD según sus características semánticas y operativas.

| Concepto Fiscal | Clasificación DDD | Justificación Arquitectónica y Semántica |
| :---- | :---- | :---- |
| **Registro Federal de Contribuyentes (RFC)** | Value Object | Inmutable, igualdad por valor, invariantes de estructura (12/13 caracteres) y algoritmo de homoclave1. |
| **Clave Única de Registro de Población (CURP)** | Value Object | Encapsula algoritmo RENAPO de 18 caracteres alfanuméricos con digito verificador11. |
| **Contribuyente / Emisor / Receptor** | Aggregate / Entity | Posee identidad de negocio propia a lo largo del tiempo, estado mutable y ciclo de vida1. |
| **Comprobante Fiscal Digital por Internet (CFDI)** | Aggregate Root | Unidad de consistencia transaccional. Agrupa conceptos, impuestos, sellos y complementos1. |
| **Fiscal Folio (UUID)** | Value Object | Encapsula el identificador único universal de 36 caracteres otorgado en el timbrado conforme a RFC 412210. |
| **Serie y Folio Interno** | Value Object | Separados en DocumentSeries y DocumentFolio. Poseen reglas de sanitización de caracteres invisibles. |
| **Timbre Fiscal Digital (TFD)** | Value Object (Integration) | Agrupa el resultado del timbrado por el PAC: UUID, sello SAT, sello CFDI, fecha de timbrado y RFC de PAC5. |
| **Certificado de Sello Digital (CSD)** | Entity / Infraestructura | Posee número de serie, par de llaves criptográficas, periodo de vigencia y estado de revocación (LCR). |
| **e.firma (Firma Electrónica Avanzada)** | Entity / Infraestructura | Entidad criptográfica de persona física o moral utilizada para trámites y firmado de declaraciones, no para CFDI. |
| **Proveedor Autorizado de Certificación (PAC)** | Integration Actor | En el dominio sólo se modela como identificador PacRfc. La infraestructura de red permanece aislada. |
| **Sello Digital / Sello SAT** | Value Object | Firma criptográfica Base64 inmutable derivada de una cadena original y una llave privada. |
| **Cadena Original** | Value Object | Cadena de texto formateada bajo reglas XSLT del SAT que constituye el insumo del sello5. |
| **Catálogos Dinámicos SAT (c\_ClaveProdServ, etc.)** | Catalog Code (SatCatalogCode\<T\>) | Códigos sujetos a mutación normativa externa. No deben ser enums cerrados de C\#1. |
| **Voucher Type / Objeto de Impuesto** | Enum | Tipos fijos e inmutables definidos por el protocolo Anexo 20 (I, E, T, N, P y 01, 02, 03, 04\)1. |
| **Monto Monetario (MonetaryAmount)** | Value Object | Encapsula un valor decimal de alta precisión y su divisa CurrencyCode (ISO 4217). |
| **Tasa de Impuesto (TaxRate)** | Value Object | Porcentaje aplicable a una base gravable, acotado a la matriz c\_TasaOCuota del SAT (hasta 6 decimales)2. |
| **Cuota de Impuesto (TaxQuota)** | Value Object | Valor monetario fijo por unidad de medida consumida (exclusivo de IEPS). |
| **Pedimento Aduanal (PedimentoNumber)** | Value Object | Estructura de 15 dígitos bajo la regla del Anexo 22 de las RGCE (Año, Aduana, Patente, Consecutivo)6. |
| **Identificador Carta Porte (IdCcp)** | Value Object | Estructura UUID v4 con prefijo obligatorio CCC conforme al estándar Carta Porte 3.110. |
| **Número de Seguridad Social (NSS)** | Value Object | Cadena de 11 dígitos con validación de algoritmo Modulo 10 de la Ley del Seguro Social. |
| **Fracción Arancelaria (TariffFraction)** | Value Object | Código numérico de 8 o 10 dígitos (NICO) de la clasificación aduanera de mercancías. |
| **Póliza Contable** | Aggregate Root | Unidad transaccional del subdominio de Contabilidad Electrónica. |
| **Cuenta Contable (AccountNumber)** | Value Object | Cadena con formato jerárquico amparado por el catálogo código agrupador del SAT. |
| **Declaración Fiscal** | Aggregate Root | Entidad con estado (normal, complementaria, borrador, presentada) y saldo a favor o a cargo. |
| **Motivo de Cancelación** | Catalog Code / Policy | Clave del catálogo c\_MotivoCancelacion vinculada con la política de sustitución. |
| **Addenda** | Integration DTO | Estructura comercial no fiscal sin validez tributaria ante el SAT. Queda fuera del dominio fiscal. |

## **Entregable B: Catálogo Maestro de Value Objects Aprobados**

A continuación se detalla cada Value Object aprobado para el núcleo del dominio fiscal mexicano. Todos los Value Objects cumplen con los principios de inmutabilidad, igualdad semántica, validación en construcción mediante el patrón Result\<T\> y optimización para el runtime de .NET 10\.

### **1\. Rfc (Registro Federal de Contribuyentes)**

* **Dominio/Contexto**: EricksonLopez.Fiscal.Mexico  
* **Propósito Fiscal**: Identificar unívocamente a personas físicas y morales ante el SAT1.  
* **Estructura y Representación**: readonly record struct de 12 (moral) o 13 (física) caracteres en mayúsculas1.  
* **Invariantes y Validaciones**:  
  * Legal Entity: 3 letras \+ 6 dígitos fecha (AAMMDD) \+ 3 caracteres homoclave7.  
  * Natural Person: 4 letras \+ 6 dígitos fecha (AAMMDD) \+ 3 caracteres homoclave7.  
  * Soporte explícito para RFCs genéricos: XAXX010101000 (Público en General) y XEXX010101000 (Extranjeros)2.  
* **API y Diseño Conceptual**:

C\#  
public readonly record struct Rfc : IParsable\<Rfc\>, ISpanParsable\<Rfc\>  
{  
    public string Value { get; }  
    public TaxpayerType PersonType { get; }  
    public bool IsGeneric { get; }

    public static Result\<Rfc\> Create(string? input);  
    public static Result\<Rfc\> CreatePublicInGeneral();  
    public static Result\<Rfc\> CreateForeigner();  
      
    public static Rfc Parse(string s, IFormatProvider? provider);  
    public static bool TryParse(string? s, IFormatProvider? provider, out Rfc result);  
    public static Rfc Parse(ReadOnlySpan\<char\> s, IFormatProvider? provider);  
    public static bool TryParse(ReadOnlySpan\<char\> s, IFormatProvider? provider, out Rfc result);  
}

### **2\. Curp (Clave Única de Registro de Población)**

* **Dominio/Contexto**: EricksonLopez.Fiscal.Mexico  
* **Propósito Fiscal**: Identificación de personas físicas en el complemento de Nómina y registros de retenciones11.  
* **Estructura y Representación**: readonly record struct de 18 caracteres alfanuméricos en mayúsculas11.  
* **Invariantes y Validaciones**: Cumplimiento del patrón RENAPO: 4 letras \+ 6 dígitos fecha \+ sexo (H/M) \+ 2 letras estado \+ 3 caracteres consonantes internas \+ 2 dígitos verificadores11.

C\#  
public readonly record struct Curp : IParsable\<Curp\>, ISpanParsable\<Curp\>  
{  
    public string Value { get; }  
    public DateOnly BirthDate { get; }  
    public Gender Gender { get; }

    public static Result\<Curp\> Create(string? input);  
    public static bool TryParse(ReadOnlySpan\<char\> input, out Curp result);  
}

### **3\. FiscalUuid (Fiscal Folio del CFDI)**

* **Dominio/Contexto**: EricksonLopez.Cfdi  
* **Propósito Fiscal**: Identificador único universal emitido por el SAT/PAC que otorga validez legal a un CFDI10.  
* **Estructura y Representación**: readonly record struct que envuelve un Guid internamente, exponiendo la cadena normalizada de 36 caracteres en mayúsculas con guiones.  
* **Invariantes y Validaciones**: Cumplimiento estricto de la RFC 4122 (versión 4).

C\#  
public readonly record struct FiscalUuid : IParsable\<FiscalUuid\>, ISpanParsable\<FiscalUuid\>  
{  
    public Guid Value { get; }  
    public string FormattedValue \=\> Value.ToString("D").ToUpperInvariant();

    public static Result\<FiscalUuid\> Create(Guid value);  
    public static Result\<FiscalUuid\> Create(string? input);  
    public static bool TryParse(ReadOnlySpan\<char\> input, out FiscalUuid result);  
}

### **4\. MonetaryAmount (Monto Monetario Unificado)**

* **Dominio/Contexto**: EricksonLopez.SharedKernel  
* **Propósito Fiscal**: Representación de subtotales, totales, bases gravables e importes de impuestos sin pérdida de precisión.  
* **Estructura y Representación**: readonly record struct compuesto por un decimal y un CurrencyCode.  
* **Invariantes y Validaciones**: Redondeo fiscal conforme a las matrices de decimales del Anexo 20 (hasta 6 decimales intermedios, 2 decimales en totales impresos)1. Operaciones aritméticas imponen coincidencia de CurrencyCode.

C\#  
public readonly record struct MonetaryAmount  
{  
    public decimal Value { get; }  
    public CurrencyCode Currency { get; }

    public static Result\<MonetaryAmount\> Create(decimal value, CurrencyCode currency);  
    public MonetaryAmount Add(MonetaryAmount other);  
    public MonetaryAmount Subtract(MonetaryAmount other);  
    public MonetaryAmount RoundToFiscalFactor(int decimals);  
}

### **5\. TaxRate (Tasa de Impuesto)**

* **Dominio/Contexto**: EricksonLopez.Fiscal.Mexico  
* **Propósito Fiscal**: Porcentaje de impuesto trasladado o retenido aplicable a una base gravable (IVA, ISR, IEPS)3.  
* **Estructura y Representación**: readonly record struct que encapsula un decimal de hasta 6 posiciones decimales.  
* **Invariantes y Validaciones**: Rango positivo (0.000000 a 1.000000). Debe validar contra los valores o rangos de la matriz c\_TasaOCuota del SAT2.

C\#  
public readonly record struct TaxRate  
{  
    public decimal Value { get; }  
    public static TaxRate IvaSixteen \=\> new(0.160000m);  
    public static TaxRate IvaZero \=\> new(0.000000m);

    public static Result\<TaxRate\> Create(decimal value);  
}

### **6\. PedimentoNumber (Pedimento Number Aduanal)**

* **Dominio/Contexto**: EricksonLopez.Cfdi.ForeignTrade  
* **Propósito Fiscal**: Acreditar la legal estancia e importación/exportación de mercancías de comercio exterior6.  
* **Estructura y Representación**: readonly record struct de 15 dígitos numéricos continuos internamente6.  
* **Invariantes y Validaciones**: Formato Anexo 22 de las RGCE: 2 dígitos año \+ 2 dígitos aduana \+ 4 dígitos patente \+ 1 dígito del año en curso \+ 6 dígitos consecutivo6. Proporciona formateo gráfico oficial con espacios de separación7.

C\#  
public readonly record struct PedimentoNumber  
{  
    public string RawValue { get; } // 15 dígitos  
    public string FormattedValue \=\> $"{RawValue\[..2\]}  {RawValue.Substring(2,2)}  {RawValue.Substring(4,4)}  {RawValue.Substring(8,1)}{RawValue.Substring(9,6)}"; \[cite: 7, 8\]

    public static Result\<PedimentoNumber\> Create(string? input);  
}

### **7\. IdCcp (Identificador de Carta Porte)**

* **Dominio/Contexto**: EricksonLopez.Cfdi.CartaPorte  
* **Propósito Fiscal**: Identificador obligatorio para el complemento Carta Porte 3.110.  
* **Estructura y Representación**: readonly record struct de 36 caracteres.  
* **Invariantes y Validaciones**: Formato estricto: Prefijo CCC seguido de 5 caracteres hex, guión, 4 hex, guión, 4 hex, guión, 4 hex, guión, 12 hex10.

C\#  
public readonly record struct IdCcp  
{  
    public string Value { get; }  
    public static Result\<IdCcp\> Create(string? input);  
    public static IdCcp NewId();  
}

### **8\. SatCatalogCode\<TCatalog\> (Código Tipado de Catálogo SAT)**

* **Dominio/Contexto**: EricksonLopez.Fiscal.Mexico  
* **Propósito Fiscal**: Encapsular claves de catálogos dinámicos del Anexo 20 (c\_ClaveProdServ, c\_ClaveUnidad, c\_RegimenFiscal, c\_UsoCFDI, c\_FormaPago, c\_MetodoPago) evitando recompilaciones por cambios normativos1.  
* **Estructura y Representación**: readonly record struct genérico con restricción de marca de catálogo.

C\#  
public readonly record struct SatCatalogCode\<TCatalog\> where TCatalog : ISatCatalogMarker  
{  
    public string Code { get; }  
    public static Result\<SatCatalogCode\<TCatalog\>\> Create(string? code);  
}

## **Entregable C: Value Objects Descartados y Justificación Arquitectónica**

Para mantener el núcleo del dominio limpio y evitar el sobre-modelado (*over-modeling*), se rechazaron explícitamente los siguientes conceptos planteados durante la investigación:

> 1. **Amount / Money / TaxAmount**: Descartados y unificados en MonetaryAmount. Mantener clases separadas para importes monetarios generalizados genera fricción de tipos y conversiones redundantes sin agregar invariantes distintas.  
> 2. **TaxpayerId**: Descartado en favor de Rfc. En el dominio fiscal mexicano no existe la abstracción genérica TaxpayerId; la legislación impone el uso exclusivo del RFC para contribuyentes nacionales1.  
> 3. **DocumentId / ElectronicDocumentId / CfdiUuid**: Descartados y fusionados en FiscalUuid. Todos hacen referencia al UUID v4 de la RFC 4122 del Timbre Fiscal Digital10.  
> 4. **Address**: Descartado en favor de FiscalAddress. Una dirección genérica contempla líneas de calle, número interior y referencias que el SAT no valida en la matriz del CFDI 4.0, donde el único requisito del receptor es el código postal (PostalCode)1.  
> 5. **RfcHomoclave**: Descartado como VO independiente. La homoclave es una parte indivisible de la estructura del Rfc7. Separarla destruye la cohesión del concepto.  
> 6. **XmlDocument / CfdiXml**: Descartados. El formato XML es una representación de transporte/infraestructura, no un Value Object del dominio de negocio1.  
> 7. **PacServiceEndpoint**: Descartado. Los endpoints y configuraciones HTTP del PAC pertenecen a la capa de Infraestructura de comunicación SOAP/REST.  
> 8. **CancelationWorkflow**: Descartado como VO. La cancelación es un proceso de negocio o máquina de estados ejecutada mediante un Domain Service o State Machine.

## **Entregable D: Matriz de Auditoría de Duplicidades e Identidades de Dominio**

Se realizó un análisis exhaustivo para identificar y consolidar conceptos duplicados o solapados:

| Concepto A | Concepto B | Decisión Arquitectónica | Regla y Justificación Normativa |
| :---- | :---- | :---- | :---- |
| Rfc | TaxpayerId | **Eliminar TaxpayerId** | El estándar mexicano del CFF exige estrictamente el RFC1. |
| Curp | IdentificationNumber | **Especializar Curp** | La CURP aplica algoritmo de validación RENAPO (18 caracteres)11. IdentificationNumber se usa como primitiva técnica para extranjeros. |
| FiscalUuid | FiscalFolio / Uuid | **Fusionar en FiscalUuid** | Representan exactamente el mismo dato criptográfico de timbrado (RFC 4122\)10. |
| Series | DocumentSeries | **Usar DocumentSeries** | Evita ambigüedad con tipos de datos o colecciones en el lenguaje C\#. |
| Folio | DocumentFolio | **Usar DocumentFolio** | El folio fiscal es el UUID; el folio interno es el consecutivo del emisor. DocumentFolio aclara el propósito. |
| TaxRate | Percentage | **Separar** | Percentage es de uso genérico. TaxRate aplica invariantes de la tabla c\_TasaOCuota del SAT2. |
| TaxRate | TaxQuota | **Mantener separados** | TaxRate es porcentual sobre base gravable3. TaxQuota es un valor en dinero por unidad consumida (ej. IEPS en combustible). |
| ClaveProdServ | ProductCode | **Separar** | ProductCode es el SKU interno del inventario. ClaveProdServ es el código del catálogo c\_ClaveProdServ del SAT1. |
| ClaveUnidad | UnitOfMeasure | **Separar** | UnitOfMeasure es la unidad comercial. ClaveUnidad es la clave ISO/SAT (c\_ClaveUnidad). |
| FiscalPeriod | TaxPeriod | **Fusionar en FiscalPeriod** | Define el intervalo (mes/ejercicio) de cumplimiento tributario. |

## **Entregable E: Matriz de Bounded Contexts y Reutilización de Value Objects**

El modelo de dominio se segmenta en Bounded Contexts autónomos para evitar la contaminación de conceptos entre áreas funcionales.

| Bounded Context | Value Objects Propios | Value Objects Consumidos del Shared Kernel / Base |
| :---- | :---- | :---- |
| **Contribuyente (Taxpayer)** | RegimenFiscal | Rfc, Curp, FiscalAddress, PostalCode1. |
| **Facturación Core (Cfdi)** | FiscalUuid, DocumentSeries, DocumentFolio, TfdSignature | Rfc, MonetaryAmount, CurrencyCode, SatCatalogCode\<T\>1. |
| **Impuestos (Tax)** | TaxRate, TaxQuota, TaxBase | MonetaryAmount, CurrencyCode3. |
| **Carta Porte (CartaPorte)** | IdCcp, VehicleLicensePlate, DriverLicense | PostalCode, SatCatalogCode\<T\>, FiscalUuid10. |
| **Comercio Exterior (ForeignTrade)** | PedimentoNumber, TariffFraction | CurrencyCode, MonetaryAmount, SatCatalogCode\<T\>6. |
| **Nómina (Payroll)** | ImssNumber, SeniorityPeriod | Rfc, Curp, MonetaryAmount, FiscalPeriod11. |
| **Recepción de Pagos (Payments)** | PaymentAmount, OutstandingBalance, InstallmentNumber | FiscalUuid, MonetaryAmount, ExchangeRate3. |
| **Contabilidad Electrónica (Accounting)** | AccountNumber, JournalEntryFolio | FiscalUuid, Rfc, MonetaryAmount, FiscalPeriod. |
| **Cancelación (Cancelation)** | CancelationTicket | FiscalUuid, Rfc, SatCatalogCode\<CancelationReason\>. |

## **Entregables F y G: Arquitectura de Librerías, Distribución de Paquetes y Matriz de Dependencias**

Para asegurar un ensamblado limpio, modular y compatible con Native AOT en .NET 10, se define la siguiente estructura de proyectos:

EricksonLopez.DomainPrimitives (Agnóstico universal, Result\<T\>)  
       │  
       ▼  
EricksonLopez.SharedKernel (MonetaryAmount, CurrencyCode, PostalCode)  
       │  
       ▼  
EricksonLopez.Fiscal.Mexico (Rfc, Curp, TaxRate, SatCatalogCode\<T\>)  
       │  
       ├───────────────────────────────┬──────────────────────────────┐  
       ▼                               ▼                              ▼  
EricksonLopez.Cfdi           EricksonLopez.Payroll          EricksonLopez.Acct  
(FiscalUuid, Core CFDI)      (ImssNumber, Seniority)        (AccountNumber)  
       │  
       ├───────────────────────────────┐  
       ▼                               ▼  
EricksonLopez.CartaPorte     EricksonLopez.ForeignTrade  
(IdCcp, LicensePlate)        (PedimentoNumber)

### **Matriz de Dependencias y Límites de API Pública**

| Paquete / Librería | Dependencias Permitidas | Dependencias Prohibidas | API Pública Expuesta |
| :---- | :---- | :---- | :---- |
| EricksonLopez.DomainPrimitives | Ninguna (System.\* únicamente) | Cualquier paquete del dominio fiscal. | Result\<T\>, Error, IValidationRule. |
| EricksonLopez.SharedKernel | DomainPrimitives | Paquetes específicos de México o CFDI. | MonetaryAmount, CurrencyCode, PostalCode, DateRange. |
| EricksonLopez.Fiscal.Mexico | SharedKernel | Paquetes de CFDI, XML o SDKs del SAT/PAC. | Rfc, Curp, TaxRate, TaxQuota, SatCatalogCode\<T\>1. |
| EricksonLopez.Cfdi | .Fiscal.Mexico, SharedKernel | SDKs de PACs, System.Xml.Serialization. | ComprobanteFiscalDigital, FiscalUuid, DocumentSeries10. |
| EricksonLopez.CartaPorte | .Cfdi, .Fiscal.Mexico | Dominio de Nómina o Contabilidad. | IdCcp, VehicleLicensePlate, DriverLicense10. |
| EricksonLopez.ForeignTrade | .Cfdi, .Fiscal.Mexico | Dominio de Nómina. | PedimentoNumber, TariffFraction6. |

## **Capa Anti-Corrupción (ACL) y Estrategia de Serialización Multicapa**

El dominio central debe mantenerse 100% aislado de los detalles de infraestructura, contratos SOAP del SAT, formatos de transporte JSON y archivos XML1.

### **Capa Anti-Corrupción (ACL)**

La transformación de datos entre los agentes externos y el núcleo del dominio sigue un flujo estricto sin dependencias circulares:

> 1. **Entrada desde el SAT / PAC (Infraestructura)**: El sistema recibe esquemas XML (CFDI 4.0) o respuestas JSON/SOAP de timbrado1. Estos datos se deserializan en DTOs de infraestructura (CfdiXmlDto, TimbreResponseDto).  
> 2. **Mapeo vía ACL (Aplicación / Adapters)**: Los Mappers de la ACL convierten los DTOs en llamadas a los métodos fábrica de los Value Objects (Rfc.Create(), FiscalUuid.Create()). Si alguna validación de invariante falla, la ACL retorna un objeto Result.Failure con errores del dominio.  
> 3. **Procesamiento en Dominio**: Las entidades y agregados ejecutan reglas de negocio operando exclusivamente con Value Objects inmutables.  
> 4. **Generación de Salida (Infraestructura)**: Cuando se requiere emitir un XML para timbrado, el agregador pasa a un CfdiXmlXmlExportAdapter que proyecta la información del dominio a los DTOs anotados con \[XmlElement\] requeridos por el parser XML4.

### **Serialización Multicapa en .NET 10**

| Capa / Target | Mecanismo Técnico | Estrategia de Representación |
| :---- | :---- | :---- |
| **REST APIs (JSON)** | System.Text.Json con JsonConverter\<T\> customizados. | Los Value Objects se serializan como cadenas o números primitivos planos (ej. Rfc \-\> "XAXX010101000"). |
| **Persistencia (PostgreSQL / EF Core)** | ValueConverter\<TModel, TProvider\> en EF Core. | Almacenamiento en columnas primitivas (varchar, numeric, uuid). |
| **Persistencia (Dapper)** | SqlMapper.TypeHandler\<T\> personalizado. | Conversión directa a tipos SQL nativos durante la materialización. |
| **XML SAT / CFDI** | Proyección explícita a DTOs de infraestructura XML. | Generación de atributos XML respetando mayúsculas, minúsculas y formatos exactos del Anexo 202. |

## **Entregable H: Roadmap de Implementación**

El desarrollo del framework fiscal se divide en cuatro fases consecutivas:

> 1. **Fase 1: Abstracciones Base y Núcleo Fiscal (Sprints 1 \- 2\)**  
   * Implementación de EricksonLopez.DomainPrimitives y EricksonLopez.SharedKernel.  
   * Construcción e inmutabilidad de Rfc, Curp, FiscalAddress, PostalCode, MonetaryAmount y CurrencyCode1.  
   * Mapeadores de validación de invariantes con pruebas unitarias exhaustivas.  
> 2. **Fase 2: Motor de Impuestos y Catálogos SAT (Sprints 3 \- 4\)**  
   * Desarrollo de SatCatalogCode\<T\> y servicios de caché de catálogos en caliente.  
   * Implementación de TaxRate, TaxQuota, TaxBase y calculadores impositivos para IVA, ISR e IEPS3.  
> 3. **Fase 3: Bounded Context CFDI 4.0 Core (Sprints 5 \- 6\)**  
   * Modelado de Aggregate Root ComprobanteFiscalDigital, FiscalUuid, DocumentSeries y DocumentFolio10.  
   * Implementación de la Capa Anti-Corrupción (ACL) para XML Anexo 20 y timbrado PAC1.  
> 4. **Fase 4: Bounded Contexts Especializados (Sprints 7 \- 9\)**  
   * Complemento Carta Porte 3.1 (IdCcp, VehicleLicensePlate)10.  
   * Comercio Exterior 2.0 (PedimentoNumber, TariffFraction)6.  
   * Complemento de Recepción de Pagos 2.0 y Nómina 1.21.

## **Entregable I: Registros de Decisiones Arquitectónicas (ADRs)**

### **ADR-001: Representación de Catálogos SAT mediante SatCatalogCode\<T\>**

* **Estatus**: Aprobado.  
* **Contexto**: El SAT modifica con alta frecuencia las claves de sus catálogos (ej. más de 800 claves añadidas a c\_ClaveProdServ al inicio del año)1. El uso de enum de C\# obligaría a publicar parches de código constantemente.  
* **Decisión**: Modelar los catálogos dinámicos mediante un Value Object genérico SatCatalogCode\<TCatalog\> que valida sintaxis básica y delega la verificación de vigencia a un servicio de infraestructura.  
* **Consecuencias**: El sistema absorbe cambios de catálogos mediante actualizaciones de base de datos sin necesidad de recompilar librerías.

### **ADR-002: Reemplazo de Clases de Montos por MonetaryAmount**

* **Estatus**: Aprobado.  
* **Contexto**: Diversas áreas del código utilizaban Money, Amount y DecimalAmount generando ambigüedad.  
* **Decisión**: Unificar la representación de valores monetarios en MonetaryAmount, acoplando un decimal y su CurrencyCode.  
* **Consecuencias**: Prevención de errores por operaciones aritméticas entre distintas divisas sin conversión de tipo de cambio.

### **ADR-003: Eliminación de Entidades XML/XSD del Modelo de Dominio**

* **Estatus**: Aprobado.  
* **Contexto**: Las clases generadas desde esquemas XSD del SAT contienen atributos de serialización que contaminan las reglas de negocio.  
* **Decisión**: Prohibir clases XSD en el Dominio. Utilizar una Capa Anti-Corrupción (ACL) en Infraestructura para transformar el dominio a XML4.  
* **Consecuencias**: Aislamiento del dominio ante cambios técnicos en el formato del XML del SAT.

### **ADR-004: Adopción de readonly record struct en .NET 10**

* **Estatus**: Aprobado.  
* **Contexto**: El procesamiento masivo de comprobantes fiscales genera alta presión sobre el Garbage Collector.  
* **Decisión**: Definir los Value Objects con tamaño menor o igual a 64 bytes como readonly record struct.  
* **Consecuencias**: Asignación en el Stack, cero asignaciones innecesarias en el Heap (*Zero-Allocation*) y compatibilidad con Native AOT.

### **ADR-005: Especialización del Pedimento Number Aduanal (PedimentoNumber)**

* **Estatus**: Aprobado.  
* **Contexto**: Representar el pedimento como una cadena simple permite la entrada de valores inválidos que son rechazados en el despacho aduanero.  
* **Decisión**: Crear el Value Object PedimentoNumber que valida la estructura exacta de 15 dígitos según la regla del Anexo 22 de las RGCE (Año, Aduana, Patente, Consecutivo)6.  
* **Consecuencias**: Detección de errores de captura previa a la transmisión aduanera.

## **Entregable J: Matriz de Riesgos Arquitectónicos y Mitigaciones**

| Riesgo Arquitectónico | Impacto | Probabilidad | Mitigación en el Diseño |
| :---- | :---- | :---- | :---- |
| **Acoplamiento Directo al SAT/PAC** | Alto | Alta | Capa Anti-Corrupción (ACL) estricta. El dominio no conoce clases XML ni contratos SOAP4. |
| **Obsesión por Tipos Primitivos (*Primitive Obsession*)** | Medio | Alta | Encapsulación de tipos con invariantes y validaciones de fábrica (Rfc, FiscalUuid, PedimentoNumber)6. |
| **Sobre-Modelado (*Over-Modeling*)** | Medio | Media | Descarte formal de VOs innecesarios (ej. XmlDocument, PacServiceEndpoint). |
| **Inconsistencias de Redondeo Decimal** | Alto | Alta | Definición de MonetaryAmount y TaxRate con precisión estricta a 6 decimales según Anexo 202. |
| **Desactualización de Catálogos SAT** | Alto | Alta | Implementación del patron SatCatalogCode\<T\> desacoplado de código duro1. |

## **Respuesta Arquitectónica Definitiva**

El conjunto mínimo, completo y arquitectónicamente correcto de Value Objects que debe constituir el núcleo del dominio fiscal mexicano en .NET 10 se organiza en las siguientes fronteras:

### **1\. Shared Kernel (EricksonLopez.SharedKernel)**

* MonetaryAmount: Importe decimal con su código de divisa ISO.  
* CurrencyCode: Código de moneda ISO 4217\.  
* PostalCode: Código postal de 5 dígitos1.  
* DateRange: Intervalo inmutable de fechas.

### **2\. Bounded Context Fiscal Mexicano (EricksonLopez.Fiscal.Mexico)**

* Rfc: Registro Federal de Contribuyentes (física, moral, genéricos)1.  
* Curp: Clave Única de Registro de Población (18 caracteres)11.  
* ForeignTaxId: Identificador fiscal para residentes extranjeros2.  
* FiscalAddress: Domicilio fiscal basado en el código postal de validación CFDI 4.01.  
* SatCatalogCode\<TCatalog\>: Clave de catálogo dinámico del SAT (c\_ClaveProdServ, c\_ClaveUnidad, c\_RegimenFiscal, etc.)1.  
* TaxRate: Tasa porcentual impositiva (hasta 6 decimales)3.  
* TaxQuota: Cuota fija en dinero por unidad consumida (IEPS).  
* TaxBase: Base imponible para cálculo de traslados o retenciones.

### **3\. Bounded Context CFDI y Complementos (EricksonLopez.Cfdi.\*)**

* FiscalUuid: Folio fiscal UUID v4 normalizado del timbrado10.  
* DocumentSeries: Serie de control interno del emisor.  
* DocumentFolio: Folio numérico de control interno del emisor.  
* IdCcp: Identificador único de complemento Carta Porte 3.1 (CCC...)10.  
* VehicleLicensePlate: Matrícula del vehículo de autotransporte.  
* PedimentoNumber: Estructura aduanal de 15 dígitos (RGCE Anexo 22\)6.

### **4\. Conceptos que Permanecen Fuera del Dominio**

* **Modelos XML/XSD (CFDI / TFD)**: Pertenecen a la capa de Infraestructura y serialización4.  
* **DTOs de Respuesta del PAC / Contratos SOAP**: Pertenecen a la capa de Infraestructura de integración.  
* **Contribuyente y ComprobanteFiscalDigital**: Son Entidades / Raíces de Agregado (*Aggregate Roots*), no Value Objects1.  
* **Certificado de Sello Digital (CSD) y e.firma**: Son entidades criptográficas de la capa de seguridad.

#### **Fuentes citadas**

> 1. Anexo 20 SAT 2026: guía completa de llenado del CFDI 4.0 para contadores, [https://siemprealdia.co/mexico/fiscal/anexo-20-sat-cfdi-4-0/](https://siemprealdia.co/mexico/fiscal/anexo-20-sat-cfdi-4-0/)  
> 2. Anexo 20 Guía de llenado de los CFDI emitidos por la Federación, Entidades Federativas y los \- SAT, [https://www.sat.gob.mx/cs/Satellite?blobcol=urldata\&blobkey=id\&blobtable=MungoBlobs\&blobwhere=1461175754221\&ssbinary=true](https://www.sat.gob.mx/cs/Satellite?blobcol=urldata&blobkey=id&blobtable=MungoBlobs&blobwhere=1461175754221&ssbinary=true)  
> 3. Apéndice 7 Preguntas y respuestas sobre el Anexo 20 versión 4.0 1\. ¿Se deberá cancelar el CFDI cuando el receptor dará un u \- SIFEI, [https://www.sifei.com.mx/slides/slide/preguntas-frecuentes-homologadas-925/pdf\_content](https://www.sifei.com.mx/slides/slide/preguntas-frecuentes-homologadas-925/pdf_content)  
> 4. Anexo 20 SAT PDF \- EdifactMx Blog, [https://www.edifact.com.mx/blog/anexo-20-sat-pdf/](https://www.edifact.com.mx/blog/anexo-20-sat-pdf/)  
> 5. Servicio de Administración Tributaria \- SAT \- Anexo 20 Versión 4.0, [http://www.gncys.com/anexo20/sat.aspx](http://www.gncys.com/anexo20/sat.aspx)  
> 6. Manual de llenado de Pedimento (Anexo 22\) \- Barra Nacional de Comercio Exterior, [https://barradecomercio.org/?page\_id=6501](https://barradecomercio.org/?page_id=6501)  
> 7. INSTRUCTIVO PARA EL LLENADO DEL PEDIMENTO, [http://aaachihuahua.mx/index.php?option=com\_content\&view=article\&id=541:instructivo-para-el-llenado-del-pedimento\&catid=45\&Itemid=373](http://aaachihuahua.mx/index.php?option=com_content&view=article&id=541:instructivo-para-el-llenado-del-pedimento&catid=45&Itemid=373)  
> 8. anexo 22 de las reglas generales de comercio exterior para 2022 \- SAT, [https://www.sat.gob.mx/cs/Satellite?blobcol=urldata\&blobkey=id\&blobtable=MungoBlobs\&blobwhere=1461175176109\&ssbinary=true](https://www.sat.gob.mx/cs/Satellite?blobcol=urldata&blobkey=id&blobtable=MungoBlobs&blobwhere=1461175176109&ssbinary=true)  
> 9. Tipos de Pedimento: Guía para Elegir el Correcto \- EP Logistics, [https://eplogistics.com/es/blog/tipos-de-pedimentos-aduanales/](https://eplogistics.com/es/blog/tipos-de-pedimentos-aduanales/)  
> 10. Carta porte v3.1 \- Factura.com, [https://factura.com/apidocs/carta-porte-31.html](https://factura.com/apidocs/carta-porte-31.html)  
> 11. CURP | Trámites \- Gob MX, [https://www.gob.mx/curp/](https://www.gob.mx/curp/)  
> 12. ▷ Qué es el Pedimento Aduanal | Tipos de Pedimento \- Reino Aduanero, [https://reinoaduanero.mx/pedimento/](https://reinoaduanero.mx/pedimento/)