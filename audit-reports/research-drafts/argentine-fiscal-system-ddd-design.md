# **Auditoría Arquitectónica y Especificación del Fiscal Domain Argentino en .NET 10: Catálogo Maestro de Value Objects y Arquitectura DDD**

## **1\. Marco Normativo Vigente y Contexto de la Transformación AFIP a ARCA**

La arquitectura de un sistema fiscal para la República Argentina exige una fundamentación jurídica y normativa rigurosa. Mediante el Decreto 953/2024, el Poder Ejecutivo Nacional dispuso la disolución de la Administración Federal de Ingresos Públicos (AFIP) y la creación de la Agencia de Recaudación y Control Aduanero (ARCA) como un ente autárquico actuante en el ámbito del Ministerio de Economía1. El mismo decreto establece en su Artículo 6° que ARCA es la continuadora jurídica del organismo disuelto, manteniendo intactas las responsabilidades, competencias y funciones asignadas por el marco legal vigente1. En consecuencia, toda la normativa técnica y tributaria dictada bajo la sigla AFIP conserva su plena vigencia operativa1.  
El dominio tributario argentino se rige primariamente por la Ley de Procedimiento Tributario (Ley Nº 11.683, t.o. en 1998 y sus modificaciones)2, el Código Aduanero (Ley Nº 22.415)2, la Ley del Impuesto al Valor Agregado (t.o. en 1997\)4, la Ley del Impuesto a las Ganancias (t.o. en 2019\)4, el Régimen Simplificado para Pequeños Contribuyentes (Monotributo, Ley Nº 24.977) y el Régimen de Factura de Crédito Electrónica MiPyME (Ley Nº 27.440)5. A nivel reglamentario, el pilar de emisión e información de comprobantes descansa sobre la Resolución General (RG) 1415/03, complementada por la RG 4290/18 y la RG 4291/18 para el régimen general de comprobantes electrónicos4. Adicionalmente, normas recientes como la RG 5616/2024 imponen la discriminación obligatoria del IVA y otros impuestos indirectos en comprobantes emitidos a Consumidores Finales y Exempts5.

┌─────────────────────────────────────────────────────────────────────────┐  
│                      MARCO NORMATIVO APLICABLE                          │  
├─────────────────────────────────────────────────────────────────────────┤  
│ Ley 11.683 (Procedimiento) | Ley de IVA | Ley de Ganancias | Ley 24.977   │  
│ Decreto 953/2024 (Creación de ARCA como continuadora de AFIP)           │  
│ RG 1415/03 (Comprobantes) | RG 4291/18 (Facturación Electrónica)          │  
│ RG 4540/19 (Notas de Crédito/Débito) | RG 5616/24 (Discriminación IVA)    │  
└─────────────────────────────────────────────────────────────────────────┘

Desde la perspectiva del diseño de sistemas, el modelado del dominio fiscal argentino requiere deslindar claramente las reglas puras del negocio tributario de los detalles de infraestructura de integración con los Web Services de ARCA (como WSFEv1, WSFEXv1 o WSMTXCA)6. La práctica habitual de acoplar el modelo de objetos de dominio a los esquemas WSDL/XSD dictados por la autoridad fiscal conduce a sistemas frágiles, difíciles de mantener y expuestos a la obsolescencia. El propósito de este informe es establecer el modelo definitivo de Value Objects (VO) bajo Domain-Driven Design (DDD), optimizado para .NET 10 y preparado para compilación nativa (Native AOT).

## **2\. Taxonomía Completa del Fiscal Domain Argentino**

Para estructurar un modelo de dominio expresivo y libre de aberraciones arquitectónicas, se realiza una clasificación exhaustiva de cada concepto del ecosistema tributario argentino según los patrones tácticos de Domain-Driven Design. Se erradica la conversión indiscriminada de primitivos en objetos de valor, exigiendo que cada Value Object esté justificado por inmutabilidad, igualdad semántica, validación de invariantes al instanciarse y ausencia de identidad única.

| Concepto Fiscal / Dominio | Clasificación DDD | Razón y Justificación Arquitectónica |
| :---- | :---- | :---- |
| **Clave Única de Identificación Tributaria (CUIT)** | Value Object | Invariable, validado mediante el algoritmo Modulo 11, representado por valor8. |
| **Clave Única de Identificación Laboral (CUIL)** | Value Object | Estructura análoga al CUIT con diferente prefijo semántico laboral8. |
| **Clave de Identificación (CDI)** | Value Object | Identificador alternativo para sujetos sin CUIT/CUIL con reglas de formato específicas. |
| **Documento Nacional de Identidad (DNI)** | Value Object | Número de identificación de persona humana, encapsulado en contexto de identificación. |
| **Contribuyente / Sujeto Pasivo** | Entity / Aggregate Root | Posee identidad duradera en el tiempo, ciclo de vida, estados fiscales y obligaciones6. |
| **Condición frente al IVA** | Catalog / Value Object | Código normalizado (RI, Monotributo, Exempt, CF). Requiere variabilidad por catálogo8. |
| **Categoría de Monotributo** | Catalog / Value Object | Escala alfabética (A a K) sujeta a actualización por tablas oficiales de ARCA. |
| **Parámetros de Monotributo** | Specification / Policy | Regla de negocio compleja (ingresos brutos, superficie, energía) evaluada dinámicamente. |
| **Point of Sale** | Value Object | Número de 1 a 99999 que identifica el centro de emisión fiscal7. |
| **Voucher Number** | Value Object | Secuencia correlativa de 8 dígitos asociada a un punto de venta y tipo de documento8. |
| **Voucher Type** | Catalog / Value Object | Código numérico oficial publicado por ARCA (ej. 001, 006, 011\)8. |
| **Letra del Comprobante** | Value Object | Atributo categórico inmutable ('A','B','C','E','M','T','R') según condición emisor-receptor6. |
| **Comprobante Electrónico** | Aggregate Root | Entidad con ciclo de vida, estados de autorización, firmas y relaciones correlativas8. |
| **Código de Autorización Electrónico (CAE)** | Value Object | Cadena inmutable de 14 dígitos acoplada a una fecha de vencimiento otorgada por ARCA8. |
| **Código de Autorización Anticipado (CAEA)** | Value Object | Código de 14 dígitos para contingencia quincenal, con régimen operativo diferenciado10. |
| **Código de Autorización de Impresión (CAI)** | Value Object | Código de autorización para impresión física en imprentas autorizadas. |
| **Autorización Fiscal** | Value Object | Abstracción compuesta que envuelve indistintamente un CAE, CAEA o CAI8. |
| **Alícuota de IVA** | Value Object | Porcentaje fiscal explícito (21%, 10.5%, 27%, etc.) vinculado a un código de ARCA5. |
| **Importe Monetario (MonetaryAmount)** | Value Object | Valor numérico decimal exacto acoplado al código de moneda ISO/ARCA8. |
| **Tipo de Cambio / Cotización** | Value Object | Relación numérica con precisión de hasta 6 decimales entre moneda extranjera y Peso Argentino8. |
| **Clave Bancaria Uniforme (CBU)** | Value Object | Cadena de 22 dígitos con doble validación Modulo 10 según normativa del BCRA. |
| **Clave Virtual Uniforme (CVU)** | Value Object | Identificador de 22 dígitos para Proveedores de Servicios de Pago (PSP), separado de CBU. |
| **Factura de Crédito Electrónica (FCE)** | Aggregate Root | Proceso con ciclo de vida legal, aceptación explícita/tácita y negociación (Ley 27.440)5. |
| **Solicitud SOAP WSFEv1** | Integration Model / DTO | Estructura de contrato XML/SOAP específica de la infraestructura de ARCA6. |
| **Código QR Fiscal (RG 4291\)** | Technical Primitive | Cadena codificada Base64 para representación en comprobantes impresos5. |
| **Verification Check Digit del CUIT** | Technical Primitive | Componente del algoritmo Modulo 11 sin semántica de dominio aislada8. |

