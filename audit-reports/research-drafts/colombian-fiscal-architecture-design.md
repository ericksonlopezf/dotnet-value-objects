# **Auditoría y Diseño Arquitectónico Exhaustivo de Value Objects para el Sistema Fiscal Colombiano en .NET 10**

## **1\. Fundamentación Normativa y Arquitectura del Dominio**

El diseño de un dominio fiscal para Colombia exige una delimitación rigurosa entre el derecho tributario sustantivo, las especificaciones técnicas de la Dirección de Impuestos y Aduanas Nacionales (DIAN) y los patrones de diseño de software guiados por el dominio (DDD). En el ecosistema tecnológico colombiano, la arquitectura de software ha sufrido históricamente de *Primitive Obsession* y de un acoplamiento tóxico con las estructuras XML/UBL 2.1 definidas por la autoridad tributaria1. La construcción de un modelo de dominio preparado para producción requiere abstraer el núcleo de negocio de los detalles de transporte e integración, tratando a la normativa tributaria no como un conjunto de campos XML, sino como un sistema de invariantes legales y comerciales.  
El marco normativo tributario colombiano combina leyes orgánicas, estatutos, resoluciones administrativas y anexos técnicos con ciclos de actualización heterogéneos. La modelación del dominio no puede asumir una estabilidad estática en los conceptos; por tanto, cada entidad y Value Object debe derivarse directamente de su fuente legal primaria.

| Fuente Normativa | Acto / Documento Oficial | Ámbito de Aplicación | Naturaleza de la Regla | Volatilidad Regulatoria |
| :---- | :---- | :---- | :---- | :---- |
| **Estatuto Tributario (ET)** | Art. 555-2, 615, 616-1, 631 | Marco tributario general, RUT, obligación de facturar, exógena | Legal / Sustantiva | Baja (Reformas tributarias quinquenales) |
| **DIAN** | Orden Adm. 004/1989 y 001/2005 | Estructura del NIT y algoritmo del Verification Check Digit2 | Técnica / Identificación | Alta Estabilidad (Invariable desde 1989\) |
| **DIAN** | Oficio 909585 de 2021 | Naturaleza jurídica del DV (no forma parte del NIT)3 | Doctrina Tributaria | Estable |
| **DIAN** | Resolución 000013 de 2021 | Documento Soporte de Pago de Nómina Electrónica y CUNE4 | Operativa / Integración | Media |
| **DIAN** | Resolución 000085 de 2022 | Registro de Electronic Invoice como Título Valor (RADIAN)5 | Comercial / Tributaria | Media |
| **DIAN** | Res. 000165/2023 y 000008/2024 | Sistema de Facturación, POS Electrónico y Anexo Técnico 1.91 | Técnica / Transaccional | Alta (Actualizaciones de anexos técnicos) |

La distinción entre reglas sustantivas y reglas técnicas es fundamental para la modelación del dominio. Las reglas legales y tributarias definen la causación del impuesto, el sujeto pasivo, la base gravable y la tarifa (por ejemplo, el Artículo 468 del Estatuto Tributario para la tarifa general del Impuesto sobre las Ventas). Estas reglas gobiernan las invariantes de negocio y el cálculo monetario dentro del dominio. Por su parte, las reglas de negocio comercial regulan la relación contractual entre el emisor y el adquirente, especificando plazos de pago y la aceptación de facturas según el Código de Comercio y la regulación RADIAN5.  
En un estrato diferente se ubican las reglas de integración y las reglas técnicas de la DIAN. Estas definen esquemas XML bajo la norma UBL 2.1, firmas digitales bajo el estándar XAdES, espacios de nombres target, algoritmos de hash SHA-384 para CUFE, CUDE y CUNE, y protocolos de transporte SOAP/REST1. Un error recurrente en la arquitectura de sistemas fiscales consiste en proyectar el esquema XML UBL directamente sobre el modelo de dominio domain-driven1. El dominio fiscal debe permanecer completamente agnóstico de la representación XML de transporte, operando las estructuras UBL únicamente como DTOs de integración en los límites de la infraestructura a través de una Capa Anticorrupción (ACL).

## **2\. Identificación Tributaria y Registro Único Tributario (RUT)**

El Registro Único Tributario (RUT) y el Número de Identificación Tributaria (NIT) constituyen los pilares de la identidad fiscal en Colombia9. Sin embargo, su representación en software suele estar contaminada por duplicidades y fallas conceptuales en la delimitación de responsabilidades.

### **2.1. Algoritmo del Verification Check Digit y Naturaleza Jurídica del NIT**

El NIT es un número único asignado por la DIAN para individualizar a las personas naturales y jurídicas en materia tributaria, aduanera y cambiaria9. El Verification Check Digit (DV) es un valor numérico entre 0 y 9 calculado mediante una ponderación sobre el módulo 112.  
La doctrina de la DIAN, expresada en el Oficio 909585 de 2021 y la Orden Administrativa 001 de 2005, clarifica que el DV no forma parte integrante del NIT, sino que actúa como un mecanismo de control de integridad para detectar errores de digitación en formularios y sistemas informáticos3. En la casilla 5 del RUT se diligencia el NIT base, mientras que en la casilla 6 se ubica el DV independientemente11.  
El algoritmo de cálculo, estipulado en la Orden Administrativa 004 de 1989, toma una secuencia de hasta 15 dígitos (completada con ceros a la izquierda si posee menor longitud) representada como ![][image1], y aplica la lista fija de factores primos de ponderación ![][image2]2:  
![][image3]  
![][image4]  
![][image5]  
Para ilustrar la mecánica del cálculo, considérese el NIT base 830.999.999. Al rellenar a 15 dígitos se obtiene la cadena 000000008309999\. Aplicando la multiplicación escalar con el vector de pesos ![][image6], la suma de los productos ![][image7] resulta en 1200\. El residuo ![][image8] equivale a 1\. Como ![][image9], el Verification Check Digit obtenido es 1, dando como resultado la representación canónica 830999999-1.

### **2.2. Jerarquía de Identificación y Eliminación de Redundancias**

Se descartan los nombres redundantes o anglicismos como TaxpayerId o TaxIdentificationNumber en el contexto colombiano, consolidando la taxonomía de la identificación en un modelo fuertemente tipado:

> 1. **Nit**: Value Object que representa exclusivamente el NIT colombiano de personas naturales o jurídicas gravadas. Mantiene como invariante que su valor base conste de 8 a 15 dígitos numéricos y calcula o valida de forma estricta su VerificationDigit derivado2.  
> 2. **VerificationDigit**: Se modela como un valor primitivo derivado (byte) encapsulado internamente dentro de Nit3. No se justifica su existencia como Value Object independiente fuera de la estructura Nit.  
> 3. **IdentificationDocument**: Value Object compuesto que modela cualquier documento de identidad aceptado por la DIAN (Cédula de Ciudadanía \- CC, Cédula de Extranjería \- CE, Pasaporte, Permiso por Protección Temporal \- PPT, NIT, Documento Extranjero)12. Encapsula el tipo de documento y el número de identificación estandarizado sin guiones ni puntos.  
> 4. **Taxpayer**: Se define conceptualmente como una **Entity / Aggregate Root** (no un Value Object). El contribuyente posee ciclo de vida, estado, responsabilidades tributarias mutables y una dirección fiscal cambiante. Su identidad está determinada de manera unívoca por su IdentificationDocument o su Nit.

### **2.3. Modelado del RUT y la Información Registral**

El Registro Único Tributario (RUT) administra la información de identificación, ubicación y clasificación de los contribuyentes9. El RUT no debe modelarse como un Value Object monolítico debido a su complejidad y mutabilidad. El RUT en sí mismo es un **Snapshot / DTO de Integración** que refleja el estado registral de la entidad Taxpayer en un instante determinado del tiempo9. Sus componentes internos se clasifican de la siguiente forma:

* **CiiuCode**: Value Object que representa la Clasificación Industrial Internacional Uniforme de 4 dígitos9.  
* **TaxResponsibilityCode**: Value Object derivado de catálogo dinámico que representa obligaciones como Gran Contribuyente (R-99-PN), Autorretenedor (O-13), o Agente de Retención en IVA (O-23).  
* **FiscalAddress**: Value Object estructurado de ubicación fiscal que incluye la combinación de vía, número, código DANE de municipio, departamento, país y código postal.

## **3\. Sistema de Facturación Electrónica, Numeración e Identificadores Criptográficos**

El sistema de facturación electrónica abarca la factura de venta, las notas crédito, las notas débito, los documentos soporte y los documentos equivalentes electrónicos12.

### **3.1. Numeración Autorizada y Resoluciones DIAN**

La autorización de numeración concedida por la DIAN no es un Value Object; es una **Entity** perteneciente al agregado de configuración de facturación o al agregado InvoicingAuthorization. Esta entidad posee un ciclo de vida delimitado (se solicita, entra en vigencia, consume consecutivos y expira por fecha o agotamiento de rango). No obstante, los atributos inmutables asociados a la numeración se componen mediante Value Objects:

* **AuthorizationRange**: Define el rango numérico inmutable autorizado por la DIAN (![][image10]). Invariante: ![][image11].  
* **DocumentPrefix**: Prefijo alfanumérico autorizado de 0 a 4 caracteres.  
* **DocumentNumber**: Representación estructurada del consecutivo de un documento fiscal. Garantiza la concatenación limpia del prefijo opcional y el valor numérico, asegurando la igualdad por valor.  
* **TechnicalKey**: Clave técnica criptográfica provista por la DIAN para la generación del CUFE en facturas de venta. Invariante: cadena hexadecimal inmutable de 64 caracteres.

### **3.2. Criptografía Fiscal: CUFE, CUDE y CUNE**

Los identificadores criptográficos aseguran la integridad, el no repudio y la trazabilidad de los documentos electrónicos mediante algoritmos de resumen criptográfico SHA-3848.  
El Código Único de Electronic Invoice (**CUFE**) aplica exclusivamente a las Facturas Electrónicas de Venta8. Se calcula mediante la concatenación estricta de los campos del documento en formato texto plano, sin espacios ni separadores, aplicando posteriormente el algoritmo SHA-3848:  
![][image12]  
El Código Único de Documento Electrónico (**CUDE**) se utiliza en Notas Crédito, Notas Débito, Documentos Soporte en Adquisiciones con No Obligados y Documentos Equivalentes Electrónicos (POS)14. La diferencia funcional con el CUFE radica en que el CUDE reemplaza la TechnicalKey de la resolución por el SoftwarePIN asignado al proveedor tecnológico o al software propio del contribuyente14.  
El Código Único de Nómina Electrónica (**CUNE**) aplica al Documento Soporte de Pago de Nómina Electrónica y sus notas de ajuste (Resolución 000013 de 2021\)4. Emplea el SoftwarePIN e integra en su cadena de concatenación el NIT del empleador, el número de documento del empleado, el período de nómina y los valores totales devengados y deducidos4.  
En el diseño del dominio, Cufe, Cude y Cune no deben fusionarse de manera indistinta porque poseen semánticas legales y reglas de construcción diferentes4. Sin embargo, comparten una abstracción base común denominada ElectronicDocumentIdentifier. Cada uno de ellos se implementa como un readonly record struct que valida que la cadena resultante sea una secuencia de exactamente 96 caracteres hexadecimales (representación en minúsculas de 384 bits).

