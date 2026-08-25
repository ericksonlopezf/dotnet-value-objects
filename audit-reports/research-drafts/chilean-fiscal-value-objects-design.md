# **Auditoría y Diseño Exhaustivo del Catálogo de Value Objects para el Ecosistema Fiscal Chileno en .NET 10**

## **1\. Marco Normativo y Taxonomía del Fiscal Domain Chileno**

El diseño de un modelo de dominio tributario para Chile exige un alineamiento riguroso con el marco regulatorio promulgado por el Servicio de Impuestos Internos (SII), el Código Tributario y la legislación mercantil chilena1. La aplicación de Domain-Driven Design (DDD) en este ámbito requiere diferenciar con precisión semántica entre las leyes vigentes dictadas por el Poder Legislativo, las resoluciones administrativas emitidas por el SII y la especificación técnica de la plataforma de Facturación Electrónica1.

### **Matriz de Fuentes Normativas y Clasificación de Reglas**

| Fuente Normativa | Disposición / Documento | Ámbito de Aplicación Fiscal | Frecuencia de Cambio | Tipo de Regla |
| :---- | :---- | :---- | :---- | :---- |
| **Código Tributario** | DFL N° 1 (Arts. 33, 66, 97\)1 | Identificación tributaria (RUT), obligaciones de declaración y régimen sancionatorio. | Baja (Anual/Decenal) | Regla Legal |
| **Ley Impuesto a Ventas y Servicios** | DL N° 825 (LIVS) | Hechos gravados con IVA, exenciones, notas de crédito/débito y crédito/débito fiscal. | Media (Reformas) | Regla Tributaria |
| **Ley Impuesto a la Renta** | DL N° 824 (LIR, Arts. 14, 31, 42 N° 2\)1 | Regímenes tributarios, gastos necesarios para producir renta y retenciones de segunda categoría1. | Media (Reformas) | Regla Tributaria |
| **Ley N° 21.133** | Modificación Protección Social Independientes5 | Escala de retención gradual para boletas de honorarios entre 2019 y 20285. | Determinística (Anual) | Regla Legal / Tributaria |
| **Ley N° 20.727** | Obligatoriedad de la Electronic Invoice2 | Uso obligatorio de Documentos Tributarios Electrónicos (DTE) en reemplazo del papel2. | Muy Baja | Regla Legal |
| **Resoluciones Exentas SII** | Res. N° 45/2003, Res. N° 11/2001, Res. N° 115/20211 | Esquemas XML, timbrado de folios (CAF), timbre electrónico (TED) y libros electrónicos1. | Alta (Resoluciones) | Regla de Negocio / Técnica |
| **Manuales y Esquemas XSD SII** | Formato DTE v1.0, RCV, BHE2 | Estructuras de datos XML, validación de firmas digitales y esquemas de intercambio2. | Baja (Estabilidad) | Regla de Integración / Técnica |

Para construir un modelo extensible que evite tanto la adicción a tipos primitivos (*Primitive Obsession*) como la proliferación artificial de clases, cada concepto del ecosistema fiscal debe categorizarse según los patrones tácticos de DDD e ingeniería de software:

* **Value Object (VO):** Objeto inmutable sin identidad conceptual propia, definido por el valor de sus atributos, con igualdad semántica y capacidad de auto-validación de invariantes.  
* **Entity:** Objeto con identidad conceptual única que se mantiene a lo largo del tiempo y posee un ciclo de vida definido.  
* **Aggregate Root:** Entidad que actúa como raíz y frontera de consistencia para un conjunto de entidades y objetos de valor asociados.  
* **DTO (Data Transfer Object):** Estructura anémica de transporte de datos sin comportamiento ni invariantes de dominio.  
* **Enum:** Tipo primitivo tipado reservado para listas finitas, estáticas y cerradas que no sufren modificaciones normativas.  
* **Catalog / Catalog-driven VO:** Objeto de valor que encapsula un código normativo respaldado por un catálogo dinámico y versionado, evitando la rigidez de un enum hardcodeado.  
* **Policy / Specification:** Objeto que encapsula reglas de negocio complejas o predicados evaluables del dominio tributario.  
* **Domain Service:** Operación sin estado (*stateless*) que coordina múltiples objetos de valor o entidades sin pertenecer de forma natural a ninguno de ellos.  
* **Infrastructure Model / Technical Primitive:** Componentes criptográficos, mappers de persistencia o clases generadas por esquemas XSD que residen fuera del dominio.

## **2\. Identificación Tributaria y el Sujeto Fiscal (RUT, RUN y Contribuyente)**

El Rol Único Tributario (RUT) y el Rol Único Nacional (RUN) constituyen la base del sistema de identificación civil y tributario en Chile2. Aunque comparten la misma estructura matemática y el mismo algoritmo de verificación, cumplen roles semánticos diferenciados dentro del ecosistema mercantil2.

### **Estructura y Algoritmo de Verificación Modulo 11**

El RUT consiste en una secuencia numérica de 1 a 8 dígitos (el cuerpo) seguida de un dígito verificador (DV), separado por un guion2. El DV puede ser un dígito del 0 al 9 o la letra 'K'2. El algoritmo oficial utiliza la serie iterativa de coeficientes \[2, 3, 4, 5, 6, 7\] aplicada de derecha a izquierda sobre los dígitos del cuerpo:  
![][image1]  
![][image2]  
Si el resultado ![][image3] es igual a ![][image4], el dígito verificador corresponde a '0'. Si ![][image3] es igual a ![][image5], el dígito verificador corresponde a 'K'. En cualquier otro caso, el dígito verificador equivale al valor numérico de ![][image3].

### **Decisión de Abstracción en la Identificación Fiscal**

Se descarta la separación del RUT en múltiples Value Objects como RutNumber y VerificationDigit. El dígito verificador no posee valor de negocio de forma aislada; es un atributo matemáticamente dependiente e intrínseco a la validez del número11. Separar el DV introduce acoplamiento innecesario y abre la posibilidad de instanciar estados inconsistentes.  
Asimismo, el RUT (para personas jurídicas y naturales en su rol tributario) y el RUN (identificador civil emitido por el Registro Civil) comparten idéntica abstracción formal dentro del dominio fiscal: el Value Object Rut2. Se eliminan los tipos redundantes TaxpayerId y TaxIdentificationNumber cuando representan la identidad tributaria chilena.  
Para sujetos extranjeros que carecen de RUT chileno (por ejemplo, receptores en DTEs de exportación), el dominio no debe forzar el uso del VO Rut. En su lugar, se define ForeignTaxId, un objeto de valor separado adaptado a identificaciones tributarias internacionales.