## **3\. Catálogo Maestro de Value Objects Aprobados**

La siguiente matriz contiene la especificación completa del conjunto definitivo de Value Objects aprobados para el núcleo fiscal argentino. Cada VO ha sido diseñado para su implementación en .NET 10 mediante readonly record struct, garantizando la inexistencia de asignaciones en el *heap* durante su instanciación y transmisión.

| Value Object | Contexto Bounded | Descripción Semántica | Invariantes y Validaciones | Fuente Normativa | Tipo .NET 10 | Prioridad | Paquete Propietario |
| :---- | :---- | :---- | :---- | :---- | :---- | :---- | :---- |
| **TaxId** | Taxpayer | Abstracción base para identificación tributaria nacional. | 11 dígitos numéricos; algoritmo Modulo 118. | Ley 11.683; RG 3889 | readonly record struct | P0 | Fiscal.Argentina |
| **Cuit** | Taxpayer | Clave Única de Identificación Tributaria. | Longitud 11; prefijos {20,23,24,27,30,33,34}; DV Modulo 118. | RG 3889/16; ARCA | readonly record struct | P0 | Fiscal.Argentina |
| **Cuil** | Taxpayer | Clave Única de Identificación Laboral. | Longitud 11; prefijos {20,23,24,27}; DV Modulo 11\. | Anses / ARCA | readonly record struct | P1 | Fiscal.Argentina |
| **Cdi** | Taxpayer | Clave de Identificación para trámites especiales. | Longitud 11; prefijos específicos asignados por ARCA. | RG 3995/17 | readonly record struct | P2 | Fiscal.Argentina |
| **VatCondition** | Taxpayer / Fiscal | Condición del sujeto frente al IVA. | Código oficial en catálogo ARCA (RI, Monotributo, Exempt, CF)8. | RG 1415/03; Ley IVA | readonly record struct | P0 | Fiscal.Argentina |
| **PointOfSale** | Voucher | Punto de venta habilitado para la emisión. | Entero de 1 a 99,999 inclusive7. | RG 1415/03; RG 4291 | readonly record struct | P0 | Fiscal.Comprobantes |
| **VoucherNumber** | Voucher | Correlativo único del comprobante por punto de venta. | Entero positivo de 1 a 99,999,999 inclusive8. | RG 1415/03 | readonly record struct | P0 | Fiscal.Comprobantes |
| **VoucherType** | Voucher | Identificador tipificado del documento fiscal. | Código numérico de catálogo oficial ARCA (1, 6, 11, etc.)8. | WSFEv1 ARCA | readonly record struct | P0 | Fiscal.Comprobantes |
| **VoucherLetter** | Voucher | Letra del comprobante según encuadre fiscal. | Carácter único perteneciendo a {'A','B','C','E','M','T','R'}6. | RG 1415/03; RG 1575 | readonly record struct | P0 | Fiscal.Comprobantes |
| **Cae** | ElectronicInvoicing | Código de Autorización Electrónico. | Criterio de 14 dígitos numéricos; requiere fecha de vencimiento8. | RG 2485/08; RG 4291 | readonly record struct | P0 | ElectronicInvoicing |
| **Caea** | ElectronicInvoicing | Código de Autorización Electrónico Anticipado. | Criterio de 14 dígitos; asociado a quincena fiscal y año10. | RG 2926/10 | readonly record struct | P1 | ElectronicInvoicing |
| **Cai** | Voucher | Código de Autorización de Impresión. | Criterio de 14 dígitos; acoplado a imprenta y rango impreso. | RG 100/98; RG 3665 | readonly record struct | P1 | Fiscal.Comprobantes |
| **FiscalAuthorization** | ElectronicInvoicing | Abstracción unificada de autorización estatal. | Contiene tipo (CAE/CAEA/CAI), código de 14 dígitos y vencimiento8. | RG 4291/18 | readonly record struct | P0 | ElectronicInvoicing |
| **MonetaryAmount** | SharedKernel | Importe numérico exacto en determinada moneda. | Decimal con redondeo a 2 o 4 decimales según regla bancaria8. | Código de Comercio | readonly record struct | P0 | DomainPrimitives |
| **Currency** | SharedKernel | Moneda de la transacción comercial. | Código alfanumérico ISO 4217 (ARS, USD) y código ARCA8. | Tabla Monedas ARCA | readonly record struct | P0 | DomainPrimitives |
| **ExchangeRate** | Fiscal | Cotización de moneda extranjera en pesos. | Decimal estrictamente mayor a 0 con hasta 6 decimales8. | RG 4291/18; ARCA | readonly record struct | P0 | Fiscal.Argentina |
| **VatRate** | Tax / IVA | Porcentaje de IVA legalmente contemplado. | Porcentaje decimal legal (0%, 2.5%, 5%, 10.5%, 21%, 27%)5. | Ley de IVA, Art. 28 | readonly record struct | P0 | Fiscal.Argentina |
| **TaxRate** | Tax | Porcentaje genérico de impuesto o percepción. | Decimal no negativo entre 0.00% y 100.00%5. | Ley 11.683 | readonly record struct | P0 | Fiscal.Core |
| **TaxBase** | Tax | Base imponible asignada a un tributo. | Valor monetario no negativo expresado en la moneda del documento. | Ley de IVA | readonly record struct | P0 | Fiscal.Core |
| **TaxAmount** | Tax | Importe resultante de la aplicación de un tributo. | Calculado como Base x Alícuota; redondeo a 2 decimales8. | RG 4291/18 | readonly record struct | P0 | Fiscal.Core |
| **GrossIncomeTaxId** | IngresosBrutos | Número de inscripción en Ingresos Brutos. | Formato válido provincial o CUIT en Convenio Multilateral6. | SIFERE / Rentas | readonly record struct | P1 | Fiscal.Argentina |
| **JurisdictionCode** | IngresosBrutos | Código de jurisdicción provincial de Convenio. | Rango numérico entre 901 (Bs. As.) y 924 (Tierra del Fuego). | Convenio Multilateral | readonly record struct | P1 | Fiscal.Argentina |
| **Cbu** | Payments | Clave Bancaria Uniforme del sistema financiero. | 22 dígitos; Pauta I (3 banco \+ 4 sucursal \+ DV) y Pauta II (DV). | Com. "A" 2622 BCRA | readonly record struct | P1 | Fiscal.Argentina |
| **Cvu** | Payments | Clave Virtual Uniforme para Proveedores de Pago. | 22 dígitos; bloque inicial 000; validación de algoritmos BCRA. | Com. "A" 6586 BCRA | readonly record struct | P1 | Fiscal.Argentina |
| **FiscalAddress** | Taxpayer | Domicilio fiscal declarado ante la autoridad. | Estructura con calle, número, localidad, provincia y código postal. | Ley 11.683, Art. 3 | readonly record struct | P0 | Fiscal.Argentina |
| **EconomicActivityCode** | Taxpayer | Código de actividad económica (NAES). | Código numérico de 6 dígitos del Formulario 883/A de ARCA. | RG 3537/13 | readonly record struct | P1 | Fiscal.Argentina |
| **UnitOfMeasure** | Voucher | Unidad de medida para ítems facturados. | Código oficial de catálogo ARCA (1=unidades, 7=litros, etc.). | Tabla oficial ARCA | readonly record struct | P0 | Fiscal.Comprobantes |
| **FiscalPeriod** | Tax / Compliance | Período de liquidación o presentación impositiva. | Formato mensual (YYYYMM) o quincenal (YYYYMMQ)12. | RG 2191/07 | readonly record struct | P0 | Fiscal.Core |
| **DocumentReference** | Voucher | Referencia a comprobante fiscal de origen. | Tipo, Point of Sale, Número y CUIT del emisor vinculado8. | RG 4540/19 | readonly record struct | P0 | Fiscal.Comprobantes |