## **4\. Documentos Equivalentes, Documento Soporte, Nómina Electrónica y Ecosistema RADIAN**

El marco de documentos electrónicos en Colombia abarca subsistemas especializados que operan bajo reglas de negocio diferenciadas.

### **4.1. Documentos Equivalentes y Documento Soporte**

La Resolución 000165 de 2023 y la Resolución 000008 de 2024 reglamentan los **Documentos Equivalentes Electrónicos** y el **Documento Soporte en Adquisiciones Efectuadas a Sujetos No Obligados a Expedir Factura**7.

* **EquivalentDocumentType**: Value Object con catálogo dinámico para los 12 tipos de documentos equivalentes oficiales (tiquete POS, boleta de cine, tiquete de transporte de pasajeros, extractos, peajes, servicios públicos)12.  
* **SupportDocumentReference**: Value Object que identifica las compras a no obligados a facturar, asegurando la generación del CUDE correspondiente14.

### **4.2. Nómina Electrónica**

Ubicada en el Bounded Context de Payroll, la nómina electrónica se estructura sobre el agregado PayrollDocument. Los Value Objects clave de este dominio son:

* **Cune**: Identificador criptográfico SHA-384 del documento de pago4.  
* **PayrollPeriod**: Rango de fechas inmutable que define la periodicidad de liquidación (StartDate, EndDate, PaymentDate).  
* **AccrualTypeCode** / **DeductionTypeCode**: Value Objects basados en catálogos DIAN para clasificar devengos (salario, horas extra, recargos, incapacidades) y deducciones (salud, pensión, retención en la fuente de trabajo)4.

### **4.3. Ecosistema RADIAN (Electronic Invoice como Título Valor)**

El sistema RADIAN (Resolución 000085 de 2022\) administra el registro, la negociación y la trazabilidad de las Facturas Electrónicas de Venta como Títulos Valores5.  
Los eventos RADIAN **no son simples cadenas ni enums de C\#**; constituyen **Domain Events** e **Integration Events** dentro del ciclo de vida del agregado ElectronicInvoiceTitle5.

| Código Evento | Nombre Oficial DIAN | Actor Emisor | Efecto Comercial y Legal | Clasificación DDD |
| :---- | :---- | :---- | :---- | :---- |
| **030** | Acuse de recibo de Electronic Invoice5 | Adquirente | Confirma la recepción del archivo XML y PDF5 | Domain Event / RADIAN Event |
| **031** | Reclamo de la Electronic Invoice5 | Adquirente | Rechazo explícito indicando motivo normativo5 | Domain Event / RADIAN Event |
| **032** | Recibo del bien y/o prestación del servicio5 | Adquirente | Habilita el plazo de 3 días para aceptación tácita5 | Domain Event / RADIAN Event |
| **033** | Aceptación Expresa5 | Adquirente | Otorga a la factura la calidad de Título Valor irrestricto5 | Domain Event / RADIAN Event |
| **034** | Aceptación Tácita5 | Emisor | Se opera tras 3 días hábiles del evento 032 sin reclamo5 | Domain Event / Policy Execution |
| **035-045** | Endoso, Mandato, Aval, Pago | Factor / Tenedor | Operaciones de circulación e intermediación financiera15 | RADIAN Transaction Entities |

Para respaldar el reclamo (evento 031), el Value Object RejectionReasonCode encapsula la codificación oficial de la DIAN5:

> 1. Documento con inconsistencias.  
> 2. Mercancía no entregada totalmente.  
> 3. Mercancía no entregada parcialmente.  
> 4. Servicio no prestado5.

## **5\. Modelo Tributario Nacional y Territorial: Impuestos, Retenciones y Monetización**

El cálculo de tributos en Colombia requiere un diseño financiero inmune a errores de redondeo y flotantes, asegurando precisión decimal estricta.

### **5.1. Dinero y Operaciones Financieras**

El modelado monetario evita el uso de primitivos decimales aislados, unificando la representación financiera en un conjunto cohesivo de Value Objects:

> 1. **Money**: Value Object principal. Estructura inmutable que contiene un decimal Amount y un VO CurrencyCode (código ISO 4217). Garantiza que las operaciones de suma y resta se realicen únicamente entre valores de la misma divisa, e implementa métodos de asignación monetaria proporcional sin pérdidas por redondeo (*pro-rata allocation*).  
> 2. **ExchangeRate**: Value Object para la conversión de divisas. Almacena SourceCurrency, TargetCurrency, Rate (con precisión de hasta 6 decimales) y RateDate.  
> 3. **TaxableBase**: Representa la base gravable sobre la cual se aplica una tarifa. Encapsula internamente un valor Money e impone la invariante de ser un valor no negativo (![][image13]).  
> 4. **UnitPrice**: Modela el valor unitario de bienes o servicios. Admite hasta 6 decimales de precisión pre-redondeo, conforme a las especificaciones del Anexo Técnico 1.9 de la DIAN1.

### **5.2. Motor de Impuestos Nacionales y Territoriales**

Los impuestos en Colombia se dividen en Impuestos Nacionales (IVA, Impuesto Nacional al Consumo \- INC, Impuesto a las Bebidas Azucaradas \- IBUA, Impuesto a los Comestibles Ultraprocesados \- ICUI)12 e Impuestos Territoriales (Impuesto de Industria y Comercio \- ICA).

                 \[TaxInformation\] (Value Object Compositor)  
                                     |  
         \+---------------------------+---------------------------+  
         |                                                       |  
  \[NationalTax\] (VO)                                      \[TerritorialTax\] (VO)  
  \- Type: TaxTypeCode (IVA, INC, IBUA)     \- Type: TaxTypeCode (ICA)  
  \- Base: TaxableBase                                     \- Base: TaxableBase  
  \- Rate: TaxRate                                         \- Rate: TaxRate  
  \- Amount: Money                                         \- Municipality: DaneMunicipalityCode

* **TaxTypeCode**: Value Object que envuelve los códigos de catálogo oficiales de la DIAN (01 \= IVA, 02 \= INC, 03 \= ICA, 22 \= IBUA, 23 \= ICUI)12.  
* **TaxRate**: Modela tanto tarifas ad-valorem (porcentajes como 19.00%, 5.00%, 0.00%) como tarifas específicas (valores monetarios fijos por unidad de medida, utilizados en impuestos saludables).  
* **TaxExemption**: Value Object opcional que justifica la exención o exclusión legal de un concepto gravado mediante el código DIAN correspondiente.

#### **Retenciones y Autorretenciones**

Las retenciones en la fuente no son impuestos independientes, sino anticipos del impuesto de Renta, IVA e ICA.

* **WithholdingTypeCode**: Código de catálogo para diferenciar Retefuente, ReteIVA y ReteICA.  
* **WithholdingRule**: Value Object que evalúa la aplicabilidad de la tarifa según la actividad y la base mínima expresada en Unidades de Valor Tributario (UVT).

#### **Tributación Territorial (ICA)**

El Impuesto de Industria y Comercio (ICA) varía según el municipio, la actividad económica CIIU y la tarifa aprobada por cada Consejo Municipal (expresada en por mil, por ejemplo 4.14 x 1000\)9. Para mantener la modularidad, el Bounded Context de TerritorialTax requiere la inyección del Value Object DaneMunicipalityCode.

## **6\. Ubicaciones, Unidades, Medios de Pago y Catálogos Dinámicos**

### **6.1. La Regla de Oro de los Catálogos Mutables**

**Regla Fundamental de Arquitectura**: Ningún catálogo normativo mutable emitido por la DIAN debe implementarse como un enum cerrado de C\#. Convertir catálogos dinámicos en enums viola el principio Open/Closed, forzando re-despliegues de código ejecutable ante actualizaciones administrativas menores (como la adición de nuevos motivos de nota crédito o nuevos tributos)12.  
Los catálogos deben modelarse como **Catalog-Driven Value Objects** que encierran un código alfanumérico inmutable y delegan la validación de vigencia a un servicio de catálogo respaldado por infraestructura.

### **6.2. Ubicación Geográfica, Unidades y Medios de Pago**

* **DaneMunicipalityCode**: Código geográfico de 5 dígitos (2 dígitos de departamento \+ 3 dígitos de municipio) definido por el DANE (por ejemplo, 11001 para Bogotá, D.C.).  
* **DaneDepartmentCode**: Código de 2 dígitos representativo del departamento.  
* **UnitOfMeasureCode**: Value Object que encapsula los códigos de unidades de medida estandarizados por la DIAN bajo la norma internacional UN/ECE Rec 20 (por ejemplo, 94 para unidad, KGM para kilogramo).  
* **PaymentMeansCode**: Value Object representativo del medio de pago (10 \= Efectivo, 48 \= Tarjeta Débito, 49 \= Tarjeta Crédito, 47 \= Transferencia Débito Bancaria).

## **7\. Infraestructura, Firma Digital, Información Exógena y Trazabilidad**

### **7.1. Firma Digital y Límites Criptográficos**

La firma digital bajo el estándar XAdES-EPES es un artefacto puramente técnico de seguridad e infraestructura. Contiene certificados X.509, huellas digitales (thumbprints), firmas RSA/ECDSA y sellos de tiempo (timestamps). Ninguno de estos componentes debe ingresar al modelo de dominio fiscal. La firma digital opera en la capa de transmisión de la infraestructura previo al envío del XML a los servidores de la DIAN1.

### **7.2. Información Exógena**

La información exógena (Artículo 631 del Estatuto Tributario) es un requisito de reporte de medios magnéticos. Los formatos de exógena (por ejemplo, Formato 1001 para pagos a terceros, Formato 1007 para ingresos) son **DTOs de Reporte/Exportation**. El dominio fiscal provee los Value Objects (Nit, Money, CiiuCode)9, pero la agregación y estructuración en archivos XML de exógena pertenece exclusivamente al Bounded Context de ExogenousReporting.