### **El Contribuyente como Raíz de Agregado**

Un error recurrente en el diseño de software fiscal es intentar modelar al Contribuyente completo como un Value Object. En el dominio tributario chileno, el Contribuyente (Taxpayer) es una Raíz de Agregado (**Aggregate Root**). Posee identidad conceptual, ciclo de vida, estado de inicio de actividades, regímenes tributarios mutables, sucursales y representantes legales1.

| Componente del Agregado | Clasificación DDD | Justificación Arquitectónica |
| :---- | :---- | :---- |
| **Taxpayer** | **Aggregate Root** | Mantiene la identidad fiscal, estado de inicio de actividades y conjunto de obligaciones tributarias1. |
| **Rut** | **Value Object** | Identificador único e inmutable de la Raíz de Agregado2. |
| **TaxpayerName** | **Value Object** | Encapsula la Razón Social. Invariantes: longitud máxima según normativa SII (40 a 100 caracteres) y limpieza de caracteres de control. |
| **TradeName** | **Value Object** | Nombre de fantasía comercial opcional. |
| **EconomicActivityCode** | **Catalog VO** | Código de actividad económica normado por el SII (ej. 620100). Valida la existencia contra el catálogo oficial. |
| **FiscalAddress** | **Value Object** | Domicilio tributario estructurado (calle, número, comuna, ciudad, región). |
| **BranchOffice** | **Entity** | Sucursal o establecimiento. Posee un identificador numérico interno asignado por el SII (CodeSII) y un ciclo de vida de apertura y cierre. |

## **3\. Modelado de Documentos Tributarios Electrónicos (DTE), Folios y Artefactos Criptográficos**

El Documento Tributario Electrónico (DTE) representa un acto de comercio respaldado legalmente y firmado digitalmente2. El DTE en sí mismo es una Entidad (**Entity**) o Raíz de Agregado dentro del contexto de facturación electrónica, ya que posee un ciclo de vida que abarca la emisión, firma, envío al SII, aceptación o reclamo comercial y recepción conforme2.  
El DTE se compone de una red de Value Objects altamente especializados que capturan su semántica sin contaminar el dominio con detalles de infraestructura XML2.

### **Catálogo de Tipos de DTE**

El Servicio de Impuestos Internos asigna códigos numéricos obligatorios a cada clase de documento tributario9.

| Código SII | Nombre del Documento Tributario | Clasificación Operativa | Exige IVA Desglosado |
| :---- | :---- | :---- | :---- |
| **33** | Electronic Invoice12 | Venta B2B / Operación Afecta13 | Sí13 |
| **34** | Factura Exenta Electrónica12 | Venta B2B / Operación Exenta13 | No13 |
| **39** | Boleta Electrónica12 | Venta B2C / Consumidor Final13 | Incluido en el Total13 |
| **41** | Boleta Exenta Electrónica12 | Venta B2C / Operación Exenta13 | No13 |
| **43** | Liquidación Electronic Invoice12 | Consignación / Mandato13 | Sí13 |
| **46** | Factura de Compra Electrónica12 | Compra / Cambio de Sujeto13 | Sí (Retención)13 |
| **52** | Dispatch Guide Electrónica12 | Traslado de Mercaderías13 | Optional según traslado13 |
| **56** | Debit Note Electrónica12 | Ajuste de Aumento de Valor13 | Sí13 |
| **61** | Credit Note Electrónica12 | Anulación / Descuento / Ajuste13 | Sí13 |
| **110** | Factura de Exportation Electrónica12 | Comercio Exterior13 | No (Exempt Exportation)13 |
| **111** | Debit Note de Exportation12 | Ajuste Exportation13 | No13 |
| **112** | Credit Note de Exportation12 | Ajuste Exportation13 | No13 |

No se debe modelar DteTypeCode como un enum nativo cerrado. Aunque los códigos principales son estables12, el SII introduce variaciones operativas y tipos especiales mediante resoluciones exentas (como los eventos de pago electrónico código 48\)1. Modelarlo como un **Catalog-driven Value Object** permite consultar capacidades normativas mediante métodos de consulta como IsExport(), RequiresIvaBreakdown(), GeneratesTaxCredit() o IsBoleta().

### **Desacople de Folios, CAF, Timbre Electrónico (TED) y Firma Digital**

Un principio fundamental de esta arquitectura es la estricta separación entre las abstracciones fiscales de negocio y los artefactos criptográficos o formatos de transmisión XML2.

* **Fiscal Folio (FiscalFolio):** Value Object que representa la secuencia numérica entera asignada a un documento dentro de un tipo de DTE. Invariante: número entero positivo mayor a cero.  
* **Rango de Folios (FolioRange):** Value Object que define un intervalo cerrado \[From, To\] de folios autorizados.  
* **Código de Autorización de Folios (CAF):** Entidad de Seguridad / Modelo de Infraestructura. El CAF es un archivo XML firmado por el SII que contiene un rango de folios autorizados y la clave privada RSA asociada al timbrado2. No es un Value Object porque posee un ciclo de vida complejo: se descarga, se almacena, se verifica su firma digital, se vigila su stock de folios y puede ser anulado o inhabilitado ante el SII2.  
* **Timbre Electrónico de Documento (TED):** Modelo de Integración / Formato de Transporte. Corresponde al nodo XML \<TED version="1.0"\> que agrupa los datos tributarios esenciales del DTE firmados con la clave privada del CAF2. El dominio fiscal solo interactúa con el Value Object FiscalStamp, el cual almacena el hash o firma criptográfica inmutable. La renderización del código de barras bidimensional PDF417 o la construcción del fragmento XML pertenecen exclusivamente a los adaptadores de la capa de infraestructura8.  
* **Firma Electrónica y Certificado Digital:** Pertenecen al límite de infraestructura de seguridad. Los certificados PKCS\#12 (.pfx), las claves privadas de contribuyente, las huellas digitales SHA-1/SHA-256 y los tokens de sesión SOAP del SII residen fuera del dominio de negocio.