## **4\. Análisis y Auditoría de Conceptos Descartados o Reclasificados**

Un diseño de dominio riguroso debe justificar con precisión por qué ciertos conceptos han sido rechazados como Value Objects, reasignándolos a sus patrones DDD correspondientes.

### **Contribuyente / Sujeto Pasivo (Taxpayer)**

* **Clasificación Adoptada**: Entity / Aggregate Root.  
* **Justificación**: El contribuyente posee un ciclo de vida duradero en el sistema, mutable a lo largo del tiempo6. Cambia su condición fiscal (ej. de Monotributista a Responsable Inscripto), modifica sus domicilios y suscribe obligaciones tributarias sin perder su identidad jurídica6. Representarlo como Value Object obligaría a reemplazar la instancia entera ante cualquier actualización administrativa, destruyendo la trazabilidad de sus operaciones.  
* **ADR Requerido**: ADR 01 (Estructura del Agregado Contribuyente y sus límites).

### **Comprobante Electrónico (ElectronicVoucher)**

* **Clasificación Adoptada**: Aggregate Root.  
* **Justificación**: Un comprobante electrónico posee un ciclo de vida transaccional con cambios de estado bien definidos (Borrador, Solicitado, Autorizado, Anulado, Rechazado)8. Almacena referencias dinámicas a sus eventos de autorización y cobro. Tratar el comprobante completo como un Value Object constituye una aberración que viola el principio de inmutabilidad y comportamiento sin estado.  
* **ADR Requerido**: ADR 04 (Comprobante como Agregado e inmutabilidad de sus componentes).

### **Proceso de Factura de Crédito Electrónica (FceWorkflow)**

* **Clasificación Adoptada**: Domain Process / Aggregate.  
* **Justificación**: El régimen FCE de la Ley 27.440 regula un proceso de interacción entre MiPyMEs, Grandes Empresas y el agente de depósito (Caja de Valores)5. Involucra ventanas de tiempo legales (30 días corridos) para la aceptación expresa, cancelación o rechazo formal mediante motivos tipificados5. Es un proceso dinámico de negociación, incompatible con la inmutabilidad de un VO.  
* **ADR Requerido**: ADR 05 (Desacoplamiento del flujo de FCE del modelo de comprobante base).

### **Verification Check Digit de CUIT (VerificationDigit)**

* **Clasificación Adoptada**: Technical Primitive / Estructura Privada de Validacion.  
* **Justificación**: El dígito verificador es el resultado del algoritmo Modulo 11 aplicado sobre los 10 dígitos precedentes del CUIT8. No posee existencia independiente, identidad semántica ni comportamiento útil de forma aislada fuera de la estructura del CUIT8. Elevarlo a VO genera una proliferación de tipos vacíos (*Over-modeling*).  
* **ADR Requerido**: ADR 01 (Especialización de Identificadores Tributarios).

### **DTOs de Servicios Web de ARCA (FECAERequest, FEAuthRequest)**

* **Clasificación Adoptada**: Integration Model / DTO.  
* **Justificación**: Son clases autogeneradas o contratos de red diseñados para serializarse en mensajes SOAP/XML según los esquemas WSDL impuestos por ARCA6. Contienen convenciones de nombres ajenas al lenguaje ubicuo del dominio (ej. FeCabReq, FeDetReq)8. Introducirlos en el dominio destruye el aislamiento del núcleo impositivo9.  
* **ADR Requerido**: ADR 07 (Estrategia de aislamiento de la Capa de Anti-Corrupción).

## **5\. Auditoría de Duplicidades y Matriz de Resoluciones Semánticas**

Para evitar la duplicación de conceptos y asegurar la cohesión del sistema, se realiza una auditoría explícita de términos equivalentes, definiendo su tratamiento arquitectónico.