### **7.3. Auditoría y Trazabilidad Técnica**

Se separan conceptualmente los identificadores de trazabilidad técnica de los identificadores de dominio fiscal. Conceptos como CorrelationId, CausationId, AuditId e IdempotencyKey son **Technical Primitives** gestionados por middleware y frameworks de mensajería, quedando excluidos del Shared Kernel fiscal.

## **8\. Diseño Técnico en .NET 10 y Rendimiento (Native AOT)**

### **8.1. Asignación Cero de Memoria y Inmutabilidad**

Para alcanzar un rendimiento óptimo en escenarios de procesamientos masivos de documentos electrónicos, todos los Value Objects del dominio se definen como readonly record struct. Esta elección de diseño garantiza:

> 1. **Memory Allocation Zero**: Asignación directa en el *stack* o en memoria continua dentro del agregador padre, eliminando la presión sobre el Garbage Collector (GC).  
> 2. **Native AOT Compliance**: Compatibilidad nativa con compilación AOT en .NET 10 al prescindir de la generación de código dinámico mediante reflexión runtime.  
> 3. **Igualdad Estructural Nativa**: Proporcionada automáticamente por el compilador para tipos record struct.

### **8.2. Contratos de Parsing y Formateo Eficiente**

Todos los Value Objects primitivos implementan las interfaces unificadas de .NET para parsing estático de alto rendimiento sobre buffers de memoria: IParsable\<T\>, ISpanParsable\<T\> y IUtf8SpanParsable\<T\>.

### **8.3. Manejo de Errores mediante el Patrón Result\<T\>**

En el diseño de la API del dominio fiscal se prohíbe el uso de excepciones (Exceptions) para validar reglas de negocio o errores de parsing. El lanzamiento de excepciones introduce un costo inaceptable de desapilamiento de memoria (*stack unwinding*). Se adopta el patrón Result\<T\> para retornar errores de validación fuertemente tipados.

## **9\. Entregables Arquitectónicos Requireds**

### **A. Taxonomía Completa de Conceptos del Fiscal Domain**

El dominio fiscal colombiano se clasifica estrictamente en 13 categorías conceptuales DDD.

| Concepto del Dominio | Categoría DDD | Justificación Arquitectónica |
| :---- | :---- | :---- |
| **Nit** | Value Object | Identidad basada en valor, inmutable, validado por Modulo 112. |
| **VerificationDigit** | Technical Primitive | Byte derivado computable dentro de Nit3. No es VO autónomo. |
| **Taxpayer** | Entity / Aggregate | Posee ciclo de vida, estado mutable (dirección, régimen) e identidad. |
| **IdentificationDocument** | Value Object | Encapsula tipo de documento y número12. Inmutable por valor. |
| **RutDocument** | Snapshot / DTO | Captura de estado del RUT en un instante de tiempo9. |
| **AuthorizationResolution** | Entity / Aggregate | Posee ciclo de vida, vigencia temporal y estado de consumo. |
| **AuthorizationRange** | Value Object | Rango numérico inmutable (![][image14]). |
| **DocumentNumber** | Value Object | Consecutivo prefijado estructurado. Igualdad por valor. |
| **Cufe / Cude / Cune** | Value Object | Identificadores criptográficos SHA-384 inmutables4. |
| **RadianEvent** | Domain Event / Entity | Registro de evento legal en RADIAN con firma y timestamp5. |
| **Money** | Value Object | Valor monetario con divisa y reglas de redondeo. |
| **TaxRate** | Value Object | Porcentaje o tarifa fija aplicable a un impuesto. |
| **TaxTypeCode** | Catalog VO | Código de impuesto DIAN validado dinámicamente12. |
| **DaneMunicipalityCode** | Catalog VO | Código DANE de 5 dígitos para ubicación e ICA. |
| **CiiuCode** | Catalog VO | Código de 4 dígitos de actividad económica9. |
| **UblDocument** | Integration Model | DTO para serialización XML/UBL1. Fuera del dominio. |
| **DianResponse** | Integration Model | Respuesta SOAP/REST de la DIAN. Objeto de infraestructura. |
| **AuditCorrelationId** | Technical Primitive | Identificador de trazabilidad de infraestructura. |

### **B. Catálogo Maestro de Value Objects Aprobados**

Esta matriz especifica los Value Objects oficiales que forman el núcleo del dominio.

| Nombre del Value Object | Bounded Context | Tipo .NET | Invariantes Principales | Comportamiento / Métodos | Clasificación de Mutabilidad |
| :---- | :---- | :---- | :---- | :---- | :---- |
| **Nit** | Taxpayer / Shared | readonly record struct | 8-15 dígitos; DV válido según Modulo 112. | CalculateDV(), ToCanonicalString() | Stable |
| **IdentificationDocument** | Taxpayer / Shared | readonly record struct | Tipo válido; cadena no vacía ni alfanumérica extraña. | Matches(), IsNit() | Stable |
| **DocumentNumber** | Invoicing / Shared | readonly record struct | Prefijo de max 4 chars; número positivo ![][image15]. | GetFormattedNumber() | Stable |
| **AuthorizationRange** | Invoicing | readonly record struct | ![][image16]. | Contains(DocumentNumber) | Configurable |
| **Cufe** | E-Invoicing | readonly record struct | Hash SHA-384 válido de 96 caracteres hex8. | Compute(Invoice, TechKey) | Regulatory |
| **Cude** | DocumentSupport / CPE | readonly record struct | Hash SHA-384 válido de 96 caracteres hex8. | Compute(Document, SoftwarePin) | Regulatory |
| **Cune** | Payroll | readonly record struct | Hash SHA-384 válido de 96 caracteres hex4. | Compute(Payroll, SoftwarePin) | Regulatory |
| **Money** | Shared Kernel | readonly record struct | Moneda válida ISO; adición/sustracción mono-moneda. | Add(), Subtract(), Allocate() | Stable |
| **ExchangeRate** | Shared Kernel | readonly record struct | Tasa ![][image17]; monedas origen/destino distintas. | Convert(Money) | Stable |
| **TaxRate** | Tax / Shared | readonly record struct | Porcentaje ![][image18] y ![][image19], o tarifa fija ![][image17]. | CalculateTax(TaxableBase) | Configurable |
| **TaxTypeCode** | Tax | readonly record struct | Código perteneciente a catálogo oficial DIAN12. | IsIva(), IsInc(), IsIca() | Catalog-driven |
| **DaneMunicipalityCode** | Shared Kernel | readonly record struct | Cadena numérica de exactamente 5 dígitos. | GetDepartmentCode() | Catalog-driven |
| **CiiuCode** | Taxpayer / Tax | readonly record struct | Cadena numérica de exactamente 4 dígitos9. | MatchesActivity() | Catalog-driven |
| **RejectionReasonCode** | RADIAN | readonly record struct | Código oficial 01-04 para rechazos de facturas5. | GetDescription() | Regulatory |

### **C. Conceptos Descartados como Value Objects**

Para evitar la sobre-modelación, se detallan los conceptos excluidos explícitamente de ser Value Objects.

| Concepto Candidato | Categoría Reasignada | Justificación Técnica de Descarte |
| :---- | :---- | :---- |
| **VerificationDigit** | Primitive (byte) | Es un simple byte derivado computable dentro de Nit3. No posee semántica autónoma fuera de su NIT. |
| **TaxpayerId** | Descartado | Redundante. Genera colisión semántica con Nit e IdentificationDocument. |
| **TaxIdentificationNumber** | Descartado | Nombre genérico anglosajón. Debe usarse Nit en el dominio colombiano. |
| **InvoiceNumber** | Descartado | Unificado en DocumentNumber para evitar duplicidad entre facturas, notas y documentos equivalentes. |
| **Rut** | Snapshot / Entity | El RUT es una estructura agregada compleja con múltiples hojas, estados y firmas9. No es inmutable por valor. |
| **AuthorizationResolution** | Entity / Aggregate | Tiene ciclo de vida, vigencia en fechas y estado consumido. No es estructuralmente inmutable. |
| **UblVersionId** | DTO / Integration | Parámetro técnico del XML UBL (2.1)1. El dominio no debe saber nada de la versión del XML de transporte. |
| **CustomizationId** | DTO / Integration | Parámetro de infraestructura del XML UBL1. |
| **DigitalSignature** | Infrastructure Model | La firma XAdES/XMLDSig es un artefacto de seguridad e infraestructura criptográfica. |
| **DianResponseStatus** | Integration Model | Código de estado HTTP/SOAP devuelto por los servidores de la DIAN. |
| **ExogenousFormat1001** | Reporting Model | Estructura de reporte de salida para la exógena. DTO de exportación, no VO de dominio. |

### **D. Matriz de Duplicidades y Consolidación**

Decisiones de unificación para eliminar abstracciones solapadas.

| Concepto A | Concepto B | Decisión Arquitectónica | Tipo Resultante Consolidado | Rationale |
| :---- | :---- | :---- | :---- | :---- |
| Nit | TaxpayerId | Fusionar | Nit | Evita duplicidad. Nit es el término legal y técnico en Colombia9. |
| VerificationDigit | CheckDigit | Consolidar en Nit | Primitive dentro de Nit | El DV no existe por sí solo en el negocio fiscal3. |
| DocumentNumber | InvoiceNumber | Fusionar | DocumentNumber | Mismo patrón de prefijo \+ consecutivo para todos los documentos electrónicos. |
| TaxAmount | Amount | Fusionar | Money | Todo valor monetario debe usar la estructura unificada Money. |
| TaxableAmount | TaxBase | Fusionar | TaxableBase | Concepto tributario único que representa el valor gravable sobre el que actúa la tarifa. |
| Cufe | Cude | Separar con base común | Cufe / Cude | Poseen algoritmos de concatenación y semánticas legales distintas (Factura vs Nota/Doc Soporte)8. |
| CiiuCode | EconomicActivityCode | Fusionar | CiiuCode | Término estándar adoptado expresamente por la DIAN y el DANE9. |
| Address | FiscalAddress | Especializar | FiscalAddress | La dirección fiscal incluye códigos DANE territoriales obligatorios no presentes en una dirección simple. |

### **E. Bounded Context Matrix**

Distribución de Value Objects a través de los Bounded Contexts del sistema.