## **4\. Semántica Fiscal de Documentos Específicos y Transacciones**

Cada tipo de DTE posee reglas de negocio particulares que dictan la obligatoriedad de ciertos datos y el comportamiento de sus montos13.

### **Referencias entre Documentos (DocumentReference)**

Las Notas de Crédito (61), Notas de Débito (56) y Facturas de Compra (46) requieren referenciar documentos previos mediante el bloque de referencia del SII13.  
The Value Object DocumentReference encapsula los siguientes elementos:

* ReferencedDocumentType: Código del documento origen (ej. 33 para Factura u 801 para Orden de Compra)14.  
* ReferencedFolio: Folio fiscal del documento referenciado14.  
* ReferenceDate: Fecha de emisión del documento de origen.  
* ReferenceCode: Código de razón normado por el SII (1 \= Anula documento referenciado, 2 \= Corrige texto, 3 \= Corrige montos).  
* Reason: Descripción textual del motivo del ajuste (máximo 90 caracteres según la especificación del SII).

### **Naturaleza del Traslado en Guías de Despacho (DispatchType)**

La Dispatch Guide Electrónica (código 52\) ampara el traslado físico de mercaderías y exige declarar la naturaleza económica de la operación13. El objeto de valor DispatchType encapsula los códigos oficiales del SII:

> 1. Operación constituye venta (genera facturación posterior)13.  
> 2. Ventas por efectuar13.  
> 3. Consignaciones13.  
> 4. Entrega gratuita o donaciones13.  
> 5. Traslados internos entre sucursales de la misma empresa13.  
> 6. Otros traslados no venta (ej. reparaciones o mantenimiento)13.  
> 7. Guía de devolución de mercaderías13.  
> 8. Traslado para exportación (sin venta directa)13.  
> 9. Venta para exportación13.

### **Cesión Electrónica de Facturas (Factoring)**

La cesión de un DTE a una institución de *factoring* se gestiona a través del Registro Público Electrónico de Transferencia de Créditos (RPETC) del SII2. Una cesión **NO es un Value Object**. Es un Agregado o Proceso de Dominio con ciclo de vida, que emite eventos de dominio (InvoiceAssigned, AssignmentAccepted, AssignmentRejected) y genera un Certificado de Cesión firmado digitalmente2.

## **5\. Dominio Monetario, Impuestos, Retenciones y Unidades Económicas**

El manejo de dinero y cálculos impositivos exige eliminar la flotación imprecisa y aplicar las reglas de redondeo legales establecidas en la legislación chilena.

### **Representación Monetaria (Money y CurrencyCode)**

El Value Object Money encapsula el monto numérico y el código de moneda ISO 4217 (CurrencyCode).

* **Pesos Chilenos (CLP):** Por regla legal de la Ley sobre Impuesto a las Ventas y Servicios (LIVS), los montos de impuestos y totales en DTEs emitidos en moneda nacional no admiten centavos o decimales. Se aplica redondeo entero simétrico al número entero más cercano.  
* **Moneda Extranjera (USD, EUR):** Se permiten hasta cuatro decimales en los precios unitarios y detalles, pero se requiere obligatoriamente el Value Object ExchangeRate para calcular la equivalencia en CLP a la fecha de emisión, necesaria para la contabilidad y el registro impositivo F297.

### **Impuestos, IVA y Retenciones Graduales**

| Concepto Fiscal | Clasificación DDD | Reglas Normativas y Fórmulas de Cálculo |
| :---- | :---- | :---- |
| **TaxRate** | **Value Object** | Tasa porcentual impositiva expresada en escala decimal (ej. 0.19 para el IVA 19%). Invariante: mayor o igual a cero. |
| **TaxTypeCode** | **Catalog VO** | Código normado por el SII (ej. 14 para IVA 19%, 27 para ILA Bebidas Analcohólicas 10%, 15 para Retención Cambio de Sujeto). |
| **TaxAmount** | **Value Object** | Monto impuesto calculado. Fórmula: ![][image6]. |
| **TaxableBase** | **Value Object** | Base imponible afecta sobre la cual se aplica la tasa correspondiente. |
| **WithholdingRate** | **Value Object** | Tasa de retención gradual para Boletas de Honorarios (segunda categoría) establecida por la **Ley N° 21.133**5. |

La **Ley N° 21.133** fijó una escala de incremento gradual de la retención sobre boletas de honorarios para financiar la incorporación de independientes a la protección social5:

* Año 2024: 13.75%15  
* Año 2025: 14.50%5  
* Año 2026: 15.25%5  
* Año 2027: 16.00%5  
* Año 2028 y posteriores: 17.00%5

## **6\. Auditoría Exhaustiva de Duplicidades y Catálogo Maestro de Value Objects**

Para garantizar un diseño cohesivo y sin redundancias, se auditan las duplicidades conceptuales identificando las abstracciones canónicas necesarias.

### **Matriz de Auditoría y Resolución de Duplicidades**

| Concepto A | Concepto B | Veredicto de Arquitectura | Justificación Técnica |
| :---- | :---- | :---- | :---- |
| RUT | TaxpayerId | **Unificar en Rut** | En Chile, el identificador tributario único de personas y empresas es el RUT2. TaxpayerId es una denominación genérica redundante. |
| RUN | IdentificationNumber | **Reemplazar por Rut / ForeignTaxId** | El RUN civil opera con la misma estructura que el RUT2. Para extranjeros sin RUT se utiliza ForeignTaxId. |
| VerificationDigit | CheckDigit | **Eliminar ambos** | El dígito verificador es un detalle de implementación del algoritmo Modulo 11 encertado en Rut, no un VO independiente. |
| DteType | DocumentType | **Unificar en DteTypeCode** | Encapsula el código del catálogo del SII para documentos electrónicos9. |
| Folio | DocumentNumber | **Unificar en FiscalFolio** | Evita la ambigüedad con correlativos internos de sistemas ERP. |
| TaxRate | Percentage | **Especializar TaxRate** | Un porcentaje genérico carece de los rangos y validaciones impositivas legales. |
| TaxAmount | Amount | **Usar Money** | Los montos impositivos son valores financieros expresados en una moneda determinada. |
| TaxBase | TaxableAmount | **Usar Money** | Representa la base monetaria sobre la cual se calculan los impuestos. |
| Address | FiscalAddress | **Unificar en FiscalAddress** | La dirección fiscal requiere comuna y ciudad estrictamente alineadas con los códigos geográficos del SII. |
| TED | ElectronicStamp | **Separar Conceptos** | TED es la estructura XML de infraestructura2; FiscalStamp es el Value Object de dominio que encapsula la firma. |