| Pair de Conceptos Evaluados | Decisión Adoptada | Justificación Técnica y Normativa |
| :---- | :---- | :---- |
| **CUIT vs TaxpayerId** | Especializar | TaxId actúa como abstracción base. Cuit es el VO concreto de Argentina con la validación Modulo 11 y prefijos fiscales8. |
| **CUIL vs PersonTaxId** | Especializar | Cuil representa la identificación laboral/previsional ante ANSES/ARCA. Se mantiene como VO especializado8. |
| **CDI vs IdentificationNumber** | Especializar | Cdi es una Clave de Identificación tributaria para sujetos no registrados en CUIT/CUIL. Se modela explícitamente. |
| **DNI vs IdentificationNumber** | Mover a Catálogo | El DNI es un tipo dentro del catálogo general de documentos de identidad de personas humanas. |
| **VerificationDigit vs CheckDigit** | Eliminar | El dígito verificador se absorbe como un campo privado dentro de los VOs Cuit, Cuil y Cbu. |
| **DocumentType vs VoucherType** | Fusionar | Se adopta VoucherType como la denominación para tipos de comprobantes fiscales de ARCA8. |
| **VoucherLetter vs DocumentLetter** | Fusionar | Se mantiene VoucherLetter para la letra del comprobante fiscal ('A','B','C','E','M')6. |
| **PointOfSale vs SalesPoint** | Fusionar | Se fija PointOfSale como el único VO para el punto de venta habilitado (1 a 99999\)7. |
| **VoucherNumber vs DocumentNumber** | Fusionar | Se adopta VoucherNumber para la secuencia correlativa de 8 dígitos del comprobante8. |
| **CAE vs CAEA vs CAI** | Mantener Separados | Cada uno posee reglas de vencimiento y contingencia distintas. Se unifican mediante FiscalAuthorization8. |
| **TaxRate vs Percentage** | Mantener Separados | Percentage es una primitiva matemática. TaxRate posee contexto de impuesto y rango legal5. |
| **VatRate vs TaxRate** | Especializar | VatRate restringe los valores a las alícuotas legalmente fijadas por la Ley de IVA (Art. 28\)5. |
| **TaxAmount vs Amount** | Fusionar | TaxAmount representa el importe monetario calculado para un tributo específico. |
| **TaxBase vs TaxableAmount** | Fusionar | Se fija TaxBase como la base imponible sobre la cual se aplica una alícuota. |
| **Money vs MonetaryAmount** | Fusionar | Se adopta MonetaryAmount para evitar colisiones con librerías financieras de terceros8. |
| **PerceptionAmount vs TaxAmount** | Mover | Las percepciones se representan como instancias de TaxAmount asociadas a un código de tributo de percepción. |
| **WithholdingAmount vs TaxAmount** | Mover | Las retenciones se representan como instancias de TaxAmount asociadas a un régimen de retención. |
| **IIBBRate vs TaxRate** | Mover | La alícuota de Ingresos Brutos utiliza la estructura de TaxRate parametrizada por la jurisdicción. |
| **CBU vs BankAccountNumber** | Mantener Separados | Cbu representa la Clave Bancaria Uniforme de 22 dígitos del BCRA. El número de cuenta interna es un string comercial. |
| **FiscalPeriod vs TaxPeriod** | Fusionar | Se adopta FiscalPeriod para representar el período de liquidación impositiva (YYYYMM)12. |
| **Address vs FiscalAddress** | Especializar | FiscalAddress impone la estructura de domicilio legal requerida por la Ley 11.683 ante ARCA. |
| **Jurisdiction vs Province** | Mantener Separados | Province es una división geográfica. JurisdictionCode representa la jurisdicción fiscal de Convenio Multilateral. |
| **DocumentReference vs RefDocument** | Fusionar | Se fija DocumentReference para vincular comprobantes asociados (ej. Notas de Crédito)8. |

## **6\. Arquitectura de Bounded Contexts y Matriz de Paquetes**

Para garantizar la mantenibilidad del sistema y evitar el acoplamiento cruzado (*Shared-Kernel Pollution*), los Value Objects se distribuyen rigurosamente entre sus Bounded Contexts correspondientes.

### **Matriz de Bounded Contexts (Entregable E)**

┌─────────────────────────────────────────────────────────────────────────┐  
│                           BOUNDED CONTEXTS                              │  
├─────────────────────────────────────────────────────────────────────────┤  
│ SHARED KERNEL        : MonetaryAmount, Currency                         │  
│ TAXPAYER CONTEXT     : Cuit, Cuil, Cdi, FiscalAddress, VatCondition    │  
│ VOUCHER CONTEXT      : PointOfSale, VoucherNumber, VoucherType, Letter │  
│ ELECTRONIC INVOICING : Cae, Caea, Cai, FiscalAuthorization              │  
│ TAX CONTEXT          : VatRate, TaxRate, TaxBase, TaxAmount             │  
│ PAYMENTS CONTEXT     : Cbu, Cvu                                         │  
└─────────────────────────────────────────────────────────────────────────┘

### **Matriz de Dependencias y Boundaries de API (Entregable F)**

| Paquete Propietario | API Pública Expuesta | Puede Consumir | Dependencias Prohibidas |
| :---- | :---- | :---- | :---- |
| **DomainPrimitives** | MonetaryAmount, Currency | Ninguna (Zero Dependencies) | System.Xml, Paquetes HTTP, SDKs externos. |
| **SharedKernel** | TaxRate, TaxBase, TaxAmount | DomainPrimitives | Código específico de un país o protocolo. |
| **Fiscal.Core** | FiscalPeriod, TaxType | SharedKernel, DomainPrimitives | Tipos específicos de Argentina, código SOAP/XML8. |
| **Fiscal.Argentina** | Cuit, Cuil, Cdi, VatCondition, Cbu, Cvu | Fiscal.Core, SharedKernel | Fiscal.Comprobantes, ElectronicInvoicing, WSDL de ARCA8. |
| **Fiscal.Comprobantes** | PointOfSale, VoucherNumber, VoucherType, VoucherLetter | Fiscal.Argentina, Fiscal.Core | Protocolos de transporte (SOAP/HTTP), XML8. |
| **ElectronicInvoicing** | Cae, Caea, Cai, FiscalAuthorization | Fiscal.Comprobantes, Fiscal.Argentina | Clientes HTTP, proxies SOAP de ARCA. |
| **ArcaIntegration** | Mappers de dominio a contratos SOAP/XML | ElectronicInvoicing, Fiscal.Comprobantes | **Prohibido ser consumido por la capa de dominio**. |

### **Matriz de Distribución de Paquetes NuGet (Entregable G)**

┌─────────────────────────────────────────────────────────────────────────┐  
│                     GRAFO DE DEPENDENCIAS DE PAQUETES                   │  
├─────────────────────────────────────────────────────────────────────────┤  
│ \[EricksonLopez.DomainPrimitives\]                                       │  
│        ▲                                                                │  
│        │                                                                │  
│ \[EricksonLopez.SharedKernel\]                                            │  
│        ▲                                                                │  
│        │                                                                │  
│ \[EricksonLopez.Fiscal.Core\]                                             │  
│        ▲                                                                │  
│        │                                                                │  
│ \[EricksonLopez.Fiscal.Argentina\]                                        │  
│        ▲                                                                │  
│        │                                                                │  
│ \[EricksonLopez.Fiscal.Comprobantes\]                                     │  
│        ▲                                                                │  
│        │                                                                │  
│ \[EricksonLopez.ElectronicInvoicing\]                                     │  
└─────────────────────────────────────────────────────────────────────────┘