| Bounded Context | Value Objects Propietarios | Value Objects Consumidos del Shared Kernel | Bounded Contexts Exportables |
| :---- | :---- | :---- | :---- |
| **Shared Kernel** | Money, ExchangeRate, TaxRate, IdentificationDocument, DaneMunicipalityCode | N/A (Módulo Raíz) | Todos |
| **Taxpayer** | Nit, FiscalAddress, CiiuCode | IdentificationDocument, DaneMunicipalityCode | Invoicing, Payroll, RADIAN |
| **Invoicing & E-Invoices** | DocumentNumber, AuthorizationRange, Cufe, Cude | Money, TaxRate, IdentificationDocument | RADIAN, Compliance |
| **Payroll** | Cune, PayrollPeriod, AccrualTypeCode | Money, IdentificationDocument | Compliance |
| **RADIAN** | RejectionReasonCode, TitleState | DocumentNumber, Money | Compliance |
| **Territorial Tax** | IcaTariffCode | Money, TaxRate, DaneMunicipalityCode | Invoicing |

### **F. Dependency & API Boundary Matrix**

Reglas estrictas de aislamiento arquitectónico entre paquetes.

| Paquete Propietario | API Pública Expuesta | API Interna Oculta | Puede Consumir | NO Puede Consumir |
| :---- | :---- | :---- | :---- | :---- |
| EricksonLopez.SharedKernel | Money, TaxRate, IdentificationDocument | Helpers de cálculo numérico | Primitivas .NET BCL | Ningún paquete fiscal o DIAN |
| EricksonLopez.Fiscal.Colombia | Nit, FiscalAddress, CiiuCode, DaneMunicipalityCode | Algoritmo Modulo 112 | SharedKernel | Integración DIAN, XML/UBL |
| EricksonLopez.EInvoice | Cufe, Cude, DocumentNumber, AuthorizationRange | Lógica de concatenación SHA-3848 | SharedKernel, Fiscal.Colombia | Paquetes de Nómina o RADIAN |
| EricksonLopez.Payroll | Cune, PayrollPeriod | Formateadores de nómina4 | SharedKernel, Fiscal.Colombia | EInvoice, RADIAN |
| EricksonLopez.DianIntegration | Adapters SOAP/REST, UBL Mappers1 | DTOs XML, Serializadores XAdES | Todos los dominios anteriores | Ninguno (Es capa exterior) |

### **G. Package Boundary Matrix**

Organización física de proyectos y librerías en .NET 10\.

| Proyecto / Ensamblado .NET 10 | Target Framework | Dependencias Permitidas | Compatibilidad Native AOT |
| :---- | :---- | :---- | :---- |
| EricksonLopez.DomainPrimitives | net10.0 | Ninguna (Solo BCL) | 100% (Zero Allocations) |
| EricksonLopez.SharedKernel | net10.0 | DomainPrimitives | 100% |
| EricksonLopez.Fiscal.Colombia | net10.0 | SharedKernel | 100% |
| EricksonLopez.EInvoicing | net10.0 | Fiscal.Colombia | 100% |
| EricksonLopez.Payroll | net10.0 | Fiscal.Colombia | 100% |
| EricksonLopez.Radian | net10.0 | EInvoicing | 100% |
| EricksonLopez.Dian.Infrastructure | net10.0 | Todos los anteriores \+ System.Private.Xml | Compatible mediante AOT Source Generators |

### **H. Roadmap de Implementación**

Fases de desarrollo ordenadas por dependencia técnica y prioridad fiscal.

| Fase | Hito / Entregables | Value Objects Involucrados | Criterios de Aceptación y Validación |
| :---- | :---- | :---- | :---- |
| **Fase 1 (P0)** | Core Primitives & Identification | Money, Nit, IdentificationDocument, DaneMunicipalityCode | Algoritmo Modulo 11 probado con 100,000 NITs reales2; parsing zero-allocation. |
| **Fase 2 (P0)** | Invoicing Base & Cryptography | DocumentNumber, AuthorizationRange, Cufe, Cude | Hashes SHA-384 validados contra la herramienta de pruebas DIAN (Anexo 1.9)1. |
| **Fase 3 (P1)** | Tax Engine & Territorial Tax | TaxRate, TaxTypeCode, TaxableBase, IcaTariffCode | Precisión matemática verificada en cálculos de IVA, INC, Retefuente y ReteICA12. |
| **Fase 4 (P1)** | Electronic Payroll & Support Doc | Cune, PayrollPeriod, EquivalentDocumentType | Generación de CUNE en cumplimiento con la Res. 000013/20214. |
| **Fase 5 (P2)** | RADIAN & Commercial Events | RejectionReasonCode, TitleState | Trazabilidad de los 5 eventos RADIAN (030-034)5. |
| **Fase 6 (P3)** | Compliance & Exogenous Prep | ExogenousConceptCode | Proyección de datos de dominio hacia DTOs de exógena sin alterar el kernel. |

### **I. Architecture Decision Records (ADRs)**

#### **ADR-001: Consolidación del NIT y el Verification Check Digit**

* **Estatus**: Aprobado.  
* **Contexto**: Existía la duda sobre si el Verification Check Digit (DV) debía ser un Value Object independiente o un campo dentro de Nit3.  
* **Decisión**: Consolidar VerificationDigit como un campo privado/propiedad interna (byte) dentro del VO Nit. La validación jurídica (Oficio DIAN 909585/2021) establece que el DV no forma parte del número de identificación, sino que es un mecanismo de comprobación3. Por ende, el VO Nit garantiza internamente la validez de su DV mediante el algoritmo del Modulo 112.  
* **Consecuencias**: Se simplifica la API del dominio, evitando firmas de métodos con parámetros redundantes (string nit, byte dv).

#### **ADR-002: Reemplazo de Enums C\# por Catálogos Dinámicos DIAN**

* **Estatus**: Aprobado.  
* **Contexto**: La DIAN publica y modifica con frecuencia resoluciones que alteran catálogos (tipos de documentos, tarifas de impuestos, motivos de rechazo)12.  
* **Decisión**: Prohibir el uso de enum en C\# para representar catálogos mutables de la DIAN. En su lugar, utilizar readonly record struct que validan el código ingresado contra un repositorio de catálogos injectable en la capa de aplicación/infraestructura.  
* **Consecuencias**: Modificaciones normativas de la DIAN no requieren re-compilación de binarios del dominio fiscal.

#### **ADR-003: Modelado de Identificadores Criptográficos (CUFE / CUDE / CUNE)**

* **Estatus**: Aprobado.  
* **Contexto**: Se requería decidir si CUFE, CUDE y CUNE debían fusionarse en un único tipo FiscalHash.  
* **Decisión**: Crear un readonly record struct para cada identificador (Cufe, Cude, Cune), derivando semánticamente de la abstracción común ElectronicDocumentIdentifier.  
* **Consecuencias**: Aunque técnicamente los tres representan una cadena SHA-384 de 96 caracteres hex8, mantener la separación tipada evita que se asigne por error un CUNE en una factura electrónica o un CUFE en un documento soporte4.

#### **ADR-004: Adopción Exclusiva de readonly record struct para Value Objects**

* **Estatus**: Aprobado.  
* **Contexto**: Compatibilidad con Native AOT y optimización de memoria en .NET 10\.  
* **Decisión**: Definir todos los Value Objects del dominio fiscal como readonly record struct.  
* **Consecuencias**: Asignaciones cero en el *heap*, inmutabilidad garantizada por el compilador, rendimiento óptimo y eliminación de GC overhead durante el procesamiento masivo de facturas.

#### **ADR-005: Aislamiento Total del Dominio frente a Schemas XML/UBL 2.1**

* **Estatus**: Aprobado.  
* **Contexto**: El estándar UBL 2.1 utilizado por la DIAN introduce elementos XML complejos (CustomizationID, ProfileID, AccountingSupplierParty)1.  
* **Decisión**: El modelo de dominio no contendrá ninguna anotación, atributo ni tipo derivado del esquema XML/UBL1. La transformación de y hacia UBL se realizará exclusivamente en la capa Dian.Infrastructure mediante Mappers y DTOs dedicados.  
* **Consecuencias**: Desacoplamiento total de cambios en el esquema técnico de transmisión de la DIAN.

#### **ADR-006: Uso del Patrón Result\<T\> en Lugar de Excepciones para Validaciones**

* **Estatus**: Aprobado.  
* **Contexto**: El lanzamiento de excepciones en validaciones de VOs afectaba severamente el rendimiento en pruebas de carga.  
* **Decisión**: Todos los métodos de creación y parsing de VOs (Create, TryParse) retornarán un tipo Result\<T\> que encapsula el éxito o la lista de errores de validación.  
* **Consecuencias**: Ejecución de código determinista, cero *stack unwinding* y mejor ergonomía de desarrollo.

#### **ADR-007: Delimitación de la Tributación Territorial (ICA)**

* **Estatus**: Aprobado.  
* **Contexto**: El impuesto ICA depende de regulaciones de más de 1,100 municipios de Colombia.  
* **Decisión**: El núcleo del dominio solo incluye las primitivas para parametrizar la tarifa ICA vinculada al código DANE municipal (DaneMunicipalityCode). Las reglas de cálculo específicas por municipio residen en un Bounded Context especializado (TerritorialTax).  
* **Consecuencias**: Mantiene el Shared Kernel ligero y evita contaminar la facturación nacional con especificidades locales.

#### **ADR-008: Definición de Entidad para las Resoluciones de Numeración**

* **Estatus**: Aprobado.  
* **Contexto**: La resolución de facturación DIAN contiene rangos, vigencia y prefijo.  
* **Decisión**: Modelar la Resolución de Facturación como una **Entity / Aggregate Root** (AuthorizationResolution) y no como un Value Object.  
* **Consecuencias**: Permite gestionar el estado mutable de la resolución (consecutivos consumidos, estado de inactivación) manteniendo sus componentes (AuthorizationRange, DocumentPrefix) como Value Objects inmutables.

#### **ADR-009: Inmutabilidad de los Objetos Monetarios**

* **Estatus**: Aprobado.  
* **Contexto**: Riesgo de errores de redondeo en importes de impuestos y totales.  
* **Decisión**: El VO Money no permite mutación interna ni redondeos imprecisos implícitos. Todas las operaciones retornan una nueva instancia de Money ajustada al exponente decimal especificado por la divisa.  
* **Consecuencias**: Consistencia contable y reproducibilidad exacta de cálculos.

#### **ADR-010: Tratamiento de los Eventos RADIAN**

* **Estatus**: Aprobado.  
* **Contexto**: Los eventos 030, 031, 032, 033 y 034 de la DIAN poseen efectos jurídicos comerciales5.  
* **Decisión**: Modelar los eventos RADIAN como Domain Events e Integration Events, desacoplándolos de meras constantes de tipo string.  
* **Consecuencias**: Se habilita el disparo de reglas de negocio (como el cómputo de 3 días para aceptación tácita) de forma limpia dentro del dominio.