### **Catálogo Maestro de Value Objects Aprobados**

#### **Módulo de Identificación y Sujetos**

* Rut: Identificador tributario nacional chileno (RUT/RUN) con validación Modulo 11 integrada y formato canónico XXXXXXXX-Y2.  
* ForeignTaxId: Identificador tributario de personas o entidades extranjeras sin residencia fiscal en Chile.

#### **Módulo de Documentos y DTE**

* DteTypeCode: Código de tipo de DTE del SII con metadatos normativos de comportamiento9.  
* FiscalFolio: Número correlativo de folio fiscal autorizado.  
* FolioRange: Rango cerrado de folios \[From, To\] autorizados por el SII.  
* DocumentReference: Estructura de referencia a documentos tributarios o comerciales previos14.  
* ReferenceTypeCode: Código de motivo de referencia del SII (1, 2, 3\)14.  
* DispatchType: Tipo de traslado de mercaderías en Guías de Despacho (códigos 1 al 9\)13.  
* FiscalStamp: Representación inmutable del digest y firma del timbre electrónico DTE2.

#### **Módulo Financiero e Impuestos**

* Money: Cantidad monetaria inmutable y código de moneda ISO 4217\.  
* CurrencyCode: Moneda de la transacción (CLP, USD, EUR).  
* ExchangeRate: Tasa de conversión de moneda extranjera a Pesos Chilenos a la fecha de emisión.  
* TaxRate: Tasa porcentual aplicable a impuestos afectos.  
* TaxTypeCode: Código normado por el SII para clasificar tipos de impuestos e ILA9.  
* WithholdingRate: Tasa de retención de segunda categoría según la escala gradual de la Ley N° 21.1335.

#### **Módulo Ubicación y Clasificación**

* FiscalAddress: Dirección tributaria declarada ante el SII.  
* EconomicActivityCode: Código de actividad económica registrado ante el SII.  
* GeoLocationCode: Código territorial oficial de comuna y región.  
* TaxPeriod: Período tributario mensual o anual expresado en formato YYYY-MM.

### **Catálogo de Conceptos Descartados o Reclasificados**

| Concepto Evaluado | Categoría Asignada | Justificación para Excluir como VO |
| :---- | :---- | :---- |
| **Taxpayer (Contribuyente)** | **Aggregate Root** | Posee estado mutable, regímenes tributarios, sucursales y ciclo de vida legal1. |
| **DTE (Documento Concreto)** | **Entity / Aggregate** | Documento con identidad única (Emisor \+ TipoDTE \+ Folio) y estados de procesamiento2. |
| **CAF (Autorización Folios)** | **Security Entity** | Documento XML con clave RSA privada que se consume y vence con el tiempo2. |
| **Firma Digital / Certificado** | **Infrastructure Model** | Componentes criptográficos y llaves privadas de la capa de seguridad. |
| **DTE XML Payload** | **DTO / Integration Model** | Representación de transmisión ajustada a los esquemas XSD del SII10. |
| **Registro Compras y Ventas** | **Domain Service / Aggregate** | Consolidado tributario oficial gestionado por la plataforma del SII2. |

## **7\. Arquitectura de Bounded Contexts, Distribución de Librerías y Anti-Corruption Layer (ACL)**

Para evitar la contaminación entre dominios y mantener la modularidad, los Value Objects aprobados se distribuyen en **Bounded Contexts** claramente delimitados.

### **Asignación de Value Objects por Bounded Context**

\+-----------------------------------------------------------------------------------+  
|                                 SHARED KERNEL                                     |  
|    \- Rut                                 \- Money                                  |  
|    \- CurrencyCode                        \- TaxPeriod                              |  
|    \- FiscalAddress                                                                |  
\+-----------------------------------------+-----------------------------------------+  
                                          |  
        \+---------------------------------+---------------------------------+  
        |                                 |                                 |  
        v                                 v                                 v  
\+-----------------------+     \+-----------------------+     \+-----------------------+  
|   TAXPAYER CONTEXT    |     |      DTE CONTEXT      |     |      TAX CONTEXT      |  
\+-----------------------+     \+-----------------------+     \+-----------------------+  
| \- EconomicActivityCode|     | \- DteTypeCode         |     | \- TaxRate             |  
| \- GeoLocationCode     |     | \- FiscalFolio         |     | \- TaxTypeCode         |  
| \- TaxRegime           |     | \- FolioRange          |     | \- WithholdingRate     |  
|                       |     | \- DocumentReference   |     |                       |  
|                       |     | \- DispatchType        |     |                       |  
|                       |     | \- FiscalStamp         |     |                       |  
\+-----------------------+     \+-----------------------+     \+-----------------------+  
                                          |  
                                          v  
                              \+-----------------------+  
                              | SII INTEGRATION CONTEXT|  
                              \+-----------------------+  
                              | \- XML Mappers (XSD)   |  
                              | \- SOAP Response DTOs  |  
                              | \- TrackId Response    |  
                              \+-----------------------+

### **Capa de Adaptación Anti-Corrupción (ACL)**

El dominio de negocio debe operar de forma independiente a la estructura XML definida por el SII10. Las reglas de formato del SII imponen nombres de etiquetas abreviados (\<RchDTE\>, \<MntTotal\>), orden estricto de nodos y codificación ISO-8859-1 en versiones históricas.  
El flujo de aislamiento se organiza en cuatro capas de transformación:

> 1. **Domain Core:** Modela el negocio con objetos inmutables puros (Rut, Money, DteTypeCode).  
> 2. **Application Layer:** Coordina los casos de uso sin exponer detalles de transmisión.  
> 3. **Anti-Corruption Layer (ACL):** Contiene los mappers (DteToXmlMapper, XmlToDteMapper) encargados de transformar las entidades y VOs de dominio en DTOs compatibles con los esquemas XSD10.  
> 4. **SII Adapter / Infrastructure:** Gestiona la comunicación HTTP/SOAP, la firma digital XmlDSig y la interacción directa con los servidores del SII3.

### **Arquitectura de Librerías y Paquetes NuGet**