## **7\. Capa de Anti-Corrupción (ACL) e Independencia de Protocolos**

El dominio fiscal debe permanecer completamente agnóstico de las decisiones tecnológicas de transporte e integración impuestas por ARCA (ex-AFIP)1. Los Web Services del organismo fiscal utilizan SOAP/WSDL, mensajes XML firmados bajo el estándar PKCS\#7 (WSAA) y contratos con tipos primitivos sueltos8. La Capa de Anti-Corrupción (ACL) es el componente de infraestructura responsable de aislar el modelo puramente domain-driven de estas complejidades.

### **Flujo de Transformación e Aislamiento**

> 1. **Fiscal Domain Puro**: El Agregado Comprobante se construye utilizando exclusivamente Value Objects fuertemente tipados (Cuit, PointOfSale, VoucherNumber, MonetaryAmount, VatRate)8. No posee ninguna anotación de serialización XML (\[XmlElement\]) ni dependencias con tipos de WSDL.  
> 2. **Capa de Aplicación de ARCA**: Orquesta la intención de autorizar el comprobante. Invoca los servicios del dominio para validar el estado del Agregado antes de iniciar el proceso de integración.  
> 3. **Adaptador de ARCA (ACL)**: Recibe el Agregado de Dominio y utiliza Mappers dedicados (ArcaVoucherMapper) para traducir los Value Objects a los DTOs primitivos exigidos por el cliente SOAP de WSFEv1 (ej. convierte el VO Cuit de 11 dígitos en un escalar long, y el DateOnly de emisión en un string YYYYMMDD)8.  
> 4. **Respuesta e Inyección de Resultados**: El cliente SOAP recibe la respuesta de ARCA conteniendo el CAE y su fecha de vencimiento en formato texto8. El adaptador parsea estos primitivos utilizando los métodos factoría del Value Object Cae (o de FiscalAuthorization) y los retorna a la capa de dominio envueltos en un objeto Result\<FiscalAuthorization\>8.

## **8\. Especificación de Diseño Técnico y APIs para .NET 10, Performance y Native AOT**

En .NET 10, el diseño de Value Objects de alto rendimiento debe priorizar el cero impacto en la recolección de basura (*Zero-Allocation*) mediante la declaración de readonly record struct. Esta estructura garantiza que las instancias se ubiquen en la pila (*stack*), ofreciendo una semántica de igualdad por valor autogenerada por el compilador con la máxima eficiencia en CPU.  
Todos los Value Objects implementan las interfaces unificadas de parseo de memoria de .NET 10: IParsable\<TSelf\>, ISpanParsable\<TSelf\> e IUtf8SpanParsable\<TSelf\>. Esto permite procesar cadenas de texto o buffers de bytes UTF-8 recibidos desde la red o la base de datos sin asignar objetos string suplementarios en el *heap*.  
A continuación se detalla la especificación de diseño conceptual para los Value Objects fundamentales del núcleo fiscal.

C\#  
// Especificación conceptual para Cuit (Fiscal.Argentina)  
public readonly record struct Cuit : ISpanParsable\<Cuit\>, IUtf8SpanParsable\<Cuit\>  
{  
    private readonly long \_value;

    private Cuit(long value) \=\> \_value \= value;

    public static Result\<Cuit\> Create(long value);  
    public static Result\<Cuit\> Create(ReadOnlySpan\<char\> value);  
      
    public static Cuit Parse(string s, IFormatProvider? provider);  
    public static bool TryParse(string? s, IFormatProvider? provider, out Cuit result);  
    public static bool TryParse(ReadOnlySpan\<char\> s, IFormatProvider? provider, out Cuit result);  
    public static bool TryParse(ReadOnlySpan\<byte\> utf8Text, IFormatProvider? provider, out Cuit result);

    public long Value \=\> \_value;  
    public string FormatCanonical(); // "XX-XXXXXXXX-X"  
    public override string ToString() \=\> FormatCanonical();  
}

// Especificación conceptual para PointOfSale (Fiscal.Comprobantes)  
public readonly record struct PointOfSale : ISpanParsable\<PointOfSale\>  
{  
    private readonly int \_value;

    private PointOfSale(int value) \=\> \_value \= value;

    public static Result\<PointOfSale\> Create(int value); // Valida rango 1..99999  
    public int Value \=\> \_value;  
    public string ToPaddedString() \=\> \_value.ToString("D5"); // Formato "00001"  
}

// Especificación conceptual para MonetaryAmount (DomainPrimitives)  
public readonly record struct MonetaryAmount  
{  
    public decimal Amount { get; }  
    public Currency Currency { get; }

    public MonetaryAmount(decimal amount, Currency currency)  
    {  
        Amount \= decimal.Round(amount, 2, MidpointRounding.ToEven);  
        Currency \= currency;  
    }

    public static MonetaryAmount Zero(Currency currency) \=\> new(0m, currency);  
    public MonetaryAmount Add(MonetaryAmount other);  
}

// Especificación conceptual para Cae (ElectronicInvoicing)  
public readonly record struct Cae : ISpanParsable\<Cae\>  
{  
    private readonly ulong \_value;  
    public DateOnly ExpirationDate { get; }

    private Cae(ulong value, DateOnly expirationDate)  
    {  
        \_value \= value;  
        ExpirationDate \= expirationDate;  
    }

    public static Result\<Cae\> Create(ReadOnlySpan\<char\> code, DateOnly expirationDate);  
    public string Value \=\> \_value.ToString("D14");  
    public bool IsExpired(DateOnly currentDate) \=\> currentDate \> ExpirationDate;  
}

### **Manejo de Errores mediante el Patrón Result\<T\>**

El dominio fiscal no utiliza excepciones para gestionar errores de validación esperados (ej. un CUIT con dígito verificador incorrecto o un punto de venta fuera del rango 1-99999)7. Las excepciones destruyen el rendimiento en escenarios de procesamiento masivo y desvirtúan el flujo del dominio. Cada factory de Value Object retorna una estructura Result\<T\> que encapsula el éxito o un error de dominio fuertemente tipado (DomainError).

## **9\. Persistencia, Serialización y Versionado Normativo**

### **Estrategia de Persistencia (PostgreSQL y Dapper / EF Core)**

* **Entity Framework Core**: Los Value Objects atómicos (Cuit, PointOfSale, VoucherNumber) se persisten en columnas primitivas de la base de datos relacional (BIGINT, INTEGER) utilizando conversores de valor de EF Core (ValueConverter\<TModel, TProvider\>). Para Value Objects compuestos (MonetaryAmount, FiscalAddress), se utilizan *Owned Entity Types* o mapeo directo a campos de tipo JSONB en PostgreSQL.  
* **Dapper**: Se implementan custom TypeHandler\<T\> para materializar los readonly record struct directamente desde las filas devueltas por el driver Npgsql de PostgreSQL sin copias intermedias de memoria.