#### **ADR-011: Separación de Representaciones de Persistencia y Dominio**

* **Estatus**: Aprobado.  
* **Contexto**: Mapeo ORM (EF Core / Dapper) hacia PostgreSQL.  
* **Decisión**: Los Value Objects se persisten en base de datos utilizando conversores de valor (*Value Converters*) o tipos estructurados compostables en columnas planas.  
* **Consecuencias**: Impedancia nula entre el modelo relacional y el modelo de objetos inmutables.

#### **ADR-012: Versionamiento de Anexos Técnicos de la DIAN**

* **Estatus**: Aprobado.  
* **Contexto**: Transición del Anexo Técnico 1.8 al 1.91.  
* **Decisión**: El dominio permanece inalterado frente a versiones de anexos. Las diferencias de formato XML de los anexos son resueltas por Source Generators en la capa de infraestructura.  
* **Consecuencias**: Portabilidad del motor fiscal a lo largo de futuros cambios reglamentarios de la DIAN.

### **J. Análisis de Riesgos y Estrategias de Mitigación**

| Riesgo Detectado | Impacto en Arquitectura | Probabilidad | Estrategia de Mitigación |
| :---- | :---- | :---- | :---- |
| **Primitive Obsession residual** | Medio | Alta | Auditoría de código mediante analizadores estáticos de Roslyn para prohibir string o decimal directos en firmas de dominio. |
| **Acoplamiento a Anexo Técnico DIAN** | Crítico | Alta | Aislamiento mediante Capa Anticorrupción (ACL) y pruebas de contrato con Golden Tests para XML/UBL1. |
| **Contaminación por cambios normativos** | Alto | Media | Despliegue de catálogos respaldados en base de datos/Redis con refresco dinámico sin reinicio de aplicación. |
| **Pérdida de Performance por AOT** | Medio | Baja | Validación continua de la compilación Native AOT en el pipeline de CI/CD para detectar reflexión no intencionada. |
| **Complejidad excesiva en Dev Experience** | Medio | Media | Provisión de Source Generators que implementen automáticamente los contratos IParsable\<T\> y operadores implícitos/explícitos seguros. |

## **10\. Respuesta Definitiva y Especificación del Núcleo del Dominio**

El **conjunto mínimo, completo y arquitectónicamente correcto** de Value Objects que debe constituir el núcleo del dominio fiscal colombiano está integrado exclusivamente por **14 Value Objects primarios**, clasificados estrictamente por sus límites de contexto:

### **1\. Pertenecientes al Shared Kernel**

* **Money**: Estructura monetaria inmutable con divisa ISO y operaciones de asignación proporcional.  
* **ExchangeRate**: Conversión cambiaria con tasa de precisión de 6 decimales y fecha de vigencia.  
* **TaxRate**: Modela porcentajes ad-valorem o tarifas fijas por unidad de medida.  
* **IdentificationDocument**: Identificador abstracto universal para cualquier documento personal o corporativo (CC, CE, Pasaporte, PPT)12.  
* **DaneMunicipalityCode**: Código DANE de 5 dígitos para ubicación geográfica y liquidación territorial.

### **2\. Pertenecientes al Contexto Fiscal Colombiano (Fiscal.Colombia & EInvoicing)**

* **Nit**: Identidad tributaria con validación del Modulo 11 y DV integrado2.  
* **FiscalAddress**: Dirección fiscal estructurada asociada a códigos DANE territoriales.  
* **CiiuCode**: Clasificación industrial uniformada de 4 dígitos para actividades económicas9.  
* **DocumentNumber**: Consecutivo estructurado con prefijo y número para todos los documentos fiscales.  
* **AuthorizationRange**: Rango numérico autorizado por la DIAN.  
* **Cufe**: Hash SHA-384 exclusivo para Facturas Electrónicas de Venta8.  
* **Cude**: Hash SHA-384 para Notas, Documentos Soporte y POS Electrónico8.  
* **TaxTypeCode**: Código dinámico de catálogo para tributos nacionales y locales12.

### **3\. Pertenecientes a Contextos Especializados (Payroll y RADIAN)**

* **Cune**: Hash SHA-384 para el Documento Soporte de Pago de Nómina Electrónica4.  
* **RejectionReasonCode**: Motivo normativo codificado para la gestión de rechazos de facturas en el ecosistema RADIAN5.

### **Conceptos que Permanecen Fuera del Dominio (Infraestructura / Integración)**

Permanecen estrictamente fuera del dominio: las clases de representación XML/UBL (InvoiceType, AccountingSupplierParty)1, los DTOs SOAP/REST de respuesta de la DIAN (DianResponse), los artefactos de firma digital XAdES, las entidades con ciclo de vida (Taxpayer, AuthorizationResolution) y los formatos de exportación masiva para Información Exógena. Esta demarcación garantiza un núcleo de dominio puro, de ultra alto rendimiento en .NET 10, inmutable y totalmente protegido frente a la volatilidad reglamentaria de la autoridad tributaria.

#### **Fuentes citadas**