| Paquete NuGet | Dependencias Permitidas | Tipos Expuestos | Prohibiciones Explícitas |
| :---- | :---- | :---- | :---- |
| EricksonLopez.DomainPrimitives | Ninguna (System únicamente) | Interfaces base DDD, Result\<T\>, Error. | No contiene lógica específica de Chile ni dependencias de terceros. |
| EricksonLopez.SharedKernel | DomainPrimitives | Money, CurrencyCode, TaxPeriod. | Prohibida la dependencia de esquemas de transmisión XML o SDKs. |
| EricksonLopez.Fiscal.Chile | SharedKernel | Rut, FiscalAddress, EconomicActivityCode. | Sin conocimiento de DTEs o contratos de integración del SII. |
| EricksonLopez.Dte.Domain | Fiscal.Chile | DteTypeCode, FiscalFolio, DocumentReference, DispatchType. | Prohibido incluir serialización XML o clientes HTTP. |
| EricksonLopez.Sii.Integration | Dte.Domain, System.Xml | Adaptadores ACL, mappers XML/XSD, clientes SOAP/REST del SII. | No debe exponer DTOs del SII hacia las capas superiores. |

## **8\. Diseño de APIs de Value Objects para .NET 10, Performance y Serialización**

En .NET 10, los Value Objects deben implementarse mediante readonly record struct para garantizar la semántica de valor, inmutabilidad, comportamiento de igualdad eficiente y **cero asignaciones en memoria (Zero-Allocations)** durante el procesamiento de pipelines de facturación masiva.

### **Diseños Conceptuales de C\# (.NET 10 / Native AOT)**

#### **Value Object: Rut**

C\#  
namespace EricksonLopez.Fiscal.Chile.ValueObjects;

/// \<summary\>  
/// Representa un Rol Único Tributario (RUT) chileno validado mediante Modulo 11\.  
/// Diseñado para cero asignaciones mediante ISpanParsable.  
/// \</summary\>  
public readonly record struct Rut : ISpanParsable\<Rut\>, IEquatable\<Rut\>  
{  
    private readonly int \_number;  
    private readonly char \_dv;

    public int Number \=\> \_number;  
    public char Dv \=\> \_dv;

    public static Result\<Rut\> Create(int number, char dv);  
    public static Result\<Rut\> Parse(ReadOnlySpan\<char\> input);  
    public static bool TryParse(ReadOnlySpan\<char\> input, out Rut result);

    public string ToCanonicalString(); // "12345678-K"  
    public string ToFormattedString(); // "12.345.678-K"

    public static Rut Parse(string s, IFormatProvider? provider);  
    public static Rut Parse(ReadOnlySpan\<char\> s, IFormatProvider? provider);  
    public static bool TryParse(string? s, IFormatProvider? provider, out Rut result);  
    public static bool TryParse(ReadOnlySpan\<char\> s, IFormatProvider? provider, out Rut result);  
}

#### **Value Object: DteTypeCode**

C\#  
namespace EricksonLopez.Dte.Domain.ValueObjects;

/// \<summary\>  
/// Representa el código normado de un Tipo de Documento Tributario Electrónico.  
/// \</summary\>  
public readonly record struct DteTypeCode : ISpanParsable\<DteTypeCode\>, IEquatable\<DteTypeCode\>  
{  
    public int Code { get; }

    public static Result\<DteTypeCode\> FromCode(int code);

    public bool IsExport \=\> Code is 110 or 111 or 112;  
    public bool RequiresIvaBreakdown \=\> Code is 33 or 46 or 43 or 56 or 61;  
    public bool IsBoleta \=\> Code is 39 or 41;  
    public bool GeneratesTaxCredit \=\> Code is 33 or 43 or 46;

    public static DteTypeCode Parse(ReadOnlySpan\<char\> s, IFormatProvider? provider);  
    public static bool TryParse(ReadOnlySpan\<char\> s, IFormatProvider? provider, out DteTypeCode result);  
}

#### **Value Object: FiscalFolio**

C\#  
namespace EricksonLopez.Dte.Domain.ValueObjects;

/// \<summary\>  
/// Representa un folio fiscal autorizado por el SII.  
/// \</summary\>  
public readonly record struct FiscalFolio : IComparable\<FiscalFolio\>, ISpanParsable\<FiscalFolio\>  
{  
    public ulong Value { get; }

    public static Result\<FiscalFolio\> Create(ulong value);  
    public bool IsWithinRange(FolioRange range) \=\> Value \>= range.From && Value \<= range.To;

    public static FiscalFolio Parse(ReadOnlySpan\<char\> s, IFormatProvider? provider);  
    public static bool TryParse(ReadOnlySpan\<char\> s, IFormatProvider? provider, out FiscalFolio result);  
}

#### **Value Object: Money**

C\#  
namespace EricksonLopez.SharedKernel.ValueObjects;

/// \<summary\>  
/// Encapsula una cantidad monetaria e impone las reglas de redondeo de la LIVS en CLP.  
/// \</summary\>  
public readonly record struct Money : IEquatable\<Money\>  
{  
    public decimal Amount { get; }  
    public CurrencyCode Currency { get; }

    public static Result\<Money\> Create(decimal amount, CurrencyCode currency);  
    public static Money FromClp(decimal amount);

    public Money Add(Money other);  
    public Money Subtract(Money other);  
    public Money ApplyTax(TaxRate rate);  
}

### **Estrategia de Mapeo y Serialización Multi-Capas**

Para evitar acoplamientos, la serialización de los Value Objects se adapta según el canal de persistencia o comunicación:

> 1. **Transporte REST / JSON:** Se utiliza System.Text.Json con Source Generators habilitados para compatibilidad con Native AOT. Cada VO implementa un JsonConverter\<T\> que escribe valores canónicos (ej. Rut como cadena "12345678-K").  
> 2. **Integración XML DTE / SII:** La capa Anti-Corruption Layer (ACL) convierte los VOs a DTOs decorados con \[XmlElement\] y \[XmlAttribute\], garantizando el cumplimiento estricto de las reglas XSD del SII10.  
> 3. **Persistencia Relacional (PostgreSQL / EF Core 10 / Dapper):** En Entity Framework Core 10, los VOs se mapean mediante ValueConverter a columnas nativas SQL (bigint, integer, varchar). En Dapper, se utilizan implementaciones de SqlMapper.TypeHandler\<T\>.