### **Estrategia de Serialización de Transporte (JSON / REST APIs)**

La serialización JSON en .NET 10 se realiza mediante System.Text.Json empleando *Source Generators* (JsonSourceGenerator) para garantizar compatibilidad total con compilación AOT nativa (Native AOT) y evitar la reflexión en tiempo de ejecución. Cada Value Object posee un JsonConverter\<T\> personalizado que serializa el objeto en su representación escalar canónica (ej. Cuit se serializa como cadena formateada "20-30123456-7" o numérica "20301234567")8.

### **Versionado y Clasificación de Variabilidad Normativa**

┌─────────────────────────────────────────────────────────────────────────┐  
│                    CLASIFICACIÓN DE VARIABILIDAD                        │  
├─────────────────────────────────────────────────────────────────────────┤  
│ STABLE        : CUIT, CBU, Correlatividad de Comprobantes               │  
│ CONFIGURABLE  : Alícuotas de IVA, Límites de Consumidor Final           │  
│ CATALOG-DRIVEN: Tipos de Comprobante, Monedas, Jurisdicciones SIFERE   │  
│ REGULATORY    : Discriminación de IVA (RG 5616/24), Formatos de QR      │  
└─────────────────────────────────────────────────────────────────────────┘

## **10\. Estrategia de Testing, Cobertura y Validación Jurídica**

La suite de pruebas automatizadas del sistema fiscal garantiza la corrección matemática, la resistencia ante mutaciones y el estricto cumplimiento normativo mediante un enfoque de múltiples capas.

### **Layering de Pruebas de Software**

> 1. **Unit Tests Deterministas**: Verificación de instanciación válida y rechazo esperado ante invariantes violadas en cada Value Object8. Pruebas explícitas sobre las fronteras numéricas de PointOfSale (1 y 99999\) y VoucherNumber (1 y 99999999\)7.  
> 2. **Property-Based Testing (CsCheck / FsCheck)**: Generación automática de miles de entradas sintéticas para probar los algoritmos de verificación:  
   * **Algoritmo Modulo 11 (CUIT/CUIL)**: Confirmar que ningún entero aleatorio que violes la fórmula del dígito verificador logre instanciar un VO Cuit válido8.  
   * **Algoritmo Modulo 10 Double-Add (CBU/CVU)**: Validar la consistencia de las pautas I y II de bancarización frente a cadenas aleatorias de 22 dígitos.  
> 3. **Mutation Testing (Stryker.NET)**: Ejecución de pruebas de mutación sobre la librería Fiscal.Argentina. Se exige un grado de cobertura de mutación superior al 95% en las clases de cálculo de dígitos verificadores para asegurar que ningún operador lógico (\<, \<=, \==, %) quede sub-probado.  
> 4. **Golden Tests de Representación Fiscal**: Comparación ciego a ciego de mensajes JSON, payloads de códigos QR (RG 4291\) y estructuras XML generadas frente a muestras de referencia validadas por el entorno de homologación de ARCA5.  
> 5. **Regulatory Tests (Pruebas Regresivas de Normativa)**: Pruebas unitarias vinculadas a la vigencia temporal de resoluciones generales de ARCA5. Verificación de que la discriminación de IVA en facturas B emitidas a consumidores finales responde correctamente al comportamiento fijado a partir del 1 de enero de 2025 por la RG 5616/20245.

## **11\. Roadmap de Implementación y Matriz de Prioridad (Entregable H)**

El desarrollo del catálogo de Value Objects y del núcleo fiscal se organiza en cuatro fases jerárquicas para permitir entregas de valor continuas y seguras.

┌─────────────────────────────────────────────────────────────────────────┐  
│                       ROADMAP DE IMPLEMENTACIÓN                         │  
├─────────────────────────────────────────────────────────────────────────┤  
│ FASE 1 (P0) : Núcleo Emisión Base (Cuit, PointOfSale, VoucherNumber,    │  
│               VoucherType, Letter, MonetaryAmount, Cae, FiscalAuth)     │  
│ FASE 2 (P1) : Facturación Avanzada (Cuil, VatCondition, DocumentRef,    │  
│               ExchangeRate, Caea, Cai, Cbu, FiscalAddress)              │  
│ FASE 3 (P2) : Regímenes Especiales (Cdi, Cvu, JurisdictionCode,         │  
│               EconomicActivityCode, Exportation, FCE MiPyME)            │  
│ FASE 4 (P3) : Fiscalidad Extendida (Declaraciones Juradas, Libros IVA)  │  
└─────────────────────────────────────────────────────────────────────────┘

## **12\. Architectural Decision Records (ADRs completos) (Entregable I)**

### **ADR 01: Especialización de Identificadores Tributarios Nacionales (CUIT, CUIL, CDI)**

* **Estatus**: Aprobado  
* **Contexto**: Los identificadores tributarios en Argentina (CUIT, CUIL, CDI) comparten la misma longitud de 11 dígitos y el algoritmo de validación por Modulo 118. Sin embargo, poseen semánticas de negocio y propósitos fiscales completamente distintos ante ARCA y ANSES6.  
* **Decisión**: Crear una interfaz/abstracción base TaxId e implementar los Value Objects concretos y fuertemente tipados Cuit, Cuil y Cdi como readonly record struct.  
* **Consecuencias**: Previene la mezcla accidental de un CUIL laboral en operaciones donde la ley exige taxativamente un CUIT10. Elimina la obsesión por los primitivos a costa de incluir tres tipos bien delimitados en el paquete Fiscal.Argentina.

### **ADR 02: Catálogos Dinámicos en lugar de Enums Cerrados**

* **Estatus**: Aprobado  
* **Contexto**: ARCA publica periódicamente actualizaciones sobre las tablas de Tipos de Comprobante, Monedas y Condiciones frente al IVA8. El uso de enum de C\# obliga a recompilar y desplegar aplicaciones cada vez que el organismo fiscal incorpora un nuevo código oficial6.  
* **Decisión**: Representar los conceptos de catálogo (VoucherType, VatCondition, UnitOfMeasure) como Value Objects que envuelven el código escalar oficial e interactúan con repositorios de catálogos en memoria.  
* **Consecuencias**: El sistema tolera la adición de nuevos códigos oficiales por parte de ARCA sin romper los contratos del dominio ni requerir despliegues imprevistos en producción.

### **ADR 03: Abstracción Compuesta de Autorización Fiscal (FiscalAuthorization)**