> 1. Anexo Técnico \- Electronic Invoice v1.9 | MATIAS API, [https://docs.matias-api.com/docs/regulatory-framework/factura-electronica/technical-annex](https://docs.matias-api.com/docs/regulatory-framework/factura-electronica/technical-annex)  
> 2. Número de Identificación Tributaria (Colombia) \- Wikipedia, la enciclopedia libre, [https://es.wikipedia.org/wiki/N%C3%BAmero\_de\_Identificaci%C3%B3n\_Tributaria\_(Colombia)](https://es.wikipedia.org/wiki/N%C3%BAmero_de_Identificaci%C3%B3n_Tributaria_\(Colombia\))  
> 3. oficio 909585 de 2021 \- Compilación Jurídica DIAN, [https://normograma.dian.gov.co/dian/compilacion/docs/oficio\_dian\_909585\_2021.htm](https://normograma.dian.gov.co/dian/compilacion/docs/oficio_dian_909585_2021.htm)  
> 4. Páginas \- Documento Soporte de Pago de Nómina Electrónica \- DIAN, [https://www.dian.gov.co/impuestos/Paginas/Sistema-de-Factura-Electronica/Documento-Soporte-de-Pago-de-Nomina-Electronica.aspx](https://www.dian.gov.co/impuestos/Paginas/Sistema-de-Factura-Electronica/Documento-Soporte-de-Pago-de-Nomina-Electronica.aspx)  
> 5. Eventos Facturación Electrónica | Softinm, [https://www.softinm.co/manual/facturacion\_electronica/Eventos\_Facturacion\_Electronica](https://www.softinm.co/manual/facturacion_electronica/Eventos_Facturacion_Electronica)  
> 6. \[l10n\_co\] Eventos RADIAN (Gestión de eventos en facturas) \- Odoo, [https://www.odoo.com/forum/help-1/l10n-co-eventos-radian-gestion-de-eventos-en-facturas-286599](https://www.odoo.com/forum/help-1/l10n-co-eventos-radian-gestion-de-eventos-en-facturas-286599)  
> 7. Anexo técnico 1.9 de facturación electrónica | Fechas \- Siempre al Día, [https://siemprealdia.co/colombia/impuestos/fechas-implementacion-anexo-1-9-de-facturacion-electronica/](https://siemprealdia.co/colombia/impuestos/fechas-implementacion-anexo-1-9-de-facturacion-electronica/)  
> 8. Reglas de rechazo anexo técnico V 1.9 \-Cambios Integración Anexo V 1.9 \- tfhkacolwiki, [https://felcowiki.thefactoryhka.com.co/index.php/Reglas\_de\_rechazo\_anexo\_t%C3%A9cnico\_V\_1.9\_-Cambios\_Integraci%C3%B3n\_Anexo\_V\_1.9](https://felcowiki.thefactoryhka.com.co/index.php/Reglas_de_rechazo_anexo_t%C3%A9cnico_V_1.9_-Cambios_Integraci%C3%B3n_Anexo_V_1.9)  
> 9. Dígito de verificación del NIT: ¿qué es y cómo calcularlo? \- Actualícese, [https://actualicese.com/guia-consulta-digito-de-verificacion-nit/](https://actualicese.com/guia-consulta-digito-de-verificacion-nit/)  
> 10. Digito de verificacion de NIT \- consultorcontable.com Contabilidad \- Impuestos, [https://www.consultorcontable.com/digito-de-verificacion-de-nit/](https://www.consultorcontable.com/digito-de-verificacion-de-nit/)  
> 11. Calculadora del dígito de verificación DIAN \- Tickelia, [https://tickelia.com/co/blog/calculadoras/digito-de-verificacion-como-usarlo/](https://tickelia.com/co/blog/calculadoras/digito-de-verificacion-como-usarlo/)  
> 12. Anexo técnico 1.9: lo nuevo de la facturación electrónica en Colombia \- Blog de Alegra, [https://blog.alegra.com/colombia/facturacion-electronica-anexo-tecnico-1-9/](https://blog.alegra.com/colombia/facturacion-electronica-anexo-tecnico-1-9/)  
> 13. Cambios en la facturación electrónica con el Anexo 1.9 \- OasisCom Software ERP, [https://www.oasiscom.com/blog/cambios-en-la-facturacion-electronica/](https://www.oasiscom.com/blog/cambios-en-la-facturacion-electronica/)  
> 14. Validación del Documento Soporte para Adquisiciones con No Obligados a Facturar \- DIAN, [https://www.dian.gov.co/Prensa/Paginas/NG-Validacion-del-Documento-Soporte-para-Adquisiciones-con-No-Obligados-a-Facturar.aspx](https://www.dian.gov.co/Prensa/Paginas/NG-Validacion-del-Documento-Soporte-para-Adquisiciones-con-No-Obligados-a-Facturar.aspx)  
> 15. Compilación Jurídica del MINTIC \- Resolución 15 de 2021 DIAN, [https://normograma.mintic.gov.co/mintic/compilacion/docs/resolucion\_dian\_0015\_2021.htm](https://normograma.mintic.gov.co/mintic/compilacion/docs/resolucion_dian_0015_2021.htm)

[image1]: <data:image/png;base64,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>

[image2]: <data:image/png;base64,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>

[image3]: <data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAmwAAABMCAYAAADQpus6AAAFWElEQVR4Xu3dTcgtYxwA8OfGLbpErnDz1ZWNkqu4onyV6+bmo4QoCwu5LGxQRBYXiZIFCwsl3SSFjVIWbnrLxsLGwoaNJMrGyoISz9/M6Zx37rnvnTnvzDlnZn6/+r9nzvOcjznzzsd/npl5JiXoxI5qwXKt+OsBAAAAlkdDyLKY0gAAAMACNCkAAAAMj3092C5LEQAA9I0sHgCgP+RuAAAAnIx9R1bFvAcA9JEcZvtMQxgRCzzQrl05zi6HD+Z4IMeBaTUAAKt2TY495fAT5XMAANZINWG7K8e+aTW0wwEiAFjcbMI2sT/H3koZAAArMpuwfZDj9rLMoVFapYUNarGoNGJyMQ6n5fgix6c5DuV4KMdTOTZmXtOhEy1oJyoHWIR1CgCDZ2NXm0kFUJMVJsBKWP0yX+M5o/Eb1kifxx0AAACgFzTAACf3fI5/c/xarZjjyhwvpOL1RzZXQUtsuwBgrkjWIgl7NtXfXH6V48lqIQDA2O3OcWs5XO3Ydjvuy/FPGdHnWh2X5jhaLaw4JccZ1UIAgCGKpOeVHDvL59Ei9v60uhVnpaKVLWLyPdsVh08vqxYCAAxRdF57/szzD3M8PvO8LX+kImF7plqxoOh8d+UtbHWP8QIwYDYGA7f6f3AkPBs5HkzTsbkqx+mTF7QoWtbe3VEkbXH4dVEHU3Ebq7+qFQ1cnDaPw67y8ebUzW8HANiW2cOVEfNarS7P8W2On7eIuiLRivPZ4ty2JuKihR/L4Ui2NqZVjcR7I37JcVGOAzn+LuuiFfDhcrgDq8/QKfhPANBH0fp1fSoSl0hguhSJV92uPmZFonekHL4wx9vTqvRYKpKvWZGIfpaOT0CvLes+ynFqjldTkbyFOHdv8vtvKx+BdSDLBkbsxcrzu3PcWCkLcUXmeam4evRE0cTvqUjc6oqk6/sc55bPn8txxbR6rli976sWluL9kwsWIlmLCxjCW6lI4gAA1sZsK1WIq0WrLVIhzvG6M8f9W0Rd/5/LVj7WFeN0LBXjEQlVXHBwTo5LUpFgHpq+tJboMuTMcvjPVCSqIc7lC+/keLkcBgBYmTis+HSOn3I8kook5pbZF3QgWr2+rhbWFO+LRO21VLQMfp6KpO2C1Py8s+jnLT7vkxxvpqKVLT4vREJ4T46Py+cAjJsD8ozKdTl+qBa2ILoguSG1m2x+k4pbZVlIgYVYeQB99V2qf2VorOuif7g6Hk3FPUvb7I4jbo3lkCgAMBpxReaXqdkOZ3QT0vQwJyygyWwJAMMVFxhE1PVGKs4p21utoF+kQgAMz9yt29zCWmbfOR1e/PMWNOlzrWncEW+mI0ufDVg7azUPrNXIDJxpDQNmAWedmB8ZGvM0AMAaGUtyNpbfCazWOq5r1nGcAIDBiW46tko7om5/tRAAgOU5XC2oiDsNxP1CAQDat1Wz0ThtmiJx/8+XUtEf2yZzptvRagH025y5HADW0L05rk7F7aPihusbc2JCwgYA0ExrrQNHysf4wD35z554nIkJCRsAwArsznEsx03Vior3cvyW4/VqBQATre1Iw+r0ZjbuzYi2Zme1AAAAemV8OTzdM1cBAKyYhAwA4HiTHEmuBAAAAIzS4BtFBv8DAYABkbkAy2WtAwAAAADM0dumw7ZGvK3PAQDgeHItAAAAAABapukZAAA6JukGWBZrXAAAADphhxMAAKA9jfaxGr0YAGjMthYAAAAAAAA64VAcAAAAbIMdawAYKUnAkC3nv7ucb+mBpU2Ibr6om09driH8BlgXlicYIQs+AAAdkm4CAF2Tb0BPWXgBAACAXtCIwaiY4QGgFptMYKz+AwKJhB+ZVa0MAAAAAElFTkSuQmCC>

[image4]: <data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAmwAAAAxCAYAAABnGvUlAAAEy0lEQVR4Xu3cy+s1YxwA8OfNpeSWSy5hIymFjezYYSnZ2NgpZEWKXBYkfwAWb6FkISU7kmy8pSzYiZSyIJcorGzI5fm+M/OeOeOc45yZOb8zl8+nvp2ZZ2bOby7PzHzP88z8UoLSsWYBk+VYw0A5OQEAYIx6zeR7/TIOyaFk1pwAh2Cv70eb/dpmGQAYoRHc8kawijA+Tqz9sn/3wm6lC/XncOx7AIC9kWoBwNFz/wUAOEqyLwAAAAbGT1UAANie/BkOw7kHTIlrGsAcXZCGfQcY+voxRmoUp8ytMoxse0e2ulB3Wo67ys+oyvHZ1hk57m4W9uSS1G3dKo/meKRZCACzI4Htxas5/iyHI1G5P8djqd/d+3WOO8rhe3P8XZvWxqfNgp7ENn+T4/LmhNJDOf7K8Xqt7KwcL+T4PsdNtfJwfY6rGmVs0GelA4Ap+SXH27Xxm3N8kePiWlkXV+d4sjYeyVA94dnVFTluaxb2aFPCFmL68voXWcbj6b8JW4jkMrpHga5k9LDO3M6OuW3vSf/keKA2/kwquvP6EknMVzmuKcejRerGxeSdRbJ2ZW08hqP1LrpJo8s1WrTiQF6b47rafCHmieTx7Eb5Ran4jvNTm4QtnfyD6xK2mD+SYKAHs7xKQxtOlkk5J8cfOT7O8WWO46lIXlZ5P8e3G6K0sobEd0diGPFwY9quIlk6vTYeCdt3OS7NcW6Oj3LcWk57Pi2Sr89zPFcOR7fsT+XwPWkxf6x8q4QtrU/YTqRiGgBAK9ECFd2hkQBFshLPZ92+NEd/ovXqw9S9u7WZLEVyFUlUiAT0RCoStxCJUpV8xXNzVVdqJFa/p8X89S7LjQnbsXYJWySOdLDyZwAzogYclN3P/1JJ9u3FHLfUxqMFbF1yES1vkcisi1WeTsvfH7o+09VMllYlbPEZ6glbvFhRrUuVsEX3aSxbzR82JmypXcKmhc25PBDtDkS7pWCSnA4cuXiD8be06F6Mz0jYIrl4KvXzYP+POV6rjT+Ripa2LuJ5u3oLXdUlGlYlbNVzbB/keKscjq7R6CIN0aIY3aLhwlSsc7wosU4kbG80C1Pxt1Y9qxZvyPaxL5e5ZADA5MUzW9UzZT/nuLMsjy7RSGpeScXLAV29meOlVLTa3ZeW30ZtK1qx6i8TxDN4sR2f5Pi1HI7PeE4thmP6gznOS8W6vJyK9bgsFk5F6hMJX7SavVMuE/thlZiv2m+f5bghFS8wVH834odTcxeiCzjebAUAmJX3mgUDFd3I1QsNwCRp7gZY591mwUBFK1/8K5HxcO8BAHr0bI4zm4UDEt3A9f8Xd3iSMQBgaxKHyXAomSUVnzlQz2lP7dmGvQQMmosUMEkubgAAAEyDX7gAAPMxzNyv/7Xq/xsZNAccAAAAoG7irSVbb97WM9KK/QsA0A95FbCO6wOdba5Em6cC8zCYK8FgVgQAAACGw8/lVuw2YMpc48bJcQMAAOZjVr+AZrWxDNj0auI4tmgcawkAAACMlbYH2J3zBoBZcgMEoEduKwDz4roPTIqLGoOlcsLunDdwAE48ADpzM2HO1H84ck67Edn5YO28AADAhMiFJsuhnQEHeT/sV2CM2l272i0FAEyEVGDEHDz2TiUDkksBQJ/+BSn/ickN0Oc9AAAAAElFTkSuQmCC>

[image5]: <data:image/png;base64,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>

[image6]: <data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABYAAAAaCAYAAACzdqxAAAACOElEQVR4XqVVPUicQRCdAa8Id00kBA4MCAkBCaTw6uu80iZgpb1NCrvDIyRFAoZUpghpgnBV0tpLOLBIYWEvWFilsrRIE32zsz+z8313Fj6Y2515s29nZ/fuiIjJw0esz9krUTur17IhvapBY1HledEEW0iw9rQKKbcdyuT9TPXlFCaYY9GyY0fFE9gf2K2xv9hogHEFdu64C9jLqLETYixxlvEYfjcwBuOQRPRWHNsNjBPh4L5JnMFr2G/Yagr4w22T7i4bGIS0CeumkmPbLx18h3EkabVgqopok3SxVGepVcwuo/BYg5zEX8G+wDrzXwXRBuw/6KmJSfYnjIfkhIEl2AGpuCnXlB53l8u6gVnhAajPpL2VHltuCIH9qjGkmr72JHxCerMd2FekvDCcCjP18Pkd9iz4Mch5j1q4D7uCzUDIwhHovchBmMOmWN5lvcRdpXK/5yIJn5FUyXyE8WncfQ12DYkZ3OeYf4P14rnTh6szgx+Tior4B9hWiCrZZ4kznWL+nuR5ZRi5UHpTvhcqwssA9wvjI5PTx6Ir1nf+k6T/ptIC72tALkwu7h/8od2b5dih9+EC5SJrNPQSisoU9oP0jRaSuYdTzjD9mDLLprq4RVtDkZJq5EfJIXDrsGVHOBj5UqxUVnMZbbEHoZy3FXPCikDGZ7Iw0SAdzOd7PyI3pJHRWGADja9dRfpAyffLFmNBdpMqkdQsnzPHv6e3XBY281xE3Bwy/8BZwOTbetumbYWJfwcluUf/Gce2iQAAAABJRU5ErkJggg==>

[image7]: <data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAA4AAAAaCAYAAACHD21cAAABu0lEQVR4XoWTvy9EQRDHZ+MuIS4kLjoauUaiIxHdVaK6hoSE7gqJQkWEaEUkCqFQ6zQqvYiSSqGRSEj8Awrl4bM/37y9wyTfmdmZ787Mvt0nYgRxykt0U9yU/bLoQCQq3+SFdZE8lxdIOl9FiXwjNXQTzIPh0KMORhU7iC9SZbR97AvYBBvggdwx6TvsZDaeq1lBnYNLjjXoYy5ZR9/j3BKpBa5FcIzM4byBKc+PtV1uD3UaKyUJ7gF4B2OBXBQ2so3XShu8pJEu8L7xdln0lSgiDWIjupmWFfEbgelgb+C18YdicaPv1AW84YvKIejAsJsdyD2KHT/u+aWrTdgx7Wc/wn6IncDIujpSidhwXvgmquoC032x3OnVaQKcMV7F71AMI9PoT3KrulOUFv41y37fzYR37dQa9hk7HsnpYWC5P0NVmS3mdDPb8fkwZtGSC7pX9jFf0WEL/4lWV0TbJE6IvxJbjtyyGBlAN0KmCmbAEokmtj9uiddXVLATpRnCiH+KPf9/nCCJ+NuG7PiZ6FiaTI2Y1mXdQ0zPnP+T1CLwMmqIdEcLnZ9TvZCii4qVvdIi1tVp5XdfoJMfA5AsUn3tSdUAAAAASUVORK5CYII=>

[image8]: <data:image/png;base64,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>

[image9]: <data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAADYAAAAaCAYAAAD8K6+QAAADG0lEQVR4Xr1WzatPQRh+p3stxLUhuktRIgu6yYYFYaFQ7kaJjcJOEfKxsLGSzd0oH/8A9ixFidTdycrChlIoxYLE887M78zHO++ZOff+rqee3++c5/2Y8847M+cQtcI0XJeFAtIE4U6L1fSxoTBAQVo0KjkrZgfNSdPraIsUXkKI0WvsgY9LF0i4GprW+leChLnwDLEgdA+hC8FhD/gJ/BvxM/gL/AO+BmfBiVHAQhFPXB3CZzt4IBcVJMH3wd/grkjjYk6TK/ASJQHpwOIxylgFXgefgxsyWwk7kZjHfWPchF/OHRR0PZ/C7wvwPbgucmBMw+sDfDLbgOVqaDV+b5EbYy8EtftxTsOFER0CD+L6B8WFNQ6+GZ5f8P8YAZOZbQf4E3wLnzWZLYUcbD34gFyH+CGlh4pu0vlnhoYV1gUfJtfqM7HV44ZfBhdSOc2cjbMJfOS5RZoL0DycPiM61og5kvtrGXjKd/Kiv+8DPwJ35Qlxl4zt1iKQVLqgwlaCz5CIT8GX/voduS7dAXl/tOAI+B08Ssr8B7FozpAWRn2FKemwv8jtL+r2F7teIXsamv0jRx1d5uyQGPCKUB7OIylMdc1mbrS/zney00fJ+DUwCIaPdWOP9VfkOlkvUH1aC6VjcZBMUNpfjONkCzY3gySDHVI9ulsBngPnwZPg8mBqhEumFKZjyujvrzkkDS9F/7Tl0qrvNBw85hhxgcYWygUXoHXAuMPDREuxMuBW8BvZ/WXc/nJRk/h7SOnb/hq4z18LiHGEYDEBnZfmU3Cj4lOKHZ2KVztFmYPdxF8T8vuQ95uF4e9DYz+luMAT4D1qWkrV7vVARJ6F9JHS5/xq3Cpbm3h2EDlidEZenvxJwyejLUoLs7o3aj4BdY8OSmcqSD3DXXuGCPwC54mYDjTJtQnXPNv+pGyejTafJcA28K5OE9/fJlfkf0RlVoQ5XoMD0BJR9imrw6Dl0PShGEOeMaSI0JutYPRdbW6u5qPpSwI/WBhTG32oHkP3kZOl+2bQHXVLCcO8UxRiY0lWl6LHVETZv6ym0H10Sx3/AKKFcQ4jM7oOAAAAAElFTkSuQmCC>

[image10]: <data:image/png;base64,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>

[image11]: <data:image/png;base64,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>

[image12]: <data:image/png;base64,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>

[image13]: <data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAACAAAAAZCAYAAABQDyyRAAACi0lEQVR4Xo1VP4sXMRCdcBx6nCgiKAfaiGAngoJgp4iVqKDF4XXaCnZWIoJYqNiI1XUWfgCLqxTuwEZUxMrGwkbwE1gKvswk2fm3v/PB22TeTDKTbDZLFFCStkxm9yh7HhK0S2ixAS2fh5eLqckOSIZHLBzSTJ90IIhqgFUizCqczZYP0Oi21xmZ2DIYV8w6EIoJgx1Sn3mlJ8FnUDbR3gJXYg7BBfAjuA4ua3eagxbp4sEZuYHmO3ga3Ac+Bt/Bc2CKbmiTrYC3wW/gHXC1e3wyb1doDf1jeP5Ad4NPq+Ag+Bm8OwJnsFxkJ76AD8D9zt/QJh7z17UPYwP8A55R/lrKG3AHWt2RxcBkS3hcRPcD+Bw85AJ8R+Ml9QIaWnmv0f0N67iM47HuHhDJW+fAbfAVuGa8CmpYTcQFiFa6c+g90IKLitkb9oAPwV/gCVO4HsLbW3YoT4QCins1cf1e0YfzHgpctQnD+Op/T/4McFvGDoRRHpgXXwESEn2l8Xm2YbxTI7J3JrMkWy1hUffjSU59Pf31XrgGLll3RHKKnlBIRL2w+gqPKk0emGSt8CEr2zDO0y6JQ8oB9lwF/4KXlGMvuAX3VusbXAHfgqeobj4X9P9IYusn+wmOR8qJg4vVF1pP4qediGLSn5c0ziLiJ4Luo38Tbb0Fn5K66mWGJHOXk3fLMLoZr1rp1oN8GbxOfD1PEPcogE/4EZJLxrLUtmjtMLnzMcrJ6xWMmgJYrn+s+ssE+de5WVTf8QWp2zCfdFqffc5BeWPgtE1Gsh2Hrsuys6hMo1yeW4c/Ic3KpqAY7dDcqnCPRGLkuleVXYI3N+1afT9O6FMKQvSEIfLPRGZJA2eQxWbawIzzHy14Q0h56LraAAAAAElFTkSuQmCC>

[image14]: <data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAHUAAAAWCAYAAAD+ZNNIAAAFUUlEQVR4XtWZX8hnQxjHn2nbWqw/i7VtISQSSW1rd2u5EKJwoUQRxcVKrojVRklt2lxhc7FI7wUXXCCJC+UtLvwpSVxJet+UEErInyzf73nm/M7MmWfOmXPOe1if+r7n9Mwzz5kzz8ycmd8rUorrNZimKVThcjFz9gTvmPPP2TtxmWq2dTQ9Ta/oLFxDmufkn5iUJIYjiFFty1fKl/Qxvuaa0t+MeNT3+x/ZVO03Ztjk95ocoI+w0fM8bAN0CrRuXPyySmVeLUZVmpFMEu6Efob+DvQD9K2/Z9nT0Kl1hRlhIj8Vfe4Kmrq1VZ5gzwbbatJT3GA4+g41SkZREifxSQwx+0Q7856WnR37AvQbdH2rbA7YSwdwXZF2UpMXiA2+j1sYPjE07YDehh5olWUx4hQT1u2M01lYxl7RpF5rRFsPHYJ+hC6Oei9x9ST24lchbMuKtJPq6a1dxjroMugdRHwM15Na5fMy9CWG+nv2ukVSY3y8C0ST+qJokuekM6lTcNr2m6BPoAeh42KP0f1nYwQzTA09Y78y5ewG0Uw1Ym/C3Ye4fg+dFxQDdzb+3A/dBfE+5Bjoauhe6FJoO7S51QjOEg6mK6HjJZ9UJuAW6CFot+hsK+Uo6HY892Nc7xBt1zRyPenJJSDE9vGGxG5gB1iQTaqHnfCWqE/9beWo52hnsreh0lW4fuk0gQzB5L+Lu52iCboV96uVr0Kfu6FvRL/lnEHPQF9IlNSqPVwqPxfG0LivQm+IDoIuOBA44D4Sjd+7yhjvXkxPH09maOggqTFBQ5ec+tCXJib3J2hX7StNks6FBxP1vLoueATa5g03ita/JChnETZKLkiqnCaa0IdrJ9HPAVcNzrqG+K2PFR2Ir8POlSYqnJdwYuSea06eEYRR4ohdGyUSztSboQ2OnSXyGXRy4Lcd9l9FB8ce6C/oSeEGS2SjVHEcZwvvl0Vn+SatuqBZfrUpjHMYujzwYcKwyZGlwGYxfUNkdsf/g+xM9bDjmYBfRJdPziJ2/HfQc6K7Y8j5q1wkujS+JPEZmMcVJrWuvyya4JD2N5UrxGF07suyeM5C8UzNw9TUR5eDkn6ve5iW2fG1fc2SAIaPJtVlk1rvft8U3XRwdnKWWjMthI86A7pNtEM54/iDh0+qW3ZmUv3yqw19AvpTdHMUYbxHJ06rXCj6PV4S/UzETm2MYsNUYdsNq2HKMcA1IZqprUD1OfV3WXz/HF044r+GzqodPedAp0P3QdcFdsbhkYgzjz8FWss3ac9UbsA4GLjsBziuBFzWi2m915nQs6IJZqKb4gE9OcC1ECOiNw1t4D5nJtXxG/SU6C9KN0RFIueLLr+PSnO8YOIeFz3aMDn+XFttCuqBUG20wBWicblhEh/6RNFvZThYuDK8JtWq4DZ7G2GSfaKTtx4CBw/bxaNSliR0YlgrBgTOuHIprH/npf6AVuG7igq8p/jNzC1TnJUfCH+zddVsXhY9lxLO1Peh96BDTo8h2A1XM4ww4DXQV6Lf3iXR5OHq2BZusnj2JdyocRnmjpd+r0D7pfeIErfZfIN/i/DhAxoywFVJKiSGYk6AtkDrgxBHi85gdjzL2t9O8Q+kD3/MZwxCP/0vjSeIyV03Z5YRa146c5IrTBzzVK5D/Y37ckbVGlUpT2e4bCEHCgdBS26rHxyhNnbESUlcE4NpGsbkAGtDrhk5e0PZYbzfZ+HBzRg/Ae1jUE78xamA/hbElPmXeDU+Oe+cfRaMhxkmG8PRMFVU9lzhcOxItpXkSyZSHNjFmS/ujxKf/xTfwLCdxW02HI1wc2DHt615u8g/OtzGaIzshygAAAAASUVORK5CYII=>

[image15]: <data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABwAAAAWCAYAAADTlvzyAAABx0lEQVR4Xp1VPUqEQQydYVlEBBtBsbcWO8FOa8F26/UAXkC8gNht5S08wRY2FiIewoP4ksxPksnI4oNMkpdMMjNflk2JkUUVs3qKNb7hfVLBhE5BJI+UQo+5LHXQRoR28ZnKtJagyrFHmNkBqC7X3AmcXXRjTLiquGTwUpWw2rPOHqp02JDz2M2NvoZ8wFtBL0toijDWSDKqBGiNc9rHuoZ8g7mHHAx5zvOfpz6ipidtO7CJbkg3/YQ8gjkU3mZpxlxOgy6incG0R15AbiDvkGfIkQ4KfAeBZ70vjcxpDIi+hNpCb+Cc+oQI481c9aHZSOzhYE/gf6DPOq1TBmOCySEKMExpjRAN00PiYVIbZgfnUFAwoCqoMDX4wo1WyFtyLi05LCUhTw4YM2gqMZ30u0x3SYYnymuIQsLp1QQYNAwbyBbcFfSixdT06s3+RkH5KW4hb5DzZHYU09eofjamSvMbCkyCz2mVbCkxJV/YHv+7YdlgQzQMEWvhG/0HNIEnmb9jZsHF6JsOgpzjVAdJ4c/WwfkvoF+hWXLRIsSXWGbuJVFz91+5M+zD+N+X8twLcmbzZ13rJlW319FtncE6KhpxFTrWO/wCHt8aQ6eTN/sAAAAASUVORK5CYII=>

[image16]: <data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAHsAAAAWCAYAAADgreP7AAAFD0lEQVR4Xs1ZXehmQxh/pqVWS2S1/EN25SPl42JDCUlutK02CuVCuWBTwpY75Ur5KCSllPS/EEK5IC7EHzdyIcmm5IZ8XKG0XJD4/WbmnDNz5pkzM+d9z65f/d5z3udrnnmeM3Pm/f9FamHGgqOJZQafFXWWUx0WDD0P5YTKFlNYzbuMxeI3BG4wrcYCMZWQiSgWJOolMDmImVavAhvYR192kDWiOl61YY92j4WQJJIIFsQSY4Ux5z9wu8Hbx8ISZoyjwGzg4zvw35hGu3+2d0tQk02NTQ6r+C4NZSeLBEYuwue94AfgP+BmqJ6FeEQ28TmI3sb1lFSd4CzwB/AT8KSRbg/4MfiyFMMcI9isqlOztZGgNqqvIpoLNNscQLyrxBV5Mxdbl+tScY15EXwXvFQCw9BD8e5W+BZ4YqyyuBh8U3TdLCg5zEJ1HJOpTXWA1dEVeXOsaADTZfKcxBvghaGiEjYP0ze79+Qq3+avXNm0y6NhwBxaQyT2sSBbmxjeKb4k980YOcfNVgbTBRaUXgl+KG5bOjtWN8CoK5vxH5OhwbdCdAauO8HzwX3gaeBe8FrweG/XAfmYO3E9BF6AezcL98nt8xxwP3gmuB3y6/z3nd5mm3GxD4C7hZ5WrhdjhHm1UULXDJlVjxR1Kzt24kpjofl+fRJKFr+IJCEv8Bet2Wzo+17XYTv4BHgE/BN8DXweUXj/jLhwbPrDuON55DJxcV4BXwJ3MAjwIPibuIPfo17HQyoPgb+Ct4CvgneDDyHsH7g+4FwnMaqNfTAdkgLkUDAsqAekhhuQlZsdA5OXn8ArImka2yEnt+iVaKjhNs7D4o/irmwEc9O2bq5A6h8Xt2o+gu99XndQ3DnkPP+dYNE/Ez4Qw+H1cnEPzHvgCV7WHRQxP9P5HyduC96S8plBr00jJku2AupWdoo9oh02RlCFOrSVzV8M45XtYPpm8xqC2/ph8B1xu0AIzvFn8FyfGbdorth7Apsuj/HJn75bUm42UVWbHtPaCtQH8JMzrtmhX10M+vMnFt9NfEfVeaXQms0V9ZTXefThc83mtv27BA/vkJCdI31u9IK9ULLZ+4OkXR4mefj5fUvqmt3B1kZWr80qnhHUlV0dezDkNsn3N99V+yDnu6sFWrOJ7jQ+BpvMRrFhIbgb/GLctsuHxcHZcI5/g1d7kV/ZJnxg1Hr471vS1uwOvjb2Pc73uTafClR3JYt+cquHsuAB6H7wc3EHnvEJOYdcs3MYmh3jZPBTSf840713vwZP97JuG1+k2Uo9y7VRnBKENva+xsmhO6AF76h65wnwwHMX+BZ46kiXwvQHI26hw7adz6jbxrstOYC5SdxJ+5pAiMOWYfyDQbDugHZzL0E9TL7Zh+HLM0EEJbcREoum2jT2U8UN4orbnXpJ/pz5ErwksEsxa+Cck9klbhX+JUMevP9eXI5j7DDu51aXN69fSZwzB7se/AK3r4tr1LfgbVbnUnla4rnTjifpI2aQfQPe4a9hjaK8cjNLUDAsqC1qbBpRE7LGpg5hpOF+LfEZhO9LPlAz35NSl4pio4jKKDgV1Bk0eCnNwE8a+9+qPN1WTPKvUQ2jtUF/OOLh9MFrbBSUDflzb6Ovj61DtlZ8EAsR8/M6WuB78oVKPiINhxodyiQV0f8Ea6nNwtOrCV9jsxpqVuosrCFEGWseZDLcpHI9SIawgkR6jJHLJycPUWNTazWBWQHanf4DlDfh4jrm4ZMAAAAASUVORK5CYII=>

[image17]: <data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABwAAAAWCAYAAADTlvzyAAABfklEQVR4XqVUO05EMQy0O3oqOqCho+UMnAzRgygQJ6BFoqBYCXEw7DiJvwlPYiQnznhiJ37ZBTgKnMOcBnQZAiUWGk+jq4Vpk7B2tYVLUqk3IcBcPhH9PI6Omp0gaQ1mLOjzsVAIN0bNCkmnJVKoghftt9horSxZPKOBjX0XiWjRLnEd2LXDbuq4pvUXzY9En8eWzFxY5HWfwtBhXQB56y05n2RvZFdCd0vg6pEL2MZtEOGG7L0ZkB/CGRrd6/4ANZVvybf9JrtrVFAoMxwjWbR7D1FfkD3DKNw+7yqZPYTh5hiQyc4gPyZ4odUPzZeNyuKEJpkpIiyjPt/uiexEBYq2ZqwFKaIEyit9pfkD5PXawzpUHGPylUBvjZQc/U+jgm4A6W3+t1TkCDPcrhNI+7iN/0AoIOdRmuZ7mh6g/csoXEv47iY2aHHsdV3eCZer8i0O84lQlAUds3jzNduxySpw/Cjkl8ewE2M/e9DIErtjg8Yf4VV8rM2h3eel4RcPPhHyExe5nQAAAABJRU5ErkJggg==>

[image18]: <data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAACsAAAAWCAYAAABZuWWzAAACEklEQVR4XqWWP0pEMRDGE0REBBtB2d5a7AQ7rQXbrfUAewHxAmJn5S08gYWNhYiH8CDOJG+SmcmX3Rf3J2My3/x5SXh5GgIR6QfT00M3BOUYc6AEYVaF870Ep8NsUZsW1an3smy4+Go+xnglOLxMT9+Wf/c1hdkpEohZlFaL2iao1NLJgPLUPw16ATbZrSJxRfZJtiTbNRH4IMSUuC4fxZAmSAzk7JN6R+MP2T0lHKCkHrPfQb0nE/OJ8+CTXVLpF40PZIcuPh+9IriWKsKwoom70h2ya7IPcp9oPCrhAL7MjSBIYNPCnLppo6VtCaZFsXdBv95pfCFbSFRj+7GHniDMiK1LaWift0fuI42/ZKcmUhh6QoGPxH7zO32QnLUSoYsX5OKtyA4kYHG701P3EPRMDHjdEqLWkRe1ovE7oE9aArfKyFFNOU1qI2wA5/Pt568Af3dvQ7poOhEXefRpzqvI9M7SwxeHLxBfpMuQvwYJW87evIaV0fxKqVSTG7I38s+qbAczHT/oBtgLIfE8ttn+QiScZl32xKxqhUZpctouDrxo7w+QSm3X6P53bbdVMnUAQjc9noT83i7iNPp5tnhM/cr7jAHnE3nBTpAcraO1cq1yz8leSZqM5pH9alHmMTyHzl+xcdacJpAwbieeFGsS1EmJP/VpUudQilS1a4+BAVtpUqa/nbCMUTuwddpvq/8AXT0aSW/JJrUAAAAASUVORK5CYII=>

[image19]: <data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAD4AAAAWCAYAAACYPi8fAAACKElEQVR4Xq2XPU4DMRCFdxVRICQaJFDKCCQkKjp6LhCJioor5ALcAHEBJE7AAaCGhiqH4QTM7NjZ9fx5nOwnjdae92a86w0O6boG+p5nfMg+Fu1GvE9r4062wATP8blOyNXH71H4RKLMSNlA2ci9SR3o0isNlYxMNYMtqm24YTLPQ3EvKaE9ySzk9ocS6+C5PC0jPL2SYymmS43tregn1xCWGYn3njrtqlOIZ4hviEumFaS9UBk+hYbWhOgRewhE3p98McgZxAvED8Q9xGIq8vrpVenlIwpE4kBi/Vbge+/oDd910SqxeZNZrQPbuBhopkWbyosTkcbXEB8pbnbZMNzO5wp8s9IkUDkQ9WlgLb7VTwh8y6tStvCX1FU9uz8H9IPSNVz+IB5oWmgNKG7xJduA++b1bAhWujvE4F7hEOuLQ2xvnPuzpDE/ceShVZQY5IrHIn9t/UKs+3SKW70wb2lIqTlOIYmExLJY+QJ7i04gNvCvyRbUJxgfU1r1uthLZFxxBnh/Ptc5gngE8xb8m442pBFayFwOBVPM8B5sVK13qJxD+JHHQ/ALbFfcyucjtoKIm/bthOWx8gOKqD+wmpyFWmepV35nuWIi4mmDd+RzgrK6Jkk+Zq9VC10kJPh3fAGxBPeSrmacg2cR6GkuPE1rP1rKqdFE4Pss9RbiLRivHW0QVRZQTlNmw2hupEsGU8hpMRa3tmn1Z7y6ygnQzszt4ngLe1oD/7zaGktsse3AAAAAAElFTkSuQmCC>