## **9\. Evolución Normativa, Estrategia de Testing y Matriz de Priorización**

### **Clasificación de Volatilidad Normativa**

| Objeto de Valor | Clasificación de Estabilidad | Estrategia de Mantenimiento y Evolución |
| :---- | :---- | :---- |
| **Rut** | **Stable** | Regla matemática inalterable (Modulo 11\)2. Definido en código core. |
| **Money / Currency** | **Stable** | Estándar financiero ISO 4217\. Definido en código core. |
| **FiscalFolio** | **Stable** | Secuencia numérica entera. Definido en código core. |
| **DteTypeCode** | **Catalog-driven** | Actualización dinámica mediante catálogos sin redeploy de la aplicación9. |
| **TaxTypeCode (IVA/ILA)** | **Regulatory / Configurable** | Configuración de alícuotas e impuestos dinámicos según publicaciones del SII9. |
| **WithholdingRate** | **Versioned / Regulatory** | Mantiene la tabla temporal de la Ley N° 21.133 parametrizada por fecha de emisión5. |
| **EconomicActivityCode** | **Catalog-driven** | Sincronización periódica con las tablas públicas de giros del SII. |

### **Estrategia de Testing Automatizado**

La validación del modelo de dominio se estructura en cuatro niveles de prueba:

* **Pruebas Unitarias (Unit Tests):** Verificación de invariantes de creación, rechazo de datos inválidos y comportamiento de igualdad sobre los readonly record struct.  
* **Pruebas Basadas en Propiedades (Property-Based Testing con FsCheck):** Generación de miles de combinaciones numéricas aleatorias para comprobar la robustez del algoritmo Modulo 11 del RUT y la idempotencia de las operaciones de parsing.  
* **Mutation Testing (Stryker.NET):** Inyección de mutaciones en el código fuente para garantizar que las pruebas unitarias detecten cualquier alteración en los límites de validación.  
* **Golden Tests de Integración XML:** Verificación de que la transformación mediante la capa ACL genere estructuras XML idénticas a las muestras autorizadas por el SII en procesos de homologación10.

### **Matriz de Priorización de Desarrollo**

| Nivel de Prioridad | Value Objects y Componentes Incluidos | Justificación de Negocio |
| :---- | :---- | :---- |
| **P0 (Imprescindible Core)** | Rut, DteTypeCode, FiscalFolio, Money, CurrencyCode, TaxRate, TaxTypeCode, FiscalAddress. | Requerido para emitir o recibir DTEs básicos (Factura 33, Boleta 39\)12. |
| **P1 (Empresarial)** | DocumentReference, DispatchType, EconomicActivityCode, TaxPeriod, WithholdingRate, FolioRange. | Necesario para notas de crédito/débito, guías de despacho y retenciones de honorarios5. |
| **P2 (Especializado)** | ForeignTaxId, ExchangeRate, GeoLocationCode, FiscalStamp. | Facturación de exportación, operaciones multimoneda y representación gráfica PDF. |
| **P3 (Avanzado / Nicho)** | UnitOfMeasure, Catálogos extendidos de impuestos específicos a productos regulados. | Facturación para industrias específicas o rubros regulados. |

## **10\. Architectural Decision Records (ADRs) y Análisis de Riesgos Arquitectónicos**

### **Registros de Decisiones de Arquitectura (ADRs)**

#### **ADR-001: Unificación de RUT, RUN y DV en el Value Object Rut**

* **Estado:** Aprobado.  
* **Contexto:** Existía el riesgo de crear objetos separados para el número de RUT y el dígito verificador.  
* **Decisión:** Unificar la estructura en el readonly record struct Rut. El dígito verificador es una propiedad matemática derivada que no posee semántica aislada.  
* **Consecuencias:** Garantiza que no existan instancias con número válido y dígito verificador inconsistente11. Maximiza el rendimiento al evitar alocaciones secundarias.

#### **ADR-002: Modelado de Catálogos SII como Catalog VOs en lugar de C\# Enums**

* **Estado:** Aprobado.  
* **Contexto:** Los códigos de tipos de DTE e impuestos son normados por el SII pero pueden incorporar nuevos valores mediante resoluciones exentas1.  
* **Decisión:** Modelar DteTypeCode y TaxTypeCode como Value Objects orientados a catálogo en lugar de enum cerrados de C\#.  
* **Consecuencias:** Permite incorporar nuevos tipos de documentos sin necesidad de recompilar el núcleo del sistema ni romper compatibilidad hacia atrás1.

#### **ADR-003: Exclusión de CAF y TED de los Value Objects de Dominio**

* **Estado:** Aprobado.  
* **Contexto:** El CAF (Autorización de Folios) y el TED (Timbre Electrónico) contienen claves privadas y datos XML2.  
* **Decisión:** Reclasificar el CAF como Entidad de Seguridad/Infraestructura y el TED como Modelo de Integración. El dominio fiscal solo interactúa con FiscalFolio y FiscalStamp.  
* **Consecuencias:** Aísla el dominio de negocio respecto a las librerías criptográficas XML (System.Security.Cryptography.Xml) y algoritmos RSA11.

#### **ADR-004: Adopción de readonly record struct e ISpanParsable\<T\> en .NET 10**

* **Estado:** Aprobado.  
* **Contexto:** Se requiere alto rendimiento para el procesamiento de volúmenes masivos de DTEs en plataformas SaaS.  
* **Decisión:** Implementar los Value Objects como readonly record struct aprovechando las interfaces de parsing sobre ReadOnlySpan\<char\>.  
* **Consecuencias:** Proporciona un procesamiento libre de asignaciones en la Heap (*Zero-Allocations*), compatibilidad plena con compilación Native AOT y menor presión sobre el Garbage Collector.

#### **ADR-005: Aislamiento Completo de Esquemas XML SII mediante Anti-Corruption Layer (ACL)**

* **Estado:** Aprobado.  
* **Contexto:** Las estructuras XML del SII utilizan convenciones rígidas y nombres abreviados que contaminan el modelo de negocio10.  
* **Decisión:** Prohibir atributos de serialización XML o referencias a esquemas XSD dentro del dominio. Toda conversión debe ser ejecutada por la capa ACL.  
* **Consecuencias:** Aumenta la mantenibilidad. Las modificaciones en la plataforma técnica del SII no afectan las reglas de negocio del core fiscal10.