* **Estatus**: Aprobado  
* **Contexto**: Un comprobante fiscal electrónico o físico puede ser autorizado legalmente mediante CAE (tiempo real), CAEA (anticipado) o CAI (imprenta)8. Cada modalidad posee estructuras de datos comunes (código de 14 dígitos y fecha de vencimiento) pero diferentes ciclos de procesamiento8.  
* **Decisión**: Diseñar FiscalAuthorization como un Value Object compuesto que encapsula las instancias concretas de Cae, Caea o Cai, exponiendo una interfaz unificada para el Agregado Comprobante8.  
* **Consecuencias**: El Agregado Comprobante no necesita conocer el mecanismo por el cual fue autorizado el documento, simplificando las reglas de validación interna.

### **ADR 04: Modelado del Comprobante como Agregado Root e Inmutabilidad de Componentes**

* **Estatus**: Aprobado  
* **Contexto**: Existe el riesgo de intentar modelar un comprobante fiscal completo como un gran Value Object inmutable debido a su naturaleza de documento firmado.  
* **Decisión**: El Comprobante se modela como un Aggregate Root que posee identidad basada en la tupla (VoucherType, PointOfSale, VoucherNumber, CuitEmisor)7. Sus atributos internos (MonetaryAmount, FiscalAuthorization, VatRate) son Value Objects inmutables.  
* **Consecuencias**: Permite gestionar adecuadamente el ciclo de vida transaccional del comprobante (Borrador \-\> Solicitado \-\> Autorizado) sin perder el rigor de inmutabilidad en sus componentes fiscales8.

### **ADR 05: Desacoplamiento del Contexto Tributario Nacional e Impuestos Provinciales (IIBB)**

* **Estatus**: Aprobado  
* **Contexto**: Las retenciones y percepciones de Ingresos Brutos (Convenio Multilateral / SIFERE) son reguladas por las provincias, mientras que el IVA y las Ganancias son tributos nacionales administrados por ARCA6.  
* **Decisión**: El paquete Fiscal.Core define abstracciones genéricas de tributos (TaxRate, TaxBase, TaxAmount). El contexto IngresosBrutos vive en un Bounded Context separado que especializa las jurisdicciones sin acoplar el núcleo de comprobantes nacionales de ARCA6.  
* **Consecuencias**: Aislamiento de la variabilidad de las 24 jurisdicciones provinciales respecto al motor de facturación nacional.

### **ADR 06: Adopción Obligatoria de readonly record struct en .NET 10**

* **Estatus**: Aprobado  
* **Contexto**: Los entornos de facturación masiva requieren procesar miles de transacciones por segundo con el mínimo consumo de CPU y memoria.  
* **Decisión**: Todos los Value Objects sin excepción se declaran como readonly record struct e implementan ISpanParsable\<T\>.  
* **Consecuencias**: Cero asignaciones suplementarias en el *heap* durante el parseo y cálculo de impuestos. Reducción drástica del trabajo del Garbage Collector y compatibilidad total con compilación Native AOT.

### **ADR 07: Independencia de Protocolos de Transporte (XML/SOAP vs Dominio)**

* **Estatus**: Aprobado  
* **Contexto**: La tentación de anotar las clases del dominio con atributos XML de C\# para serializar directamente los mensajes de ARCA arruina la limpieza de la arquitectura.  
* **Decisión**: Prohibir categóricamente cualquier referencia a clases de serialización XML o clientes SOAP dentro de los proyectos de dominio. La Capa de Anti-Corrupción (ACL) en ArcaIntegration realiza la traducción bidireccional mediante mappers dedicados9.  
* **Consecuencias**: El dominio fiscal puede evolucionar o adaptarse a nuevas APIs REST de ARCA sin alterar su modelo de objetos8.

### **ADR 08: Versionado Normativo y Desacoplamiento de Reglas Mutables**

* **Estatus**: Aprobado  
* **Contexto**: Resoluciones como la RG 5616/2024 modifican la forma en que debe desglosarse el IVA a partir de fechas específicas5.  
* **Decisión**: Encapsular las reglas de cálculo e impresión en clases de especificación/política (VatBreakdownPolicy) asociadas a la fecha de emisión del comprobante, manteniendo la estructura básica de los VOs (VatRate, TaxAmount) inmutable5.  
* **Consecuencias**: Facilita la coexistencia de reglas históricas y vigentes dentro del mismo sistema sin duplicar clases de dominio.

### **ADR 09: Separación Estricta entre el Fiscal Domain y el Dominio Contable**

* **Estatus**: Aprobado  
* **Contexto**: Confundir los importes fiscales de una factura con los asientos de contabilidad general conduce a sistemas sobrecargados e inmanejables.  
* **Decisión**: El sistema fiscal finaliza su responsabilidad al emitir y autorizar el comprobante válido8. El sistema contable escucha los eventos de dominio (VoucherAuthorizedEvent) e interpreta las cuentas contables de imputación de manera independiente.  
* **Consecuencias**: Desacoplamiento completo entre la normativa tributaria impositiva y el plan de cuentas interno de la empresa.

## **13\. Matriz de Riesgos Arquitectónicos y Estrategias de Mitigación (Entregable J)**

| Riesgo Arquitectónico | Severidad | Mitigación Diseñada |
| :---- | :---- | :---- |
| **Obsesión por Primitivos** (*Primitive Obsession*) | Alta | Reemplazo total de escalares por readonly record struct fuertemente tipados (Cuit, PointOfSale, Cae)7. |
| **Sobre-modelado** (*Over-modeling*) | Media | Reclasificación estricta de agregados, procesos y primitivas internas (Verification Check Digit descartado como VO)8. |
| **Contaminación del Kernel Compartido** | Alta | Aislamiento de objetos en Bounded Contexts específicos (Fiscal.Argentina, ElectronicInvoicing). |
| **Acoplamiento a Esquemas WSDL/SOAP de ARCA** | Crítico | Introducción de una Capa de Anti-Corrupción (ACL) estricta en el paquete ArcaIntegration9. |
| **Catálogos Hardcodeados en Enums** | Alta | Transformación de catálogos en Value Objects dinámicos basados en repositorios de datos8. |
| **Normativa Mutable y Reescritura de Código** | Crítico | Encapsulamiento de reglas en patrones *Policy* y *Specification* parametrizados por fecha5. |
| **Inprecisión Numérica en Impuestos** | Crítico | Uso de MonetaryAmount con tipo de dato decimal de C\# y redondeo bancario de 2 decimales8. |
| **Incompatibilidad con Native AOT / Trimming** | Alta | Uso exclusivo de System.Text.Json con *Source Generators* y ausencia de reflexión en VOs. |
| **Desperdicio de Memoria por Asignaciones en Heap** | Alta | Adopción de readonly record struct e implementación de ISpanParsable\<T\> en .NET 10\. |
| **Mezcla de Fiscalidad y Contabilidad General** | Media | Desacoplamiento de contextos mediante eventos de dominio (VoucherAuthorizedEvent). |