### **Análisis de Riesgos Arquitectónicos**

> 1. **Riesgo de Acoplamiento al Formato XML del SII:** Forzar las entidades de dominio para que coincidan con los nodos XML del DTE degrada el modelo a una estructura anémica10. *Mitigación:* Aislamiento estricto mediante la capa Anti-Corruption Layer (ACL) y ensamblados NuGet independientes.  
> 2. **Riesgo de Sobre-Modelado (Over-Modeling):** Transformar cada tipo primitivo (string, decimal, int) en un Value Object incrementa la complejidad del sistema sin aportar valor. *Mitigación:* Aplicación rigurosa del catálogo mínimo coherente, descartando VOs sin invariantes de negocio claras.  
> 3. **Riesgo de Inflexibilidad por Cambios Legales:** Cambios en alícuotas o reglas de retención que rompan el cálculo del dominio5. *Mitigación:* Encapsulamiento de reglas en políticas versionadas (ej. la escala de retención de la Ley N° 21.133) y uso de Value Objects guiados por catálogo5.

## **11\. Respuesta Definitiva a la Pregunta Final de Arquitectura**

El núcleo del dominio fiscal chileno debe estructurarse mediante un conjunto mínimo, completo y coherente de abstractions que balancee expresividad, rendimiento y mantenibilidad normativa:

### **Value Objects del Núcleo del Fiscal Domain Chileno**

* Rut: Identificador tributario nacional validado con Modulo 112.  
* DteTypeCode: Código de tipo de DTE con metadatos de capacidades tributarias9.  
* FiscalFolio: Número correlativo de folio fiscal autorizado.  
* FolioRange: Rango cerrado de folios autorizados.  
* DocumentReference: Estructura de referencia a documentos tributarios o comerciales14.  
* ReferenceTypeCode: Código de motivo de referencia del SII14.  
* DispatchType: Tipo de traslado de mercaderías13.  
* FiscalAddress: Dirección tributaria declarada ante el SII.  
* EconomicActivityCode: Código de giro registrado en el SII.  
* TaxTypeCode: Código del impuesto según el catálogo oficial del SII9.  
* WithholdingRate: Tasa de retención gradual según la Ley N° 21.1335.

### **Value Objects Pertenecientes al Shared Kernel**

* Money: Cantidad monetaria inmutable con redondeo entero para CLP.  
* CurrencyCode: Moneda de la transacción (ISO 4217).  
* ExchangeRate: Tasa de conversión de moneda extranjera a Pesos Chilenos.  
* TaxPeriod: Período tributario impositivo (YYYY-MM).  
* TaxRate: Tasa impositiva general.  
* UnitOfMeasure: Unidad de medida estandarizada.  
* ForeignTaxId: Identificador para sujetos tributarios extranjeros.

### **Abstracciones Específicas del Ecosistema DTE**

* FiscalStamp: Digest inmutable y firma del timbre DTE2.  
* DteTypeCode: Clasificación operativa del documento electrónico9.  
* DocumentReference: Vinculación de trazabilidad entre DTEs14.

### **Conceptos Excluidos del Dominio de Value Objects**

* **Agregados y Entidades:** Taxpayer (Contribuyente), DTE (Documento concreto), BranchOffice (Sucursal) y ElectronicAssignment (Cesión de facturas)1.  
* **Modelos de Infraestructura y Seguridad:** CAF (Autorización de Folios XML), TED (Fragmento XML e imagen PDF417), Certificados Digitales PKCS\#12 y Tokens SOAP del SII2.  
* **Modelos de Transmisión:** Clases DTO generadas a partir de esquemas XSD y estructuras de petición/respuesta HTTP/SOAP10.  
* **Primitivos Descartados:** VerificationDigit, RutNumber, DocumentNumber y TaxpayerId (absorbidos o unificados bajo la abstracción canónica Rut).

#### **Fuentes citadas**