## **14\. Respuesta a la Pregunta Arquitectónica Final**

En respuesta al interrogante final de la auditoría sobre cuál debe ser la constitución exacta del modelo de dominio fiscal argentino, se dictamina la siguiente distribución arquitectónica oficial:

### **1\. Conjunto Mínimo, Completo y Arquitectónicamente Correcto del Núcleo Fiscal Argentino**

El núcleo impositivo nacional debe estar constituido exclusivamente por los siguientes Value Objects:

* **Identificadores Tributarios**: Cuit, Cuil, Cdi8.  
* **Atributos de Comprobantes**: PointOfSale, VoucherNumber, VoucherType, VoucherLetter, DocumentReference6.  
* **Autorizaciones Fiscales**: Cae, Caea, Cai, unificados en la abstracción FiscalAuthorization8.  
* **Encuadres y Unidades Fiscales**: VatCondition, VatRate, FiscalAddress, ExchangeRate, EconomicActivityCode, UnitOfMeasure5.

### **2\. Value Objects Pertenecientes al Shared Kernel (Universales)**

Únicamente dos objetos primitivos residen en el Kernel Compartido del ecosistema general:

* MonetaryAmount (importe decimal exacto acoplado al código de moneda)8.  
* Currency (código y denominación normalizada de la moneda)8.  
* TaxRate, TaxBase y TaxAmount (estructuras cuantitativas universales para impuestos).

### **3\. Value Objects Exclusivos de los Bounded Contexts Argentinos**

Residen en los paquetes específicos de la Argentina (Fiscal.Argentina / Fiscal.Comprobantes):

* Cuit, Cuil, Cdi, Cbu, Cvu, GrossIncomeTaxId, JurisdictionCode y VatCondition6. Ninguno de estos tipos se expone en librerías internacionales agnósticas.

### **4\. Componentes Específicos del Ecosistema de Facturación Electrónica / ARCA**

Pertenecen exclusivamente al paquete de integración con la autoridad fiscal (ElectronicInvoicing / ArcaIntegration):

* Cae, Caea, FiscalAuthorization, Mappers de traducción a contratos SOAP/WSDL, generadores de payload de códigos QR (RG 4291\) y decodificadores de respuestas de red de ARCA5.

### **5\. Conceptos que Permanecen Completamente FUERA del Dominio de Value Objects**

Deben modelarse bajo otros patrones tácticos de DDD o mantenerse en la capa de infraestructura:

* **Contribuyente (Taxpayer)**: Es una **Entidad / Aggregate Root** con identidad duradera y ciclo de vida6.  
* **Comprobante Electrónico (ElectronicVoucher)**: Es un **Aggregate Root** transaccional8.  
* **Flujo de Factura de Crédito MiPyME (FceWorkflow)**: Es un **Domain Process / Aggregate** de negociación (Ley 27.440)5.  
* **Verification Check Digit del CUIT**: Es una **Primitiva Técnica / Algoritmo Interno** de validación8.  
* **Contratos XML/SOAP y DTOs de ARCA**: Pertenecen a la **Capa de Infraestructura / ACL**4.  
* **Imputaciones Contables y Asientos**: Pertenecen al **Bounded Context de Contabilidad**, completamente desligado del motor fiscal impositivo.

#### **Fuentes citadas**

> 1. ARCA \- Institucional, [https://www.afip.gob.ar/institucional/arca/](https://www.afip.gob.ar/institucional/arca/)  
> 2. Se disuelve la Administración Federal de Ingresos Públicos (AFIP) y se crea la Agencia de Recaudación y Control Aduanero (ARCA). Decreto 953/2024 | Liga del Consorcista, [https://ligadelconsorcista.org/decreto-953-2024-disuelve-afip-crea-arca-](https://ligadelconsorcista.org/decreto-953-2024-disuelve-afip-crea-arca-)  
> 3. El Poder Ejecutivo oficializó la disolución de la AFIP \- CPBA, [https://www.cpba.com.ar/noticias/item/15052-el-poder-ejecutivo-oficializo-la-disolucion-de-la-afip](https://www.cpba.com.ar/noticias/item/15052-el-poder-ejecutivo-oficializo-la-disolucion-de-la-afip)  
> 4. Impacto de la factura apócrifa y consecuencias tributarias de su utilización \- RepHip UNR, [https://rephip.unr.edu.ar/bitstreams/f78f7c5d-2ee7-4a16-87c1-9bfb81875e70/download](https://rephip.unr.edu.ar/bitstreams/f78f7c5d-2ee7-4a16-87c1-9bfb81875e70/download)  
> 5. ARCA reglamentó el “Régimen de Transparencia Fiscal al Consumidor” \- NeoFactura, [https://neofactura.com.ar/blog\_post.asp?id=81](https://neofactura.com.ar/blog_post.asp?id=81)  
> 6. Facturación y registración \- ARCA \- Consulta Frecuentes, [https://servicioscf.afip.gob.ar/publico/abc/ABCpaso2.aspx?cat=1462](https://servicioscf.afip.gob.ar/publico/abc/ABCpaso2.aspx?cat=1462)  
> 7. Guía para Alta en Monotributo AFIP | PDF | Documento de identidad | Jubilación \- Scribd, [https://es.scribd.com/document/787808800/MONOTRIBUTO](https://es.scribd.com/document/787808800/MONOTRIBUTO)  
> 8. Referencia API AFIP ARCA \- TusFacturasAPP \- API Facturación Electrónica AFIP, [https://developers.tusfacturas.app/api-factura-electronica-afip-facturacion-ventas/referencia-api-afip-arca](https://developers.tusfacturas.app/api-factura-electronica-afip-facturacion-ventas/referencia-api-afip-arca)  
> 9. Facturación \- ARCA \- Consulta Frecuentes, [https://servicioscf.afip.gob.ar/publico/abc/ABCpaso2.aspx?cat=2924](https://servicioscf.afip.gob.ar/publico/abc/ABCpaso2.aspx?cat=2924)  
> 10. Facturación Electrónica AFIP/ARCA Argentina \- YoFacturo, [https://yo-facturo.com/facturacion-electronica/](https://yo-facturo.com/facturacion-electronica/)  
> 11. Factura electrónica AFIP: cómo hacerla paso a paso 2026 \- YoFacturo, [https://yo-facturo.com/blog/afip-facturacion-como-hacer-factura-electronica/](https://yo-facturo.com/blog/afip-facturacion-como-hacer-factura-electronica/)  
> 12. Constatación de Comprobantes con CAEA \- AFIP, [https://servicioscf.afip.gob.ar/publico/comprobantes/caea.aspx](https://servicioscf.afip.gob.ar/publico/comprobantes/caea.aspx)