> 1. Servicio de Impuestos Internos \- Sii, [https://www.sii.cl/normativa\_legislacion/resoluciones/2021/res\_ind2021.htm](https://www.sii.cl/normativa_legislacion/resoluciones/2021/res_ind2021.htm)  
> 2. Glosario — Términos técnicos y regulatorios chilenos \- Moit, [https://moit.cl/recursos/glosario/](https://moit.cl/recursos/glosario/)  
> 3. Todo lo que debes saber sobre facturas en Chile \- Laudus ERP, [https://laudus.cl/contenidos/gestion-erp/todo-lo-que-debes-saber-sobre-facturas-en-chile/](https://laudus.cl/contenidos/gestion-erp/todo-lo-que-debes-saber-sobre-facturas-en-chile/)  
> 4. Servicio de Impuestos Internos \- Sii, [https://www.sii.cl/normativa\_legislacion/resoluciones/2020/res\_ind2020.htm](https://www.sii.cl/normativa_legislacion/resoluciones/2020/res_ind2020.htm)  
> 5. SII informa retención de impuestos del 15,25% en 2026 para trabajadores independientes que emiten Boletas de Honorarios, [https://www.sii.cl/noticias/2025/261225noti01smn.htm](https://www.sii.cl/noticias/2025/261225noti01smn.htm)  
> 6. Boleta de honorarios: Conoce la tabla de retención hasta el 2028 \- Rex+, [https://rexmas.com/blog/retencion-anual-boleta-de-honorarios/](https://rexmas.com/blog/retencion-anual-boleta-de-honorarios/)  
> 7. Aumento Gradual de Retención de Honorarios y Protección Social \- SII, [https://www.sii.cl/destacados/boletas\_honorarios/aumento\_gradual.html](https://www.sii.cl/destacados/boletas_honorarios/aumento_gradual.html)  
> 8. Convertir documentos del SII (Chile), de XML a PDF \- Antonio Cañada \- Medium, [https://antoniocanada.medium.com/convertir-documentos-del-sii-chile-de-xml-a-pdf-4edcd444959e](https://antoniocanada.medium.com/convertir-documentos-del-sii-chile-de-xml-a-pdf-4edcd444959e)  
> 9. Tipos de Códigos de los Documentos Tributarios en el SII | Centro de Ayuda Clay, [https://ayuda.clay.cl/es/article/tipos-de-codigos-de-los-documentos-tributarios-en-el-sii-vxrbkq/](https://ayuda.clay.cl/es/article/tipos-de-codigos-de-los-documentos-tributarios-en-el-sii-vxrbkq/)  
> 10. Formato XML de documentos electrónicos \- SII | Servicio de Impuestos Internos, [https://www.sii.cl/servicios\_online/3532-formato\_xml-3811.html](https://www.sii.cl/servicios_online/3532-formato_xml-3811.html)  
> 11. Validar Documento SII \- Validar Factura y XML DTE Gratis | DTEPDF.cl, [https://dtepdf.cl/validar](https://dtepdf.cl/validar)  
> 12. Lista de tipos de documentos electrónicos con código SII (Numeración), [https://ayuda.relbase.cl/lista-de-tipos-de-documentos-electr%C3%B3nicos-con-c%C3%B3digo-sii-numeraci%C3%B3n](https://ayuda.relbase.cl/lista-de-tipos-de-documentos-electr%C3%B3nicos-con-c%C3%B3digo-sii-numeraci%C3%B3n)  
> 13. Tipos de Documentos Tributarios Electrónicos \- Integración para la emisión de DTE | API Gateway, [https://www.apigateway.cl/academy/integracion-para-la-emision-de-dte/introduccion/tipos-documentos](https://www.apigateway.cl/academy/integracion-para-la-emision-de-dte/introduccion/tipos-documentos)  
> 14. Referencias de Documentos Electrónicos | Centro de Ayuda Manager+, [https://ayuda.managermas.cl/es/articles/5790131-referencias-de-documentos-electronicos](https://ayuda.managermas.cl/es/articles/5790131-referencias-de-documentos-electronicos)  
> 15. Normativa de Retención de Impuestos para Socios Conductores \- Uber, [https://www.uber.com/cl/es/blog/retencion-de-impuestos-socios-conductores/](https://www.uber.com/cl/es/blog/retencion-de-impuestos-socios-conductores/)

[image1]: <data:image/png;base64,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>

[image2]: <data:image/png;base64,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>

[image3]: <data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABIAAAAZCAYAAAA8CX6UAAABnUlEQVR4Xo2TsUoDQRCGZyGComIjKGhlIQTsRG1iIVgH8SV8n2BpY2dhIyhYWIg+g9hGEQQhWilo0Pjd3V525naT3A+zM/vPzr+7s3ciCk5PhnAFHyUjQhJcolgTEW8mkZqH5cMsIRpr2C3DPj6O4KolssfwSjjIzeX+Hf7Ncz/El/DruiioRNu4E4Y+fKvCr7L0Av6DybbNKXjxecZ77AFbtMkcS8SP+GtsJpEfxk2sR3yOb1R75HFKnK3J1haIbiXShh3AH1UTCgjJF7aVEijRQaiPb6WOC2aJb/Cf2OYooTkSt1TZ/ihQt4LrEmWvuzZkA/JeNBl6UvbH96Wy6T72h11h0zYVGtmW/NtxRX/iY2fMMSNC7tDQQcMVz+7Us3sh75axOyzrz4JNljsWkw1c9qFxLddQ/BTugLjL5ExyEQV16l3sCab4LUR+sRfsWfJfw31L9luI7Igp8xjP2DsFTODtPWug1lItWqvAI7qQHwJvv6OR2ro4ELogIiapJfIRYfessKWPsgkqcWWjoRFdwR537Pr6J9KwJ/oHizk2LNsaP7kAAAAASUVORK5CYII=>

[image4]: <data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABUAAAAYCAYAAAAVibZIAAABcElEQVR4XpVUO07EQAydERXiBNttuwUVoqGioKFA2gpxBi5AyQ2QOAIFEqfgENwAiaWkYunQ8mbiZJ4/2YQnOeM828+eJJOUCHm4MKGpGKOJmbjAD5rNwuxix5vGPJEDZwa+6++m6pHTEpebKEQFB/CvsS5bLDm9FewW9orAL9anoOkh7Ar2CH6DdQs7UcMbFNE17Az2gZROVKOIXsLOYfewbS6iAj9EYxaw9yLK4YbhOd6lflLhebFjL0AYUc4Yiqtom9SoGIhoUqKtZPAgmtX2K2o4t1Uuk9sXmO0L1MBOlCcNYba//wEEovojF7RJXdCMm0NRThNPXlQaRI0y35LoM4eCuvKifrCedrdu3IqLVD/6epp2SNoh7Rv+G+xYco7Av4D/gkbN6XLTJ+yhZnhtzZgdTKLl9B7vL1CY3UAFAz1XzI0ZQe0YQwhO1N7uBnFc/9Vdzn8aNGi16QaO7TEWCE9Xheelgws4ghE8a8Ef/74x4g/sd50AAAAASUVORK5CYII=>

[image5]: <data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABUAAAAYCAYAAAAVibZIAAABuUlEQVR4XpVTPUtDQRDcKwQlgoiVoCAiiIUgaGUtlhLQQsw/EGtrQSy0tLRLkZ+QSpHU2lnYWIgg+AdMGXTu693u3d5DB+Z23+ze3F7yHhnSYEjXOZQOJ0WdP8iQUAgKhB+HckBCuAEvKzmDVFIT1+XPUss1rKDlODUV7evgDfQ7xBNwJqsHGNrAeortj8gnyPt5S/A+RHhF3AJnwUvwHpxLjRHetIsNu8g/IfSLmxMtg2+QekybB5/ReMa0gLAZYRHhA/SmQQ0HWLMxuN2UfHGAMCI/uZNyNKZcDKa3JEybP832foGrUWfRLfVJ3WYzNnJSW7am8gZ8WFMxJX+1EVia1g7zCJMaZpqcO+ADaaZuUuMnjf3Z5sY0FRtcUbwm1/318cbQEtPi4hBMjTeVOIBu3+E91j+NfIg4NDaP4KYm/aaDpAYYWsD6BF4kidbIT+m/QLkDp6MIbYLCD3LLb/AFnZusbwc974jn4BG5F5+uwanSNBtX1ZPQQdhH3iX/lWU9AvltMxRCQHGw0ORErR6tQoRiqoQMSn+Rx8fC6B9TR1QLDf4yNYfoqO/iUyuhDbylvqs4QHn6BWXyPpqKmoWEAAAAAElFTkSuQmCC>

[image6]: <data:image/png;base64,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>